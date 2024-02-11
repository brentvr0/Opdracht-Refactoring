using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using VivesBlog.Models;

namespace VivesBlog.Core
{
	public class Database : DbContext
	{
		public Database(DbContextOptions<Database> options) : base(options)
		{
		}
		public DbSet<Article> Articles { get; set; }
		public DbSet<Person> People { get; set; }

		public void Seed()
		{
			var bavo = new Person { Id = 1, FirstName = "Bavo", LastName = "Ketels" };
			var john = new Person { Id = 2, FirstName = "John", LastName = "Doe" };

			People.Add(bavo);
			People.Add(john);

			var first = new Article
			{
				Id = 1,
				Title = "First article title",
				Description = "Short description of first article",
				Content = "The first article",
				AuthorId = bavo.Id,
				Author = bavo,
				CreatedDate = DateTime.Now
			};

			var second = new Article
			{
				Id = 2,
				Title = "Second article title",
				Description = "Short description of second article",
				Content = "The second article",
				AuthorId = john.Id,
				Author = john,
				CreatedDate = DateTime.Now.AddHours(-4)
			};

			Articles.Add(first);
			Articles.Add(second);
			SaveChanges();
		}
	}
}