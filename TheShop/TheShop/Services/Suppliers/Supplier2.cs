using TheShop.DAL.Models;
using TheShop.Services.Interfaces;

namespace TheShop.Services.Suppliers
{
	public class Supplier2 : Supplier
	{
		public override bool ArticleInInventory(int id)
		{
			return base.ArticleInInventory(id);
		}

		public override Article GetArticle(int id)
		{
			if (id == 100)
			{
				return new Article()
				{
					ID = 100,
					Name_of_article = "Article from supplier2",
					ArticlePrice = 459
				};
			}
			return null;
		}
	}

}
