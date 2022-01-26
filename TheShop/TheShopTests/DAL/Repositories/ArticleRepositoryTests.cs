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
				TypeId = 1,
				Price = 200,
				BuyerUserId = 0,
				Name = "NonSoldArticle",
				IsSold = false,
				SoldDate = DateTime.Now
			});
			DbContext.Articles.Add(new Article()
			{
				TypeId = 2,
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
			int id = 1;

			// Act
			var result = _articleRepository.Get(id);

			// Assert			
			Assert.IsNotNull(result);
			Assert.AreEqual(id, result.TypeId);			
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
			Assert.AreEqual(id, result.TypeId);
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


		#region Create
		[TestMethod]
		public void CreateArticle_ArticleAddSucceeded_IncreasedArticleCount()
		{			
			// Arrange
			var newArticle = new Article()
			{			
				TypeId = 1,
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
			Assert.IsTrue(createdArticle.TypeId != 0);
			Assert.AreEqual(expectedNumberOfArticles,DbContext.Articles.Count());
		}
		#endregion

		#region Update
		[TestMethod]
		public void UpdateArticle_ArticleExists_ArticleUpdated()
		{
			// Arrange
			var typeId = 1;
			var article = _articleRepository.Get(typeId);

			article.Name = "Updated test article";
			
			// Act
			var updatedArticle = _articleRepository.Update(article);

			// Assert			
			Assert.IsNotNull(updatedArticle);
			Assert.AreEqual(article.Name, updatedArticle.Name);
			Assert.AreEqual(article.TypeId,updatedArticle.TypeId);			
		}

		[TestMethod]
		public void UpdateArticle_ArticleDoesntExist_ArticleNotUpdated()
		{
			// Arrange
			var typeId = 0;
			var article = _articleRepository.Get(typeId);			

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
			var typeId = 1;
			var article = _articleRepository.Get(typeId);

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
			var typeId = 0;
			var article = _articleRepository.Get(typeId);

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
