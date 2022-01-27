using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using TheShop.DAL.Models;
using TheShop.Services.Interfaces;
using TheShop.Services.Suppliers;

namespace TheShop.Services
{
	public class SupplierService : ISupplierService
	{
		private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		List<ISupplier> _suppliers;

		public SupplierService()
		{
			_suppliers = new List<ISupplier>();
			PopulateSuppliers();
		}

		public SupplierService(ICollection<ISupplier> suppliers)
		{
			_suppliers = (List<ISupplier>)suppliers;			
		}

		private void PopulateSuppliers()
		{
			_logger.Info("Populating Suppliers using data from configuration");
			List<SuppliersFromConfig> supplierNames  = GetSuppliersFromConfiguration();
			foreach (var supplierName in supplierNames)
			{
				_suppliers.Add(GetConcreteSupplier(supplierName));
			}			
		}

		public bool ArticleInInventory(int articleId, ISupplier supplier)
		{
			// This method represents the supplier contact point
			_logger.Debug($"Checking if Supplier has Article with ArticleId: {articleId}.");
			try
			{
				return supplier.ArticleInInventory(articleId);
			}
			catch (Exception ex)
			{
				_logger.ErrorFormat($"Error occured while Getting Article from a supplier. Error message: {ex.Message}");
				return false;
			}			
		}

		public Article GetArticle(int articleId, ISupplier supplier)
		{
			// This method represents the supplier contact point
			_logger.Debug($"Trying to Get Article with ArticleId: {articleId} from supplier.");
			try
			{
				var article = supplier.GetArticle(articleId);
				if (article != null)
					_logger.Debug($"Article with ArticleId: {articleId} ordered from supplier.");
				else
					_logger.Debug($"Article with ArticleId: {articleId} was not ordered from supplier.");
				return article;
			}
			catch (Exception ex)
			{				
				_logger.Error($"Error occured while Getting Article with ArticleId: {articleId} from a supplier. Error message: {ex.Message}");
				return null;
			}			
		}

		public Article GetArticleFromAnySupplier(int articleId, int maxExpectedPrice)
		{
			_logger.Debug($"Getting Article with ArticleId: {articleId} from avaliable suppliers.");
			Article article;
			foreach (var supplier in _suppliers)
			{				
				article = GetArticleFromSupplier(articleId, maxExpectedPrice, supplier);
				if (article != null)				
					return article;				
			}			
			return null;
		}

		public Article GetArticleFromSupplier(int articleId, int maxExpectedPrice, ISupplier supplier)
		{			
			Article tempArticle;
			if (ArticleInInventory(articleId,supplier))
			{
				tempArticle = GetArticle(articleId,supplier);
				if (tempArticle != null && maxExpectedPrice >= tempArticle.Price)				
					return tempArticle;				
			}
			return null;
		}

	
		private ISupplier GetConcreteSupplier(SuppliersFromConfig supplierName)
		{
			switch (supplierName)
			{
				case SuppliersFromConfig.Supplier1:
					return new Supplier1();					
				case SuppliersFromConfig.Supplier2:
					return new Supplier2();					
				case SuppliersFromConfig.Supplier3:
					return new Supplier3();					
				default:
					return new Supplier1();					
			}
		}

		private List<SupplierService.SuppliersFromConfig> GetSuppliersFromConfiguration()
		{
			// imitate reading from config
			_logger.Info("Getting suppliers information from the configuration");
			List<SupplierService.SuppliersFromConfig> suppliersFromConfigs = new List<SupplierService.SuppliersFromConfig>()
			{
				SupplierService.SuppliersFromConfig.Supplier1,
				SupplierService.SuppliersFromConfig.Supplier2,
				SupplierService.SuppliersFromConfig.Supplier3,
			};

			return suppliersFromConfigs;
		}

		private enum SuppliersFromConfig
		{
			Supplier1,
			Supplier2,
			Supplier3
		}
	}
}
