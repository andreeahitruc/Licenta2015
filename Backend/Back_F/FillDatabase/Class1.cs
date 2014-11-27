using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Back_F;
using Newtonsoft.Json;


namespace FillDatabase
{
    public class Read
    {
        flickr_tablesEntities db = new flickr_tablesEntities();
        public void LoadJson()
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader("cat.txt");
            while ((line = file.ReadLine()) != null)
            {
                category cat = new category() { 
                    Category1 = line.ToString()
                };
                db.categories.Add(cat);
            }
            db.SaveChanges();
        }
    }
}
