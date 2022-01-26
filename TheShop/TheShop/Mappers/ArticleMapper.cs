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
				ArticleId = model.ArticleId,
				Name = model.Name,
				Price = model.Price
			};
		}

		public Article ToEntity(ArticleDTO dto)
		{
			return new Article()
			{
				ArticleId = dto.ArticleId,
				Name = dto.Name,
				Price = dto.Price
			};
		}
	}
}
