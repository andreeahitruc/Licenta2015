using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FillDatabase;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new FillDatabase.Read();
            x.LoadJson();
        }
    }
}
