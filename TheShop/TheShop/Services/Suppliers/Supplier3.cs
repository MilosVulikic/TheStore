using TheShop.DAL.Models;
using TheShop.Services.Interfaces;

namespace TheShop.Services.Suppliers
{
	public class Supplier3 : Supplier
	{
		public override bool ArticleInInventory(int id)
		{
			return base.ArticleInInventory(id);
		}

		public override Article GetArticle(int id)
		{
			if (id == 150)
			{
				return new Article()
				{
					ID = 150,
					Name_of_article = "Article from supplier3",
					ArticlePrice = 460
				};
			}
			return null;
		}
	}

}
