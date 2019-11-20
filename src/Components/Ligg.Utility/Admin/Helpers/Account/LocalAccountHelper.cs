using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Ligg.Base.DataModel;

namespace Ligg.Utility.Admin.Helpers.Account
{
    internal class Win32API
    {
        [DllImport("netapi32.dll", EntryPoint = "NetApiBufferFree")]
        internal static extern void NetApiBufferFree(IntPtr bufptr);

        [DllImport("netapi32.dll", EntryPoint = "NetLocalGroupGetMembers")]
        internal static extern uint NetLocalGroupGetMembers(
            [MarshalAs(UnmanagedType.LPWStr)] string servername,
            [MarshalAs(UnmanagedType.LPWStr)] string localgroupname,
            uint level,
            ref IntPtr siPtr,
            uint prefmaxlen,
            ref uint entriesread,
            ref uint totalentries,
            IntPtr resumeHandle);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct LOCALGROUP_MEMBERS_INFO_2
        {
            public IntPtr lgrmi2_sid;
            public IntPtr lgrmi2_sidusage;
            public IntPtr lgrmi2_domainandname;
        }
    }

    public static class LocalAccountHelper
    {
        //#remark
        /*如果group里有乱字符账号，foreach (var acct in group.GetMembers()) 第二轮循环会throw error;
         * group.GetMembers().Count();foreach (var acct in group.GetMembers())会throw error;
         * ListByGroup/IsGroupValid/IsInGroup有乱码子帐号时会throw error
         ** group里有乱字符账号,有2种情况：1)里面有已删除的域帐号，2）里面正常域账号在断局域网后OK,断局域网重启后会变为乱码。
         *** 如何判断账号是否在group是一个难题。
        */
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        private static int LOCALGROUP_MEMBERS_INFO_2_SIZE;

        //#list
        //get error "network path not found, influenced By DNS issue, see problem notes"
        //when disconnected , domain account can't get out.
        //乱字符影响       
        public static List<LocalAccountInfo> List(string grpName)
        {

            try
            {
                if (!IsGroupValid(grpName)) return null; ;
                var group = GetGroupByName(grpName);
                if (group == null) return null;
                var acctInfos = new List<LocalAccountInfo>();
                foreach (var acct in group.GetMembers())
                {
                    var acctInfo = new LocalAccountInfo();
                    var fullName = acct.GetType().FullName;
                    acctInfo.TypeName = "Unknown";
                    if (!string.IsNullOrEmpty(fullName))
                    {
                        int n = fullName.LastIndexOf('.');
                        if (n != -1)
                        {
                            acctInfo.TypeName = fullName.Substring(n, fullName.Length - n);
                        }

                    }
                    acctInfo.SamName = acct.SamAccountName == null ? "Unknown" : acct.SamAccountName.ToLower();
                    acctInfos.Add(acctInfo);
                }
                return acctInfos;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".List Error: " + ex.Message);
            }

        }

        public static List<ValueText> List(string serverName, string grpName)
        {
            //unsafe
            {
                LOCALGROUP_MEMBERS_INFO_2_SIZE = Unsafe.SizeOf<Win32API.LOCALGROUP_MEMBERS_INFO_2>();
            }

            IntPtr UserInfoPtr = IntPtr.Zero;
            try
            {
                uint prefmaxlen1 = 0xFFFFFFFF, entriesread1 = 0, totalentries1 = 0;

                Win32API.NetLocalGroupGetMembers(serverName, grpName, 2, ref UserInfoPtr, prefmaxlen1, ref entriesread1, ref totalentries1, IntPtr.Zero);

                var valTexts = new List<ValueText>();
                for (int j = 0; j < totalentries1; j++)
                {
                    //converting unmanaged code to managed codes with using Marshal class 
                    //int newOffset1 = UserInfoPtr.ToInt32() + LOCALGROUP_MEMBERS_INFO_2_SIZE * j; //get a ex "算术运算导致溢出"
                    var newOffset1 = UserInfoPtr.ToInt64() + LOCALGROUP_MEMBERS_INFO_2_SIZE * j;
                    Win32API.LOCALGROUP_MEMBERS_INFO_2 memberInfo = (Win32API.LOCALGROUP_MEMBERS_INFO_2)Marshal.PtrToStructure(new IntPtr(newOffset1), typeof(Win32API.LOCALGROUP_MEMBERS_INFO_2));
                    string domainAndUserName = Marshal.PtrToStringAuto(memberInfo.lgrmi2_domainandname);
                    var domainAndUserNameArray = domainAndUserName.Split('\\');
                    var valText = new ValueText();
                    valText.Value = domainAndUserNameArray[0];
                    valText.Text = domainAndUserNameArray[1];
                    valTexts.Add(valText);
                }
                Win32API.NetApiBufferFree(UserInfoPtr);
                return valTexts;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".List Error: " + ex.Message);
            }

        }

 
        public static bool Add(string userName, string passWord, string displayName, string description, string groupName, bool userCannotChangePassword, bool passwordNeverExpires)
        {
            try
            {
                var ctx = new PrincipalContext(ContextType.Machine);
                var user = new UserPrincipal(ctx);
                user.SetPassword(passWord);
                user.DisplayName = displayName;
                user.Name = userName;
                user.Description = description;
                user.UserCannotChangePassword = userCannotChangePassword;
                user.PasswordNeverExpires = passwordNeverExpires;
                user.Save();
                if (!string.IsNullOrEmpty(groupName))
                {
                    var group = GroupPrincipal.FindByIdentity(ctx, groupName);
                    group.Members.Add(user);
                    group.Save();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

 
 

 
        //#get
        static GroupPrincipal GetGroupByName(string groupName)
        {
            try
            {
                var ctx = new PrincipalContext(ContextType.Machine);
                var groupWtCtx = new GroupPrincipal(ctx);
                var sch = new PrincipalSearcher(groupWtCtx);
                foreach (GroupPrincipal grpPrincipal in sch.FindAll())
                {
                    if (grpPrincipal != null && grpPrincipal.Name.ToLower().Equals(groupName.ToLower()))
                    {
                        return grpPrincipal;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }



        //**fit for local id, domain id; also fit for LAN is disconnected 
        public static bool IsValidAccountAndPassword(string domain, string id, string password)
        {
            try
            {
                if (!string.IsNullOrEmpty(domain))
                {
                    using (new Impersonator(domain, id, password))//when LAN is disconnected, ok? to be tested
                    {
                        return true;
                    }
                }
                else
                {
                    //return IsValidLocalAccountAndPassword(id, password); //when LAN is disconnected, always return false.
                    using (new Impersonator("", id, password)) //when LAN is disconnected, ok.
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static bool IsInGroup(string domain, string account, string localGrpName)
        {
            try
            {   //避免受乱码用户影响
                if (string.IsNullOrEmpty(domain))
                    return IsInGroupForLocalAccount(account, localGrpName);

                if (IsInGroupForDomainUserOrGroup(domain, account, localGrpName)) return true;

                //check if domain account groups are in localGrpName
                var ctx = new PrincipalContext(ContextType.Domain, domain);
                var userPrincipal = UserPrincipal.FindByIdentity(ctx, account);
                var groups = userPrincipal.GetGroups(ctx);
                foreach (GroupPrincipal g in groups)
                {
                    if (IsInGroupForDomainUserOrGroup(domain, g.SamAccountName, localGrpName))
                    {
                        return true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        private static bool IsInGroupForDomainUserOrGroup(string domain, string domainAcctOrGrp, string localGrpName)
        {
            try
            {
                var accouts = List("", localGrpName);
                foreach (var accout in accouts)
                {
                    if (accout.Value.ToLower() == domain.ToLower())
                    {
                        if (accout.Text.ToLower() == domainAcctOrGrp.ToLower()) return true;
                    }
                }
                return false;
            }

            catch (Exception ex)
            {
                return false;
            }
        }


        public static bool IsInGroupForLocalAccount(string account, string localGrpName)
        {
            try
            {
                var accouts = List("", localGrpName);
                foreach (var accout in accouts)
                {
                    if (accout.Value.ToLower() ==  Environment.MachineName.ToLower())
                    {
                        if (accout.Text.ToLower() == account.ToLower()) return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static bool IsGroupValid(string localGrpName)
        {
            try
            {
                var group = GetGroupByName(localGrpName);
                if (group == null) return false;
                //有乱码账号时会throw
                var qty = group.GetMembers().Count();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }



    public class LocalAccountInfo
    {
        public string TypeName;
        public string DistinguishedName;//CN(common Name)=foracctest02,OU=行长室,OU=宁波分行本部,OU=宁波分行,OU=uctestRoot,DC=uctest,DC=com
        public string SamName;  //chris.li
        public string Password;
        public string IsLocked;
        public int PasswordPolicy;
    }


}
