using System;
using System.Collections.Generic;
using TheShop.DAL.Models;
using TheShop.Services.Interfaces;
using TheShop.Services.Suppliers;

namespace TheShop.Services
{
	public class SupplierService : ISupplierService
	{
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
			List<SuppliersFromConfig> supplierNames  = GetSuppliersFromConfiguration();
			foreach (var supplierName in supplierNames)
			{
				_suppliers.Add(GetConcreteSupplier(supplierName));
			}			
		}

		public bool ArticleInInventory(int articleId, ISupplier supplier)
		{
			return supplier.ArticleInInventory(articleId);
		}

		public Article GetArticle(int articleId, ISupplier supplier)
		{			
			return supplier.GetArticle(articleId);
		}

		public Article GetArticleFromAnySupplier(int articleId, int maxExpectedPrice)
		{
			Article article;
			foreach (var supplier in _suppliers)
			{
				article = GetArticleFromSupplier(articleId, maxExpectedPrice, supplier);
				if (article != null)
				{
					return article;
				}
			}			
			return null;
		}

		public Article GetArticleFromSupplier(int articleId, int maxExpectedPrice, ISupplier supplier)
		{
			Article tempArticle;
			if (ArticleInInventory(articleId,supplier))
			{
				tempArticle = GetArticle(articleId,supplier);
				if (maxExpectedPrice >= tempArticle.Price)
				{
					return tempArticle;
				}
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
