using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;


namespace Ligg.Base.Helpers
{
    public static class ListHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        //#get
        public static long GetLargestId<T>(List<T> list)
        {
            try
            {
                long result = 0;
                if (list.Count > 0)
                {
                    Type tp = typeof(T);
                    var idField = tp.GetField("Id", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    for (int i = 0; i < list.Count; i++)
                    {
                        var id = Convert.ToInt64(idField.GetValue(list[i]));
                        if (id > result) result = id;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".GetLargestId Error: " + ex.Message);
            }
        }


        //#convert
        //##RichText
        public static string ConvertToRichText<T>(List<T> list, bool ignoreFields)
        {
            try
            {
                var strBlder = new StringBuilder();
                if (list.Count > 0)
                {
                    Type tp = typeof(T);
                    var fields = tp.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (!ignoreFields)
                    {
                        var txt = "";
                        var ct = 0;
                        foreach (var field in fields)
                        {
                            txt = ct == 0 ? field.Name : txt + "\t" + field.Name;
                            ct++;
                        }
                        strBlder.AppendLine(txt);
                    }


                    for (int i = 0; i < list.Count; i++)
                    {
                        var ct = 0;
                        var txt1 = "";
                        foreach (var field in fields)
                        {
                            object obj = field.GetValue(list[i]);
                            txt1 = ct == 0 ? "" + obj : txt1 + "\t" + obj;
                            ct++;
                        }
                        strBlder.AppendLine(txt1);
                    }
                }

                return strBlder.ToString();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".ConvertToRichText Error: " + ex.Message);
            }
        }



    }
}
