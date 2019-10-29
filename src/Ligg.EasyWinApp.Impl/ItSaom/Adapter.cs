using System;
using System.Data;
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
            throw new NotImplementedException();
            return string.Empty;
        }


        public string Act(string actionName, string[] actionParamArray)
        {
            return AdapterActHelper.Act(actionName, actionParamArray);
        }


        public string GetText(string funName, string[] paramArray)
        {
            return AdapterGetHelper.GetText(funName, paramArray);
        }


        public DataTable GetValueTextDataTable(string funName, string[] paramArray)
        {
            return AdapterGetHelper.GetValueTextDataTable(funName, paramArray);
        }


    }
}
