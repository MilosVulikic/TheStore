using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheShop.DAL;
using TheShop.DAL.Interfaces;
using TheShop.DAL.Models;
using TheShop.DAL.UnitOfWork;

namespace TheShopTests.DAL
{
	[TestClass]
	public class UnitOfWorkTests
	{
		protected readonly Mock<ApplicationDbContext> DbContextMock;		
		private readonly IUnitOfWork _unitOfWork;
		

		public UnitOfWorkTests()
		{
			DbContextMock = new Mock<ApplicationDbContext>();			
			_unitOfWork = new UnitOfWork(DbContextMock.Object);
		}

		[TestMethod]
		public void SaveChanges_ChangeIrrelevant_CalledComplete()
		{
			// Arrange			

			// Act
			_unitOfWork.Complete();			

			// Assert
			DbContextMock.Verify(x => x.SaveChanges());
		}
	
	}
}

