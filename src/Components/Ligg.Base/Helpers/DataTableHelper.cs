using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using Ligg.Base.Extension;

namespace Ligg.Base.Helpers
{
    /// <summary>
    /// Static class with path related methods. Can be useful to combine url or to retrive the server root directory.
    /// </summary>
    public static class DataTableHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;
        public static IList<T> ConvertToList<T>(DataTable dt)
        {
            try
            {
                var result = new List<T>();
                //T t = (T)Activator.CreateInstance(typeof(T));
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    var t = System.Activator.CreateInstance(typeof(T));
                    PropertyInfo[] propertys = t.GetType().GetProperties();
                    foreach (PropertyInfo pi in propertys)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            // 属性与字段名称一致的进行赋值
                            if (pi.Name.Equals(dt.Columns[i].ColumnName))
                            {
                                // 数据库NULL值单独处理
                                if (dt.Rows[j][i] != DBNull.Value)
                                {
                                    var objVal = dt.Rows[j][i].ToString().ConvertToAnyType(pi.PropertyType, '`', '~');
                                    pi.SetValue(t, objVal, null);
                                }
                                else
                                    pi.SetValue(t, null, null);
                                break;
                            }
                        }
                    }
                    result.Add((T)t);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".ConvertToList Error: " + ex.Message);
            }
        }

    }
}
