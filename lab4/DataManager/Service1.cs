using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using LibraryForFiles;
using ServiceLayer;

namespace DataManager
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var config = new LibraryForFiles();
            List<Option> options = config.GetOptions();

            try
            {
                var personService = new PersonService(@"Data Source=ASUS;Initial Catalog=AdventureWorks2019;Integrated Security=True");
                var personsInfo = personService.personRepository.GetAll();

                XmlCreator persons = new XmlCreator(options[0].Target);
                persons.XmlGenerate(personsInfo);
            }
            catch (Exception excep)
            {
                throw excep;
            }
        }

        protected override void OnStop()
        {
        }
    }
}
