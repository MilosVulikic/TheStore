namespace TheShop.Mappers
{
	public interface IMapper<TEntity,TDto>
	{
		TEntity ToEntity(TDto dto);
		TDto ToDto(TEntity model);
	}
}
