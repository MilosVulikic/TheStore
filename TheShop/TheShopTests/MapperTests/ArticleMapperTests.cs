using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheShop.DAL.Models;
using TheShop.DTOs;
using TheShop.Mappers;

namespace TheShopTests.MapperTests
{
	[TestClass]
	public class ArticleMapperTests
	{
        private readonly ArticleMapper _articleMapper;

        public ArticleMapperTests()
        {
            _articleMapper = new ArticleMapper();
        }


        [TestMethod]
        public void DtoToEntityTest()
        {
            // Arrange
            var dto = new ArticleDTO
            {
                ArticleId = 1,
                Name = "test article",
                Price = 100                
            };

            // Act
            var model = _articleMapper.ToEntity(dto);

            // Assert
            Assert.AreEqual(dto.ArticleId, model.ArticleId);
            Assert.AreEqual(dto.Name, model.Name);
            Assert.AreEqual(dto.Price, model.Price);
        }

        [TestMethod]
        public void EntityToDtoTest()
        {
            // Arrange
            var model = new Article
            {                
                ArticleId = 1,
                Name = "Test article",
                IsSold = true,
                Price = 200,
                BuyerUserId = 100,
                SoldDate = DateTime.Now
            };

            // Act
            var dto = _articleMapper.ToDto(model);

            // Assert
            Assert.AreEqual(model.ArticleId, dto.ArticleId);
            Assert.AreEqual(model.Name, dto.Name);
            Assert.AreEqual(model.Price, dto.Price);
        }
    }
}
