using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HandControl.Model.Dto;

namespace HandControl.Services.ProstheticServices
{
    public interface IMioPatternsService
    {
        Task<IEnumerable<MioPatternDto>> GetMioPatternsAsync();

        Task SetMioPatternsAsync(IEnumerable<MioPatternDto> mioPatterns);
    }

    public class MioPatternsService : IMioPatternsService
    {
        private readonly IProstheticConnector _prostheticConnector;
        private IEnumerable<MioPatternDto> _cache;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);

        public MioPatternsService(IProstheticConnector prostheticConnector)
        {
            _prostheticConnector = prostheticConnector ?? throw new ArgumentNullException(nameof(prostheticConnector));
        }

        public async Task<IEnumerable<MioPatternDto>> GetMioPatternsAsync()
        {
            await _semaphoreSlim.WaitAsync();
            if (_cache == null)
            {
                var result = await _prostheticConnector.GetMioPatternsAsync().ConfigureAwait(false);
                _cache = result.Patterns;
            }

            _semaphoreSlim.Release();

            return _cache;
        }

        public async Task SetMioPatternsAsync(IEnumerable<MioPatternDto> mioPatterns)
        {
            await _semaphoreSlim.WaitAsync();
            var mioPatternDto = mioPatterns.ToList();
            _cache = mioPatternDto;

            var setSettingsDto = new SetMioPatternsDto {Patterns = mioPatternDto};

            await _prostheticConnector.SetMioPatternsAsync(setSettingsDto).ConfigureAwait(false);

            _semaphoreSlim.Release();
        }
    }
}
