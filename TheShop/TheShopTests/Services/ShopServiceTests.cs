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
		private readonly Mock<ISupplierService> _supplierServiceMock;
		private readonly Mock<IUnitOfWork> _unitOfWorkMock;

		private Article _testArticle;


		public ShopServiceTests()
		{			
			_unitOfWorkMock = new Mock<IUnitOfWork>();
			_supplierServiceMock = new Mock<ISupplierService>();
			_shopService = new ShopService(_unitOfWorkMock.Object, _supplierServiceMock.Object);

			_testArticle = new Article()
			{
				ID = 1,
				Name = "Test article",
				IsSold = false,
				Price = 200,
				BuyerUserId = 100
			};
		}

		private Article GetTestArticleWithId(int articleId)
		{
			_testArticle.ID = articleId;
			return _testArticle;
		}


		#region Get
		[TestMethod]
		public void Get_ArticleExists_ShouldReturnArticle()
		{
			// Arrange
			var articleId = 1;
			var article = this.GetTestArticleWithId(articleId);

			_unitOfWorkMock
				.Setup(x => x.Articles.Get(articleId))
				.Returns(() => article);
			// Act
			var result = _shopService.GetArticle(articleId);

			// Assert
			Assert.IsNotNull(result);
			_unitOfWorkMock.Verify(x => x.Articles.Get(articleId), Times.Once);
		}

		[TestMethod]
		public void Get_ArticleDoesntExist_ShouldReturnNull()
		{
			// Arrange
			var articleId = 1;

			_unitOfWorkMock
				.Setup(x => x.Articles.Get(articleId))
				.Returns(() => null);

			// Act
			var result = _shopService.GetArticle(articleId);

			// Assert
			Assert.IsNull(result);
			_unitOfWorkMock.Verify(x => x.Articles.Get(articleId), Times.Once);
		}
		#endregion


		#region GetArticleInPriceRange
		[TestMethod]
		public void GetArticleInPriceRange_ArticleExistsPriceInRange_ShouldReturnArticle()
		{
			// Arrange
			var articleId = 1;
			var maxExpectedPrice = 200;

			var article = this.GetTestArticleWithId(articleId);
			article.Price = maxExpectedPrice;

			_unitOfWorkMock
				.Setup(x => x.Articles.Get(articleId))
				.Returns(() => article);

			// Act
			var result = _shopService.GetArticleInPriceRange(articleId, maxExpectedPrice);

			// Assert
			Assert.IsNotNull(result);
			_unitOfWorkMock.Verify(x => x.Articles.Get(articleId), Times.Once);
		}

		[TestMethod]
		public void GetArticleInPriceRange_ArticleExistsPriceOutOfRange_ShouldReturnNull()
		{
			// Arrange
			var articleId = 1;
			var maxExpectedPrice = 200;

			var article = this.GetTestArticleWithId(articleId);
			article.Price = maxExpectedPrice + 1;

			_unitOfWorkMock
				.Setup(x => x.Articles.Get(articleId))
				.Returns(() => article);

			// Act
			var result = _shopService.GetArticleInPriceRange(articleId, maxExpectedPrice);

			// Assert
			Assert.IsNull(result);
			_unitOfWorkMock.Verify(x => x.Articles.Get(articleId), Times.Once);
		}

		[TestMethod]
		public void GetArticleInPriceRange_ArticleDoesntExist_ShouldReturnNull()
		{
			// Arrange
			var articleId = 1;
			var maxExpectedPrice = 200;

			_unitOfWorkMock
				.Setup(x => x.Articles.GetNonSold(articleId))
				.Returns(() => null);

			// Act
			var result = _shopService.GetArticleInPriceRange(articleId, maxExpectedPrice);

			// Assert
			Assert.IsNull(result);
			_unitOfWorkMock.Verify(x => x.Articles.Get(articleId), Times.Once);
		}
		#endregion


		#region OrderArticle
		[TestMethod]
		public void OrderArticle_ArticleFoundAmongSuppliers_ShouldCallRepositorySave()
		{
			// Arrange
			var articleId = 1;
			var article = this.GetTestArticleWithId(articleId);
			var maxExpectedPrice = 200;

			_unitOfWorkMock
				.Setup(x => x.Articles.Get(articleId))
				.Returns(() => article);

			_supplierServiceMock
				.Setup(x => x.GetArticleFromAnySupplier(articleId, maxExpectedPrice))
				.Returns(() => article);

			// Act
			_shopService.OrderArticle(articleId, maxExpectedPrice);

			// Assert
			_unitOfWorkMock.Verify(x => x.Articles.Add(article), Times.Once);
			_unitOfWorkMock.Verify(x => x.Complete(), Times.Once);
		}

		[TestMethod]
		public void OrderArticle_ArticleNotFoundAmongSuppliers_ShouldNotCallUnitOfWorkComplete()
		{
			// Arrange
			var articleId = 1;
			var article = this.GetTestArticleWithId(articleId);
			var maxExpectedPrice = 200;

			_unitOfWorkMock
				.Setup(x => x.Articles.Get(articleId))
				.Returns(() => article);

			_supplierServiceMock
				.Setup(x => x.GetArticleFromAnySupplier(articleId, maxExpectedPrice))
				.Returns(() => null);

			// Act
			_shopService.OrderArticle(articleId, maxExpectedPrice);

			// Assert
			_unitOfWorkMock.Verify(x => x.Articles.Add(article), Times.Never);
			_unitOfWorkMock.Verify(x => x.Complete(), Times.Never);
		}
		#endregion


		#region SellArticle
		[TestMethod]
		public void SellArticle_NonSoldArticleExists_ShouldCallUnitOfWorkComplete()
		{
			// Arrange			
			var articleId = 1;
			var buyerId = 100;
			var dateBeforeExecution = DateTime.Now;

			var article = GetTestArticleWithId(articleId);

			_unitOfWorkMock
				.Setup(x => x.Articles.GetNonSold(articleId))
				.Returns(() => article);

			// Act
			_shopService.SellArticle(articleId, buyerId);



			// Assert			
			Assert.AreEqual(buyerId, article.BuyerUserId);
			Assert.IsTrue(dateBeforeExecution <= article.SoldDate);			
			_unitOfWorkMock.Verify(x => x.Complete(), Times.Once);
		}

		[TestMethod]
		public void SellArticle_ArticleSold_ShouldSetPropertySoldToTrueAndCallUnitOfWorkComplete()
		{
			// Arrange			
			var articleId = 1;
			var buyerId = 100;

			var article = GetTestArticleWithId(articleId);

			_unitOfWorkMock
				.Setup(x => x.Articles.GetNonSold(articleId))
				.Returns(() => article);

			// Act
			_shopService.SellArticle(articleId, buyerId);

			// Assert			
			Assert.AreEqual(true, article.IsSold);
			_unitOfWorkMock.Verify(x => x.Complete(), Times.Once);
		}

		[TestMethod]
		public void SellArticle_ArticleSold_ShouldUpdateSoldDatePropertyAndCallUnitOfWorkComplete()
		{
			
			// Arrange			
			var articleId = 1;
			var buyerId = 100;
			var dateBeforeExecution = DateTime.Now;

			var article = GetTestArticleWithId(articleId);

			_unitOfWorkMock
				.Setup(x => x.Articles.GetNonSold(articleId))
				.Returns(() => article);

			// Act
			_shopService.SellArticle(articleId, buyerId);

			// Assert			
			Assert.IsTrue(dateBeforeExecution <= article.SoldDate);
			_unitOfWorkMock.Verify(x => x.Complete(),Times.Once);
		}

		[TestMethod]
		public void SellArticle_NonSoldArticleDoesntExist_ShouldNotCallUnitOfWorkComplete()
		{
			// Arrange			
			var articleId = 1;
			var buyerId = 100;
			var dateBeforeExecution = DateTime.Now;

			var article = GetTestArticleWithId(articleId);

			_unitOfWorkMock
				.Setup(x => x.Articles.GetNonSold(articleId))
				.Returns(() => null);

			// Act
			_shopService.SellArticle(articleId, buyerId);

			// Assert
			_unitOfWorkMock.Verify(x => x.Complete(), Times.Never);
		}
		#endregion
	}
}