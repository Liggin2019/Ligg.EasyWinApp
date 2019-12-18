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


    }
}
