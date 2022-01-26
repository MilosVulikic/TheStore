using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
				ID = model.ID,
				Name_of_article = model.Name_of_article,
				ArticlePrice = model.ArticlePrice
			};
		}

		public Article ToEntity(ArticleDTO dto)
		{
			return new Article()
			{
				ID = dto.ID,
				Name_of_article = dto.Name_of_article,
				ArticlePrice = dto.ArticlePrice
			};
		}
	}
}
