using System.Collections.Generic;

namespace baze_mongo.Models
{
    public class Klub
    {
        public double ID { get;}
        public string Naziv { get; }

        public string Drzava { get; }

        public string[] Liga { get; }

        public string Godina_osnivanja { get; }


        public Klub(double id, string naziv, string drzava, string[] liga, string godina_osnivanja) 
        {
            ID = id;
            Naziv = naziv;
            Drzava = drzava;
            Liga = liga;
            Godina_osnivanja = godina_osnivanja;
        }
    }
}
