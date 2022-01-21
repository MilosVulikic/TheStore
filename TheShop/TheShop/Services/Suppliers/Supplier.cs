using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheShop.DAL.Models;
using TheShop.Services.Interfaces;

namespace TheShop.Services.Suppliers
{
	public abstract class Supplier : ISupplier
	{
		public virtual bool ArticleInInventory(int id)
		{
			if (GetArticle(id) != null)
			{
				return true;
			}
			return false; 
		}

		public virtual Article GetArticle(int id)
		{			
			return new Article();		
		}
	}
}
