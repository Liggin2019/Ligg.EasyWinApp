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
        public static List<T> EnumToList<T>()
        {

            Type enumType = typeof(T);

            // Can't use type constraints on Value types, so have to do check like this

            if (enumType.BaseType != typeof(Enum))

                throw new ArgumentException("EnumHelper.T must be of type System.Enum");

            return new List<T>(Enum.GetValues(enumType) as IEnumerable<T>);

        }

        public static List<IdName> EnumToIdNames<T>()
        {

            Type enumType = typeof(T);
            // Can't use type constraints on Value types, so have to do check like this

            if (enumType.BaseType != typeof(Enum))

                throw new ArgumentException("EnumHelper.T must be of type System.Enum");
            var idNames = new List<IdName>();
            foreach (var v in Enum.GetValues(enumType))
            {
                var idName = new IdName();
                idName.Id = (int)v;
                idName.Name = v.ToString();
                idNames.Add(idName);
            }
            return idNames;
        }

        public static List<ValueText> EnumToValueTexts<T>()
        {

            Type enumType = typeof(T);
            // Can't use type constraints on Value types, so have to do check like this

            if (enumType.BaseType != typeof(Enum))

                throw new ArgumentException("EnumHelper.T must be of type System.Enum");
            var valueTexts = new List<ValueText>();
            foreach (var v in Enum.GetValues(enumType))
            {
                var valueText = new ValueText();
                valueText.Value = Convert.ToString((int)v);
                valueText.Text = v.ToString();
                valueTexts.Add(valueText);
            }
            return valueTexts;
        }

        public static DataTable EnumToDataTable<T>()
        {

            Type enumType = typeof(T);
            if (enumType.BaseType != typeof(Enum))

                throw new ArgumentException("EnumHelper.T must be of type System.Enum");
            DataTable dt = new DataTable();
            dt.Columns.Add("Value");
            dt.Columns.Add("Text");
            foreach (var v in Enum.GetValues(enumType))
            {
                dt.Rows.Add(Convert.ToString((int)v), v.ToString());
            }
            return dt;
        }

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