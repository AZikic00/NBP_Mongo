using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using baze_mongo.Models;
using baze_mongo.Repositories;
using MongoDB.Bson;

namespace baze_mongo.Controllers
{
    [ApiController]
    [Route("/")]
    public class BasketballController
    {
        private readonly IBasketbalRepository _baskteballRepository;
        public BasketballController(IBasketbalRepository basketballRepository)
        {
            _baskteballRepository = basketballRepository;
        }

        [Route("/sviKlubovi")]
        [HttpGet]
        public Task<List<Klub>> sviKlubovi()
        {
            return Task.FromResult(_baskteballRepository.sviKlubovi());
        }

        [Route("/sveDrzave")]
        [HttpGet]
        public Task<List<string>> sveDrzave()
        {
            return Task.FromResult(_baskteballRepository.sveDrzave());
        }

        [Route("/sveLige")]
        [HttpGet]
        public Task<List<string>> sveLige()
        {
            return Task.FromResult(_baskteballRepository.sveLige());
        }

        [Route("/sviIgraci")]
        [HttpGet]
        public Task<List<Igrac>> sviIgraci()
        {
            return Task.FromResult(_baskteballRepository.sviIgraci());
        }

        [Route("/igraciKlub/{klub}")]
        [HttpGet]
        public Task<List<Igrac>> IgraciKlub([FromRoute(Name = "klub")] string klub)
        {
            return Task.FromResult(_baskteballRepository.IgraciKlub(klub));
        }

        [Route("/igraciLiga/{liga}")]
        [HttpGet]
        public Task<List<Igrac>> IgraciLiga([FromRoute(Name = "liga")] string liga)
        {
            return Task.FromResult(_baskteballRepository.IgraciLiga(liga));
        }

        [Route("/igraciDrzava/{Drzava}")]
        [HttpGet]
        public Task<List<Igrac>> igraciDrzava([FromRoute(Name = "Drzava")] string Drzava)
        {
            return Task.FromResult(_baskteballRepository.igraciDrzava(Drzava));
        }

        /*[Route("/statistika/{Igrac}/{Liga}")]
        [HttpGet]
        public Task<Statistika> Statistika([FromRoute(Name = "Igrac")] string igrac, [FromRoute(Name = "Liga")] string liga)
        {
            return Task.FromResult(_baskteballRepository.Statistika(igrac,liga));
        }*/

        [Route("/statistikaLiga/{Liga}")]
        [HttpGet]
        public Task<List<Statistika>> StatistikaLiga([FromRoute(Name = "Liga")] string liga)
        {
            return Task.FromResult(_baskteballRepository.StatistikaLiga(liga));
        }
        [Route("/statistikaKlub/{Liga}/{Klub}")]
        [HttpGet]
        public Task<List<Statistika>> StatistikaKlub([FromRoute(Name = "Liga")] string liga, [FromRoute(Name = "Klub")] string klub)
        {
            return Task.FromResult(_baskteballRepository.StatistikaKlub(liga,klub));
        }
        [Route("/statistikaDrzava/{Liga}/{Drzava}")]
        [HttpGet]
        public Task<List<Statistika>> StatistikaDrzava([FromRoute(Name = "Liga")] string liga, [FromRoute(Name = "Drzava")] string drzava)
        {
            return Task.FromResult(_baskteballRepository.StatistikaDrzava(liga,drzava));
        }
        [Route("/statistika/{Liga}/{Klub}/{Drzava}")]
        [HttpGet]
        public Task<List<Statistika>> Statistika([FromRoute(Name = "Liga")] string liga, [FromRoute(Name = "Klub")] string klub, [FromRoute(Name = "Drzava")] string drzava)
        {
            return Task.FromResult(_baskteballRepository.Statistika(liga,klub,drzava));
        }

        [Route("/kluboviLiga/{Liga}")]
        [HttpGet]
        public Task<List<Klub>> kluboviLiga([FromRoute(Name = "Liga")] string liga)
        {
            return Task.FromResult(_baskteballRepository.kluboviLiga(liga));
        }

        [Route("/dodajIgraca/{Ime}/{Drzava}/{Klub}/{Plata}")]
        [HttpPost]
        public void dodajIgraca([FromRoute(Name = "Ime")] string Ime, [FromRoute(Name = "Drzava")] string Drzava, [FromRoute(Name = "Klub")] string Klub, [FromRoute(Name = "Plata")] double Plata)
        {
            _baskteballRepository.dodajIgraca(Ime,Drzava,Klub,Plata);
        }

        [Route("/dodajStatistiku/{Igrac}/{Liga}/{Poeni}/{Skokovi}/{Asistencije}")]
        [HttpPost]
        public void dodajStatistiku([FromRoute(Name = "Igrac")] string Igrac, [FromRoute(Name = "Liga")] string Liga, [FromRoute(Name = "Poeni")] double Poeni, [FromRoute(Name = "Skokovi")] double Skokovi, [FromRoute(Name = "Asistencije")] double Asistencije)
        {
            _baskteballRepository.dodajStatistiku(Igrac, Liga, Poeni, Skokovi, Asistencije);
        }

        [Route("/izmeniPoene/{Igrac}/{Liga}/{Poeni}")]
        [HttpPut]
        public void izmeniPoene([FromRoute(Name = "Igrac")] string Igrac, [FromRoute(Name = "Liga")] string Liga, [FromRoute(Name = "Poeni")] double Poeni)
        {
            _baskteballRepository.izmeniPoene(Igrac, Liga, Poeni);
        }
        [Route("/izmeniSkokove/{Igrac}/{Liga}/{Skokovi}")]
        [HttpPut]
        public void izmeniSkokove([FromRoute(Name = "Igrac")] string Igrac, [FromRoute(Name = "Liga")] string Liga, [FromRoute(Name = "Skokovi")] double Skokovi)
        {
            _baskteballRepository.izmeniSkokove(Igrac, Liga, Skokovi);
        }
        [Route("/izmeniAsistencije/{Igrac}/{Liga}/{Asistencije}")]
        [HttpPut]
        public void izmeniAsistencije([FromRoute(Name = "Igrac")] string Igrac, [FromRoute(Name = "Liga")] string Liga, [FromRoute(Name = "Asistencije")] double Asistencije)
        {
            _baskteballRepository.izmeniAsistencije(Igrac, Liga, Asistencije);
        }

        [Route("/obrisiIgraca/{Igrac}")]
        [HttpDelete]
        public void obrisiIgraca([FromRoute(Name = "Igrac")] string Igrac)
        {
            _baskteballRepository.obrisiIgraca(Igrac);
        }
    }
}

