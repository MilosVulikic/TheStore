using System.Collections.Generic;
using System.Linq;
using TheShop.DAL.Models;
using TheShop.DAL.Interfaces;
using System.Data.Entity;

namespace TheShop.DAL.Repositories
{

	public class ArticleRepository : IArticleRepository
	{
		protected readonly ApplicationDbContext DatabaseContext;
		protected readonly DbSet<Article> _entities;

		public ArticleRepository()
		{
			DatabaseContext = new ApplicationDbContext();
			_entities = DatabaseContext.Set<Article>();
		}

		public Article Get(int id)
		{
			return _entities.FirstOrDefault(s => s.ID == id);
		}

		public Article Save(Article entity)
		{
			var result = _entities.Add(entity);
			DatabaseContext.SaveChanges();
			return result;
		}
	}

}
