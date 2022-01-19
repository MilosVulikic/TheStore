using TheShop.DAL.Models;
using TheShop.DAL.Interfaces;

namespace TheShop.DAL.Repositories
{

	public class ArticleRepository : BaseRepository<Article>, IArticleRepository
	{
		public ArticleRepository() : base()
		{
		}
	}

}
