using TheShop.DAL.Models;

namespace TheShop.DAL.Interfaces
{
	public interface IArticleRepository
	{
		Article Get(int id);

		void Save(Article article);		
	}
}
