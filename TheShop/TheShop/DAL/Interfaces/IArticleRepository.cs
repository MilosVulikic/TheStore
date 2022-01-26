using TheShop.DAL.Models;

namespace TheShop.DAL.Interfaces
{
	public interface IArticleRepository
	{
		Article Get(int id);

		Article GetNonSold(int id);

		Article Create(Article article);

		Article Update(Article article);

		Article Delete(Article article);
	}
}
