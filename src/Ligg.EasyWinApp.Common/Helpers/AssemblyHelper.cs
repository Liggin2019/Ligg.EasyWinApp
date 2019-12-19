using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ligg.EasyWinApp.Common.Helpers
{
    public class AssemblyHelper
    {
        private static readonly string TypeName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName;

        public static object CreateObject(string assemblyPath, string namespaceDotClassName)
        {
            object objType = null;
            try
            {
                //objType = Assembly.Load(assName).CreateInstance(namespaceDotClassName);//only valid for that in same folder as main exe
                objType = Assembly.LoadFrom(assemblyPath).CreateInstance(namespaceDotClassName);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> " + TypeName + ".CreateObject Error: " + ex.Message);
            }
            return objType;
        }
        private static ObjectDictionary<string, object> _objectList = new ObjectDictionary<string, object>();
        internal static ObjectDictionary<string, object> ObjectList
        {
            get { return _objectList; }
            set { _objectList = value; }
        }

        public static void SetCache(string key, object value)
        {
            lock (ObjectList.LockObj)
            {
                ObjectList[key] = value;
            }
        }

        public static object GetCache(string key)
        {
            lock (ObjectList.LockObj)
            {
                return ObjectList.ContainsKey(key) ? ObjectList[key] : null;
            }
        }
    }

    internal class ObjectDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        internal object LockObj = new object();
    }
}
