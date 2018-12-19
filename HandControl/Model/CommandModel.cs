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
                if (InfoCommand != null && DataAction != null)
                {
                    FileIOManager.DeleteFolder(PathManager.GetCommandFolderPath(lastName)); // Сначала удаляем прдыдущую папку с командой
                    SaveCommand(this);
                }
                OnPropertyChanged();
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
            JsonSerDer.SaveObject(command, PathManager.GetCommandPath(command.Name));
        }

        public object Clone()
        {
            return new CommandModel()
            {
                Name = this.Name,
                InfoCommand = (InfoCommandModel)this.InfoCommand.Clone()
                // dataAction = (List<Action>)this.dataAction.Clone()
            };
        }
    }

    public class ActionModel : ICloneable
    {
        [JsonProperty(PropertyName = "name_action")]
        public string NameAction { get; set; }
        [JsonProperty(PropertyName = "thumb_finger")]       // Большой палец
        public int ThumbFinger { get; set; }
        [JsonProperty(PropertyName = "pointer_finger")]     // Указательный палец
        public int PointerFinger { get; set; }
        [JsonProperty(PropertyName = "middle_finger")]      // Средний палец
        public int MiddleFinger { get; set; }
        [JsonProperty(PropertyName = "ring_finder")]        // Безымянный палец
        public int RingFinder { get; set; }
        [JsonProperty(PropertyName = "little_finger")]      // Мезинец
        public int LittleFinger { get; set; }
        [JsonProperty(PropertyName = "del_action")]         // Задержка между действиями
        public int DelAction { get; set; }
        [JsonProperty(PropertyName = "state_pos_brush")]    // Положение кисти
        public int StatePosBrush { get; set; }
        [JsonProperty(PropertyName = "state_pos_thumb")]    // Положение большого пальца
        public int StatePosThumb { get; set; }

        public ActionModel() { }

        public static ActionModel GetDefault(string numNameAction)
        {
            ActionModel result = new ActionModel()
            {
                NameAction = numNameAction,
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
    }

    public class InfoCommandModel : ICloneable
    {
        [JsonProperty(PropertyName = "iterable_actions")]
        public bool IterableActions { get; set; }
        [JsonProperty(PropertyName = "combined")]
        public bool CombinedCommand { get; set; }
        [JsonProperty(PropertyName = "num_act_rep")]
        public int NumActRep { get; set; }
        [JsonProperty(PropertyName = "count_command")]
        public int CountCommand { get; set; }
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        public InfoCommandModel() { }

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
