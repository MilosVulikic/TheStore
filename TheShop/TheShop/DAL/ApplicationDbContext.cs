using System.Data.Entity;
using TheShop.DAL.Models;

namespace TheShop.DAL
{
	public class ApplicationDbContext : DbContext
	{
		
		public DbSet<Article> Articles { get; set; }

		public override int SaveChanges()
		{			
			return base.SaveChanges();
		}

	}
}
