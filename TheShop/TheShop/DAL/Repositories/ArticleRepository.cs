using TheShop.DAL.Models;
using TheShop.DAL.Interfaces;
using System.Linq;

namespace TheShop.DAL.Repositories
{

	public class ArticleRepository : BaseRepository<Article>, IArticleRepository
	{
		public ArticleRepository(ApplicationDbContext context) : base(context)
		{
		}

		public Article GetNonSold(int id)
		{
			return _entities.FirstOrDefault(s => s.TypeId == id && s.IsSold == false);
		}
	}
	
}
