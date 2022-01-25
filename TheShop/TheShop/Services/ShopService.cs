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

		
		public Article GetArticle(int id)
		{
			return _articleRepository.Get(id);
		}

		public Article GetArticleInPriceRange(int id, int maxExpectedPrice)
		{
			var article = _articleRepository.Get(id);
			if (article != null && article.ArticlePrice <= maxExpectedPrice)
			{
				return article;
			}
			return null;
		}

		public void OrderArticle(int id, int maxExpectedPrice)
		{
			Article article = null;
			article = _supplierService.GetArticleFromAnySupplier(id, maxExpectedPrice);

			if (article != null)
			{
				_articleRepository.Save(article);
			}
		}

		public void SellArticle(int id, int buyerId)
		{
			var article = _articleRepository.GetNonSold(id);
			if (article != null)
			{
				logger.Debug("Trying to sell article with id=" + id);

				article.IsSold = true;
				article.SoldDate = DateTime.Now;
				article.BuyerUserId = buyerId;

				try
				{
					_articleRepository.Save(article);
					logger.Info("Article with id=" + id + " is sold.");
				}
				catch (ArgumentNullException ex)
				{
					logger.Error("Could not save article with id=" + id);
					throw new Exception("Could not save article with id");
				}
				catch (Exception)
				{
				}				
			}			
		}

	}

}
