using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using TheShop.DAL.Models;
using TheShop.Services;
using TheShop.Services.Interfaces;
using TheShop.Services.Suppliers;

namespace TheShopTests.Services
{
	[TestClass]
	public class SupplierServiceTests
	{
		private readonly ISupplierService _supplierService;
		private readonly Mock<ISupplier> _supplierMock;
		

		private ISupplier _testSupplier;
		private Article _testArticle;

		public SupplierServiceTests()
		{
			_supplierMock = new Mock<ISupplier>();
			

			_testArticle = new Article()
			{
				TypeId = 1,
				Name_of_article = "Test article from supplier1",
				ArticlePrice = 200
			};

			_testSupplier = new Supplier1(_testArticle);
			_supplierService = new SupplierService(new List<ISupplier>() { _testSupplier });
		}

		#region ArticleInInventory
		[TestMethod]
		public void CheckIfArticleInInventory_ArticleWithIdExists_ShouldReturnTrue()
		{
			// Arrange
			var id = 1;			
			var expectedValue = true;									

			_supplierMock.Setup(x => x.ArticleInInventory(id)).Returns(() => expectedValue);

			// Act
			var result = _supplierService.ArticleInInventory(id, _testSupplier);

			// Assert
			Assert.AreEqual(expectedValue, result);			
		}

		[TestMethod]
		public void CheckIfArticleInInventory_ArticleWithIdDoesntExist_ShouldReturnFalse()
		{
			// Arrange
			var id = 2;			
			var expectedValue = false;

			_supplierMock.Setup(x => x.ArticleInInventory(id)).Returns(() => expectedValue);
			

			// Act
			var result = _supplierService.ArticleInInventory(id, _testSupplier);

			// Assert
			Assert.AreEqual(expectedValue, result);			
		}
		#endregion


		#region GetArticle
		[TestMethod]
		public void GetArticleById_ArticleExists_ShouldReturnArticle()
		{
			// Arrange
			int id = 1;

			// Act
			var result = _supplierService.GetArticle(id,_testSupplier);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(_testArticle,result);
		}

		[TestMethod]
		public void GetArticleById_ArticleDoesntExist_ShouldReturnNull()
		{
			// Arrange
			int id = 2;

			// Act
			var result = _supplierService.GetArticle(id, _testSupplier);

			// Assert
			Assert.IsNull(result);			
		}
		#endregion


		#region GetArticleFromAnySupplier
		[TestMethod]
		public void GetArticleFromAnySupplier_ExistsSupplierWithArticleId_ShouldGetArticleFromSupplier()
		{
			// Arrange
			int id = 1;
			var maxExpectedPrice = 200;
			

			// Act
			var result = _supplierService.GetArticleFromAnySupplier(id, maxExpectedPrice);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(_testArticle,result);
		}

		[TestMethod]
		public void GetArticleFromAnySupplier_DoesntExistSupplierWithArticleId_ShouldGetNull()
		{
			// Arrange
			int id = 2;
			var maxExpectedPrice = 200;


			// Act
			var result = _supplierService.GetArticleFromAnySupplier(id, maxExpectedPrice);

			// Assert
			Assert.IsNull(result);
		}
		#endregion


		#region GetArticleFromSupplier
		[TestMethod]
		public void GetArticleFromSupplier_ArticleExistsPriceInRange_ShouldReturnArticle()
		{
			// Arrange
			int id = 1;
			var maxExpectedPrice = 200;

			// Act
			var result = _supplierService.GetArticleFromSupplier(id, maxExpectedPrice, _testSupplier);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(_testArticle, result);
		}

		[TestMethod]
		public void GetArticleFromSupplier_ArticleExistsPriceOutOfRange_ShouldReturnNull()
		{
			// Arrange
			int id = 1;
			var maxExpectedPrice = 199;

			// Act
			var result = _supplierService.GetArticleFromSupplier(id, maxExpectedPrice,_testSupplier);

			// Assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public void GetArticleFromSupplier_ArticleDoesntExist_ShouldReturnNull()
		{
			// Arrange
			int id = 2;
			var maxExpectedPrice = 200;

			// Act
			var result = _supplierService.GetArticleFromSupplier(id, maxExpectedPrice, _testSupplier);

			// Assert
			Assert.IsNull(result);
		}
		#endregion




	}
}
