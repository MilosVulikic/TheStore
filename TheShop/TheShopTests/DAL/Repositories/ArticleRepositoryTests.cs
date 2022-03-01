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
			DbContext = new ApplicationDbContext();
			_articleRepository = new ArticleRepository(DbContext);
			DbContext.Articles.Add(new Article()
			{
				ID = 1,
				Price = 200,
				BuyerUserId = 0,
				Name = "NonSoldArticle",
				IsSold = false,
				SoldDate = DateTime.Now
			});
			DbContext.Articles.Add(new Article()
			{
				ID = 2,
				Price = 200,
				BuyerUserId = 100,
				Name = "SoldArticle",
				IsSold = true,
				SoldDate = DateTime.Now
			});
			DbContext.SaveChanges();
		}

		#region Get
		[TestMethod]
		public void GetArticleById_ArticleExists_ShouldReturnArticle()
		{
			// Arrange
			int articleId = 1;

			// Act
			var result = _articleRepository.Get(articleId);

			// Assert			
			Assert.IsNotNull(result);
			Assert.AreEqual(articleId, result.ID);
		}

		[TestMethod]
		public void GetArticleById_ArticleDoesntExist_ShouldReturnNull()
		{
			// Arrange
			int articleId = 3;

			// Act
			var result = _articleRepository.Get(articleId);

			// Assert			
			Assert.IsNull(result);
		}
		#endregion

		#region GetNonSold
		[TestMethod]
		public void GetNonSoldArticleById_NonSoldArticleExists_ShouldReturnArticle()
		{
			// Arrange
			int articleId = 1;

			// Act
			var result = _articleRepository.GetNonSold(articleId);

			// Assert			
			Assert.IsNotNull(result);
			Assert.IsFalse(result.IsSold);
			Assert.AreEqual(articleId, result.ID);
		}

		[TestMethod]
		public void GetNonSoldArticleById_SoldArticleExist_ShouldReturnNull()
		{
			// Arrange
			int articleId = 2;

			// Act
			var result = _articleRepository.GetNonSold(articleId);

			// Assert			
			Assert.IsNull(result);
		}

		[TestMethod]
		public void GetNonSoldArticleById_ArticleDoesntExist_ShouldReturnNull()
		{
			// Arrange
			int articleId = 3;

			// Act
			var result = _articleRepository.GetNonSold(articleId);

			// Assert			
			Assert.IsNull(result);
		}
		#endregion


		#region Create
		[TestMethod]
		public void CreateArticle_ArticleAddSucceeded_IncreasedArticleCount()
		{
			// Arrange
			var newArticle = new Article()
			{
				ID = 1,
				Price = 400,
				BuyerUserId = 0,
				Name = "testCreatedArticle",
				IsSold = false,
				SoldDate = DateTime.Now
			};
			var expectedNumberOfArticles = DbContext.Articles.Count() + 1;
			

			// Act
			_articleRepository.Add(newArticle);
			DbContext.SaveChanges();

			// Assert						
			Assert.AreEqual(expectedNumberOfArticles, DbContext.Articles.Count());
		}
		#endregion

		#region Find
		[TestMethod]
		public void FindArticle_ArticleExists_ArticleFound()
		{
			// Arrange
			var articleId = 1;
			var article = _articleRepository.Get(articleId);

			article.Name = "Updated test article";

			// Act
			var foundArticle = _articleRepository.Find(x => x.ID == articleId && x.Name == "NonSoldArticle");

			// Assert			
			Assert.IsTrue(foundArticle.Count() > 0);		
		}

		[TestMethod]
		public void FindArticle_ArticleDoesntExist_ArticleNotFound()
		{
			// Arrange
			var articleId = 0;
			var article = _articleRepository.Get(articleId);

			// Act
			var foundArticle = _articleRepository.Find(x => x.ID == articleId && x.Name == "InvalidName");

			// Assert
			Assert.AreEqual(0, foundArticle.Count());
		}
		#endregion

		#region Remove
		[TestMethod]
		public void RemoveArticle_ArticleExists_ArticleDeleted()
		{
			// Arrange
			var articleId = 1;
			var article = _articleRepository.Get(articleId);

			var expectedNumberOfArticles = DbContext.Articles.Count() - 1;

			// Act
			_articleRepository.Remove(article);
			DbContext.SaveChanges();

			// Assert			
			Assert.AreEqual(expectedNumberOfArticles, DbContext.Articles.Count());
		}

		[TestMethod]
		public void RemoveArticle_ArticleDoesntExist_ArticleNotDeleted()
		{
			// Arrange
			var articleId = 0;
			var article = _articleRepository.Get(articleId);

			var expectedNumberOfArticles = DbContext.Articles.Count();

			// Act
			if (article != null)
			{
				_articleRepository.Remove(article);
				DbContext.SaveChanges();
			}

			// Assert			
			Assert.IsNull(article);
			Assert.AreEqual(expectedNumberOfArticles, DbContext.Articles.Count());
		}
		#endregion
	}
}
