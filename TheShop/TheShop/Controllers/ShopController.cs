using System;
using TheShop.DAL.Models;
using TheShop.DAL.Repositories;
using TheShop.DTOs;
using TheShop.Mappers;
using TheShop.Services;
using TheShop.Services.Interfaces;

namespace TheShop.Controllers
{
	public class ShopController
	{
		IShopService _shopService;
		IMapper<Article, ArticleDTO> _articleMapper;
		public ShopController(IShopService shopService, IMapper<Article, ArticleDTO> mapper)
		{
			_shopService = shopService;
			_articleMapper = mapper;
		}

		public ArticleDTO OrderAndSellArticle(int articleId, int maxExpectedPrice, int buyerId)
		{
			try
			{
				var article = _shopService.GetArticleInPriceRange(articleId, maxExpectedPrice);
				if (article is null)
					article = _shopService.OrderArticle(articleId, maxExpectedPrice);
				
				article = _shopService.SellArticle(articleId, buyerId);
				
				if (article != null)				
					return _articleMapper.ToDto(article);				
				return null;

			}
			catch (Exception ex)
			{
				// Log				
				return null;
			}
		}

		public ArticleDTO GetById(int articleId)
		{
			try
			{
				var article = _shopService.GetArticle(articleId);
				if (article != null)
					return _articleMapper.ToDto(article);
				return null;
			}
			catch (Exception ex)
			{
				// Log				
				return null;
			}			
		}
	}
}
