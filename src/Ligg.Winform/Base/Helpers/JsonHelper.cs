using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Newtonsoft.Json;


namespace Ligg.Base.Helpers
{
    public static class JsonHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        public static T ConvertToObject<T>(string input)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(input);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".ConvertToObject Error: " + ex.Message);
            }
        }

        public static DataTable ConvertToDataTable(string input)
        {
            DataTable dataTable = new DataTable();
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue;
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(input);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }

                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }

                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }

                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".ConvertToDataTable Error: " + ex.Message);
            }
            result = dataTable;
            return result;
        }

       




    }
}
