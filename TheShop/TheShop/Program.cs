using System;
using TheShop.Controllers;
using TheShop.DTOs;

namespace TheShop
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var shopController = Startup.Instance.Instantiator<ShopController>();  // Client sent requests will be handled by controller

			var response = shopController.OrderAndSellArticle(1, 2000, 10);
			ProcessResponseOrderAndSellArticle(response);

			response = shopController.GetById(1);
			ProcessResponseGetById(response);

			response = shopController.GetById(2);
			ProcessResponseGetById(response);

			response = shopController.GetById(3);
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

	public interface IAgentBase
	{
		/// <summary>
		/// Source key to identify which back-end system this module communicates with.
		/// Must conform with values in [Config].[BackEndSourceKeys]
		/// </summary>
		string SourceKey { get; }
	}



}