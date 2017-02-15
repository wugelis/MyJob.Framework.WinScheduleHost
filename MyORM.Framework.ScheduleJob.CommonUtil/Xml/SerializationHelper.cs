using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MyORM.Framework.ScheduleJob.CommonUtil.Xml
{
    /// <summary>
    /// 執行 XML 序列化 & 反序列化.
    /// </summary>
    public class SerializationHelper
    {
        /// <summary>
        /// 執行XML反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="XmlResultStr"></param>
        /// <returns></returns>
        public static T DeSerializeAnObject<T>(string XmlResultStr) where T : class
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(XmlResultStr));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return xs.Deserialize(memoryStream) as T;
        }
        /// <summary>
        /// 執行XML序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeAnObject(object obj)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            try
            {
                serializer.Serialize(stream, obj);
                stream.Position = 0;
                TextReader tr = new StreamReader(stream);
                return tr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }
        }
    }
}
