using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using VivesBlog.Core;
using VivesBlog.Models;

namespace VivesBlog.Controllers
{
	public class BlogController : Controller
	{
		private readonly Database _database;
		public BlogController(Database database)
		{
			_database = database;
		}
		public IActionResult Index()
		{
			// Controleer of _database niet null is voordat je het gebruikt
			if (_database != null)
			{
				var blog = _database.Articles.Include(a => a.Author).ToList();
				return View(blog);
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
			var articleModel = CreateArticleModel();
			return View(articleModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Article article)
		{
			if (!ModelState.IsValid)
			{
				var articleModel = CreateArticleModel(article);
				return View(articleModel);
			}

			article.CreatedDate = DateTime.Now;
			_database.Articles.Add(article);
			_database.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
			var article = _database.Articles.Single(p => p.Id == id);
			var articleModel = CreateArticleModel(article);
			return View(articleModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Article article)
		{
			if (!ModelState.IsValid)
			{
				var articleModel = CreateArticleModel(article);
				return View(articleModel);
			}
			var dbArticle = _database.Articles.Single(p => p.Id == article.Id);
			dbArticle.Title = article.Title;
			dbArticle.Description = article.Description;
			dbArticle.Content = article.Content;
			dbArticle.AuthorId = article.AuthorId;
			_database.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult Delete(int id)
		{
			var article = _database.Articles.Include(a => a.Author).Single(p => p.Id == id);
			return View(article);
		}

		[HttpPost("[Controller]/delete/{id:int}")]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteConfirmed(int id)
		{
			var dbArticle = _database.Articles.Single(p => p.Id == id);
			_database.Articles.Remove(dbArticle);
			_database.SaveChanges();
			return RedirectToAction("Index");
		}

		private ArticleModel CreateArticleModel(Article article = null)
		{
			if (article is null)
			{
				return null;
			}
			else
			{
				article ??= new Article
				{
					AuthorId = article.AuthorId,
					Content = article.Content,
					Description = article.Description,
					Title = article.Title,
				};
			}
			var authors = _database.People.OrderBy(a => a.FirstName).ThenBy(a => a.LastName).ToList();
			var articleModel = new ArticleModel
			{
				Article = article,
				Authors = authors
			};

			return articleModel;
		}
	}
}