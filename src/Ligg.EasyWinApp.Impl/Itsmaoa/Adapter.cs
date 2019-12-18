using System;
using Ligg.EasyWinApp.Implementation.Services;
using Ligg.EasyWinApp.ImplInterface;

namespace Ligg.EasyWinApp.Implementation
{
    public class Adapter : IAdapter
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        public void Initialize()
        {
            try
            {
                Configuration.Init();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".Initialize Error: " + ex.Message);
            }
        }

        public string ResolveConstants(string text)
        {
            //throw new NotImplementedException();
            return AdapterResolveHelper.ResolveConstants(text);
        }

        public string GetText(string funName, string[] paramArray)
        {
            return AdapterGetHelper.GetText(funName, paramArray);
        }

        public string Act(string actionName, string[] actionParamArray)
        {
            return AdapterActHelper.Act(actionName, actionParamArray);
        }

        public bool Logon(string userCode, string userPassword)
        {
            return new UserService().Logon(userCode, userPassword).IsOk;
        }


    }
}
