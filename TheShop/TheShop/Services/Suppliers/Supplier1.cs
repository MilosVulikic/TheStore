using TheShop.DAL.Models;
using TheShop.Services.Interfaces;

namespace TheShop.Services.Suppliers
{
	public class Supplier1 : Supplier
	{
		public override bool ArticleInInventory(int id)
		{
			return base.ArticleInInventory(id);
		}

		public override Article GetArticle(int id)
		{
			if (id == 50)
			{
				return new Article()
				{
					ID = 50,
					Name_of_article = "Article from supplier1",
					ArticlePrice = 458
				};
			}
			return null;
			
		}
	}

}
