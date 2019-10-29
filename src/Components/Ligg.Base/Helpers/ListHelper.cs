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

        //##datatable
        public static DataTable ConvertToDataTable(IList list)
        {
            try
            {
                var result = new DataTable();
                if (list.Count > 0)
                {
                    PropertyInfo[] propertys = list[0].GetType().GetProperties();
                    foreach (PropertyInfo pi in propertys)
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }

                    for (int i = 0; i < list.Count; i++)
                    {
                        ArrayList tempArrayList = new ArrayList();
                        foreach (PropertyInfo pi in propertys)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempArrayList.Add(obj);
                        }
                        object[] array = tempArrayList.ToArray();
                        result.LoadDataRow(array, true);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".ConvertToDataTable Error: " + ex.Message);
            }
        }



    }
}
