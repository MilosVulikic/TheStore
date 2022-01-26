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
		private readonly Mock<ISupplierService> _supplierServiceMock;		

		private Article _testArticle;


		public ShopServiceTests()
		{
			_articleRepositoryMock = new Mock<IArticleRepository>();
			_supplierServiceMock = new Mock<ISupplierService>();
			_shopService = new ShopService(_articleRepositoryMock.Object, _supplierServiceMock.Object);						

			_testArticle = new Article()
			{
				ID = 1,
				Name_of_article = "Test article",
				IsSold = false,
				ArticlePrice = 200,
				BuyerUserId = 100				
			};
		}

		private Article GetTestArticleWithId(int id)
		{
			_testArticle.TypeId = id;
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


		#region OrderArticle
		[TestMethod]
		public void OrderArticle_ArticleFoundAmongSuppliers_ShouldCallRepositorySave()
		{
			// Arrange
			var id = 1;
			var article = this.GetTestArticleWithId(id);
			var maxExpectedPrice = 200;

			_articleRepositoryMock
				.Setup(x => x.Get(id))
				.Returns(() => article);

			_supplierServiceMock
				.Setup(x => x.GetArticleFromAnySupplier(id,maxExpectedPrice))
				.Returns(() => article);

			// Act
			_shopService.OrderArticle(id, maxExpectedPrice);

			// Assert
			_articleRepositoryMock.Verify(x => x.Save(article), Times.Once);
		}

		[TestMethod]
		public void OrderArticle_ArticleNotFoundAmongSuppliers_ShouldNotCallRepositorySave()
		{
			// Arrange
			var id = 1;
			var article = this.GetTestArticleWithId(id);
			var maxExpectedPrice = 200;

			_articleRepositoryMock
				.Setup(x => x.Get(id))
				.Returns(() => article);

			_supplierServiceMock
				.Setup(x => x.GetArticleFromAnySupplier(id, maxExpectedPrice))
				.Returns(() => null);

			// Act
			_shopService.OrderArticle(id, maxExpectedPrice);

			// Assert
			_articleRepositoryMock.Verify(x => x.Save(article), Times.Never);
		}
		#endregion


		#region SellArticle
		[TestMethod]
		public void SellArticle_NonSoldArticleExists_ShouldCallRepositoryUpdate()
		{
			// Arrange			
			var id = 1;
			var buyerId = 100;
			var dateBeforeExecution = DateTime.Now;

			var article = GetTestArticleWithId(id);

			_articleRepositoryMock
				.Setup(x => x.GetNonSold(id))
				.Returns(() => article);

			// Act
			_shopService.SellArticle(id, buyerId);

			

			// Assert			
			Assert.AreEqual(buyerId, article.BuyerUserId);
			Assert.IsTrue(dateBeforeExecution <= article.SoldDate);
			_articleRepositoryMock.Verify(x => x.Update(article), Times.Once);
		}

		[TestMethod]
		public void SellArticle_ArticleSold_ShouldSetPropertySoldToTrue()
		{
			// Arrange			
			var id = 1;
			var buyerId = 100;			

			var article = GetTestArticleWithId(id);

			_articleRepositoryMock
				.Setup(x => x.GetNonSold(id))
				.Returns(() => article);

			// Act
			_shopService.SellArticle(id, buyerId);

			// Assert			
			Assert.AreEqual(true, article.IsSold);			
		}

		[TestMethod]
		public void SellArticle_ArticleSold_ShouldUpdateSoldDateProperty()
		{
			// Arrange			
			var id = 1;
			var buyerId = 100;
			var dateBeforeExecution = DateTime.Now;

			var article = GetTestArticleWithId(id);

			_articleRepositoryMock
				.Setup(x => x.GetNonSold(id))
				.Returns(() => article);

			// Act
			_shopService.SellArticle(id, buyerId);

			// Assert			
			Assert.IsTrue(dateBeforeExecution <= article.SoldDate);			
		}

		[TestMethod]
		public void SellArticle_NonSoldArticleDoesntExist_ShouldNotCallRepositorySave()
		{
			// Arrange			
			var id = 1;
			var buyerId = 100;
			var dateBeforeExecution = DateTime.Now;

			var article = GetTestArticleWithId(id);

			_articleRepositoryMock
				.Setup(x => x.GetNonSold(id))
				.Returns(() => null);

			// Act
			_shopService.SellArticle(id, buyerId);

			// Assert
			_articleRepositoryMock.Verify(x => x.Update(article), Times.Never);
		}
		#endregion
	}
}