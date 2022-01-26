using TheShop.DAL.Models;
using TheShop.Services.Interfaces;

namespace TheShop.Services.Suppliers
{
	public class Supplier3 : Supplier
	{		
		public Supplier3()
			: base(new Article()
			{
				TypeId = 150,
				Name_of_article = "Article from supplier3",
				ArticlePrice = 460
			})
		{						
		}

		public Supplier3(Article article) : base(article)
		{
		}

		public override bool ArticleInInventory(int id)
		{
			return base.ArticleInInventory(id);
		}

		public override Article GetArticle(int id)
		{
			return base.GetArticle(id);
		}
	}

}
