using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HandControl.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HandControl.Model
{
    public class CommandModel : ICloneable, INotifyPropertyChanged
    {

        /// <summary>
        /// ID команды, уникально
        /// </summary>
        public int ID { get; set; }

        string name = "";
        /// <summary>
        /// Имя команды
        /// </summary>
        [JsonProperty(PropertyName = "name_command")]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                string lastName = name;
                name = value;
 
                    //if (InfoCommand != null && DataAction != null)
                    //{
                    //    FileIOManager.DeleteFolder(PathManager.GetCommandFolderPath(lastName)); // Сначала удаляем прдыдущую папку с командой
                    //    SaveCommand(this);
                    //}
            }
        }
        /// <summary>
        /// Содержит информацию о команде, такую как время, кол-во действий, кол-во повторений действия и итеративность действий
        /// </summary>
        [JsonProperty(PropertyName = "info_actions")]
        public InfoCommandModel InfoCommand { get; set; }

        /// <summary>
        /// Список данных действий
        /// </summary>
        [JsonProperty(PropertyName = "data_actions")]
        public ObservableCollection<ActionModel> DataAction { get; set; }

        /// <summary>
        /// Возвращает имя команды и дату ее изменения в бинарном виде
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
                        byteArray[i] = byteName[i];
                    else
                        byteArray[i] = Convert.ToByte('\0');
                }

                byte[] byteDate = System.Text.Encoding.UTF8.GetBytes(this.InfoCommand.Date);
                for (int i = 0; i < 12; i++)
                {
                    byteArray[20 + i] = byteDate[i];
                }

                return byteArray;
            }
            private set { }
        }

        /// <summary>
        /// Возвращает полную информацию и данные команды в бинарном виде
        /// </summary>
        [JsonIgnore]
        public byte[] BinaryDate
        {
            get {
                byte[] byteArray = new byte[20 + 12 + 4 + this.InfoCommand.CountCommand * 8];


                byte[] byteName = Encoding.GetEncoding(1251).GetBytes(this.Name);

                if (byteName.Length == 20)
                {
                    byteName[18] = Convert.ToByte('\0');
                    byteName[19] = Convert.ToByte('\0');
                }

                for (int i = 0; i < 20; i++)
                {
                    if (byteName.Length > i)
                        byteArray[i] = byteName[i];
                    else
                        byteArray[i] = Convert.ToByte('\0');
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
            private set { }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static ObservableCollection<CommandModel> GetCommands()
        {
            ObservableCollection<CommandModel> sessionLoaded = new ObservableCollection<CommandModel>();
            foreach (var item in PathManager.GetCommandsFilesPaths())
            {
                CommandModel loadedCommand = (CommandModel)JsonSerDer.LoadObject<CommandModel>(item);

                if (loadedCommand.InfoCommand == null)
                    loadedCommand.InfoCommand = InfoCommandModel.GetDefault();

                sessionLoaded.Add(loadedCommand);
            }
            // var profilesSorted = sessionLoaded.OrderBy(x => x.Id).ToList();
            return sessionLoaded;
        }

        public static void SaveCommand(CommandModel command)
        {
            JsonSerDer.SaveObject(command, PathManager.GetCommandPath(command.ID.ToString()));
        }

        public static void DeleteCommand(CommandModel command)
        {
            FileIOManager.DeleteFolder(PathManager.GetCommandFolderPath(command.ID.ToString())); // Сначала удаляем прдыдущую папку с командой
        }

        public object Clone()
        {
            var newDataAction = new ObservableCollection<ActionModel>();

            if (DataAction != null)
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
    }

    public class ActionModel : ICloneable, INotifyPropertyChanged
    {
        /// <summary>
        /// Номер действия
        /// </summary>
        [JsonProperty(PropertyName = "id_action")]
        public int Id { get; set; }

        /// <summary>
        /// Большой палец
        /// </summary>
        [JsonProperty(PropertyName = "thumb_finger")]
        public int ThumbFinger { get; set; }

        /// <summary>
        /// Указательный палец
        /// </summary>
        [JsonProperty(PropertyName = "pointer_finger")]
        public int PointerFinger { get; set; }

        /// <summary>
        /// Средний палец
        /// </summary>
        [JsonProperty(PropertyName = "middle_finger")]
        public int MiddleFinger { get; set; }

        /// <summary>
        /// Безымянный палец
        /// </summary>
        [JsonProperty(PropertyName = "ring_finder")]     
        public int RingFinder { get; set; }

        /// <summary>
        /// Мезинец
        /// </summary>
        [JsonProperty(PropertyName = "little_finger")]
        public int LittleFinger { get; set; }

        /// <summary>
        /// Задержка между действиями
        /// </summary>
        [JsonProperty(PropertyName = "del_action")]
        public int DelAction { get; set; }

        /// <summary>
        /// Положение кисти
        /// </summary>
        [JsonProperty(PropertyName = "state_pos_brush")]
        public int StatePosBrush { get; set; }

        /// <summary>
        /// Положение большого пальца
        /// </summary>
        [JsonProperty(PropertyName = "state_pos_thumb")]
        public int StatePosThumb { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

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

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public static int GetNewId(List<ActionModel> listActions)
        {
            int newId = 0;
            int maxId = 0;

            for (int i = 0; i < listActions.Count; i++)
            {
                if (listActions[i].Id > maxId)
                    maxId = listActions[i].Id;
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
    }

    public class InfoCommandModel : ICloneable, INotifyPropertyChanged
    {
        /// <summary>
        /// Бесконечность действий
        /// </summary>
        [JsonProperty(PropertyName = "iterable_actions")]
        public bool IterableActions { get; set; }

        /// <summary>
        /// Команда комбинированного управления
        /// </summary>
        [JsonProperty(PropertyName = "combined")]
        public bool CombinedCommand { get; set; }

        /// <summary>
        /// Количество повторений действий
        /// </summary>
        [JsonProperty(PropertyName = "num_act_rep")]
        public int NumActRep { get; set; }

        /// <summary>
        /// Количество действий в команде
        /// </summary>
        [JsonProperty(PropertyName = "count_command")]
        public int CountCommand { get; set; }

        /// <summary>
        /// Время последнего изменения/создания команды
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public static InfoCommandModel GetDefault()
        {
            InfoCommandModel result = new InfoCommandModel()
            {
                Date = "",
                IterableActions = false,
                CombinedCommand = false,
                NumActRep = 0,
                CountCommand = 0
            };
            return result;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

}
