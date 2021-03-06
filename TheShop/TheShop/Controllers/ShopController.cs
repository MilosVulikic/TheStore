using log4net;
using System.Reflection;
using TheShop.DAL.Models;
using TheShop.DTOs;
using TheShop.Mappers;
using TheShop.Services.Interfaces;

namespace TheShop.Controllers
{
	public class ShopController
	{
		private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		IShopService _shopService;
		IMapper<Article, ArticleDTO> _articleMapper;
		public ShopController(IShopService shopService, IMapper<Article, ArticleDTO> mapper)
		{
			_shopService = shopService;
			_articleMapper = mapper;
		}

		public ArticleDTO OrderAndSellArticle(int articleId, int maxExpectedPrice, int buyerId)
		{
			_logger.Info($"Starting OrderAndSellArticle for ArticleId: {articleId}, maxExpectedPrice: {maxExpectedPrice}, buyerId: {buyerId}");

			var article = _shopService.GetArticleInPriceRange(articleId, maxExpectedPrice);
			if (article is null)
				article = _shopService.OrderArticle(articleId, maxExpectedPrice);
				
			article = _shopService.SellArticle(articleId, buyerId);
				
			if (article != null) 
			{
				_logger.Info($"OrderAndSellArticle successful for ArticleId: {articleId}, maxExpectedPrice: {maxExpectedPrice}, buyerId: {buyerId}");
				return _articleMapper.ToDto(article);
			}								
			return null;
		}

		public ArticleDTO GetById(int articleId)
		{
			_logger.Info($"Getting Article with ArticleId: {articleId}");
			var article = _shopService.GetArticle(articleId);
			if (article != null)			
				return _articleMapper.ToDto(article);			
				
			return null;
		}
	}
}
