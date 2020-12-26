using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ServiceLayer
{
    interface ICreateXml
    {
        void XmlGenerate<T>(IEnumerable<T> info);
    }

    public class XmlCreator : ICreateXml
    {
        private readonly string path;

        public XmlCreator(string path)
        {
            this.path = path;
        }

        public void XmlGenerate<T>(IEnumerable<T> info)
        {
            try
            {
                List<T> emp = new List<T>(info);

                XmlSerializer formatter = new XmlSerializer(typeof(List<T>));

                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, emp);
                }
            }
            catch (Exception trouble)
            {
                throw trouble;
            }
        }
    }
}
