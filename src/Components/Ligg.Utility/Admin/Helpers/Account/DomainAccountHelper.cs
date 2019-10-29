using System;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using System.Threading;
using System.Web;

namespace Ligg.Utility.Admin.Helpers.Account
{
    public sealed class DomainAccountHelper
    {
        ///域名
        public static string DomainName = "";
        /// LDAP 地址
        public static string LdapDomain = "";
        /// LDAP绑定路径
        public static string AdPath = "";
        public static string OuPath = "";
        private static string LdapStr = AdPath + OuPath + LdapDomain;
        ///登录帐号
        public static string DomainAdminAccount = "";
        ///登录密码
        public static string DomainAdminPassword = "";

        ///用户登录验证结果
        public enum LoginResult
        {
            ///正常登录
            LOGIN_USER_OK = 0,
            ///用户不存在
            LOGIN_USER_DOESNT_EXIST,
            ///用户帐号被禁用
            LOGIN_USER_ACCOUNT_INACTIVE,
            ///用户密码不正确
            LOGIN_USER_PASSWORD_INCORRECT
        }
        ///用户属性定义标志
        public enum ADS_USER_FLAG_ENUM
        {
            ///登录脚本标志。如果通过 ADSI LDAP 进行读或写操作时，该标志失效。如果通过 ADSI WINNT，该标志为只读。
            ADS_UF_SCRIPT = 0X0001,
            ///用户帐号禁用标志
            ADS_UF_ACCOUNTDISABLE = 0X0002,
            ///主文件夹标志
            ADS_UF_HOMEDIR_REQUIRED = 0X0008,
            ///过期标志
            ADS_UF_LOCKOUT = 0X0010,
            ///用户密码不是必须的
            ADS_UF_PASSWD_NOTREQD = 0X0020,
            ///密码不能更改标志
            ADS_UF_PASSWD_CANT_CHANGE = 0X0040,
            ///使用可逆的加密保存密码
            ADS_UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 0X0080,
            ///本地帐号标志
            ADS_UF_TEMP_DUPLICATE_ACCOUNT = 0X0100,
            ///普通用户的默认帐号类型
            ADS_UF_NORMAL_ACCOUNT = 0X0200,
            ///跨域的信任帐号标志
            ADS_UF_INTERDOMAIN_TRUST_ACCOUNT = 0X0800,
            ///工作站信任帐号标志
            ADS_UF_WORKSTATION_TRUST_ACCOUNT = 0x1000,
            ///服务器信任帐号标志
            ADS_UF_SERVER_TRUST_ACCOUNT = 0X2000,
            ///密码永不过期标志
            ADS_UF_DONT_EXPIRE_PASSWD = 0X10000,
            /// MNS 帐号标志
            ADS_UF_MNS_LOGON_ACCOUNT = 0X20000,
            ///交互式登录必须使用智能卡
            ADS_UF_SMARTCARD_REQUIRED = 0X40000,
            ///当设置该标志时，服务帐号（用户或计算机帐号）将通过 Kerberos 委托信任
            ADS_UF_TRUSTED_FOR_DELEGATION = 0X80000,
            ///当设置该标志时，即使服务帐号是通过 Kerberos 委托信任的，敏感帐号不能被委托
            ADS_UF_NOT_DELEGATED = 0X100000,
            ///此帐号需要 DES 加密类型
            ADS_UF_USE_DES_KEY_ONLY = 0X200000,
            ///不要进行 Kerberos 预身份验证
            ADS_UF_DONT_REQUIRE_PREAUTH = 0X4000000,
            ///用户密码过期标志
            ADS_UF_PASSWORD_EXPIRED = 0X800000,
            ///用户帐号可委托标志
            ADS_UF_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION = 0X1000000
        }

        public DomainAccountHelper()
        {
            //
        }

        //#list
        public static DataTable GetOuUsers(string ouString)
        {
            DirectoryEntry de;
            if (!String.IsNullOrEmpty(ouString))
                de = new DirectoryEntry(AdPath + ouString + LdapDomain, DomainAdminAccount, DomainAdminPassword);
            else
                de = new DirectoryEntry(AdPath + LdapDomain, DomainAdminAccount, DomainAdminPassword);
            string flag = "0";
            try
            {
                var mySearcher = new System.DirectoryServices.DirectorySearcher(de);

            }
            catch { flag = "1"; }

            if (flag == "1")
            {
                de.Close(); return null;
            }
            else
            {
                var mySearcher = new System.DirectoryServices.DirectorySearcher(de);
                mySearcher.Filter = "(&(&(objectCategory=person)(objectClass=user)))";
                int id = 0;
                var dt = new DataTable();
                foreach (System.DirectoryServices.SearchResult result in mySearcher.FindAll())
                {

                    DataRow dr = dt.NewRow();
                    id = id + 1;
                    dr[0] = id.ToString();
                    dr[1] = result.Properties["samAccountName"][0].ToString();
                    string strr = result.Properties["samAccountName"][0].ToString();
                    dr[2] = result.Properties["samaccounttype"][0].ToString();
                    dr[3] = result.Properties["name"][0].ToString();
                    string strr1 = result.Properties["name"][0].ToString();
                    try
                    {
                        dr[4] = result.Properties["memberof"][0].ToString();
                    }
                    catch (Exception err) { dr[4] = ""; Console.WriteLine("Debug", err.ToString()); }
                    string status_str = result.GetDirectoryEntry().Properties["userAccountControl"][0].ToString();
                    if (status_str == "514")
                        dr[5] = "No";
                    else dr[5] = "Yes";

                    dt.Rows.Add(dr);
                }
                de.Close();
                return dt;
            }

        }

        //public static List<AdAccount> GetOuUsersToList(string ou_string)
        //{
        //    DirectoryEntry de1;
        //    List<AdAccount> accountList = new List<AdAccount>();
        //    string memberof_OK;

        //    if (!String.IsNullOrEmpty(ou_string))
        //        de1 = new DirectoryEntry(AdPath + ou_string + "," + LdapDomain, DomainAdminAccount, DomainAdminPassword);
        //    else
        //        de1 = new DirectoryEntry(AdPath + LdapDomain, DomainAdminAccount, DomainAdminPassword);
        //    string flag = "0";
        //    try
        //    {
        //        System.DirectoryServices.DirectorySearcher mySearcher = new System.DirectoryServices.DirectorySearcher(de1);

        //    }
        //    catch { flag = "1"; }

        //    if (flag == "1")
        //    {
        //        de1.Close(); return null;
        //    }
        //    else
        //    {
        //        System.DirectoryServices.DirectorySearcher mySearcher = new System.DirectoryServices.DirectorySearcher(de1);
        //        mySearcher.Filter = "(&(&(objectCategory=person)(objectClass=user)))";
        //        int id = 0;
        //        foreach (System.DirectoryServices.SearchResult result in mySearcher.FindAll())
        //        {
        //            string strr = result.Properties["samAccountName"][0].ToString();
        //            string strr1 = result.Properties["name"][0].ToString();
        //            try
        //            {
        //                memberof_OK = result.Properties["memberof"][0].ToString();
        //            }
        //            catch (Exception err) { memberof_OK = ""; Console.WriteLine("Debug", err.ToString()); }
        //            string status_str = result.GetDirectoryEntry().Properties["userAccountControl"][0].ToString();
        //            string logonName = result.Properties["samAccountName"][0].ToString();
        //            string name = result.Properties["name"][0].ToString();
        //            int user_status = 0;
        //            user_status = status_str == "514" ? 1 : 2;
        //            string remark = "";
        //            if (status_str == "66048") remark = "password never expires";

        //            //status_str:514,disnable;512,OK;66048,密码永不过期
        //            accountList.Add(new AdAccount
        //            {
        //                LogonName = logonName,
        //                Status = user_status,
        //                //Memberof=memberof_OK, 
        //                UserName = name,
        //                Remark = remark
        //            });
        //        }
        //        de1.Close();
        //        return accountList;
        //    }

        //}


        public static DataTable GetChildOus(DataTable dt, string ouString, int level)
        {
            level = level + 1;
            DirectoryEntry de;
            if (!String.IsNullOrEmpty(ouString))
                de = new DirectoryEntry(AdPath + ouString + "," + LdapDomain, DomainAdminAccount, DomainAdminPassword);
            else
                de = new DirectoryEntry(AdPath + LdapDomain, DomainAdminAccount, DomainAdminPassword);
            try
            {
                foreach (DirectoryEntry child in de.Children)
                {
                    string str = child.Name.ToString();
                    string str1 = "";
                    str1 = child.SchemaClassName.ToString();
                    if (str1 == "organizationalUnit")
                    {
                        DataRow dr = dt.NewRow();
                        string str2;
                        // if (!String.IsNullOrEmpty(ou_string))
                        {
                            str2 = child.Name.ToString() + "," + ouString;
                            dr[0] = child.Name.ToString();
                            dr[1] = str2;
                            dr[2] = level.ToString();
                            dt.Rows.Add(dr);
                            dt = GetChildOus(dt, str2, level);
                        }
                    }
                }

                de.Close();
            }
            catch (Exception err)
            {
                throw new ArgumentException("\n>> FindChildOus Error:" + err.ToString());
            }

            return dt;
        }


        //#add
        public static DirectoryEntry Create(string ldapDN, string commonName, string samAccountName, string password)
        {
            DirectoryEntry entry = GetDirectoryObject();
            DirectoryEntry subEntry = entry.Children.Find(ldapDN);
            DirectoryEntry deUser = subEntry.Children.Add("CN=" + commonName, "user");
            deUser.Properties["samAccountName"].Value = samAccountName;
            deUser.CommitChanges();
            EnableUserByCommonName(commonName);
            SetPasswordByCommonName(commonName, password);
            deUser.Close();
            return deUser;
        }

        public static DirectoryEntry CreateNewAccount(string commonName, string samAccountName, string password)
        {
            return Create("CN=Users", commonName, samAccountName, password);
        }



        //#del


        //#edit
        public static void AddToGroup(string commonName, string groupName)
        {
            using (new Impersonator(DomainName, DomainAdminAccount, DomainAdminPassword))
            {
                DirectoryEntry group = GetDirectoryEntryByGroupName(groupName);
                DirectoryEntry user = GetDirectoryEntryByCommonName(commonName);
                group.Properties["member"].Add(user.Properties["distinguishedName"].Value);
                group.CommitChanges();
                group.Close();
                user.Close();
            }
        }

        public static void RemoveFromGroup(string userCommonName, string groupName)
        {
            using (new Impersonator(DomainName, DomainAdminAccount, DomainAdminPassword))
            {
                DirectoryEntry group = GetDirectoryEntryByGroupName(groupName);
                DirectoryEntry user = GetDirectoryEntryByCommonName(userCommonName);
                group.Properties["member"].Remove(user.Properties["distinguishedName"].Value);
                group.CommitChanges();
                group.Close();
                user.Close();
            }
        }

        public static void SetPasswordByCommonName(string commonName, string newPassword)
        {
            using (new Impersonator(DomainName, DomainAdminAccount, DomainAdminPassword))
            {
                DirectoryEntry de = GetDirectoryEntryByCommonName(commonName);
                de.Invoke("SetPassword", new object[] { newPassword });
                de.Close();
            }
        }

        public static void SetPasswordBySamAccountName(string samAccountName, string newPassword)
        {
            using (new Impersonator(DomainName, DomainAdminAccount, DomainAdminPassword))
            {
                DirectoryEntry de = GetDirectoryEntryBySamAccountName(samAccountName);
                de.Invoke("SetPassword", new object[] { newPassword });
                de.Close();
            }
        }

        public static void EnableUserByCommonName(string commonName)
        {
            Enable(GetDirectoryEntryByCommonName(commonName));
        }


        public static void DisableUserByCommonName(string commonName)
        {
            Disable(GetDirectoryEntryByCommonName(commonName));
        }

        //#get
        public static string GetCurrentIdentityName(string getType)
        {
            IPrincipal principal = Thread.CurrentPrincipal;
            var str = principal.GetType().ToString();
            if (typeof(WindowsPrincipal).IsAssignableFrom(principal.GetType())) //验证Windows账户标识
            {
                if (getType == "0")
                {
                    var windowsprinciple = (WindowsPrincipal)principal;
                    str = windowsprinciple.Identity.Name;
                }
                else if (getType == "1")
                {
                    str = new WindowsPrincipal(WindowsIdentity.GetCurrent()).Identity.Name;
                }
                else
                {
                    HttpContext myContext = HttpContext.Current;
                    str = myContext.User.Identity.Name; ;
                }
                if (!String.IsNullOrEmpty(str))
                    str = str.Split('\\')[1];
            }
            else
            {
                var genericprincipal = (GenericPrincipal)principal;
                str = genericprincipal.Identity.Name;
            }
            return str;
        }

        public static string GetProperty(DirectoryEntry de, string propertyName)
        {
            if (de.Properties.Contains(propertyName))
            {
                return de.Properties[propertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetProperty(SearchResult searchResult, string propertyName)
        {
            if (searchResult.Properties.Contains(propertyName))
            {
                return searchResult.Properties[propertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static void SetProperty(DirectoryEntry de, string propertyName, string propertyValue)
        {
            if (propertyValue != string.Empty || propertyValue != "" || propertyValue != null)
            {
                if (de.Properties.Contains(propertyName))
                {
                    de.Properties[propertyName][0] = propertyValue;
                }
                else
                {
                    de.Properties[propertyName].Add(propertyValue);
                }
            }
        }

        //#Check
        public static LoginResult VerifyByCommonNameAndPassword(string commonName, string password)
        {
            DirectoryEntry de = GetDirectoryEntryByCommonName(commonName);

            if (de != null)
            {
                // 必须在判断用户密码正确前，对帐号激活属性进行判断；否则将出现异常。
                int userAccountControl = Convert.ToInt32(de.Properties["userAccountControl"][0]);
                de.Close();

                if (!IsActive(userAccountControl))
                    return LoginResult.LOGIN_USER_ACCOUNT_INACTIVE;

                if (GetDirectoryEntryByCommonNameAndPassword(commonName, password) != null)
                    return LoginResult.LOGIN_USER_OK;
                else
                    return LoginResult.LOGIN_USER_PASSWORD_INCORRECT;
            }
            else
            {
                return LoginResult.LOGIN_USER_DOESNT_EXIST;
            }
        }

        public static LoginResult VerifyBySamAccountNameAndPassword(string samAccountName, string password)
        {
            try
            {
                //var impersonate = new IdentityImpersonation(samAccountName, password, DomainName);
                //impersonate.BeginImpersonate();
                //impersonate.StopImpersonate();
                using (new Impersonator(DomainName, samAccountName, password))
                {
                    //isValid = true;
                }
            }
            catch
            {
                return LoginResult.LOGIN_USER_PASSWORD_INCORRECT;
            }
            return LoginResult.LOGIN_USER_OK;
        }

        public static bool IsExisting(string commonName)
        {
            DirectoryEntry de = GetDirectoryObject();
            DirectorySearcher deSearch = new DirectorySearcher(de);
            deSearch.Filter = "(&(&(objectCategory=person)(objectClass=user))(cn=" + commonName + "))";       // LDAP 查询串
            SearchResultCollection results = deSearch.FindAll();

            if (results.Count == 0)
                return false;
            else
                return true;
        }

        public static bool IsActive(int userAccountControl)
        {
            int userAccountControl_Disabled = Convert.ToInt32(ADS_USER_FLAG_ENUM.ADS_UF_ACCOUNTDISABLE);
            int flagExists = userAccountControl & userAccountControl_Disabled;

            if (flagExists > 0)
                return false;
            else
                return true;
        }

        public static bool IsInGroup(string account, string domainGrpName)
        {
            var ctx = new PrincipalContext(ContextType.Domain, DomainName);
            var userPrincipal = UserPrincipal.FindByIdentity(ctx, account);
            var groups = userPrincipal.GetGroups(ctx);
            foreach (GroupPrincipal g in groups)
            {
                if (g.SamAccountName.ToLower() == domainGrpName.ToLower()) return true;
            }
            return false;

        }

        public static bool IsInGroup(string domainName, string account, string domainGrpName)
        {
            var ctx = new PrincipalContext(ContextType.Domain, domainName);
            var userPrincipal = UserPrincipal.FindByIdentity(ctx, account);
            var groups = userPrincipal.GetGroups(ctx);
            foreach (GroupPrincipal g in groups)
            {
                if (g.SamAccountName.ToLower() == domainGrpName.ToLower()) return true;
            }
            return false;
        }

        //#common
        private static DirectoryEntry GetDirectoryObject()
        {
            var entry = new DirectoryEntry(LdapStr, DomainAdminAccount, DomainAdminPassword, AuthenticationTypes.Secure);
            return entry;
        }

        private static DirectoryEntry GetDirectoryObject(string domainReference)
        {
            var entry = new DirectoryEntry(AdPath + domainReference, DomainAdminAccount, DomainAdminPassword, AuthenticationTypes.Secure);
            return entry;
        }

        private static DirectoryEntry GetDirectoryObject(string userName, string password)
        {
            string adpth = AdPath + LdapDomain;
            var entry = new DirectoryEntry(adpth, userName, password, AuthenticationTypes.None);
            return entry;
        }

        private static DirectoryEntry GetDirectoryObject(string domainReference, string userName, string password)
        {
            var entry = new DirectoryEntry(AdPath + domainReference, userName, password, AuthenticationTypes.Secure);
            return entry;
        }

        private static DirectoryEntry GetDirectoryEntryByCommonName(string commonName)
        {
            DirectoryEntry de = GetDirectoryObject();
            DirectorySearcher deSearch = new DirectorySearcher(de);
            deSearch.Filter = "(&(&(objectCategory=person)(objectClass=user))(cn=" + commonName + "))";
            deSearch.SearchScope = SearchScope.Subtree;
            try
            {
                SearchResult result = deSearch.FindOne();
                de = new DirectoryEntry(result.Path);
                return de;
            }
            catch (Exception err)
            {
                throw new ArgumentException("GetDirectoryEntry failed:" + err.ToString());
            }
        }

        private static DirectoryEntry GetDirectoryEntryByCommonNameAndPassword(string commonName, string password)
        {
            DirectoryEntry de = GetDirectoryObject(commonName, password);
            DirectorySearcher deSearch = new DirectorySearcher(de);
            deSearch.Filter = "(&(&(objectCategory=person)(objectClass=user))(cn=" + commonName + "))";
            deSearch.SearchScope = SearchScope.Subtree;

            try
            {
                SearchResult result = deSearch.FindOne();
                de = new DirectoryEntry(result.Path);
                return de;
            }
            catch
            {
                return null;
            }
        }

        private static DirectoryEntry GetDirectoryEntryBySamAccountName(string samAccountName)
        {
            DirectoryEntry de = GetDirectoryObject();
            DirectorySearcher deSearch = new DirectorySearcher(de);
            deSearch.Filter = "(&(&(objectCategory=person)(objectClass=user))(samAccountName=" + samAccountName + "))";
            deSearch.SearchScope = SearchScope.Subtree;
            try
            {
                SearchResult result = deSearch.FindOne();
                de = new DirectoryEntry(result.Path);
                return de;
            }
            catch
            {
                return null;
            }
        }

        private static DirectoryEntry GetDirectoryEntryBySamAccountNameAndPassword(string samAccountName, string password)
        {
            DirectoryEntry de = GetDirectoryEntryBySamAccountName(samAccountName);
            if (de != null)
            {
                string commonName = de.Properties["cn"][0].ToString();
                if (GetDirectoryEntryByCommonNameAndPassword(commonName, password) != null)
                    return GetDirectoryEntryByCommonNameAndPassword(commonName, password);
                else return null;
            }
            else
            {
                return null;
            }
        }

        private static DirectoryEntry GetDirectoryEntryByGroupName(string groupName)
        {
            DirectoryEntry de = GetDirectoryObject();
            DirectorySearcher deSearch = new DirectorySearcher(de);
            deSearch.Filter = "(&(objectClass=group)(cn=" + groupName + "))";
            deSearch.SearchScope = SearchScope.Subtree;

            try
            {
                SearchResult result = deSearch.FindOne();
                de = new DirectoryEntry(result.Path);
                return de;
            }
            catch
            {
                return null;
            }
        }

        private static void Enable(DirectoryEntry de)
        {
            using (new Impersonator(DomainName, DomainAdminAccount, DomainAdminPassword))
            {
                de.Properties["userAccountControl"][0] = ADS_USER_FLAG_ENUM.ADS_UF_NORMAL_ACCOUNT | ADS_USER_FLAG_ENUM.ADS_UF_DONT_EXPIRE_PASSWD;
                de.CommitChanges();
                de.Close();
            }
        }

        private static void Disable(DirectoryEntry de)
        {
            using (new Impersonator(DomainName, DomainAdminAccount, DomainAdminPassword))
            {
                de.Properties["userAccountControl"][0] = ADS_USER_FLAG_ENUM.ADS_UF_NORMAL_ACCOUNT |
                                                         ADS_USER_FLAG_ENUM.ADS_UF_DONT_EXPIRE_PASSWD |
                                                         ADS_USER_FLAG_ENUM.ADS_UF_ACCOUNTDISABLE;
                de.CommitChanges();
                de.Close();
            }
        }
    }
}
