using System;
using TheShop.Services.Suppliers;
using TheShop.Utilities;
using TheShop.DAL.Models;
using TheShop.DAL.Repositories;
using TheShop.Services.Interfaces;
using TheShop.DAL.Interfaces;
using System.Collections.Generic;

namespace TheShop.Services
{
	public class ShopService : IShopService
	{
		IArticleRepository _articleRepository;
		private Logger logger;

		private Supplier1 Supplier1;
		private Supplier2 Supplier2;
		private Supplier3 Supplier3;

		public ShopService(IArticleRepository articleRepository)
		{
			_articleRepository = articleRepository;			
			logger = new Logger();
			Supplier1 = new Supplier1();
			Supplier2 = new Supplier2();
			Supplier3 = new Supplier3();
		}


		
		public Article GetArticle(int id)
		{
			return _articleRepository.Get(id);
		}

		public void OrderArticle(int id, int maxExpectedPrice, int buyerId)
		{						
			Article article = GetArticleFromSupplier(Supplier1, id, maxExpectedPrice);
			if (article is null)
				article = GetArticleFromSupplier(Supplier2, id, maxExpectedPrice);
			if (article is null)
				article = GetArticleFromSupplier(Supplier3, id, maxExpectedPrice);

			_articleRepository.Save(article);
		}

		public void SellArticle(int id, int maxExpectedPrice, int buyerId)
		{
			var article = _articleRepository.Get(id);
			if (article == null)
			{
				throw new Exception("Could not order article");
			}

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

		private Article GetArticleFromSupplier(ISupplier supplier, int id, int maxExpectedPrice)
		{
			Article tempArticle;
			if (supplier.ArticleInInventory(id))
			{
				tempArticle = supplier.GetArticle(id);
				if (maxExpectedPrice > tempArticle.ArticlePrice)
				{
					return tempArticle;
				}
			}
			return null;
		}
	}

}
