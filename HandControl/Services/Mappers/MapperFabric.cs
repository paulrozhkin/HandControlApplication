using System;
using AutoMapper;
using HandControl.Model;
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

                cfg.CreateMap<SaveGestureDto, SaveGesture>()
                    .ForMember(dest => dest.TimeSync,
                        opt => opt.MapFrom(src => UnixTimestampConverter.DateTimeToUnix(src.TimeSync)));

                cfg.CreateMap<DeleteGestureDto, DeleteGesture>()
                    .ForMember(dest => dest.TimeSync,
                        opt => opt.MapFrom(src => UnixTimestampConverter.DateTimeToUnix(src.TimeSync)))
                    .ForMember(dest => dest.Id,
                        opt => opt.MapFrom(src => new UUID() { Value = src.Id.ToString() }));

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

                cfg.CreateMap<Telemetry, TelemetryDto>()
                    .ForMember(dest => dest.LastTimeSync,
                        opt => opt.MapFrom(src => UnixTimestampConverter.DateTimeFromUnix(src.LastTimeSync)))
                    .ForMember(dest => dest.ExecutableGesture,
                        opt => opt.MapFrom((src, dest) =>
                            Guid.TryParse(src.ExecutableGesture.Value, out var result) ? result : (Guid?) null));

                cfg.CreateMap<GetSettings, GetSettingsDto>();
                cfg.CreateMap<SetSettingsDto, SetSettings>();

                cfg.CreateMap<UpdateLastTimeSyncDto, UpdateLastTimeSync>()
                    .ForMember(dest => dest.LastTimeSync,
                        opt => opt.MapFrom(src => UnixTimestampConverter.DateTimeToUnix(src.LastTimeSync)));

                // Dto to Models
                cfg.CreateMap<GestureActionDto, GestureModel.ActionModel>()
                    .ForMember(dest => dest.LittleFinger,
                        opt => opt.MapFrom(src => src.LittleFingerPosition))
                    .ForMember(dest => dest.RingFinder,
                        opt => opt.MapFrom(src => src.RingFingerPosition))
                    .ForMember(dest => dest.MiddleFinger,
                        opt => opt.MapFrom(src => src.MiddleFingerPosition))
                    .ForMember(dest => dest.PointerFinger,
                        opt => opt.MapFrom(src => src.PointerFingerPosition))
                    .ForMember(dest => dest.ThumbFinger,
                        opt => opt.MapFrom(src => src.ThumbFingerPosition))
                    .ForMember(dest => dest.DelMotion,
                        opt => opt.MapFrom(src => src.Delay))
                    .ForMember(dest => dest.DelMotionSec,
                        opt => opt.Ignore())
                    .ForMember(dest => dest.Id,
                        opt => opt.Ignore());


                cfg.CreateMap<GestureModel.ActionModel, GestureActionDto>()
                    .ForMember(dest => dest.LittleFingerPosition,
                        opt => opt.MapFrom(src => src.LittleFinger))
                    .ForMember(dest => dest.RingFingerPosition,
                        opt => opt.MapFrom(src => src.RingFinder))
                    .ForMember(dest => dest.MiddleFingerPosition,
                        opt => opt.MapFrom(src => src.MiddleFinger))
                    .ForMember(dest => dest.PointerFingerPosition,
                        opt => opt.MapFrom(src => src.PointerFinger))
                    .ForMember(dest => dest.ThumbFingerPosition,
                        opt => opt.MapFrom(src => src.ThumbFinger))
                    .ForMember(dest => dest.Delay,
                        opt => opt.MapFrom(src => src.DelMotion));

                cfg.CreateMap<GestureDto, GestureModel>()
                    .ForMember(dest => dest.InfoGesture,
                        opt => opt.MapFrom((src, dest) =>
                        {
                            var defaultInfo = GestureModel.InfoGestureModel.GetDefault();
                            defaultInfo.TimeChange = src.LastTimeSync;
                            defaultInfo.IterableGesture = src.Iterable;
                            defaultInfo.NumberOfGestureRepetitions = src.Repetitions;
                            return defaultInfo;
                        }))
                    .ForMember(dest => dest.ListMotions,
                        opt => opt.MapFrom(src => src.Actions))
                    .AfterMap((dest, src) =>
                    {
                        var minId = 0;
                        foreach (var motion in src.ListMotions)
                        {
                            motion.Id = minId++;
                        }
                    });

                cfg.CreateMap<GestureModel, GestureDto>()
                    .ForMember(dest => dest.Iterable,
                        opt => opt.MapFrom(src => src.InfoGesture.IterableGesture))
                    .ForMember(dest => dest.LastTimeSync,
                        opt => opt.MapFrom(src => src.InfoGesture.TimeChange))
                    .ForMember(dest => dest.Repetitions,
                        opt => opt.MapFrom(src => src.InfoGesture.NumberOfGestureRepetitions))
                    .ForMember(dest => dest.Actions,
                        opt => opt.MapFrom(src => src.ListMotions));
            });

            return config;
        }
    }
}