using System.Collections.Generic;
using System.Linq;
using TheShop.DAL.Models;
using TheShop.DAL.Interfaces;

namespace TheShop.DAL.Repositories
{
	//in memory implementation
	public class DatabaseDriver : IArticleRepository
	{
		private List<Article> _articles = new List<Article>();

		public Article GetById(int id)
		{
            return _articles.Single(x => x.ID == id);
		}

		public void Save(Article article)
		{
			_articles.Add(article);
		}
	}

}
