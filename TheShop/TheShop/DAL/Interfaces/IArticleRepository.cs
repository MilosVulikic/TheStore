using TheShop.DAL.Models;

namespace TheShop.DAL.Interfaces
{
	public interface IArticleRepository
	{
		Article Get(int articleId);

		Article GetNonSold(int articleId);

		Article Create(Article article);

		Article Update(Article article);

		Article Delete(Article article);
	}
}
