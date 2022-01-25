using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;
using TheShop.DAL;
using TheShop.DAL.Interfaces;
using TheShop.DAL.Models;
using TheShop.DAL.Repositories;

namespace TheShopTests.DAL.Repositories
{
	[TestClass]
	public class ArticleRepositoryTests
	{
		protected readonly ApplicationDbContext DbContext;
		IArticleRepository _articleRepository;

		public ArticleRepositoryTests()
		{
			DbContext = new ApplicationDbContext();						// how to make entity freamework start with "new db" each time?
			_articleRepository = new ArticleRepository(DbContext);      // added reference for EntityFramework
			DbContext.Articles.Add(new Article() 
			{ 
				ID = 1,
				ArticlePrice = 200,
				BuyerUserId = 0,
				Name_of_article = "NonSoldArticle",
				IsSold = false,
				SoldDate = DateTime.Now	// MIN i NULL ne rade		-- treba da sredim da bude nullabilno
			});
			DbContext.Articles.Add(new Article()
			{
				ID = 2,
				ArticlePrice = 200,
				BuyerUserId = 100,
				Name_of_article = "SoldArticle",
				IsSold = true,
				SoldDate = DateTime.Now
			});
			DbContext.SaveChanges();
		
			var a = DbContext.Articles.ToList();

		}

		#region Get
		[TestMethod]
		public void GetArticleById_ArticleExists_ShouldReturnArticle()
		{
			// Arrange
			int id = 1;

			// Act
			var result = _articleRepository.Get(id);

			// Assert			
			Assert.IsNotNull(result);
			Assert.AreEqual(id, result.ID);			
		}

		[TestMethod]
		public void GetArticleById_ArticleDoesntExist_ShouldReturnNull()
		{
			// Arrange
			int id = 3;

			// Act
			var result = _articleRepository.Get(id);

			// Assert			
			Assert.IsNull(result);			
		}
		#endregion

		#region GetNonSold
		[TestMethod]
		public void GetNonSoldArticleById_NonSoldArticleExists_ShouldReturnArticle()
		{
			// Arrange
			int id = 1;

			// Act
			var result = _articleRepository.GetNonSold(id);

			// Assert			
			Assert.IsNotNull(result);
			Assert.IsFalse(result.IsSold);
			Assert.AreEqual(id, result.ID);
		}

		[TestMethod]
		public void GetNonSoldArticleById_SoldArticleExist_ShouldReturnNull()
		{
			// Arrange
			int id = 2;

			// Act
			var result = _articleRepository.GetNonSold(id);

			// Assert			
			Assert.IsNull(result);
		}

		[TestMethod]
		public void GetNonSoldArticleById_ArticleDoesntExist_ShouldReturnNull()
		{
			// Arrange
			int id = 3;

			// Act
			var result = _articleRepository.GetNonSold(id);

			// Assert			
			Assert.IsNull(result);
		}
		#endregion


		#region Save
		[TestMethod]
		public void SaveArticle_ArticleAddSucceeded_IncreasedArticleCount()
		{
			//DbContext.Articles.RemoveRange(DbContext.Articles);		// MOVE OUT TO DB SETUP...
			//DbContext.SaveChanges();
			//var a = DbContext.Articles.ToList();

			// Arrange
			var newArticle = new Article()
			{				
				ArticlePrice = 400,
				BuyerUserId = 0,
				Name_of_article = "testCreatedArticle",
				IsSold = false,
				SoldDate = DateTime.Now // MIN i NULL ne rade
			};
			var expectedNumberOfArticles = DbContext.Articles.Count() + 1;


			// Act
			var createdArticle = _articleRepository.Save(newArticle);

			// Assert			
			Assert.IsNotNull(createdArticle);
			Assert.AreEqual(newArticle.Name_of_article,createdArticle.Name_of_article);
			Assert.IsTrue(createdArticle.ID != 0);
			Assert.AreEqual(expectedNumberOfArticles,DbContext.Articles.Count());
		}
		#endregion
	}
}
