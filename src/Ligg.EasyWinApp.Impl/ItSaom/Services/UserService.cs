using System;
using Ligg.Base.DataModel;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.EasyWinApp.ImplInterface;

namespace Ligg.EasyWinApp.Implementation.Services
{
    internal class UserService
    {
        internal UniversalResult Logon(string acct, string password)
        {
            try
            {
                GlobalConfiguration.UserCode = acct;
                GlobalConfiguration.UserToken = " mOtXb01/2Mp/eAJFSdreDDSklgvf=";
                return new UniversalResult(true, "");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".Logon Error: " + ex.Message);
            }
        }




    }
}