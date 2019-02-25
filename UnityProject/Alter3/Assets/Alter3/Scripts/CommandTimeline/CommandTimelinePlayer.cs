using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace XFlag.Alter3Simulator
{
    public class CommandTimelinePlayer
    {
        public event Action<MoveAxisCommand> OnCommand = delegate { };

        private CommandTimelineRecord[] _records;
        private Task _playTask;
        private CancellationTokenSource _cancellationTokenSource;

        public CommandTimelinePlayer(CommandTimelineRecord[] records)
        {
            _records = records;
            Array.Sort(_records, (a, b) => Comparer<long>.Default.Compare(a.MicroSeconds, b.MicroSeconds));
        }

        public void Start()
        {
            if (_playTask == null)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                _playTask = Task.Factory.StartNew(async () => await PlayAsync(_cancellationTokenSource.Token), TaskCreationOptions.LongRunning);
            }
        }

        public void Stop()
        {
            if (_playTask != null)
            {
                _cancellationTokenSource.Cancel();
                _playTask.Wait();

                _playTask = null;
                _cancellationTokenSource = null;
            }
        }

        private async Task PlayAsync(CancellationToken cancellationToken)
        {
            var baseTime = DateTime.Now.Ticks / 10;
            var index = 0;
            while (!cancellationToken.IsCancellationRequested && index < _records.Length)
            {
                var time = DateTime.Now.Ticks / 10 - baseTime;
                while (!cancellationToken.IsCancellationRequested && index < _records.Length && _records[index].MicroSeconds <= time)
                {
                    OnCommand(_records[index].ToMoveAxisCommand());
                    ++index;
                }
                await Task.Yield();
            }
        }
    }
}
