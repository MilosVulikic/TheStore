using System;
using TheShop.Controllers;
using TheShop.DTOs;

namespace TheShop
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			
			var shopController = Startup.Instance.Instantiatior<ShopController>();  // Client sent requests will be handled by controller

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

		#region ClientMethods
		private static void ProcessResponseGetById(ArticleDTO article)
		{
			if (article != null)			
				Console.WriteLine("Found article with ID: " + article.ArticleId);
			else
				Console.WriteLine("Article not found.");
		}

		private static void ProcessResponseOrderAndSellArticle(ArticleDTO responseOrderAndSellArticle)
		{
			if (responseOrderAndSellArticle != null)
				Console.WriteLine($"Article ID: {responseOrderAndSellArticle.ArticleId} ordered succesfully");
			else
				Console.WriteLine("Article sold out");
		}
		#endregion
	}


	


}