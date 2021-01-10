// -------------------------------------------------------------------------------------
// <copyright file = "GestureRepository.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using AutoMapper;
using Google.Protobuf;
using HandControl.Model.Dto;
using HandControl.Model.Protobuf;
using Newtonsoft.Json;

namespace HandControl.Services.LocalStorage
{
    /// <summary>
    ///     Репозиторий, содержащий жесты, хранимые в системе.
    /// </summary>
    public class GesturesLocalStorage : IGesturesLocalStorage
    {
        private readonly IFileSystemFacade _fileSystemFacade;
        private readonly IMapper _mapper;
        private readonly GesturesInfo _gesturesInfo;

        public GesturesLocalStorage(IFileSystemFacade fileSystemFacade, IMapper mapper)
        {
            _fileSystemFacade = fileSystemFacade ?? throw new ArgumentNullException(nameof(fileSystemFacade));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(fileSystemFacade));
            _gesturesInfo = GesturesInfo.InfoLoad();
        }

        /// <summary>
        ///     Коллекция жестов.
        /// </summary>
        private GetGesturesDto _gesturesCacheField;

        public DateTime LastTimeSync => _gesturesInfo.LastTimeSync;

        public void Add(SaveGestureDto saveGestureDto)
        {
            var gestureDto = saveGestureDto.Gesture;
            var gestureProtobuf = _mapper.Map<GestureDto, Gesture>(gestureDto);
            _fileSystemFacade.WriteBinaryData(PathManager.GetGesturePath(gestureDto.Id.ToString()), gestureProtobuf.ToByteArray());

            _gesturesInfo.LastTimeSync = saveGestureDto.TimeSync;
            GesturesInfo.InfoSave(_gesturesInfo);
        }

        public void Remove(DeleteGestureDto deleteGestureDto)
        {
            _fileSystemFacade.DeleteFolder(PathManager.GetGestureFolderPath(deleteGestureDto.Id.ToString()));

            _gesturesInfo.LastTimeSync = deleteGestureDto.TimeSync;
            GesturesInfo.InfoSave(_gesturesInfo);
        }

        public void UpdateLastTimeSync(UpdateLastTimeSyncDto updateLastTimeSyncDto)
        {
            _gesturesInfo.LastTimeSync = updateLastTimeSyncDto.LastTimeSync;
            GesturesInfo.InfoSave(_gesturesInfo);
        }

        private GetGesturesDto LoadGestures()
        {
            var gestures = new List<GestureDto>();

            foreach (var file in PathManager.GetGesturesFilesPaths())
            {
                try
                {
                    var data = _fileSystemFacade.ReadBinaryData(file);
                    var gestureProtobuf = Gesture.Parser.ParseFrom(data);
                    var gestureDto = _mapper.Map<Gesture, GestureDto>(gestureProtobuf);
                    gestures.Add(gestureDto);
                }
                catch (Exception)
                {
                }
            }

            var getGestureDto = new GetGesturesDto()
            {
                Gestures = gestures,
                LastTimeSync = DateTime.MinValue
            };

            return getGestureDto;
        }

        public GetGesturesDto GetGestures()
        {
            if (_gesturesCacheField == null)
            {
                _gesturesCacheField = LoadGestures();
            }

            return _gesturesCacheField;
        }

        /// <summary>
        ///     Класса экземпляры которого содержат информацию о жестах.
        ///     Выполняет загрузку и сохранение из файловой системы экземпляров.
        ///     \brief Информация о жестах.
        ///     \authors Paul Rozhkin(blackiiifox@gmail.com)
        /// </summary>
        private class GesturesInfo
        {
            #region Properties

            /// <summary>
            ///     Gets or sets время синхронизации жестов между протезом и приложением.
            /// </summary>
            [JsonProperty(PropertyName = "LastTimeSync")]
            public DateTime LastTimeSync { get; set; }

            #endregion

            #region Methods

            /// <summary>
            ///     Загрузка информации из файловой системы информации о жестах..
            /// </summary>
            /// <returns>Экземпляр <see cref="GesturesInfo" />.</returns>
            public static GesturesInfo InfoLoad()
            {
                var info =
                    (GesturesInfo)JsonSerDer.LoadObject<GesturesInfo>(PathManager.GetGestureInfoPath());

                if (info == null)
                {
                    info = GetDefault();
                    InfoSave(info);
                }

                return info;
            }

            /// <summary>
            ///     Сохранение информации о жестах в файловую систему.
            /// </summary>
            /// <param name="info">Сохраняемый экземпляр <see cref="GesturesInfo" />.</param>
            public static void InfoSave(GesturesInfo info)
            {
                JsonSerDer.SaveObject(info, PathManager.GetGestureInfoPath());
            }

            /// <summary>
            ///     Фабричный метод для получения дефолтных параметров информации о жестах.
            /// </summary>
            /// <returns>Экземпляр класса <see cref="GesturesInfo"/> с дефольными параметрами.</returns>
            private static GesturesInfo GetDefault()
            {
                var newInfo = new GesturesInfo
                {
                    LastTimeSync = DateTime.MinValue
                };
                return newInfo;
            }

            #endregion
        }
    }
}