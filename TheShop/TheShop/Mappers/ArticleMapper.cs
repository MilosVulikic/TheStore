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
				TypeId = model.TypeId,
				Name = model.Name,
				Price = model.Price
			};
		}

		public Article ToEntity(ArticleDTO dto)
		{
			return new Article()
			{
				TypeId = dto.TypeId,
				Name = dto.Name,
				Price = dto.Price
			};
		}
	}
}
