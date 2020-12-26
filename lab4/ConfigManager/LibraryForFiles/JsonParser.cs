using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace LibraryForFiles
{
    class JsonParser : IParser
    {
        public JsonParser()
        { }

        public T Parse<T>(string path) where T : new()
        {
            T property = new T();

            using (var reader = new StreamReader(path))
            {
                string values = reader.ReadToEnd();

                values = GetString(values, typeof(T).Name);

                property = JsonSerializer.Deserialize<T>(values);
            }

            return property;
        }

        private string GetString(string info, string nameObject)
        {
            StringBuilder jsonString = new StringBuilder(info);

            jsonString.Remove(0, info.IndexOf(nameObject) + nameObject.Length + 3);

            char[] symbols = jsonString.ToString().ToCharArray();

            int brackets = 0;
            int count = 0;

            do
            {
                if (symbols[count] == '{')
                    brackets++;
                if (symbols[count] == '}')
                    brackets--;

                count++;
            } while (brackets != 0);

            info = jsonString.ToString().Substring(0, count);

            return info;
        }
    }
}
