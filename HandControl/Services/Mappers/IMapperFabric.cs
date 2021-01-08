using AutoMapper;

namespace HandControl.Services.Mappers
{
    public interface IMapperFabric
    {
        IMapper CreateMapper();
    }
}