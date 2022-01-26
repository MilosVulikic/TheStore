using System;
using TheShop.Services.Suppliers;
using TheShop.Utilities;
using TheShop.DAL.Models;
using TheShop.DAL.Repositories;
using TheShop.Services.Interfaces;
using TheShop.DAL.Interfaces;
using System.Collections.Generic;
using TheShop.Controllers;

namespace TheShop.Services
{
	public class ShopService : IShopService
	{
		IArticleRepository _articleRepository;
		ISupplierService _supplierService;		
		
		private Logger logger;

		public ShopService(IArticleRepository articleRepository, ISupplierService supplierService)
		{
			_articleRepository = articleRepository;						
			_supplierService = supplierService;			
			logger = new Logger();
		}

		
		public Article GetArticle(int articleId)
		{
			return _articleRepository.Get(articleId);
		}

		public Article GetArticleInPriceRange(int articleId, int maxExpectedPrice)
		{
			var article = _articleRepository.Get(articleId);
			if (article != null && article.Price <= maxExpectedPrice)
			{
				return article;
			}
			return null;
		}

		public Article OrderArticle(int articleId, int maxExpectedPrice)
		{
			Article article = null;
			article = _supplierService.GetArticleFromAnySupplier(articleId, maxExpectedPrice);

			if (article != null)
			{
				_articleRepository.Create(article);
			}
			return article;
		}

		public Article SellArticle(int articleId, int buyerId)
		{
			var article = _articleRepository.GetNonSold(articleId);
			if (article != null)
			{
				logger.Debug("Trying to sell article with id=" + articleId);

				article.IsSold = true;
				article.SoldDate = DateTime.Now;
				article.BuyerUserId = buyerId;

				try
				{
					_articleRepository.Update(article);					
					logger.Info("Article with id=" + articleId + " is sold.");
				}
				catch (ArgumentNullException ex)
				{
					logger.Error("Could not save article with typeId=" + articleId);
					throw new Exception("Could not save article with id");
				}
				catch (Exception)
				{
				}
			}
			return article;
		}

	}

}
