using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Ligg.Base.Extension;

namespace Ligg.Base.Helpers
{
    public static class DataTableHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        public static string ConvertToRichText(DataTable dt, bool hasHead, string[] fieldArray)
        {
            var headStr = "";
            var strBlder = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                if (hasHead)
                {
                    var tm = 0;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        var columnName = dt.Columns[j].ColumnName;
                        if (columnName.IsBeContainedInStringArray(fieldArray) | fieldArray == null)
                        {
                            if (tm == 0)
                            {
                                headStr =columnName;
                            }
                            else
                            {
                                headStr = headStr + "\t" + columnName;
                            }
                            tm++;
                        }
                    }
                    strBlder.AppendLine(headStr);
                }


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var txt = "";
                    var ct = 0;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        var columnName = dt.Columns[j].ColumnName;
                        if (columnName.IsBeContainedInStringArray(fieldArray) | fieldArray == null)
                        {
                            if (ct == 0)
                            {
                                txt = dt.Rows[i][j].ToString();
                            }
                            else
                            {
                                txt = txt + "\t" + dt.Rows[i][j];
                            }
                            ct++;
                        }
                    }
                    strBlder.AppendLine(txt);
                }
            }
            return strBlder.ToString();
        }

        public static string ConvertToJson(DataTable table)
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
                throw new ArgumentException("\n>> " + TypeName + ".ConvertToJson Error: " + ex.Message);
            }
        }





    }
}
