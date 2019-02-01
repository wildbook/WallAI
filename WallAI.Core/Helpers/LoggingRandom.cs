using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using NLog;

namespace WallAI.Core.Helpers
{
    public class LoggingRandom : Random
    {
        private static Logger Log { get; } = LogManager.GetCurrentClassLogger();

        public LoggingRandom()         : base()     { LogValue("Initialized without seed."); }
        public LoggingRandom(int seed) : base(seed) { LogValue($"Initialized with seed: {seed}."); }
        
        public override int Next()                           => LogValue(base.Next());
        public override int Next(int maxValue)               => LogValue(base.Next(maxValue));
        public override int Next(int minValue, int maxValue) => LogValue(base.Next(minValue, maxValue));
        public override double NextDouble()                  => LogValue(base.NextDouble());

        public override void NextBytes(Span<byte> buffer)
        {
            base.NextBytes(buffer);

            var sb = new StringBuilder();
            sb.Append("[");
            foreach (var val in buffer)
                sb.Append(val.ToString("x2"));
            sb.Append("]");

            LogValue(sb.ToString());
        }

        public override void NextBytes(byte[] buffer)
        {
            base.NextBytes(buffer);
            LogValue($"[{string.Join(", ", buffer.Select(x => x.ToString("x2")))}]");
        }

        private static T LogValue<T>(T value, [CallerMemberName] string prefix = null)
        {
            Log.Debug($"{prefix}: {value}");
            return value;
        }
    }
}
