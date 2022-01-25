using TheShop.DAL.Models;
using TheShop.Services.Interfaces;

namespace TheShop.Services.Suppliers
{
	public class Supplier1 : Supplier
	{	
		public Supplier1() 
			: base(new Article()
			{
				ID = 50,
				Name_of_article = "Article from supplier1",
				ArticlePrice = 458
			})
		{
		}


		public Supplier1(Article article) : base(article)
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
