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
    class CommandModel : ICloneable
    {

        [JsonProperty(PropertyName = "name_command")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "info_actions")]
        public InfoCommand infoCommand { get; set; }
        [JsonProperty(PropertyName = "data_actions")]
        public List<Action> dataAction { get; set; }


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

    class Action : ICloneable
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

    class InfoCommand : ICloneable
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
