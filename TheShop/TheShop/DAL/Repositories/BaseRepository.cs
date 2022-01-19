using System.Data.Entity;
using System.Linq;
using TheShop.DAL.Models;

namespace TheShop.DAL.Repositories
{
	public abstract class BaseRepository<T> where T : Article    // Article to be replaced with more generic type
	{
		protected readonly ApplicationDbContext DatabaseContext;
		protected readonly DbSet<T> _entities;

		public virtual T Get(int id)
		{
			return _entities.FirstOrDefault(s => s.ID == id);
		}

		public virtual T Save(T entity)
		{
			var result = _entities.Add(entity);
			DatabaseContext.SaveChanges();
			return result;
		}


		public BaseRepository()
		{
			
			DatabaseContext = new ApplicationDbContext();
			_entities = DatabaseContext.Set<T>();
		}


	}
}
