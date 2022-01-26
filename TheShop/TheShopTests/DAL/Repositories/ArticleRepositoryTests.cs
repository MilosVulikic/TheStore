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


		#region Save
		[TestMethod]
		public void SaveArticle_ArticleAddSucceeded_IncreasedArticleCount()
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
			var createdArticle = _articleRepository.Save(newArticle);

			// Assert			
			Assert.IsNotNull(createdArticle);
			Assert.AreEqual(newArticle.Name,createdArticle.Name);
			Assert.IsTrue(createdArticle.TypeId != 0);
			Assert.AreEqual(expectedNumberOfArticles,DbContext.Articles.Count());
		}
		#endregion

		#region Update
		[TestMethod]
		public void TempName()
		{
			// Arrange
			int id = 1;

			var a = DbContext.Articles.ToArray();
			var b = DbContext.Articles.FirstOrDefault();
			var c = DbContext.Articles.FirstOrDefault(x => x.TypeId == 1);
			var d = DbContext.Articles.Select(x => x.TypeId == 1).ToList();

			// Act
			var result = _articleRepository.Get(id);

			result.Name = "changing after result";

			_articleRepository.Update(result);

			result.Name = "changing after udate";
			_articleRepository.Save(result);

			a = DbContext.Articles.ToArray();
			b = DbContext.Articles.FirstOrDefault();
			c = DbContext.Articles.FirstOrDefault(x => x.TypeId == 1);
			d = DbContext.Articles.Select(x => x.TypeId == 1).ToList();

			// Assert			
			Assert.IsNotNull(result);
			Assert.AreEqual(id, result.TypeId);
		}
		#endregion
	}
}
