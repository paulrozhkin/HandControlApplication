// --------------------------------------------------------------------------------------
// <copyright file = "GestureModel.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------
namespace HandControl.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using HandControl.Services;
    using Newtonsoft.Json;

    /// <summary>
    /// Класс содержащий целостный жест (некоторое действие или движение протеза, имеющее определённое значение или смысл) протеза.
    /// Экземпляр данного класса содержит информацию о комманде и положения принимаемые протезом в разные единицы времени.
    /// Содержит методы для сохранения и загрузки данных.
    /// \brief Класс содержащий целостный жест протеза.
    /// \version 1.1
    /// \date Март 2019 года
    /// \authors Paul Rozhkin(blackiiifox@gmail.com)
    /// </summary>
    public class GestureModel : BaseModel, ICloneable
    {
        #region Fields
        /// <summary>
        /// Название жеста. Выступает в качестве идентификатора в системе. 
        /// </summary>
        private string name = string.Empty;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GestureModel" /> class.
        /// </summary>
        /// <param name="nameGesture">Имя жеста.</param>
        private GestureModel(string nameGesture)
        {
            this.Name = nameGesture;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets идентификатор жеста, должен быть уникальным.
        /// </summary>
        [JsonProperty(PropertyName = "id_gesture")]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets имя жеста, должно быть уникальным.
        /// </summary>
        [JsonProperty(PropertyName = "name_gesture")]
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                string lastName = this.name;
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets информацию о жесте, такую как время создания/изменения жеста, кол-во действий, кол-во повторений действия и итеративность действий.
        /// </summary>
        [JsonProperty(PropertyName = "info_gesture")]
        public InfoCommandModel InfoGesture { get; set; }

        /// <summary>
        /// Gets or sets список действий жеста.
        /// </summary>
        [JsonProperty(PropertyName = "list_motions")]
        public ObservableCollection<MotionModel> ListMotions { get; set; }

        /// <summary>
        /// Gets полную информацию и данные жеста в бинарном виде
        /// </summary>
        [JsonIgnore]
        public byte[] BinaryDate
        {
            get
            {
                byte[] byteArray = new byte[20 + 12 + 4 + (this.InfoGesture.NumberOfMotions * 8)];

                byte[] byteName = Encoding.GetEncoding(1251).GetBytes(this.Name);

                if (byteName.Length == 20)
                {
                    byteName[18] = Convert.ToByte('\0');
                    byteName[19] = Convert.ToByte('\0');
                }

                for (int i = 0; i < 20; i++)
                {
                    if (byteName.Length > i)
                    {
                        byteArray[i] = byteName[i];
                    }
                    else
                    {
                        byteArray[i] = Convert.ToByte('\0');
                    }
                }

                byte[] byteDate = System.Text.Encoding.UTF8.GetBytes(this.InfoGesture.Date);
                for (int i = 0; i < 12; i++)
                {
                    byteArray[20 + i] = byteDate[i];
                }

                byteArray[32] = Convert.ToByte(this.InfoGesture.IsCombinedGesture);
                byteArray[33] = Convert.ToByte(this.InfoGesture.IterableGesture);
                byteArray[34] = Convert.ToByte(this.InfoGesture.GestureRepetitions);
                byteArray[35] = Convert.ToByte(this.InfoGesture.NumberOfMotions);

                for (int i = 0; i < this.InfoGesture.NumberOfMotions; i++)
                {
                    int index = 36 + (i * 8);
                    byteArray[index] = Convert.ToByte(this.ListMotions[i].LittleFinger);
                    byteArray[index + 1] = Convert.ToByte(this.ListMotions[i].RingFinder);
                    byteArray[index + 2] = Convert.ToByte(this.ListMotions[i].MiddleFinger);
                    byteArray[index + 3] = Convert.ToByte(this.ListMotions[i].PointerFinger);
                    byteArray[index + 4] = Convert.ToByte(this.ListMotions[i].DelMotion);
                    byteArray[index + 5] = Convert.ToByte(this.ListMotions[i].StatePosThumb);
                    byteArray[index + 6] = Convert.ToByte(this.ListMotions[i].StatePosBrush);
                    byteArray[index + 7] = Convert.ToByte(this.ListMotions[i].ThumbFinger);
                }

                return byteArray;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Фабричный метод для получения экземпляра <see cref="GestureModel"/> с дефолтными параметрами, но с уникальным именем.
        /// </summary>
        /// <param name="nameGesture">Имя жеста.</param>
        /// <returns>Экземпляра <see cref="GestureModel"/>.</returns>
        public static GestureModel GetDefault(string nameGesture)
        {
            GestureModel result = new GestureModel(nameGesture)
            {
                InfoGesture = InfoCommandModel.GetDefault(),
                ListMotions = new ObservableCollection<GestureModel.MotionModel>()
            };

            return result;
        }

        /// <summary>
        /// Извлечение списка жестов системы.
        /// </summary>
        /// <returns>Коллекция жестов хранимых в системе.</returns>
        public static ObservableCollection<GestureModel> GetCommands()
        {
            ObservableCollection<GestureModel> sessionLoaded = new ObservableCollection<GestureModel>();
            foreach (var item in PathManager.GetCommandsFilesPaths())
            {
                GestureModel loadedCommand = (GestureModel)JsonSerDer.LoadObject<GestureModel>(item);

                if (loadedCommand.InfoGesture == null)
                {
                    loadedCommand.InfoGesture = InfoCommandModel.GetDefault();
                }

                sessionLoaded.Add(loadedCommand);
            }

            return sessionLoaded;
        }

        /// <summary>
        /// Сохранение жеста в файловую систему.
        /// </summary>
        /// <param name="command">Экземпляр сохраняемого жеста.</param>
        public static void Save(GestureModel command)
        {
            JsonSerDer.SaveObject(command, PathManager.GetCommandPath(command.Name));
        }

        /// <summary>
        /// Удаление жеста из файловой системы.
        /// </summary>
        /// <param name="command">Экземпляр удаляемого жеста.</param>
        public static void Delete(GestureModel command)
        {
            FileIOManager.DeleteFolder(PathManager.GetCommandFolderPath(command.Name));
        }

        /// <summary>
        /// Полное клонирование экземпляра CommandModel.
        /// </summary>
        /// <returns>Клонированный экземпляр CommandModel.</returns>
        public object Clone()
        {
            var newDataMotion = new ObservableCollection<MotionModel>();

            if (this.ListMotions != null)
            {
                foreach (var action in this.ListMotions)
                {
                    newDataMotion.Add((MotionModel)action.Clone());
                }
            }

            return new GestureModel((string)this.Name.Clone())
            {
                InfoGesture = (InfoCommandModel)this.InfoGesture.Clone(),
                ListMotions = newDataMotion
            };
        }
        #endregion

        #region Classes
        /// <summary>
        /// Класс содержащий единичное положение протеза.
        /// </summary>
        public class MotionModel : BaseModel, ICloneable
        {
            #region Constructors
            /// <summary>
            /// Prevents a default instance of the <see cref="MotionModel" /> class from being created.
            /// </summary>
            private MotionModel()
            {
            }
            #endregion

            #region Propeties
            /// <summary>
            /// Gets or sets номер действия.
            /// </summary>
            [JsonProperty(PropertyName = "id_action")]
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets положение большого пальца в градусах.
            /// </summary>
            [JsonProperty(PropertyName = "thumb_finger")]
            public int ThumbFinger { get; set; }

            /// <summary>
            /// Gets or sets положение указательного пальца в градусах.
            /// </summary>
            [JsonProperty(PropertyName = "pointer_finger")]
            public int PointerFinger { get; set; }

            /// <summary>
            /// Gets or sets положение среднего пальца в градусах.
            /// </summary>
            [JsonProperty(PropertyName = "middle_finger")]
            public int MiddleFinger { get; set; }

            /// <summary>
            /// Gets or sets положение безымянного пальца в градусах.
            /// </summary>
            [JsonProperty(PropertyName = "ring_finder")]
            public int RingFinder { get; set; }

            /// <summary>
            /// Gets or sets положение мезинца в градусах.
            /// </summary>
            [JsonProperty(PropertyName = "little_finger")]
            public int LittleFinger { get; set; }

            /// <summary>
            /// Gets or sets задержку между действиями.
            /// </summary>
            [JsonProperty(PropertyName = "del_action")]
            public int DelMotion { get; set; }

            /// <summary>
            /// Gets or sets положение кисти в градусах.
            /// </summary>
            [JsonProperty(PropertyName = "state_pos_brush")]
            public int StatePosBrush { get; set; }

            /// <summary>
            /// Gets or sets положение поворота большого пальца в градусах.
            /// </summary>
            [JsonProperty(PropertyName = "state_pos_thumb")]
            public int StatePosThumb { get; set; }
            #endregion

            #region Methods
            /// <summary>
            /// Фабричный метод для получения экземпляра MotionModel с дефолтными параметрами.
            /// Для создания экземпляра требуется передача Id действия.
            /// </summary>
            /// <param name="idMotion">Id действия</param>
            /// <returns>Новый уникальный идентификатор действия.</returns>
            public static MotionModel GetDefault(int idMotion)
            {
                MotionModel result = new MotionModel()
                {
                    Id = idMotion,
                    ThumbFinger = 0,
                    PointerFinger = 0,
                    MiddleFinger = 0,
                    RingFinder = 0,
                    LittleFinger = 0,
                    DelMotion = 0,
                    StatePosBrush = 0,
                    StatePosThumb = 0
                };
                return result;
            }

            /// <summary>
            /// Генерация нового Id единичного действия на основании коллекции имеющихся действий жеста.
            /// </summary>
            /// <param name="listMotions">Коллекция имеющихся действий в жесте.</param>
            /// <returns>Коллекция действий жеста.</returns>
            public static int GetNewId(List<MotionModel> listMotions)
            {
                int maxId = 0;

                for (int i = 0; i < listMotions.Count; i++)
                {
                    if (listMotions[i].Id > maxId)
                    {
                        maxId = listMotions[i].Id;
                    }
                }

                for (int i = 1; i < maxId; i++)
                {
                    bool state_search = false;
                    for (int j = 0; j < listMotions.Count; j++)
                    {
                        if (listMotions[j].Id == i)
                        {
                            state_search = true;
                            break;
                        }
                    }

                    if (state_search == false)
                    {
                        int newId = i;
                        return newId;
                    }
                }

                return maxId + 1;
            }

            /// <summary>
            /// Полное клонирование экземпляра MotionModel.
            /// </summary>
            /// <returns>Клонированный экземпляр MotionModel.</returns>
            public object Clone()
            {
                return this.MemberwiseClone();
            }
            #endregion
        }

        /// <summary>
        /// Класс содержащий информацию о жесте <see cref="GestureModel"/>.
        /// </summary>
        public class InfoCommandModel : BaseModel, ICloneable
        {
            #region Constructors
            /// <summary>
            /// Prevents a default instance of the <see cref="InfoCommandModel" /> class from being created.
            /// </summary>
            private InfoCommandModel()
            {
            }
            #endregion

            #region Properties
            /// <summary>
            /// Gets or sets a value indicating whether итерируемость жеста.
            /// </summary>
            [JsonProperty(PropertyName = "iterable_gestures")]
            public bool IterableGesture { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether состояние доступности комбинированного управления для жеста.
            /// </summary>
            [JsonProperty(PropertyName = "combined_gesture")]
            public bool IsCombinedGesture { get; set; }

            /// <summary>
            /// Gets or sets количество повторений жеста.
            /// </summary>
            [JsonProperty(PropertyName = "num_act_rep")]
            public int GestureRepetitions { get; set; }

            /// <summary>
            /// Gets or sets количество действий в жесте.
            /// </summary>
            [JsonProperty(PropertyName = "count_command")]
            public int NumberOfMotions { get; set; }

            /// <summary>
            /// Gets or sets время последнего изменения/создания жеста.
            /// </summary>
            [JsonProperty(PropertyName = "date")]
            public string Date { get; set; }
            #endregion

            #region Methods
            /// <summary>
            /// Фабричный метод для получения экземпляра InfoCommandModel с дефолтными параметрами.
            /// </summary>
            /// <returns>Экземпляр InfoCommandModel.</returns>
            public static InfoCommandModel GetDefault()
            {
                InfoCommandModel result = new InfoCommandModel()
                {
                    Date = string.Empty,
                    IterableGesture = false,
                    IsCombinedGesture = false,
                    GestureRepetitions = 0,
                    NumberOfMotions = 0
                };
                return result;
            }

            /// <summary>
            /// Полное клонирование экземпляра InfoCommandModel.
            /// </summary>
            /// <returns>Клонированный экземпляр InfoCommandModel.</returns>
            public object Clone()
            {
                return this.MemberwiseClone();
            }
            #endregion
        }
        #endregion
    }
}