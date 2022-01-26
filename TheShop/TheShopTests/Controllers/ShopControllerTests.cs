using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheShop.Controllers;
using TheShop.DAL;
using TheShop.DAL.Interfaces;
using TheShop.DAL.Models;
using TheShop.DAL.Repositories;
using TheShop.DTOs;
using TheShop.Mappers;
using TheShop.Services;
using TheShop.Services.Interfaces;

namespace TheShopTests.Controllers
{
	// This test class would require complete rewrite 
	// in case project is migrated to .Net Core.
	// It would require in-test creation of httpClient
	// HttpMessage would be deserialized and asserted
	[TestClass]
	public class ShopControllerTests
	{
		private readonly ShopController _shopController;
		private readonly IMapper<Article,ArticleDTO> _articleMapper;
		private readonly Mock<IShopService> _shopServiceMock;
		

		private Article _testArticle;		

		public ShopControllerTests()
		{
			_shopServiceMock = new Mock<IShopService>();
			_articleMapper = new ArticleMapper();
			_shopController = new ShopController(_shopServiceMock.Object, _articleMapper);

			_testArticle = new Article()
			{
				ArticleId = 1,
				Name = "Test article",
				IsSold = false,
				Price = 200,
				BuyerUserId = 100,
				SoldDate = DateTime.Now
			};
			
		}

		#region GetById
		[TestMethod]
		public void GetArticleById_OK()
		{
			// Arrange
			var id = 1;
			var articleDTO = _articleMapper.ToDto(_testArticle);

			_shopServiceMock.Setup(x => x.GetArticle(id)).Returns(_testArticle);

			// Act
			var result = _shopController.GetById(id);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(articleDTO.ArticleId, result.ArticleId);

		}

		[TestMethod]
		public void GetArticleById_NOK()
		{
			// Arrange
			var id = 2;

			_shopServiceMock.Setup(x => x.GetArticle(id)).Returns(() => null);

			// Act
			var result = _shopController.GetById(id);

			// Assert
			Assert.IsNull(result);
		}
		#endregion

		#region OrderAndSellArticle
		[TestMethod]
		public void OrderAndSellArticle_ArticleOnStock_ShouldNotCallOrderArticleShouldCallSellArticle()
		{
			// Arrange
			var id = 1;
			var maxExpectedPrice = 200;
			var buyerId = 100;

			_shopServiceMock.Setup(x => x.GetArticleInPriceRange(id,maxExpectedPrice)).Returns(() => _testArticle);			

			// Act
			var result = _shopController.OrderAndSellArticle(id, maxExpectedPrice, buyerId);

			// Assert			
			_shopServiceMock.Verify(x => x.OrderArticle(id,maxExpectedPrice), Times.Never);
			_shopServiceMock.Verify(x => x.SellArticle(id, buyerId), Times.Once);			
		}

		[TestMethod]
		public void OrderAndSellArticle_ArticleNotOnStock_ShouldCallOrderArticleShouldCallSellArticle()
		{
			// Arrange
			var id = 2;
			var maxExpectedPrice = 200;
			var buyerId = 100;

			_shopServiceMock.Setup(x => x.GetArticleInPriceRange(id,buyerId)).Returns(() => null);			

			// Act
			_shopController.OrderAndSellArticle(id, maxExpectedPrice, buyerId);

			// Assert
			_shopServiceMock.Verify(x => x.OrderArticle(id, maxExpectedPrice), Times.Once);
			_shopServiceMock.Verify(x => x.SellArticle(id, buyerId), Times.Once);			
		}

		[TestMethod]
		public void OrderAndSellArticle_ArticleExists_OrderAndSellSuccessfully()
		{
			// Arrange
			var id = 1;
			var maxExpectedPrice = 200;
			var buyerId = 100;
			var articleDTO = _articleMapper.ToDto(_testArticle);

			_shopServiceMock.Setup(x => x.GetArticleInPriceRange(id, maxExpectedPrice)).Returns(() => _testArticle);
			_shopServiceMock.Setup(x => x.SellArticle(id, buyerId)).Returns(() => _testArticle);

			// Act
			var result = _shopController.OrderAndSellArticle(id,maxExpectedPrice,buyerId);

			// Assert
			Assert.AreEqual(articleDTO.ArticleId, result.ArticleId);
			_shopServiceMock.Verify(x => x.GetArticleInPriceRange(id, maxExpectedPrice), Times.Once);
		}

		[TestMethod]
		public void OrderAndSellArticle_ArticleDoenstExist_OrderAndSellNotSuccessful()
		{
			// Arrange
			var id = 1;
			var maxExpectedPrice = 200;
			var buyerId = 100;
			
			_shopServiceMock.Setup(x => x.SellArticle(id, buyerId)).Returns(() => null);

			// Act
			var result = _shopController.OrderAndSellArticle(id, maxExpectedPrice, buyerId);

			// Assert
			Assert.IsNull(result);
			_shopServiceMock.Verify(x => x.GetArticleInPriceRange(id, maxExpectedPrice), Times.Once);
		}
		#endregion
	}
}
