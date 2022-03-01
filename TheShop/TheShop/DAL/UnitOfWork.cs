using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheShop.DAL.Interfaces;
using TheShop.DAL.Repositories;

namespace TheShop.DAL.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context; // This should be a specific context
		
		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
			Articles = new ArticleRepository(_context);
		}	
		
		public IArticleRepository Articles { get; private set; }

		public int Complete()		// instead of Save();
		{
			return _context.SaveChanges();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
