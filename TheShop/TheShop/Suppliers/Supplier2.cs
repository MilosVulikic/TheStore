using TheShop.DAL.Models;

namespace TheShop.Suppliers
{
	public class Supplier2
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
				Name_of_article = "Article from supplier2",
				ArticlePrice = 459
			};
		}
	}

}
