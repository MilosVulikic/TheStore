using System;
using TheShop.Controllers;
using TheShop.DAL.Models;
using TheShop.DAL.Repositories;
using TheShop.Services;
using TheShop.Services.Interfaces;

namespace TheShop
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			ShopController shopController = new ShopController(GetShopServiceInstance());   // Client sent requests will be handled by controller


			try
			{
				//order and sell
				shopController.OrderAndSellArticle(50, 20, 10);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			var responseGetById = shopController.GetById(1);
			ProcessResponseGetById(responseGetById);
			
			responseGetById = shopController.GetById(12);
			ProcessResponseGetById(responseGetById);

			Console.ReadKey();
		}

		private static IShopService GetShopServiceInstance()
		{
			return new ShopService(new ArticleRepository(new DAL.ApplicationDbContext()), new SupplierService());
		}

		
		private static void ProcessResponseGetById(Article article)
		{
			if (article != null)
			{
				Console.WriteLine("Found article with ID: " + article.ID);
			}
			Console.WriteLine("Article not found: ");
		}
	}
}