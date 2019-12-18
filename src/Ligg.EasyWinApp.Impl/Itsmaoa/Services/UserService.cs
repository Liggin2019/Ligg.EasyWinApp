using System;
using Ligg.Base.DataModel;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.EasyWinApp.Implementation.DataModel;
using Ligg.EasyWinApp.ImplInterface;

namespace Ligg.EasyWinApp.Implementation.Services
{
    internal class UserService
    {
        internal UniversalResult Logon(string userCode, string password)
        {
            try
            {
                var logonResult = new LogonResult
                { UserId = 1, UserToken = "mOtXb01/2Mp/eAJFSdreDDSklgvf=", UniversalResult = new UniversalResult { IsOk = true, Message = "" } };
                if (logonResult.UniversalResult.IsOk)
                {
                    GlobalConfiguration.UserId = logonResult.UserId;
                    GlobalConfiguration.UserToken = logonResult.UserToken;
                    GlobalConfiguration.UserCode = userCode;
                }
                return logonResult.UniversalResult;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + GetType().FullName + ".Logon Error: " + ex.Message);
            }
        }




    }
}