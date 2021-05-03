using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HandControl.Model;
using HandControl.Model.Dto;
using HandControl.Services.ProstheticServices;
using Prism.Commands;

namespace HandControl.ViewModel
{
    public class MioPatternsSettingsViewModel: ViewModelBase
    {
        private readonly IMioPatternsService _mioPatternsService;

        public MioPatternsSettingsViewModel(IEnumerable<GestureModel> gestures, IMioPatternsService mioPatternsService)
        {
            _mioPatternsService = mioPatternsService ?? throw new ArgumentNullException(nameof(mioPatternsService));

            _ = InitializationAsync(gestures);
            SaveMioSettingsCommand = new DelegateCommand(SaveMioSettingsAsync);
        }

        public bool IsLoaded { get; private set; }

        public ObservableCollection<MioPatternSettings> MioSettings { get; private set; }

        public ObservableCollection<GestureModel> AllGestures { get; private set; }

        public ICommand SaveMioSettingsCommand { get; }

        private async Task InitializationAsync(IEnumerable<GestureModel> gestures)
        {
            var mioPatternsSettings = await _mioPatternsService.GetMioPatternsAsync();

            var settings = new ObservableCollection<MioPatternSettings>();

            AllGestures = new ObservableCollection<GestureModel>(gestures);

            foreach (var pattern in mioPatternsSettings)
            {
                var gesture = AllGestures.FirstOrDefault(x => x.Id == pattern.GestureId);
                var patternSettings = new MioPatternSettings()
                {
                    Pattern = pattern.Pattern,
                    Gesture = gesture
                };

                settings.Add(patternSettings);
            }

            MioSettings = settings;

            IsLoaded = true;
        }

        private void SaveMioSettingsAsync()
        {
            var settings = new List<MioPatternDto>();

            foreach (var setting in MioSettings)
            {
                settings.Add(new MioPatternDto
                {
                    Pattern = setting.Pattern,
                    GestureId = setting.Gesture != null ? setting.Gesture.Id : Guid.Empty
                });
            }

            _mioPatternsService.SetMioPatternsAsync(settings);
        }
    }

    public class MioPatternSettings : ViewModelBase
    {
        public int Pattern { get; set; }
        public GestureModel Gesture { get; set; }
    }
}
