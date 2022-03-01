using TheShop.DAL.Models;
using TheShop.Services.Interfaces;

namespace TheShop.Services.Suppliers
{
	public abstract class Supplier : ISupplier
	{
		Article _article;

		public Supplier()
		{
			_article = new Article();
		}

		public Supplier(Article article)
		{
			_article = article;
		}

		public virtual bool ArticleInInventory(int id)
		{
			if (_article.ID == id)			
				return true;			
			return false; 
		}

		public virtual Article GetArticle(int id)
		{
			if (_article.ID == id)			
				return _article;			
			return null;
		}
	}
}
