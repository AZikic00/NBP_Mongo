using System.Collections.Generic;

namespace baze_mongo.Models
{
    public class Statistika
    {
        public double ID { get; }
        public Igrac Igrac { get; }

        public string Liga { get; }

        public double Poeni { get; }
        public double Skokovi { get; }
        public double Asistencije { get; }


        public Statistika(double id,Igrac igrac, string liga, double poeni, double skokovi, double asistencije) 
        {
            ID = id;
            Igrac = igrac;
            Liga = liga;
            Poeni = poeni;
            Skokovi = skokovi;
            Asistencije = asistencije;
        }

        
    }
}
