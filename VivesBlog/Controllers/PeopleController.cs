using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using VivesBlog.Core;
using VivesBlog.Models;

namespace VivesBlog.Controllers
{
	public class PeopleController : Controller
	{
		private readonly Database _database;
		public PeopleController(Database database)
		{
			_database = database;
		}

		public IActionResult Index()
		{
			// Controleer of _database niet null is voordat je het gebruikt
			if (_database != null)
			{
				var people = _database.People.ToList();
				return View(people);
			}
			else
			{
				// Hier kun je een fallback gebruiken als de database niet is geïnitialiseerd
				return Content("Database is niet geïnitialiseerd.");
			}
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Person person)
		{
			if (!ModelState.IsValid)
			{
				return View(person);
			}
			_database.People.Add(person);
			_database.SaveChanges();

			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
			var person = _database.People.Single(p => p.Id == id);
			return View(person);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Person person)
		{
			if (!ModelState.IsValid)
			{
				return View(person);
			}
			var dbPerson = _database.People.Single(p => p.Id == person.Id);
			dbPerson.FirstName = person.FirstName;
			dbPerson.LastName = person.LastName;
			_database.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult Delete(int id)
		{
			var person = _database.People.Single(p => p.Id == id);

			return View(person);
		}

		[HttpPost("[Controller]/delete/{id:int}")]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteConfirmed(int id)
		{
			var dbPerson = _database.People.Single(p => p.Id == id);

			_database.People.Remove(dbPerson);

			_database.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
