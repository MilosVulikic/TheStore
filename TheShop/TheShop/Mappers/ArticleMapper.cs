using TheShop.DAL.Models;
using TheShop.DTOs;

namespace TheShop.Mappers
{
	public class ArticleMapper : IMapper<Article, ArticleDTO>
	{
		public ArticleDTO ToDto(Article model)
		{
			return new ArticleDTO()
			{
				ArticleId = model.ID,
				Name = model.Name,
				Price = model.Price
			};
		}

		public Article ToEntity(ArticleDTO dto)
		{
			return new Article()
			{
				ID = dto.ArticleId,
				Name = dto.Name,
				Price = dto.Price
			};
		}
	}
}
