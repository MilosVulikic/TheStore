using System.Collections.Generic;
using System.Linq;
using TheShop.DAL.Models;
using TheShop.DAL.Interfaces;
using System.Data.Entity;

namespace TheShop.DAL.Repositories
{

	public class ArticleRepository : BaseRepository<Article>, IArticleRepository
	{
		public ArticleRepository() : base()
		{
		}
	}

}
