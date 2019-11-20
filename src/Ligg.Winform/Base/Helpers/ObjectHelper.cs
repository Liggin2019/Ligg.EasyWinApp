using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Ligg.Base.DataModel;
using Ligg.Base.DataModel.Enums;
using Ligg.Base.Helpers;
using Newtonsoft.Json;


namespace Ligg.Base.Helpers
{
    public static class ObjectHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;


        public static bool IsType(Type type, string typeName)
        {
            try
            {
                if (type.ToString() == typeName) return true;
                if (type.ToString() == "System.Object") return false;
                return IsType(type.BaseType, typeName);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".IsType Error: " + ex.Message);
            }
        }


    }
}
