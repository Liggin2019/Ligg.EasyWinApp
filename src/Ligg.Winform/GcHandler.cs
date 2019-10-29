using System;
using System.Data;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Extension;
using Ligg.Base.Helpers;
using Ligg.EasyWinApp.ImplInterface;

namespace Ligg.Winform
{
    public class GcHandler
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        public void Init()
        {
            GlobalConfiguration.init();
        }
        //#get
        public string GetGlobalKey1()
        {
            return GlobalConfiguration.GlobalKey1;
        }

        public string GetGlobalKey2()
        {
            return GlobalConfiguration.GlobalKey2;
        }


    }
}
