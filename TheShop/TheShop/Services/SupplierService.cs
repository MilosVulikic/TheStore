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

		public bool ArticleInInventory(int id, ISupplier supplier)
		{
			return supplier.ArticleInInventory(id);
		}

		public Article GetArticle(int id, ISupplier supplier)
		{			
			return supplier.GetArticle(id);
		}

		public Article GetArticleFromAnySupplier(int id, int maxExpectedPrice)
		{
			Article article;
			foreach (var supplier in _suppliers)
			{
				article = GetArticleFromSupplier(id, maxExpectedPrice, supplier);
				if (article != null)
				{
					return article;
				}
			}			
			return null;
		}

		public Article GetArticleFromSupplier(int id, int maxExpectedPrice, ISupplier supplier)
		{
			Article tempArticle;
			if (ArticleInInventory(id,supplier))
			{
				tempArticle = GetArticle(id,supplier);
				if (maxExpectedPrice >= tempArticle.ArticlePrice)
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
					break;
				case SuppliersFromConfig.Supplier2:
					return new Supplier2();
					break;
				case SuppliersFromConfig.Supplier3:
					return new Supplier3();
					break;
				default:
					return new Supplier1();
					break;
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
