using TheShop.DAL.Models;

namespace TheShop.Services.Suppliers
{
	public class Supplier1
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
