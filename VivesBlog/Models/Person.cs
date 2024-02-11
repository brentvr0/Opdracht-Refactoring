using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VivesBlog.Models
{
	public class Person
	{
		public Person()
		{
			Articles = new List<Article>();
		}
		public int Id { get; set; }

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		public IList<Article> Articles { get; set; }
	}
}
