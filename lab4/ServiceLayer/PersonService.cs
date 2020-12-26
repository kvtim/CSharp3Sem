using DAL.Repositories;

namespace ServiceLayer
{
    public class PersonService
    {
        public PersonRepository personRepository { get; set; }
        public PersonService(string connectionString)
        {
            personRepository = new PersonRepository(connectionString);
        }
    }
}
