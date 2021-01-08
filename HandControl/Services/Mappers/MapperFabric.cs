using System;
using AutoMapper;
using HandControl.Model.Dto;
using HandControl.Model.Protobuf;

namespace HandControl.Services.Mappers
{
    public class MapperFabric : IMapperFabric
    {
        public IMapper CreateMapper()
        {
            var config = ConfigureMapper();
            return config.CreateMapper();
        }

        private static MapperConfiguration ConfigureMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Protobuf mapper
                cfg.CreateMap<GestureActionDto, GestureAction>();
                cfg.CreateMap<GestureAction, GestureActionDto>();

                cfg.CreateMap<GetGestures, GetGesturesDto>()
                    .ForMember(dest => dest.LastTimeSync,
                        opt => opt.MapFrom(src => UnixTimestampConverter.DateTimeFromUnix(src.LastTimeSync)));

                cfg.CreateMap<GestureDto, Gesture>()
                    .ForMember(dest => dest.LastTimeSync,
                        opt => opt.MapFrom(src => UnixTimestampConverter.DateTimeToUnix(src.LastTimeSync)))
                    .ForMember(dest => dest.Id,
                        opt => opt.MapFrom(src => new UUID() {Value = src.Id.ToString()}));

                cfg.CreateMap<Gesture, GestureDto>()
                    .ForMember(dest => dest.LastTimeSync,
                        opt => opt.MapFrom(src => UnixTimestampConverter.DateTimeFromUnix(src.LastTimeSync)))
                    .ForMember(dest => dest.Id,
                        opt => opt.MapFrom(src => Guid.Parse(src.Id.Value)));
            });

            return config;
        }
    }
}