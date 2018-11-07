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
        public InfoCommand infoCommand { get; set; }
        [JsonProperty(PropertyName = "data_actions")]
        public List<Action> dataAction { get; set; }

        public byte[] BinaryDate
        {
            get {
                byte[] byteArray = new byte[20 + 12 + 4 + this.infoCommand.countCommand * 8];

                byte[] byteName = System.Text.Encoding.UTF8.GetBytes(this.Name);
                
                for (int i = 0; i < 20; i++)
                {
                    if (byteName.Length > i)
                        byteArray[i] = byteName[i];
                    else
                        byteArray[i] = Convert.ToByte('\0');
                }

                byte[] byteDate = System.Text.Encoding.UTF8.GetBytes(this.infoCommand.Date);
                for (int i = 0; i < 12; i++)
                {
                    byteArray[20 + i] = byteDate[i];
                }

                byteArray[32] = Convert.ToByte(this.infoCommand.combinedCommand);
                byteArray[33] = Convert.ToByte(this.infoCommand.iterableActions);
                byteArray[34] = Convert.ToByte(this.infoCommand.numActRep);
                byteArray[35] = Convert.ToByte(this.infoCommand.countCommand);

                for (int i = 0; i < this.infoCommand.countCommand; i++)
                {
                    int index = 36 + (i * 8);
                    byteArray[index] = Convert.ToByte(this.dataAction[i].littleFinger);
                    byteArray[index + 1] = Convert.ToByte(this.dataAction[i].ringFinder);
                    byteArray[index + 2] = Convert.ToByte(this.dataAction[i].middleFinger);
                    byteArray[index + 3] = Convert.ToByte(this.dataAction[i].pointerFinger);
                    byteArray[index + 4] = Convert.ToByte(this.dataAction[i].delAction);
                    byteArray[index + 5] = Convert.ToByte(this.dataAction[i].statePosThumb);
                    byteArray[index + 6] = Convert.ToByte(this.dataAction[i].statePosBrush);
                    byteArray[index + 7] = Convert.ToByte(this.dataAction[i].thumbFinger);

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
                infoCommand = (InfoCommand)this.infoCommand.Clone()
                // dataAction = (List<Action>)this.dataAction.Clone()
            };
        }
    }

    public class Action : ICloneable
    {
        [JsonProperty(PropertyName = "thumb_finger")]
        public int thumbFinger { get; set; }
        [JsonProperty(PropertyName = "pointer_finger")]
        public int pointerFinger { get; set; }
        [JsonProperty(PropertyName = "middle_finger")]
        public int middleFinger { get; set; }
        [JsonProperty(PropertyName = "ring_finder")]
        public int ringFinder { get; set; }
        [JsonProperty(PropertyName = "little_finger")]
        public int littleFinger { get; set; }
        [JsonProperty(PropertyName = "del_action")]
        public int delAction { get; set; }
        [JsonProperty(PropertyName = "state_pos_brush")]
        public int statePosBrush { get; set; }
        [JsonProperty(PropertyName = "state_pos_thumb")]
        public int statePosThumb { get; set; }

        public Action() { }

        public static Action GetDefault()
        {
            Action result = new Action()
            {
                thumbFinger = 0,
                pointerFinger = 0,
                middleFinger = 0,
                ringFinder = 0,
                littleFinger = 0,
                delAction = 0,
                statePosBrush = 0,
                statePosThumb = 0
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
        public bool iterableActions { get; set; }
        [JsonProperty(PropertyName = "combined")]
        public bool combinedCommand { get; set; }
        [JsonProperty(PropertyName = "num_act_rep")]
        public int numActRep { get; set; }
        [JsonProperty(PropertyName = "count_command")]
        public int countCommand { get; set; }
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        public InfoCommand() { }

        public static InfoCommand GetDefault()
        {
            InfoCommand result = new InfoCommand()
            {
                Date = "",
                iterableActions = false,
                combinedCommand = false,
                numActRep = 0,
                countCommand = 0
            };
            return result;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

}
