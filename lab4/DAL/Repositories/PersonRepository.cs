using DAL.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.Repositories
{
    public class PersonRepository : IRepository<Person>
    {
        private readonly string connectionString;

        public PersonRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IEnumerable<Person> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var persons = new List<Person>();

                try
                {
                    var command = new SqlCommand("GetPersonsInfo", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var person = new Person();
                            person.Id = reader.GetInt32(0);
                            person.FIO = reader.GetString(1);
                            person.PhoneNumber = reader.GetString(2);
                            person.Job = reader.GetString(3);
                            person.BirthDay = reader.GetDateTime(4);

                            persons.Add(person);
                        }
                    }
                    reader.Close();

                    return persons;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
