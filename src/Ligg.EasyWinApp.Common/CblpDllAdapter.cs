using System;
using System.IO;
using Ligg.EasyWinApp.Implementation;
using Ligg.EasyWinApp.ImplInterface;
using AssemblyHelper = Ligg.EasyWinApp.Common.Helpers.AssemblyHelper;

namespace Ligg.EasyWinApp.Common
{
    public static class CblpDllAdapter
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        public static IAdapter Adapter = null;
        public static void Init(bool debug, string implementationDllPath, string adapterFullClassName)
        {
            if (debug)
            {
                //rd-2
                Adapter = new Adapter();
                Adapter.Initialize();
            }
            else
            {
                //rd + 5
                if (File.Exists(implementationDllPath))
                {
                    Adapter = CreateAdapter(implementationDllPath, adapterFullClassName);
                    Adapter?.Initialize();
                }
            }

        }

        private static IAdapter CreateAdapter(string dllPath, string adapterClassFullName)
        {
            try
            {

                if (string.IsNullOrEmpty(dllPath)) return null;
                if (!File.Exists(dllPath))
                {
                    throw new ArgumentException("File: " + dllPath + " does not exists!");
                }
                string key = adapterClassFullName;//namespaceDotClassName;
                var objType = AssemblyHelper.GetCache(key) as IAdapter;
                if (objType == null)
                {
                    objType = (IAdapter)AssemblyHelper.CreateObject(dllPath, adapterClassFullName);
                    AssemblyHelper.SetCache(key, objType);
                }
                return objType;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".CreateAdapter Error: " + ex.Message);
            }
        }

    }

}
