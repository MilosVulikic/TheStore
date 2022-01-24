using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using TheShop.DAL.Interfaces;
using TheShop.DAL.Models;
using TheShop.Services;
using TheShop.Services.Interfaces;

namespace TheShopTests.Services
{
	[TestClass]
	public class ShopServiceTests
	{
		private readonly IShopService _shopService;
		private readonly Mock<IArticleRepository> _articleRepositoryMock;		
		private Article _testArticle;


		public ShopServiceTests()
		{
			_articleRepositoryMock = new Mock<IArticleRepository>();
			_shopService = new ShopService(_articleRepositoryMock.Object);
			
			_testArticle = new Article()
			{
				//ID = 1,
				Name_of_article = "Test article",
				IsSold = false,
				ArticlePrice = 200,
				BuyerUserId = 100,
				SoldDate = DateTime.Now
			};
		}

		private Article GetTestArticleWithId(int id)
		{
			_testArticle.ID = id;
			return _testArticle;
		}

		
		#region Get
		[TestMethod]
		public void Get_ArticleExists_ShouldReturnArticle()
		{
			// Arrange
			var id = 1;
			var article = this.GetTestArticleWithId(id);

			_articleRepositoryMock
				.Setup(x => x.Get(id))
				.Returns(() => article);
			// Act
			var result = _shopService.GetArticle(id);
			
			// Assert
			Assert.IsNotNull(result);
			_articleRepositoryMock.Verify(x => x.Get(id), Times.Once);
		}

		[TestMethod]
		public void Get_ArticleDoesntExist_ShouldReturnNull()
		{
			// Arrange
			var id = 1;

			_articleRepositoryMock
				.Setup(x => x.Get(id))
				.Returns(() => null);

			// Act
			var result = _shopService.GetArticle(id);			

			// Assert
			Assert.IsNull(result);
			_articleRepositoryMock.Verify(x => x.Get(id), Times.Once);
		}
		#endregion


		#region GetArticleInPriceRange
		[TestMethod]
		public void GetArticleInPriceRange_ArticleExistsPriceInRange_ShouldReturnArticle()
		{
			// Arrange
			var id = 1;
			var maxExpectedPrice = 200;

			var article = this.GetTestArticleWithId(id);
			article.ArticlePrice = maxExpectedPrice;

			_articleRepositoryMock
				.Setup(x => x.Get(id))
				.Returns(() => article);
			
			// Act
			var result = _shopService.GetArticleInPriceRange(id,maxExpectedPrice);			

			// Assert
			Assert.IsNotNull(result);
			_articleRepositoryMock.Verify(x => x.Get(id), Times.Once);
		}

		[TestMethod]
		public void GetArticleInPriceRange_ArticleExistsPriceOutOfRange_ShouldReturnNull()
		{
			// Arrange
			var id = 1;
			var maxExpectedPrice = 200;

			var article = this.GetTestArticleWithId(id);
			article.ArticlePrice = maxExpectedPrice + 1;

			_articleRepositoryMock
				.Setup(x => x.Get(id))
				.Returns(() => article);

			// Act
			var result = _shopService.GetArticleInPriceRange(id, maxExpectedPrice);

			// Assert
			Assert.IsNull(result);
			_articleRepositoryMock.Verify(x => x.Get(id), Times.Once);
		}

		[TestMethod]
		public void GetArticleInPriceRange_ArticleDoesntExist_ShouldReturnNull()
		{
			// Arrange
			var id = 1;
			var maxExpectedPrice = 200;

			_articleRepositoryMock
				.Setup(x => x.GetNonSold(id))
				.Returns(() => null);

			// Act
			var result = _shopService.GetArticleInPriceRange(id, maxExpectedPrice);

			// Assert
			Assert.IsNull(result);
			_articleRepositoryMock.Verify(x => x.Get(id), Times.Once);
		}
		#endregion		

	}
}