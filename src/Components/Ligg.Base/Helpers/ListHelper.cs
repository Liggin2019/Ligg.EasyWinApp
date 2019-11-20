using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;


namespace Ligg.Base.Helpers
{
    public static class ListHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        //#get


        //#convert
        //##RichText

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

        public static DataTable ConvertToDataTableByPropertyNames(IList list, params string[] propertyNames)
        {
            try
            {
                var propertyNameList = new List<string>();
                if (propertyNames != null)
                    propertyNameList.AddRange(propertyNames);

                var result = new DataTable();
                if (list.Count > 0)
                {
                    PropertyInfo[] propertys = list[0].GetType().GetProperties();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            result.Columns.Add(pi.Name, pi.PropertyType);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                                result.Columns.Add(pi.Name, pi.PropertyType);
                        }
                    }

                    for (int i = 0; i < list.Count; i++)
                    {
                        var tempList = new ArrayList();
                        foreach (PropertyInfo pi in propertys)
                        {
                            if (propertyNameList.Count == 0)
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                            else
                            {
                                if (propertyNameList.Contains(pi.Name))
                                {
                                    object obj = pi.GetValue(list[i], null);
                                    tempList.Add(obj);
                                }
                            }
                        }
                        object[] array = tempList.ToArray();
                        result.LoadDataRow(array, true);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".ConvertToDataTableByPropertyNames Error: " + ex.Message);
            }
        }



    }
}
