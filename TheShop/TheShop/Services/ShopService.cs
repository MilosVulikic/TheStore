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
		SupplierController _supplierController;
		List<SupplierController.SuppliersFromConfig> _allSuppliers;
		
		private Logger logger;

		public ShopService(IArticleRepository articleRepository)
		{
			_articleRepository = articleRepository;						
			_supplierController = new SupplierController();
			_allSuppliers = new List<SupplierController.SuppliersFromConfig>();
			PopulateSuppliersFromConfiguration();
			logger = new Logger();
		}

		
		public Article GetArticle(int id)
		{
			return _articleRepository.Get(id);
		}

		public Article GetArticleInPriceRange(int id, int maxExpectedPrice)
		{
			var article = _articleRepository.Get(id);
			if (article != null && article.ArticlePrice < maxExpectedPrice)
			{
				return article;
			}
			return null;
		}

		public void OrderArticle(int id, int maxExpectedPrice, int buyerId)
		{
			Article article = null;
			foreach (var currentSupplier in _allSuppliers)
			{				
				article = _supplierController.GetArticleFromSupplier(currentSupplier, id, maxExpectedPrice);
				if (article != null)
					break;
			}

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


		private void PopulateSuppliersFromConfiguration()
		{
			// imitate reading from config
			_allSuppliers.Add(SupplierController.SuppliersFromConfig.Supplier1);
			_allSuppliers.Add(SupplierController.SuppliersFromConfig.Supplier2);
			_allSuppliers.Add(SupplierController.SuppliersFromConfig.Supplier3);
		}
	}

}
