using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using AutoMapper;
using HandControl.Model;
using HandControl.Model.Dto;
using HandControl.Services.LocalStorage;

namespace HandControl.Services.ProstheticServices
{
    public class GestureService : IGestureService
    {
        private readonly IProstheticConnector _prostheticConnector;
        private readonly IMapper _mapper;
        private readonly IGesturesLocalStorage _gesturesLocalStorage;
        private readonly ReplaySubject<GestureModel> _gestureReplaySubject = new ReplaySubject<GestureModel>();

        public GestureService(IProstheticConnector prostheticConnector, IMapper mapper,
            IGesturesLocalStorage gesturesLocalStorage)
        {
            _prostheticConnector = prostheticConnector ?? throw new ArgumentNullException(nameof(prostheticConnector));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _gesturesLocalStorage =
                gesturesLocalStorage ?? throw new ArgumentNullException(nameof(gesturesLocalStorage));
            Gestures = _gestureReplaySubject.AsObservable().Publish().RefCount();
            LoadGestures();
        }

        public DateTime LastTimeSync => _gesturesLocalStorage.LastTimeSync;
        public IObservable<GestureModel> Gestures { get; }

        public async Task AddGestureAsync(GestureModel gesture)
        {
            try
            {
                var gestureDto = _mapper.Map<GestureModel, GestureDto>(gesture);
                var saveGesture = new SaveGestureDto()
                {
                    Gesture = gestureDto,
                    TimeSync = DateTime.Now
                };

                await _prostheticConnector.SaveGesturesAsync(saveGesture);
                _gesturesLocalStorage.Add(saveGesture);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task RemoveGestureAsync(GestureModel gesture)
        {
            try
            {
                var deleteGesture = new DeleteGestureDto()
                {
                    Id = gesture.Id,
                    TimeSync = DateTime.Now
                };

                await _prostheticConnector.DeleteGestureAsync(deleteGesture);
                _gesturesLocalStorage.Remove(deleteGesture);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task SyncGesturesAsync()
        {
            var getGesturesRemote = await _prostheticConnector.GetGesturesAsync();
            var getLocalGestures = _gesturesLocalStorage.GetGestures();
            
            var newTimeSync = DateTime.Now;
            newTimeSync = new DateTime(
                newTimeSync.Ticks - (newTimeSync.Ticks % TimeSpan.TicksPerSecond),
                newTimeSync.Kind
            );

            if (getGesturesRemote.LastTimeSync != getLocalGestures.LastTimeSync)
            {
                var (absentLocal, outdatedLocal) =
                    GetAbsentAndOutDatedGestures(getGesturesRemote.Gestures, getLocalGestures.Gestures);

                var (absentRemote, outdatedRemote) =
                    GetAbsentAndOutDatedGestures(getGesturesRemote.Gestures, getLocalGestures.Gestures);

                foreach (var gesture in absentLocal.Concat(outdatedLocal))
                {
                    _gesturesLocalStorage.Add(new SaveGestureDto()
                        {Gesture = gesture, TimeSync = newTimeSync});

                    var gestureModel = _mapper.Map<GestureDto, GestureModel>(gesture);
                    _gestureReplaySubject.OnNext(gestureModel);
                }
            }

            var updateLastTimeSyncDto = new UpdateLastTimeSyncDto()
            {
                LastTimeSync = newTimeSync
            };

            await _prostheticConnector.UpdateLastTimeSyncAsync(updateLastTimeSyncDto);
            _gesturesLocalStorage.UpdateLastTimeSync(updateLastTimeSyncDto);
        }

        private Tuple<IEnumerable<GestureDto>, IEnumerable<GestureDto>> GetAbsentAndOutDatedGestures(
            IEnumerable<GestureDto> source, IEnumerable<GestureDto> filter)
        {
            var gestureDtos = filter as GestureDto[] ?? filter.ToArray();
            var absentGestures = new List<GestureDto>();
            var outDatedGestures = new List<GestureDto>();

            foreach (var gestureInSource in source)
            {
                var gestureInFilter = gestureDtos.FirstOrDefault(x => x.Id == gestureInSource.Id);
                if (gestureInFilter == null)
                {
                    absentGestures.Add(gestureInSource);
                }
                else
                {
                    if (gestureInSource.LastTimeSync > gestureInFilter.LastTimeSync)
                    {
                        outDatedGestures.Add(gestureInSource);
                    }
                }
            }

            return new Tuple<IEnumerable<GestureDto>, IEnumerable<GestureDto>>(absentGestures, outDatedGestures);
        }

        private void LoadGestures()
        {
            var gesturesDto = _gesturesLocalStorage.GetGestures();

            foreach (var gestureDto in gesturesDto.Gestures)
            {
                var gestureModel = _mapper.Map<GestureDto, GestureModel>(gestureDto);
                _gestureReplaySubject.OnNext(gestureModel);
            }
        }
    }
}