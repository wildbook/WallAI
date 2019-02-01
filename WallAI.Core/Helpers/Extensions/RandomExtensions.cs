using System;
using System.Collections.Generic;
using System.Linq;

namespace WallAI.Core.Helpers.Extensions
{
    public static class RandomExtensions
    {
        public static T NextEnum<T>(this Random rand) where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            if (values.Length == 0)
                throw new IndexOutOfRangeException("The specified enum contains no values.");
            return (T)values.GetValue(rand.Next(0, values.Length));
        }

        public static T NextEnumerable<T>(this Random rand, IEnumerable<T> sequence)
        {
            var items = sequence.ToArray();
            return items[rand.Next(items.Length)];
        }

        public static Guid NextGuid(this Random rand)
        {
            var guid = new byte[16];
            rand.NextBytes(guid);

            return new Guid(guid);
        }

        public static T NextByWeight<T>(this Random random, IEnumerable<T> sequence, Func<T, float> weightSelector)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            if (weightSelector == null) throw new ArgumentNullException(nameof(weightSelector));

            var enumerable = sequence as T[] ?? sequence.ToArray();
            var totalWeight = enumerable.Sum(weightSelector);

            var itemWeightIndex = random.NextDouble() * totalWeight;
            var currentWeightIndex = 0f;

            var weightedItems = enumerable.Select(x => (Value: x, Weight: weightSelector(x)));

            foreach (var item in weightedItems)
            {
                currentWeightIndex += item.Weight;

                // If we've hit or passed the weight we are after for this item then it's the one we want....
                if (currentWeightIndex >= itemWeightIndex)
                    return item.Value;
            }

            return default;
        }
    }
}
