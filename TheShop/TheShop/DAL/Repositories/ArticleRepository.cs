using TheShop.DAL.Models;
using TheShop.DAL.Interfaces;
using System.Linq;
using log4net;
using System.Reflection;

namespace TheShop.DAL.Repositories
{

	public class ArticleRepository : BaseRepository<Article>, IArticleRepository
	{
		private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public ArticleRepository(ApplicationDbContext context) : base(context)
		{
		}

		public Article GetNonSold(int articleId)
		{
			var article = _entities.FirstOrDefault(s => s.ArticleId == articleId && s.IsSold == false);
			if (article is null)			
				_logger.Debug($"Not found unsold article with ArticleId: {articleId}");
			return article;
		}
	}
	
}
