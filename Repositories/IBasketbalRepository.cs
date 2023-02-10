using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using baze_mongo.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System.Xml.Linq;


namespace baze_mongo.Repositories
{
    public interface IBasketbalRepository
    {
        List<Klub> sviKlubovi();
        List<string> sveDrzave();
        List<string> sveLige();
        List<Igrac> sviIgraci();
        List<Igrac> IgraciKlub(string klub);
        List<Igrac> IgraciLiga(string liga);
        List<Igrac> igraciDrzava(string drzava);
        //Statistika Statistika(string igrac, string liga);
        List<Statistika> StatistikaLiga(string liga);
        List<Statistika> StatistikaKlub(string liga,string klub);
        List<Statistika> StatistikaDrzava(string liga,string drzava);
        List<Statistika> Statistika(string liga, string klub,string drzava);
        List<Klub> kluboviLiga(string liga);
        void dodajIgraca(string Ime,string  Drzava,string Klub,double Plata);
        void dodajStatistiku(string Igrac, string Liga, double Poeni, double Skokovi, double Asistencije);
        void izmeniPoene(string Igrac, string Liga, double Poeni);
        void izmeniSkokove(string Igrac, string Liga, double Skokovi);
        void izmeniAsistencije(string Igrac, string Liga, double Asistencije);
        void obrisiIgraca(string Igrac);

        void sortiraj(List<Statistika> statistika);
    }
    public class BasketballRepositories : IBasketbalRepository
    {
        public List<Klub> sviKlubovi()
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("Basketball");
            var collection = db.GetCollection<BsonDocument>("Klubovi");
            List<Klub> Klubovi = new List<Klub>();
            var documents = collection.Find(new BsonDocument()).ToList();
            
            foreach (BsonDocument doc in documents)
            {
                BsonValue b = doc[3];
                string[] list = b.ToString().Split(',');
                list[0] = list[0].Remove(0, 1);
                list[list.Length-1] = list[list.Length - 1].Remove(list[list.Length-1].Length-1, 1);

                Klub k = new Klub((double)doc[0], (string)doc[1], (string)doc[2], list, (string)doc[4]);
                Klubovi.Add(k);

            }

            return Klubovi;
        }
        public List<string> sveLige()
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("Basketball");

            var collection = db.GetCollection<BsonDocument>("Klubovi");
            List<string> Lige = new List<string>();
            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (BsonDocument doc in documents)
            {
                BsonValue b = doc[3];
                string[] list = b.ToString().Split(',');
                list[0] = list[0].Remove(0, 1);
                list[list.Length - 1] = list[list.Length - 1].Remove(list[list.Length - 1].Length - 1, 1);
                foreach(string liga in list)
                {
                    if (Lige.IndexOf(liga) == -1)
                    {
                        Lige.Add(liga);
                    }
                }
            }

            return Lige;
        }
        public List<string> sveDrzave()
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("Basketball");

            var collection = db.GetCollection<BsonDocument>("Igraci");
            List<string> Drzave = new List<string>();
            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (BsonDocument doc in documents)
            {
                if (Drzave.IndexOf((string)doc[2]) == -1)
                {
                    Drzave.Add((string)doc[2]);
                }
            }

            return Drzave;
        }
        public List<Igrac> sviIgraci()
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("Basketball");

            var collection = db.GetCollection<BsonDocument>("Igraci");
            List<Igrac> Igraci = new List<Igrac>();
            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (BsonDocument doc in documents)
            {
                collection = db.GetCollection<BsonDocument>("Klubovi");
                var filter = Builders<BsonDocument>.Filter.Eq("_id", (double)doc[3]);
                var k = collection.Find(filter).FirstOrDefault();
                string[] list = k.ToString().Split(',');
                list[0] = list[0].Remove(0, 1);
                list[list.Length - 1] = list[list.Length - 1].Remove(list[list.Length - 1].Length - 1, 1);
                Klub klub = new Klub((double)k[0], (string)k[1], (string)k[2], list, (string)k[4]);
                Igrac Igrac = new Igrac((double)doc[0], (string)doc[1], (string)doc[2], klub, (double)doc[4]);
                Igraci.Add(Igrac);

            }

            return Igraci;
        }
        public List<Igrac> IgraciKlub(string klub)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("Basketball");

            var collection = db.GetCollection<BsonDocument>("Klubovi");
            double idKluba;
            var filter = Builders<BsonDocument>.Filter.Eq("Naziv",klub);
            var Klub = collection.Find(filter).FirstOrDefault();
            BsonValue b = Klub[3];
            string[] list = b.ToString().Split(',');
            list[0] = list[0].Remove(0, 1);
            list[list.Length - 1] = list[list.Length - 1].Remove(list[list.Length - 1].Length - 1, 1);
            Klub k = new Klub((double)Klub[0], (string)Klub[1], (string)Klub[2], list, (string)Klub[4]);
            idKluba = k.ID;

            List<Igrac> Igraci = new List<Igrac>();
            collection = db.GetCollection<BsonDocument>("Igraci");
            filter = Builders<BsonDocument>.Filter.Eq("Klub", idKluba);
            var IgraciLista = collection.Find(filter).ToList();
            foreach (BsonDocument doc in IgraciLista)
            {
                Igrac i = new Igrac((double)doc[0], (string)doc[1], (string)doc[2], k, (double)doc[4]);
                Igraci.Add(i);
            }
            return Igraci;
        }
        public List<Igrac> IgraciLiga(string liga)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("Basketball");

            var collection = db.GetCollection<BsonDocument>("Klubovi");
            var filter = Builders<BsonDocument>.Filter.AnyEq("Liga", liga);
            var KluboviLista = collection.Find(filter).ToList();
            List<Klub> Klubovi = new List<Klub>();
            foreach (BsonDocument doc in KluboviLista)
            {
                BsonValue b = doc[3];
                string[] list = b.ToString().Split(',');
                list[0] = list[0].Remove(0, 1);
                list[list.Length - 1] = list[list.Length - 1].Remove(list[list.Length - 1].Length - 1, 1);
                Klub k = new Klub((double)doc[0], (string)doc[1], (string)doc[2], list, (string)doc[4]);
                Klubovi.Add(k);
            }

            List<Igrac> Igraci = new List<Igrac>();
            foreach (Klub k in Klubovi)
            {
                List<Igrac> pom = IgraciKlub(k.Naziv);
                foreach (Igrac i in pom)
                {
                    Igraci.Add(i);
                }
            }
            return Igraci;
        }
        public List<Igrac> igraciDrzava(string drzava)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("Basketball");

            List<Igrac> Igraci = new List<Igrac>();
            var collection = db.GetCollection<BsonDocument>("Igraci");
            var filter = Builders<BsonDocument>.Filter.Eq("Drzava", drzava);
            var IgraciLista = collection.Find(filter).ToList();
            foreach (BsonDocument doc in IgraciLista)
            {
                collection = db.GetCollection<BsonDocument>("Klubovi");
                filter = Builders<BsonDocument>.Filter.Eq("_id", (double)doc[3]);
                var k = collection.Find(filter).FirstOrDefault();
                string[] list = k.ToString().Split(',');
                list[0] = list[0].Remove(0, 1);
                list[list.Length - 1] = list[list.Length - 1].Remove(list[list.Length - 1].Length - 1, 1);
                Klub klub = new Klub((double)k[0], (string)k[1], (string)k[2], list, (string)k[4]);
                Igrac i = new Igrac((double)doc[0], (string)doc[1], (string)doc[2], klub, (double)doc[4]);
                Igraci.Add(i);
            }
            return Igraci;
        }
       /*public Statistika Statistika(string igrac, string liga)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("Basketball");

            var collection = db.GetCollection<BsonDocument>("Igraci");
            var filter = Builders<BsonDocument>.Filter.Eq("Ime", igrac);
            var i = collection.Find(filter).FirstOrDefault();
            collection = db.GetCollection<BsonDocument>("Klubovi");
            filter = Builders<BsonDocument>.Filter.Eq("_id", i[3]);
            var k = collection.Find(filter).FirstOrDefault();
            string[] list = k.ToString().Split(',');
            list[0] = list[0].Remove(0, 1);
            list[list.Length - 1] = list[list.Length - 1].Remove(list[list.Length - 1].Length - 1, 1);
            Klub klub = new Klub((double)k[0], (string)k[1], (string)k[2], list, (string)k[4]);
            Igrac Igrac = new Igrac((double)i[0], (string)i[1], (string)i[2], klub, (double)i[4]);

            collection = db.GetCollection<BsonDocument>("Statistika");
            filter = Builders<BsonDocument>.Filter.Eq("Igrac", Igrac.ID);
            filter &= Builders<BsonDocument>.Filter.Eq("Liga", liga);
            var s = collection.Find(filter).FirstOrDefault();
            Statistika stats = new Statistika((double)s[0], Igrac, (string)s[2], (double)s[3], (double)s[4], (double)s[5]);
            return stats;
        }*/
        public List<Statistika> StatistikaLiga(string liga)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("Basketball");

            var collection = db.GetCollection<BsonDocument>("Statistika");
            var filter = Builders<BsonDocument>.Filter.Eq("Liga", liga);
            var s = collection.Find(filter).ToList();
            List<Statistika> stats = new List<Statistika>();
            foreach (BsonDocument b in s)
            {
                collection = db.GetCollection<BsonDocument>("Igraci");
                filter = Builders<BsonDocument>.Filter.Eq("_id", b[1]);
                var i = collection.Find(filter).FirstOrDefault();
                collection = db.GetCollection<BsonDocument>("Klubovi");
                filter = Builders<BsonDocument>.Filter.Eq("_id", (double)i[3]);
                var k = collection.Find(filter).FirstOrDefault();
                string[] list = k.ToString().Split(',');
                list[0] = list[0].Remove(0, 1);
                list[list.Length - 1] = list[list.Length - 1].Remove(list[list.Length - 1].Length - 1, 1);
                Klub klub = new Klub((double)k[0], (string)k[1], (string)k[2], list, (string)k[4]);
                Igrac Igrac = new Igrac((double)i[0], (string)i[1], (string)i[2], klub, (double)i[4]);
                Statistika stat = new Statistika((double)b[0], Igrac, (string)b[2], (double)b[3], (double)b[4], (double)b[5]);
                stats.Add(stat);
            }

            sortiraj(stats);
            return stats;
        }
        public List<Statistika> StatistikaKlub(string liga, string klub)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("Basketball");

            var collection = db.GetCollection<BsonDocument>("Statistika");
            var filter = Builders<BsonDocument>.Filter.Eq("Liga", liga);
            var s = collection.Find(filter).ToList();
            List<Statistika> stats = new List<Statistika>();
            foreach (BsonDocument b in s)
            {
                collection = db.GetCollection<BsonDocument>("Igraci");
                filter = Builders<BsonDocument>.Filter.Eq("_id", b[1]);
                var i = collection.Find(filter).FirstOrDefault();
                collection = db.GetCollection<BsonDocument>("Klubovi");
                filter = Builders<BsonDocument>.Filter.Eq("_id", (double)i[3]);
                filter &= Builders<BsonDocument>.Filter.Eq("Naziv", klub);
                var k = collection.Find(filter).FirstOrDefault();
                if (k==null)
                {
                    continue;
                }
                string[] list = k.ToString().Split(',');
                list[0] = list[0].Remove(0, 1);
                list[list.Length - 1] = list[list.Length - 1].Remove(list[list.Length - 1].Length - 1, 1);
                Klub klub1 = new Klub((double)k[0], (string)k[1], (string)k[2], list, (string)k[4]);
                Igrac Igrac = new Igrac((double)i[0], (string)i[1], (string)i[2], klub1, (double)i[4]);
                Statistika stat = new Statistika((double)b[0], Igrac, (string)b[2], (double)b[3], (double)b[4], (double)b[5]);
                stats.Add(stat);
            }
            sortiraj(stats);
            return stats;
        }
        public List<Statistika> StatistikaDrzava(string liga, string drzava)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("Basketball");

            var collection = db.GetCollection<BsonDocument>("Statistika");
            var filter = Builders<BsonDocument>.Filter.Eq("Liga", liga);
            var s = collection.Find(filter).ToList();
            List<Statistika> stats = new List<Statistika>();
            foreach (BsonDocument b in s)
            {
                collection = db.GetCollection<BsonDocument>("Igraci");
                filter = Builders<BsonDocument>.Filter.Eq("_id", b[1]);
                filter &= Builders<BsonDocument>.Filter.Eq("Drzava", drzava);
                var i = collection.Find(filter).FirstOrDefault();
                if (i==null)
                {
                    continue;
                }
                collection = db.GetCollection<BsonDocument>("Klubovi");
                filter = Builders<BsonDocument>.Filter.Eq("_id", (double)i[3]);
                var k = collection.Find(filter).FirstOrDefault();
                string[] list = k.ToString().Split(',');
                list[0] = list[0].Remove(0, 1);
                list[list.Length - 1] = list[list.Length - 1].Remove(list[list.Length - 1].Length - 1, 1);
                Klub klub1 = new Klub((double)k[0], (string)k[1], (string)k[2], list, (string)k[4]);
                Igrac Igrac = new Igrac((double)i[0], (string)i[1], (string)i[2], klub1, (double)i[4]);
                Statistika stat = new Statistika((double)b[0], Igrac, (string)b[2], (double)b[3], (double)b[4], (double)b[5]);
                stats.Add(stat);
            }
            sortiraj(stats);
            return stats;
        }
        public List<Statistika> Statistika(string liga, string klub, string drzava)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("Basketball");

            var collection = db.GetCollection<BsonDocument>("Statistika");
            var filter = Builders<BsonDocument>.Filter.Eq("Liga", liga);
            var s = collection.Find(filter).ToList();
            List<Statistika> stats = new List<Statistika>();
            foreach (BsonDocument b in s)
            {
                collection = db.GetCollection<BsonDocument>("Igraci");
                filter = Builders<BsonDocument>.Filter.Eq("_id", b[1]);
                filter &= Builders<BsonDocument>.Filter.Eq("Drzava", drzava);
                var i = collection.Find(filter).FirstOrDefault();
                if (i == null)
                {
                    continue;
                }
                collection = db.GetCollection<BsonDocument>("Klubovi");
                filter = Builders<BsonDocument>.Filter.Eq("_id", (double)i[3]);
                filter &= Builders<BsonDocument>.Filter.Eq("Naziv", klub);
                var k = collection.Find(filter).FirstOrDefault();
                if (k == null)
                {
                    continue;
                }
                string[] list = k.ToString().Split(',');
                list[0] = list[0].Remove(0, 1);
                list[list.Length - 1] = list[list.Length - 1].Remove(list[list.Length - 1].Length - 1, 1);
                Klub klub1 = new Klub((double)k[0], (string)k[1], (string)k[2], list, (string)k[4]);
                Igrac Igrac = new Igrac((double)i[0], (string)i[1], (string)i[2], klub1, (double)i[4]);
                Statistika stat = new Statistika((double)b[0], Igrac, (string)b[2], (double)b[3], (double)b[4], (double)b[5]);
                stats.Add(stat);
            }
            sortiraj(stats);
            return stats;
        }
        public List<Klub> kluboviLiga(string liga)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("Basketball");

            var collection = db.GetCollection<BsonDocument>("Klubovi");
            var filter = Builders<BsonDocument>.Filter.Eq("Liga", liga);
            var s = collection.Find(filter).ToList();
            List<Klub> klubovi = new List<Klub>();
            foreach (BsonDocument b in s)
            {
                string[] list = b[3].ToString().Split(',');
                list[0] = list[0].Remove(0, 1);
                list[list.Length - 1] = list[list.Length - 1].Remove(list[list.Length - 1].Length - 1, 1);
                Klub klub = new Klub((double)b[0], (string)b[1], (string)b[2], list, (string)b[4]);
                klubovi.Add(klub);
            }

            return klubovi;
        }
        public void dodajIgraca(string Ime, string Drzava, string Klub, double Plata)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("Basketball");
            var collection = db.GetCollection<BsonDocument>("Klubovi");
            var filter = Builders<BsonDocument>.Filter.Eq("Naziv", Klub);
            var k = collection.Find(filter).FirstOrDefault();
            collection = db.GetCollection<BsonDocument>("Igraci");
            var listaIgraca = collection.Find(new BsonDocument()).ToList();
            List<double> listaID = new List<double>();
            foreach (BsonDocument doc in listaIgraca)
            {
                listaID.Add((double)doc[0]);
            }
            double x = 1;
            while(listaID.IndexOf(x) != -1) 
            {
                x++;
            }
            var document = new BsonDocument
            {
                {"_id",x},
                {"Ime",Ime},
                {"Drzava",Drzava},
                {"Klub",k[0]},
                {"Plata",Plata},
            };
            collection.InsertOne(document);
        }
        public void dodajStatistiku(string Igrac, string Liga, double Poeni, double Skokovi, double Asistencije)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("Basketball");
            var collection = db.GetCollection<BsonDocument>("Igraci");
            var filter = Builders<BsonDocument>.Filter.Eq("Ime", Igrac);
            var i = collection.Find(filter).FirstOrDefault();
            collection = db.GetCollection<BsonDocument>("Statistika");
            var listaStatistika = collection.Find(new BsonDocument()).ToList();
            List<double> listaID = new List<double>();
            foreach (BsonDocument doc in listaStatistika)
            {
                listaID.Add((double)doc[0]);
            }
            double x = 1;
            while (listaID.IndexOf(x) != -1)
            {
                x++;
            }
            var document = new BsonDocument
            {
                {"_id",x},
                {"Igrac",i[0]},
                {"Liga",Liga},
                {"Poeni",Poeni},
                {"Skokovi",Skokovi},
                {"Asistencije",Asistencije}
            };
            collection.InsertOne(document);
        }
        public void izmeniPoene(string Igrac, string Liga, double Poeni)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("Basketball");
            var collection = db.GetCollection<BsonDocument>("Igraci");
            var filter = Builders<BsonDocument>.Filter.Eq("Ime", Igrac);
            var i = collection.Find(filter).FirstOrDefault();
            collection = db.GetCollection<BsonDocument>("Statistika");
            filter = Builders<BsonDocument>.Filter.Eq("Igrac", i[0]);
            filter &= Builders<BsonDocument>.Filter.Eq("Liga", Liga);
            var update = Builders<BsonDocument>.Update.Set("Poeni", Poeni);
            collection.UpdateOne(filter, update);
        }
        public void izmeniSkokove(string Igrac, string Liga, double Skokovi)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("Basketball");
            var collection = db.GetCollection<BsonDocument>("Igraci");
            var filter = Builders<BsonDocument>.Filter.Eq("Ime", Igrac);
            var i = collection.Find(filter).FirstOrDefault();
            collection = db.GetCollection<BsonDocument>("Statistika");
            filter = Builders<BsonDocument>.Filter.Eq("Igrac", i[0]);
            filter &= Builders<BsonDocument>.Filter.Eq("Liga", Liga);
            var update = Builders<BsonDocument>.Update.Set("Skokovi", Skokovi);
            collection.UpdateOne(filter, update);
        }
        public void izmeniAsistencije(string Igrac, string Liga, double Asistencije)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("Basketball");
            var collection = db.GetCollection<BsonDocument>("Igraci");
            var filter = Builders<BsonDocument>.Filter.Eq("Ime", Igrac);
            var i = collection.Find(filter).FirstOrDefault();
            collection = db.GetCollection<BsonDocument>("Statistika");
            filter = Builders<BsonDocument>.Filter.Eq("Igrac", i[0]);
            filter &= Builders<BsonDocument>.Filter.Eq("Liga", Liga);
            var update = Builders<BsonDocument>.Update.Set("Asistencije", Asistencije);
            collection.UpdateOne(filter, update);
        }
        public void obrisiIgraca(string Igrac)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase("Basketball");
            var collection = db.GetCollection<BsonDocument>("Igraci");
            var filter = Builders<BsonDocument>.Filter.Eq("Ime", Igrac);
            var i = collection.Find(filter).FirstOrDefault();
            var collection1 = db.GetCollection<BsonDocument>("Statistika");
            var filter2 = Builders<BsonDocument>.Filter.Eq("Igrac", i[0]);
            collection1.DeleteMany(filter2);
            collection.DeleteOne(filter);
        }

        public void sortiraj(List<Statistika> statistika)
        {
            Statistika temp;
            for (int i = 0; i < statistika.Count-1; i++)
                for (int j = 0; j < statistika.Count-1; j++)
                {
                    if (statistika[j].Poeni < statistika[j + 1].Poeni)
                    {
                        temp = statistika[j+ 1];
                        statistika[j + 1] = statistika[j];
                        statistika[j] = temp;
                    }
                }
        }
    }
}
