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
			return _entities.FirstOrDefault(s => s.TypeId == id);
		}

		public virtual T Create(T entity)
		{
			try
			{
				var result = _entities.Add(entity);
				DatabaseContext.SaveChanges();
				return result;
			}
			catch (System.Exception)
			{
				// LOG
				throw;
			}
		}

		public virtual T Update(T entity)
		{
			try
			{
				var result = _entities.Find(entity.ID);
				if (result != null)
				{
					DatabaseContext.Entry(entity).CurrentValues.SetValues(entity);
					DatabaseContext.SaveChanges();
				}
				return result;
			}
			catch (System.Exception)
			{
				// LOG
				return null;
			}									
		}

		public virtual T Delete(T entity)
		{
			try
			{
				var result = _entities.Find(entity.ID);
				if (result != null)
				{
					DatabaseContext.Articles.Attach(entity);
					DatabaseContext.Articles.Remove(entity);
					DatabaseContext.SaveChanges();
				}
				return result;
			}
			catch (System.Exception)
			{
				// LOG
				return null;
			}
		}

		public BaseRepository(ApplicationDbContext context)
		{
			
			DatabaseContext = context;
			_entities = DatabaseContext.Set<T>();
		}


	}
}
