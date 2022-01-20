using System;
using TheShop.Controllers;
using TheShop.DAL.Repositories;
using TheShop.Services;

namespace TheShop
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			ShopController shopController = new ShopController();	// Client sent requests will be handled by controller
						
			try
			{
				//order and sell
				shopController.OrderAndSellArticle(1, 20, 10);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			try
			{
				//print article on console
				var article = shopController.GetById(1);
				Console.WriteLine("Found article with ID: " + article.ID);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Article not found: " + ex);
			}

			try
			{
				//print article on console				
				var article = shopController.GetById(12);
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