using log4net;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using TheShop.DAL.Models;

namespace TheShop.DAL.Repositories
{
	public abstract class BaseRepository<T> where T : Article    // Article to be replaced with more generic type
	{
		private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		protected readonly ApplicationDbContext DatabaseContext;
		protected readonly DbSet<T> _entities;

		public virtual T Get(int id)
		{
			try
			{
				return _entities.FirstOrDefault(s => s.ArticleId == id);
			}
			catch (System.Exception ex)
			{
				_logger.Error($"Error while getting entity from DB. Error message: {ex.Message}. Stack trace: {ex.StackTrace}");
				throw;
			}
			
		}

		public virtual T Create(T entity)
		{
			try
			{
				var result = _entities.Add(entity);
				DatabaseContext.SaveChanges();
				return result;
			}
			catch (System.Exception ex)
			{
				_logger.Error($"Error while inserting entity into DB. Error message: {ex.Message}. Stack trace: {ex.StackTrace}");
				return null;
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
			catch (System.Exception ex)
			{
				_logger.Error($"Error while updating entity into DB. Error message: {ex.Message}. Stack trace: {ex.StackTrace}");
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
			catch (System.Exception ex)
			{
				_logger.Error($"Error while deleting entity from the DB. Error message: {ex.Message}. Stack trace: {ex.StackTrace}");
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
