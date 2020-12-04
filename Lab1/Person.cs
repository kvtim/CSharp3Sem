using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1
{
    class Person
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Height { get; set; }
   
        public Person(int age, string name, string surname, int height)
        {
            this.Age = age;
            this.Name = name;
            this.Surname = surname;
            this.Height = height;
        }
    }
}
