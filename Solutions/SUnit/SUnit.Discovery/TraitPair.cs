using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SUnit.Discovery
{
    [Serializable]
    internal struct TraitPair : IEquatable<TraitPair>
    {
        private static readonly Regex preamble = new Regex(
            @"(?<nameLength>[0-9]+),(?<valueLength>[0-9]+):");

        public TraitPair(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; }
        public string Value { get; }

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }

        /// <summary>
        /// Gets the length of the string that the TraitPair is serialized to.
        /// </summary>
        public int SerializedLength => SaveToText().Length;

        public static bool Equals(TraitPair left, TraitPair right)
        {
            return left.Name == right.Name &&
                left.Value == right.Value;
        }
        public static bool operator ==(TraitPair left, TraitPair right) => Equals(left, right);
        public static bool operator !=(TraitPair left, TraitPair right) => !Equals(left, right);
        public bool Equals(TraitPair other) => Equals(this, other);
        public override bool Equals(object obj)
        {
            return obj is TraitPair pair && Equals(this, pair);
        }
        public override int GetHashCode() => (Name, Value).GetHashCode();

        public static TraitPair Parse(string text, int startAt)
        {
            Match match = preamble.Match(text, startAt);

            if (!match.Success)
                throw new FormatException("String is not a valid TraitPair.");
            if (match.Index != startAt)
                throw new FormatException("TraitPair preamble match did not start at the indicated index.");

            int nameLength = int.Parse(match.Groups["nameLength"].Value, CultureInfo.InvariantCulture);
            int valueLength = int.Parse(match.Groups["valueLength"].Value, CultureInfo.InvariantCulture);

            int index = match.Index + match.Length;
            string name = text.Substring(index, nameLength);
            string value = text.Substring(index + nameLength, valueLength);

            return new TraitPair(name, value);
        }

        public static TraitPair Parse(string text) => Parse(text, 0);

        public static IEnumerable<TraitPair> ParseAll(string text, int startAt)
        {
            if (startAt < 0)
                throw new ArgumentOutOfRangeException(nameof(startAt), "Argument can't be negative.");

            static IEnumerable<TraitPair> iterator(string text, int startAt)
            {
                int index = startAt;
                while (index < text.Length)
                {
                    TraitPair pair = Parse(text, index);
                    index += pair.SerializedLength;

                    yield return pair;
                }
            }

            return iterator(text, startAt);
        }

        public static IEnumerable<TraitPair> ParseAll(string text) => ParseAll(text, 0);

        /// <summary>
        /// Saves the <see cref="TraitPair"/> as a string.
        /// </summary>
        /// <returns>The round-trippable string representation for the current <see cref="TraitPair"/>.</returns>
        public string SaveToText() => $"{Name.Length},{Value.Length}:{Name}{Value}";

        public static string SaveAll(IEnumerable<TraitPair> pairs)
        {
            if (pairs is null) throw new ArgumentNullException(nameof(pairs));

            var sb = new StringBuilder();
            foreach (var pair in pairs)
                sb.Append(pair.SaveToText());

            return sb.ToString();
        }
    }
}
