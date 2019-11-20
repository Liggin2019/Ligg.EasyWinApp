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

        public static string DataTableToJson(DataTable table)
        {
            try
            {
                var JsonString = new StringBuilder();
                if (table.Rows.Count > 0)
                {
                    JsonString.Append("[");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        JsonString.Append("{");
                        for (int j = 0; j < table.Columns.Count; j++)
                        {
                            if (j < table.Columns.Count - 1)
                            {
                                JsonString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                            }
                            else if (j == table.Columns.Count - 1)
                            {
                                JsonString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                            }
                        }
                        if (i == table.Rows.Count - 1)
                        {
                            JsonString.Append("}");
                        }
                        else
                        {
                            JsonString.Append("},");
                        }
                    }
                    JsonString.Append("]");
                }
                return JsonString.ToString();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".DataTableToJson Error: " + ex.Message);
            }
        }





    }
}
