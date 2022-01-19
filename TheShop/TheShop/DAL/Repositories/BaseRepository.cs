using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheShop.DAL.Models;

namespace TheShop.DAL.Repositories
{
	public class BaseRepository<T> where T : Article    // Article to be replaced with more generic type
	{
		protected readonly ApplicationDbContext DatabaseContext;
		protected readonly DbSet<T> _entities;

		public T Get(int id)
		{
			return _entities.FirstOrDefault(s => s.ID == id);
		}

		public T Save(T entity)
		{
			var result = _entities.Add(entity);
			DatabaseContext.SaveChanges();
			return result;
		}


		public BaseRepository()
		{
			DatabaseContext = new ApplicationDbContext(); //context;
			_entities = DatabaseContext.Set<T>(); // _entities = context.Set<T>();
		}


	}
}
