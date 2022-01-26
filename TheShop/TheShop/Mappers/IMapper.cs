using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheShop.Mappers
{
	public interface IMapper<TEntity,TDto>
	{
		TEntity ToEntity(TDto dto);
		TDto ToDto(TEntity model);
	}
}
