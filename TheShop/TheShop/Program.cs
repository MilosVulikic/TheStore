using System;
using TheShop.Controllers;
using TheShop.DAL.Models;
using TheShop.DAL.Repositories;
using TheShop.DTOs;
using TheShop.Mappers;
using TheShop.Services;
using TheShop.Services.Interfaces;

namespace TheShop
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			ShopController shopController = new ShopController(GetShopServiceInstance(), new ArticleMapper());   // Client sent requests will be handled by controller

			var response = shopController.OrderAndSellArticle(50, 2000, 10);
			ProcessResponseOrderAndSellArticle(response);

			response = shopController.GetById(1);
			ProcessResponseGetById(response);

			response = shopController.GetById(50);
			ProcessResponseGetById(response);

			response = shopController.GetById(12);
			ProcessResponseGetById(response);

			Console.ReadKey();
		}

		private static IShopService GetShopServiceInstance()
		{
			return new ShopService(new ArticleRepository(new DAL.ApplicationDbContext()), new SupplierService());
		}

		
		private static void ProcessResponseGetById(ArticleDTO article)
		{
			if (article != null)			
				Console.WriteLine("Found article with ID: " + article.TypeId);
			else
				Console.WriteLine("Article not found.");
		}

		private static void ProcessResponseOrderAndSellArticle(ArticleDTO responseOrderAndSellArticle)
		{
			if (responseOrderAndSellArticle != null)
				Console.WriteLine($"Article ID: {responseOrderAndSellArticle.TypeId} ordered succesfully");
			else
				Console.WriteLine("Article sold out");
		}
	}
}