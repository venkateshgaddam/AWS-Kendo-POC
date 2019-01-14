using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using ApplicationTracker.Models;
using ApplicationTracker.Common;
using System.Threading.Tasks;

namespace ApplicationTracker.Controllers
{
    public class HomeController : Controller
    {
        private jagdevEntities entities = new jagdevEntities();

        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Create()
        {
            ViewBag.Message = "Your app description page.";
				//This is the Test of the Branch Code
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(FormCollection movieModel)
        {
            ViewBag.Message = "Your app description page.";

            Movie model = new Movie();
            if (ModelState.IsValid)
            {
                model.MovieName = movieModel["moviename"].ToString();
                model.Producer = movieModel["producer"].ToString();
                model.DateOfRelease = DateTime.Parse((movieModel["datepicker"].ToString()));

                await clsDbOperations.AddUser(model);
            }

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult AddMovie()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMovie(FormCollection fc)
        {

            return View();
        }

        public ActionResult Albums()
        {

            return View(clsDbOperations.Movies);

        }
        public ActionResult CreateAlbums()
        {
            var producers = new string[] { "Walt Disney", "Warner Bros", "Eros Now" };
            ViewBag.ProducersList = entities.Movies.Select(m => m.MovieName).ToArray();
            return View();
        }
        [HttpPost]
        public ActionResult CreateAlbums(MoviesModel model)
        {
            var producers = new string[] { "Walt Disney", "Warner Bros", "Eros Now" };
            ViewBag.ProducersList = entities.Movies.ToArray();
            if (!ModelState.IsValid)
            {
                return View(clsDbOperations.Movies);
            }
            else
            {
                clsDbOperations.AddMovie(model);
            }

            return View();
        }


        public ActionResult AddActor()
        {

            return View();
        }

        [HttpPost]
        public ActionResult AddActor(TechnicianModel fc)
        {

            return View();
        }

    }
}
