using System;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

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



        public static void SetPasswordByCommonName(string commonName, string newPassword)
        {
            using (new Impersonator(DomainName, DomainAdminAccount, DomainAdminPassword))
            {
                DirectoryEntry de = GetDirectoryEntryByCommonName(commonName);
                de.Invoke("SetPassword", new object[] { newPassword });
                de.Close();
            }
        }

        public static void EnableUserByCommonName(string commonName)
        {
            Enable(GetDirectoryEntryByCommonName(commonName));
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


        private static DirectoryEntry GetDirectoryObject(string userName, string password)
        {
            string adpth = AdPath + LdapDomain;
            var entry = new DirectoryEntry(adpth, userName, password, AuthenticationTypes.None);
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


        private static void Enable(DirectoryEntry de)
        {
            using (new Impersonator(DomainName, DomainAdminAccount, DomainAdminPassword))
            {
                de.Properties["userAccountControl"][0] = ADS_USER_FLAG_ENUM.ADS_UF_NORMAL_ACCOUNT | ADS_USER_FLAG_ENUM.ADS_UF_DONT_EXPIRE_PASSWD;
                de.CommitChanges();
                de.Close();
            }
        }

    }
}
