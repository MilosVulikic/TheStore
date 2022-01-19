using TheShop.DAL.Models;

namespace TheShop.DAL.Interfaces
{
	public interface IArticleRepository
	{
		Article GetById(int id);

		void Save(Article article);		
	}
}
