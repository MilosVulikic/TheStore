using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheShop.DAL.Models;
using TheShop.Services.Interfaces;
using TheShop.Services.Suppliers;

namespace TheShop.Controllers
{
	class SupplierController
	{
		ISupplier _supplier;
			
		public bool ArticleInInventory(int id)
		{
			return _supplier.ArticleInInventory(id);
		}

		public Article GetArticle(int id)
		{
			return _supplier.GetArticle(id);
		}

		public Article GetArticleFromSupplier(SuppliersFromConfig supplier, int id, int maxExpectedPrice)
		{
			SetSupplier(supplier);
			
			Article tempArticle;
			if (ArticleInInventory(id))
			{
				tempArticle = GetArticle(id);
				if (maxExpectedPrice > tempArticle.ArticlePrice)
				{
					return tempArticle;
				}
			}
			return null;
		}

		private void SetSupplier(SuppliersFromConfig supplier)
		{
			switch (supplier)   // to be replaced with Factory
			{
				case SuppliersFromConfig.Supplier1:
					_supplier = new Supplier1();
					break;
				case SuppliersFromConfig.Supplier2:
					_supplier = new Supplier2();
					break;
				case SuppliersFromConfig.Supplier3:
					_supplier = new Supplier3();
					break;
				default:
					_supplier = new Supplier1();
					break;
			}
		}

		public enum SuppliersFromConfig
		{
			Supplier1,
			Supplier2,
			Supplier3
		}
	}
}
