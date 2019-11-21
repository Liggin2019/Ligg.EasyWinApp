using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ligg.Base.DataModel;
using Ligg.Base.Extension;


namespace Ligg.Base.Helpers
{
    public static class EnumHelper
    {

        public static string GetNameById<T>(int id)
        {
            Type enumType = typeof(T);
            string name = "";
            name = Enum.GetName(enumType, id);
            return name;
        }


    }
}