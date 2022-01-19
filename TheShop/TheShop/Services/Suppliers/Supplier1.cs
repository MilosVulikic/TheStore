using TheShop.DAL.Models;
using TheShop.Services.Interfaces;

namespace TheShop.Services.Suppliers
{
	public class Supplier1 : ISupplier
	{
		public bool ArticleInInventory(int id)
		{
			return true;
		}

		public Article GetArticle(int id)
		{
			return new Article()
			{
				ID = 1,
				Name_of_article = "Article from supplier1",
				ArticlePrice = 458
			};
		}
	}

}
