using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicProxy.Demo
{
    public class User : MarshalByRefObject
    {
        public int Age { get; set; }

        public string Name { get; set; }

        public bool Add(string name,int age ,out int count)
        {
            Name =name;

            Age = age;

            count = 1;

            Console.WriteLine("Add " + Name+" ...");

            return true;
        }


        public string SayName()
        {

            Console.WriteLine("My name is "+Name);

            return Name;
        }

        


    }
}
