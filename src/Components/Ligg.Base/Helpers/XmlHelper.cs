using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Ligg.Base.Helpers
{

    public static class XmlHelper
    {

        public static T ConvertToObject<T>(string xmlStr)
        {
            try
            {
                return ConvertToObject<T>(xmlStr, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> XmlHelper.ConvertToObject Error: " + ex.Message);
            }
        }

        public static T ConvertToObject<T>(string xmlStr, Encoding encoding)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var stream = new MemoryStream(encoding.GetBytes(xmlStr)))
                {
                    return (T)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> XmlHelper.ConvertToObject Error: " + ex.Message);
            }
        }



        public static string GetFilePath(string filePath)
        {
            try
            {
                if (!(filePath.ToLower().EndsWith(".xml") | filePath.ToLower().EndsWith(".exml")))
                {
                    if (System.IO.File.Exists(filePath + ".xml"))
                    {
                        filePath = filePath + ".xml";
                    }
                    else if (System.IO.File.Exists(filePath + ".exml"))
                    {
                        filePath = filePath + ".exml";
                    }
                }

                return filePath;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("\n>> XmlHelper.GetFilePath Error: " + ex.Message);
            }
        }



    }
}
