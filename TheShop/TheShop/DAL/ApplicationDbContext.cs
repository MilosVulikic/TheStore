using System.Data.Entity;
using TheShop.DAL.Models;

namespace TheShop.DAL
{
	public class ApplicationDbContext : DbContext
	{
		//	Since (EF Core) DbContextOptionsBuilder not available (for UseInMemoryDatabase setup),
		//	and we will always start with the clean inMemory DB
		//	Should we attach the DB, ApplicationDbContext will need to be updated as well as Repostitory tests
		public ApplicationDbContext()
		{
			InitializeDbContext();
		}

		private void InitializeDbContext()
		{
			this.Database.Delete();
			this.Database.Initialize(true);
		}

		public DbSet<Article> Articles { get; set; }

		public override int SaveChanges()
		{			
			return base.SaveChanges();
		}

	}
}
