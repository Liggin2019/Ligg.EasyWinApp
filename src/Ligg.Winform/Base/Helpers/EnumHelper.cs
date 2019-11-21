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

        public static int GetIdByName<T>(string name)
        {
            if (name.IsNullOrEmpty()) return 0;
            Type enumType = typeof(T);
            int returnVal = 0;
            var isOk = false;
            foreach (var v in Enum.GetValues(enumType))
            {
                var id = (int)v;
                var name1 = v.ToString();
                if (name1 == name)
                {
                    returnVal = id;
                    isOk = true;
                    break;
                }
            }
            if(!isOk) throw new ArgumentException("Enum Name: "+name+" dismatches enum type "+ enumType+ "'s all values!");
            return returnVal;
        }



    }
}