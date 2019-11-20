using System;
using Newtonsoft.Json;


namespace Ligg.Base.Helpers
{
    public static class ObjectHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        //#convert




        //##json
        public static string ConvertToJson (this object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".ConvertToJson Error: " + ex.Message);
            }
        }



        //#judge
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
