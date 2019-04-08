// --------------------------------------------------------------------------------------
// <copyright file = "CommandModel.cs" company = "Студенческий проект HandControl‎"> 
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
    /// Класс содержащий целостную команду протеза.
    /// Экземпляр данного класса содержит информацию о комманде и положения принимаемые протезом в разные единицы времени.
    /// Содержит методы для сохранения и загрузки данных.
    /// \brief Класс содержащий целостную команду протеза.
    /// \version 1.1
    /// \date Март 2019 года
    /// \authors Paul Rozhkin(blackiiifox@gmail.com)
    /// </summary>
    public class CommandModel : BaseModel, ICloneable
    {
        #region Fields
        /// <summary>
        /// Имя команды
        /// </summary>
        private string name = string.Empty;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets уникальный идентификатор команды.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets имя команды.
        /// </summary>
        [JsonProperty(PropertyName = "name_command")]
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
        /// Gets or sets информацию о команде, такую как время, кол-во действий, кол-во повторений действия и итеративность действий.
        /// </summary>
        [JsonProperty(PropertyName = "info_actions")]
        public InfoCommandModel InfoCommand { get; set; }

        /// <summary>
        /// Gets or sets список данных действий
        /// </summary>
        [JsonProperty(PropertyName = "data_actions")]
        public ObservableCollection<ActionModel> DataAction { get; set; }

        /// <summary>
        /// Gets имя команды и дату ее изменения в бинарном виде
        /// </summary>
        [JsonIgnore]
        public byte[] BinaryInfo
        {
            get
            {
                byte[] byteArray = new byte[20 + 12];

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

                byte[] byteDate = System.Text.Encoding.UTF8.GetBytes(this.InfoCommand.Date);
                for (int i = 0; i < 12; i++)
                {
                    byteArray[20 + i] = byteDate[i];
                }

                return byteArray;
            }
        }

        /// <summary>
        /// Gets полную информацию и данные команды в бинарном виде
        /// </summary>
        [JsonIgnore]
        public byte[] BinaryDate
        {
            get
            {
                byte[] byteArray = new byte[20 + 12 + 4 + (this.InfoCommand.CountCommand * 8)];

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

                byte[] byteDate = System.Text.Encoding.UTF8.GetBytes(this.InfoCommand.Date);
                for (int i = 0; i < 12; i++)
                {
                    byteArray[20 + i] = byteDate[i];
                }

                byteArray[32] = Convert.ToByte(this.InfoCommand.CombinedCommand);
                byteArray[33] = Convert.ToByte(this.InfoCommand.IterableActions);
                byteArray[34] = Convert.ToByte(this.InfoCommand.NumActRep);
                byteArray[35] = Convert.ToByte(this.InfoCommand.CountCommand);

                for (int i = 0; i < this.InfoCommand.CountCommand; i++)
                {
                    int index = 36 + (i * 8);
                    byteArray[index] = Convert.ToByte(this.DataAction[i].LittleFinger);
                    byteArray[index + 1] = Convert.ToByte(this.DataAction[i].RingFinder);
                    byteArray[index + 2] = Convert.ToByte(this.DataAction[i].MiddleFinger);
                    byteArray[index + 3] = Convert.ToByte(this.DataAction[i].PointerFinger);
                    byteArray[index + 4] = Convert.ToByte(this.DataAction[i].DelAction);
                    byteArray[index + 5] = Convert.ToByte(this.DataAction[i].StatePosThumb);
                    byteArray[index + 6] = Convert.ToByte(this.DataAction[i].StatePosBrush);
                    byteArray[index + 7] = Convert.ToByte(this.DataAction[i].ThumbFinger);
                }

                return byteArray;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Извлечение списка команд системы.
        /// </summary>
        /// <returns>Коллекция команд хранимых в системе.</returns>
        public static ObservableCollection<CommandModel> GetCommands()
        {
            ObservableCollection<CommandModel> sessionLoaded = new ObservableCollection<CommandModel>();
            foreach (var item in PathManager.GetCommandsFilesPaths())
            {
                CommandModel loadedCommand = (CommandModel)JsonSerDer.LoadObject<CommandModel>(item);

                if (loadedCommand.InfoCommand == null)
                {
                    loadedCommand.InfoCommand = InfoCommandModel.GetDefault();
                }

                sessionLoaded.Add(loadedCommand);
            }

            return sessionLoaded;
        }

        /// <summary>
        /// Сохранение команды в файловую систему.
        /// </summary>
        /// <param name="command">Экземпляр сохраняемой команды.</param>
        public static void SaveCommand(CommandModel command)
        {
            JsonSerDer.SaveObject(command, PathManager.GetCommandPath(command.ID.ToString()));
        }

        /// <summary>
        /// Удаление команды из файловой системы.
        /// </summary>
        /// <param name="command">Экземпляр удаляемой команды.</param>
        public static void DeleteCommand(CommandModel command)
        {
            FileIOManager.DeleteFolder(PathManager.GetCommandFolderPath(command.ID.ToString()));
        }

        /// <summary>
        /// Полное клонирование экземпляра CommandModel.
        /// </summary>
        /// <returns>Клонированный экземпляр CommandModel.</returns>
        public object Clone()
        {
            var newDataAction = new ObservableCollection<ActionModel>();

            if (this.DataAction != null)
            {
                foreach (var action in this.DataAction)
                {
                    newDataAction.Add((ActionModel)action.Clone());
                }
            }

            return new CommandModel()
            {
                ID = this.ID,
                Name = (string)this.Name.Clone(),
                InfoCommand = (InfoCommandModel)this.InfoCommand.Clone(),
                DataAction = newDataAction
            };
        }
        #endregion

        #region Classes
        /// <summary>
        /// Класс содержащий единичное положение протеза.
        /// </summary>
        public class ActionModel : BaseModel, ICloneable
        {
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
            public int DelAction { get; set; }

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

            /// <summary>
            /// Фабричный метод для получения экземпляра ActionModel с дефолтными параметрами.
            /// Для создания экземпляра требуется передача Id действия.
            /// </summary>
            /// <param name="idAction">Id действия</param>
            /// <returns>Новый уникальный идентификатор действия.</returns>
            public static ActionModel GetDefault(int idAction)
            {
                ActionModel result = new ActionModel()
                {
                    Id = idAction,
                    ThumbFinger = 0,
                    PointerFinger = 0,
                    MiddleFinger = 0,
                    RingFinder = 0,
                    LittleFinger = 0,
                    DelAction = 0,
                    StatePosBrush = 0,
                    StatePosThumb = 0
                };
                return result;
            }

            /// <summary>
            /// Генерация нового Id единичного действия на основании коллекции имеющихся действий команды.
            /// </summary>
            /// <param name="listActions">Коллекция имеющихся действий команды.</param>
            /// <returns>Коллекция действий команды.</returns>
            public static int GetNewId(List<ActionModel> listActions)
            {
                int newId = 0;
                int maxId = 0;

                for (int i = 0; i < listActions.Count; i++)
                {
                    if (listActions[i].Id > maxId)
                    {
                        maxId = listActions[i].Id;
                    }
                }

                for (int i = 1; i < maxId; i++)
                {
                    bool state_search = false;
                    for (int j = 0; j < listActions.Count; j++)
                    {
                        if (listActions[j].Id == i)
                        {
                            state_search = true;
                            break;
                        }
                    }

                    if (state_search == false)
                    {
                        newId = i;
                        return newId;
                    }
                }

                return maxId + 1;
            }

            /// <summary>
            /// Полное клонирование экземпляра ActionModel.
            /// </summary>
            /// <returns>Клонированный экземпляр ActionModel.</returns>
            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }

        /// <summary>
        /// Класс содержащий информацию о команде.
        /// </summary>
        public class InfoCommandModel : BaseModel, ICloneable
        {
            /// <summary>
            /// Gets or sets a value indicating whether итерируемость действий команды.
            /// </summary>
            [JsonProperty(PropertyName = "iterable_actions")]
            public bool IterableActions { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether состояние доступности комбинированного управления для команды.
            /// </summary>
            [JsonProperty(PropertyName = "combined")]
            public bool CombinedCommand { get; set; }

            /// <summary>
            /// Gets or sets количество повторений действий команды.
            /// </summary>
            [JsonProperty(PropertyName = "num_act_rep")]
            public int NumActRep { get; set; }

            /// <summary>
            /// Gets or sets количество действий в команде.
            /// </summary>
            [JsonProperty(PropertyName = "count_command")]
            public int CountCommand { get; set; }

            /// <summary>
            /// Gets or sets время последнего изменения/создания команды.
            /// </summary>
            [JsonProperty(PropertyName = "date")]
            public string Date { get; set; }

            /// <summary>
            /// Фабричный метод для получения экземпляра InfoCommandModel с дефолтными параметрами.
            /// </summary>
            /// <returns>Экземпляр InfoCommandModel.</returns>
            public static InfoCommandModel GetDefault()
            {
                InfoCommandModel result = new InfoCommandModel()
                {
                    Date = string.Empty,
                    IterableActions = false,
                    CombinedCommand = false,
                    NumActRep = 0,
                    CountCommand = 0
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
        }
        #endregion
    }
}