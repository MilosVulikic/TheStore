using TheShop.DAL.Models;
using TheShop.DAL.Interfaces;
using System.Linq;
using log4net;
using System.Reflection;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Data.Entity;

namespace TheShop.DAL.Repositories
{

	public class ArticleRepository : BaseRepository<Article>, IArticleRepository
	{
		private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		
		public ArticleRepository(ApplicationDbContext context) : base(context)
		{

		}

		public Article GetNonSold(int id)
		{
			var article = DatabaseContext.Set<Article>().FirstOrDefault(s => s.ID == id && s.IsSold == false);
			if (article is null)			
				_logger.Debug($"Not found unsold article with ArticleId: {id}");
			return article;
		}

	}
	
}
