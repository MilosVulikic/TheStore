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
				ArticleId = 1,
				Price = 200,
				BuyerUserId = 0,
				Name = "NonSoldArticle",
				IsSold = false,
				SoldDate = DateTime.Now
			});
			DbContext.Articles.Add(new Article()
			{
				ArticleId = 2,
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
			Assert.AreEqual(articleId, result.ArticleId);			
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
			Assert.AreEqual(articleId, result.ArticleId);
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
				ArticleId = 1,
				Price = 400,
				BuyerUserId = 0,
				Name = "testCreatedArticle",
				IsSold = false,
				SoldDate = DateTime.Now
			};
			var expectedNumberOfArticles = DbContext.Articles.Count() + 1;


			// Act
			var createdArticle = _articleRepository.Create(newArticle);

			// Assert			
			Assert.IsNotNull(createdArticle);
			Assert.AreEqual(newArticle.Name,createdArticle.Name);
			Assert.IsTrue(createdArticle.ArticleId != 0);
			Assert.AreEqual(expectedNumberOfArticles,DbContext.Articles.Count());
		}
		#endregion

		#region Update
		[TestMethod]
		public void UpdateArticle_ArticleExists_ArticleUpdated()
		{
			// Arrange
			var articleId = 1;
			var article = _articleRepository.Get(articleId);

			article.Name = "Updated test article";
			
			// Act
			var updatedArticle = _articleRepository.Update(article);

			// Assert			
			Assert.IsNotNull(updatedArticle);
			Assert.AreEqual(article.Name, updatedArticle.Name);
			Assert.AreEqual(article.ArticleId,updatedArticle.ArticleId);			
		}

		[TestMethod]
		public void UpdateArticle_ArticleDoesntExist_ArticleNotUpdated()
		{
			// Arrange
			var articleId = 0;
			var article = _articleRepository.Get(articleId);			

			// Act
			var updatedArticle = _articleRepository.Update(article);			

			// Assert			
			Assert.IsNull(updatedArticle);			
		}
		#endregion

		#region Delete
		[TestMethod]
		public void DeleteArticle_ArticleExists_ArticleDeleted()
		{
			// Arrange
			var articleId = 1;
			var article = _articleRepository.Get(articleId);

			var expectedNumberOfArticles = DbContext.Articles.Count() - 1;

			// Act
			var deletedArticle = _articleRepository.Delete(article);

			// Assert			
			Assert.IsNotNull(deletedArticle);
			Assert.AreEqual(expectedNumberOfArticles, DbContext.Articles.Count());			
		}

		[TestMethod]
		public void DeleteArticle_ArticleDoesntExist_ArticleNotDeleted()
		{
			// Arrange
			var articleId = 0;
			var article = _articleRepository.Get(articleId);

			var expectedNumberOfArticles = DbContext.Articles.Count();

			// Act
			var updatedArticle = _articleRepository.Delete(article);

			// Assert
			Assert.IsNull(updatedArticle);
			Assert.AreEqual(expectedNumberOfArticles, DbContext.Articles.Count());			
		}
		#endregion
	}
}
