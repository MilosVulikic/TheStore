using TheShop.DAL.Models;
using TheShop.Services.Interfaces;

namespace TheShop.Services.Suppliers
{
	public class Supplier2 : Supplier
	{		
		public Supplier2() 
			: base(new Article()
			{
				ArticleId = 100,
				Name = "Article from supplier2",
				Price = 459
			})
		{			
		}

		public Supplier2(Article article) : base(article)
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
