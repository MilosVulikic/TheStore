using System;
using TheShop.DAL.Repositories;
using TheShop.Services;

namespace TheShop
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var shopService = new ShopService(new ArticleRepository());	// until Controller is introduced

			try
			{
				//order and sell
				shopService.OrderAndSellArticle(1, 20, 10);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			try
			{
				//print article on console
				var article = shopService.GetById(1);
				Console.WriteLine("Found article with ID: " + article.ID);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Article not found: " + ex);
			}

			try
			{
				//print article on console				
				var article = shopService.GetById(12);
				Console.WriteLine("Found article with ID: " + article.ID);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Article not found: " + ex);
			}

			Console.ReadKey();
		}
	}
}