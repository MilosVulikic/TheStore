using TheShop.DAL.Models;
using TheShop.DAL.Interfaces;
using System.Linq;

namespace TheShop.DAL.Repositories
{

	public class ArticleRepository : BaseRepository<Article>, IArticleRepository
	{
		public ArticleRepository() : base()
		{
		}

		public Article GetNonSold(int id)
		{
			return _entities.FirstOrDefault(s => s.ID == id && s.IsSold == false);
		}
	}
	
}
