using System.IO;
using System.Xml.Serialization;

namespace SV_Client.Classes
{
    public class XmlSerializer
    {
        /// <summary>
        /// a generic serialitation method to serialize the object and returns the xml File
        /// 
        /// </summary>
        /// <typeparam name="T">the type of the object that needs to be serialized</typeparam>
        /// <param name="serializeObject">the object that should be serialized</param>
        /// <returns>the xml string </returns>
        public static string Serialize<T>(T serializeObject)
        {


            var mySerlializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            TextWriter reader = new StringWriter();
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            mySerlializer.Serialize(reader, serializeObject,ns);

            return reader.ToString();
        }

        /// <summary>
        /// generic method to deserialize the string 
        /// </summary>
        /// <typeparam name="T">the type of the object that needs to be deserialized</typeparam>
        /// <param name="xmlString">the xml string, it needs to be object of the type T </param>
        /// <returns>the object of the XML string in the format of the type param</returns>
        public static T Deserialize<T>(string xmlString)
        {
            var mySerlializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            TextReader txtReader = new StringReader(xmlString);

            System.Console.WriteLine(typeof(T));
            var myDesirializedObject = mySerlializer.Deserialize(txtReader);

            txtReader.Close();

            return (T)myDesirializedObject;

        }

    }
}
