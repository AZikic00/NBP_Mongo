using System.Collections.Generic;

namespace baze_mongo.Models
{
    public class Igrac
    {
        public double ID { get; }
        public string Ime { get; }

        public string Drzava { get; }

        public Klub Klub { get; }

        public double Plata { get; }


        public Igrac(double id,string ime, string drzava,Klub klub, double plata) 
        {
            ID = id;
            Ime = ime;
            Drzava = drzava;
            Klub = klub;
            Plata = plata;
        }
    }
}
