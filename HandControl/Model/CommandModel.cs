using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HandControl.Services;
using System.Collections.ObjectModel;

namespace HandControl.Model
{
    public class CommandModel : ICloneable
    {

        [JsonProperty(PropertyName = "name_command")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "info_actions")]
        public InfoCommand InfoCommand { get; set; }
        [JsonProperty(PropertyName = "data_actions")]
        public List<Action> DataAction { get; set; }

        public byte[] BinaryDate
        {
            get {
                byte[] byteArray = new byte[20 + 12 + 4 + this.InfoCommand.CountCommand * 8];

                
                byte[] byteName = Encoding.UTF8.GetBytes(this.Name);

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
                sessionLoaded.Add((CommandModel)JsonSerDer.LoadObject<CommandModel>(item));
            }
            // var profilesSorted = sessionLoaded.OrderBy(x => x.Id).ToList();
            return sessionLoaded;
        }

        public static void SaveSession(CommandModel command)
        {
            JsonSerDer.SaveObject(command, PathManager.GetCommandPath(command.Name));
        }

        public object Clone()
        {
            return new CommandModel()
            {
                Name = this.Name,
                InfoCommand = (InfoCommand)this.InfoCommand.Clone()
                // dataAction = (List<Action>)this.dataAction.Clone()
            };
        }
    }

    public class Action : ICloneable
    {
        [JsonProperty(PropertyName = "thumb_finger")]
        public int ThumbFinger { get; set; }
        [JsonProperty(PropertyName = "pointer_finger")]
        public int PointerFinger { get; set; }
        [JsonProperty(PropertyName = "middle_finger")]
        public int MiddleFinger { get; set; }
        [JsonProperty(PropertyName = "ring_finder")]
        public int RingFinder { get; set; }
        [JsonProperty(PropertyName = "little_finger")]
        public int LittleFinger { get; set; }
        [JsonProperty(PropertyName = "del_action")]
        public int DelAction { get; set; }
        [JsonProperty(PropertyName = "state_pos_brush")]
        public int StatePosBrush { get; set; }
        [JsonProperty(PropertyName = "state_pos_thumb")]
        public int StatePosThumb { get; set; }

        public Action() { }

        public static Action GetDefault()
        {
            Action result = new Action()
            {
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

    public class InfoCommand : ICloneable
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

        public InfoCommand() { }

        public static InfoCommand GetDefault()
        {
            InfoCommand result = new InfoCommand()
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
