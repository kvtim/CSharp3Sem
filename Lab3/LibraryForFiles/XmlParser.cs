using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LibraryForFiles
{
    class XmlParser : IParser
    {
        public XmlParser()
        { }

        public T Parse<T>(string path) where T : new()
        {
            T property = new T();
            var serializer = new XmlSerializer(typeof(T));

            using (var stream = new FileStream(path, FileMode.Open))
            {
                var xmlReader = XmlReader.Create(stream);

                xmlReader.ReadToDescendant(typeof(T).Name);

                property = (T)serializer.Deserialize(xmlReader);
            }

            return property;
        }
    }
}