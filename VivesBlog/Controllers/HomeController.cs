using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VivesBlog.Core;

namespace VivesBlog.Controllers
{
	public class HomeController : Controller
	{
		private readonly Database _database;
		// Constructor toegevoegd om _database te initialiseren
		public HomeController(Database database)
		{
			_database = database;
		}
		public IActionResult Index()
		{
			// Controleer of _database niet null is voordat je het gebruikt
			if (_database != null)
			{
				var articles = _database.Articles.Include(a => a.Author).ToList();
				return View(articles);
			}
			else
			{
				// Hier kun je een fallback gebruiken als de database niet is geïnitialiseerd
				return Content("Database is niet geïnitialiseerd.");
			}
		}

		public IActionResult Details(int id)
		{
			// Hier ook controleren op _database null voordat je het gebruikt
			if (_database != null)
			{
				var article = _database.Articles.Include(a => a.Author).SingleOrDefault(a => a.Id == id);
				return View(article);
			}
			else
			{
				return Content("Database is niet geïnitialiseerd.");
			}
		}
	}
}