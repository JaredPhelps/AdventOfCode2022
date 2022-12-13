using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace AdventOfCode.Days
{
    public class Day13Part1
    {
        public int Solve(string input)
        {
            using var sr = new StringReader(input);
            string? line = null;
            int sum = 0;
            int index = 1;
            while ((line = sr.ReadLine()) != null)
            {
                string nextLine = sr.ReadLine()!;
                _ = sr.ReadLine();

                var leftPacket = ParsePacket(line);
                var rightPacket = ParsePacket(nextLine);
                if (leftPacket.CompareTo(rightPacket) <= 0)
                    sum+= index;
                index++;
            }

            return sum;
        }

        public int SolvePart2(string input)
        {
            using var sr = new StringReader(input);
            string? line = null;
            List<MixedPacket> packets = new List<MixedPacket>();
            while ((line = sr.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                var packet = ParsePacket(line);
                packets.Add(packet);
            }

            var dividerPacket1 = ParsePacket("[[2]]");
            var dividerPacket2 = ParsePacket("[[6]]");
            packets.Add(dividerPacket1);
            packets.Add(dividerPacket2);
            packets.Sort((x, y) => x.CompareTo(y));

            return (packets.IndexOf(dividerPacket1) + 1) * (packets.IndexOf(dividerPacket2) + 1);
        }

        private MixedPacket ParsePacket(string line) => GetPart(JsonConvert.DeserializeObject<dynamic>(line));

        private MixedPacket GetPart(dynamic parsed)
        {
            List<IPacketPart> parts = new List<IPacketPart>();
            for (int i = 0; i < parsed!.Count; i++)
            {
                if (parsed[i].Type == JTokenType.Integer)
                {
                    int partValue = parsed[i];
                    parts.Add(new PrimitivePacket(partValue));
                }
                else
                {
                    IPacketPart mixedPart = GetPart(parsed[i]);
                    parts.Add(mixedPart);
                }
            }
            return new MixedPacket(parts);
        }
    }

    public interface IPacketPart
    {
        int CompareTo(IPacketPart other);
    }

    public record PrimitivePacket : IPacketPart
    {
        public int Value { get; set; }

        public PrimitivePacket(int value)
        {
            Value = value;
        }

        public int CompareTo(IPacketPart other)
        {
            if (other is PrimitivePacket p)
                return Value - p.Value;
            else
                return new MixedPacket(new List<IPacketPart> { this }).CompareTo(other);
        }
    }

    public record MixedPacket : IPacketPart
    {
        public List<IPacketPart> Parts { get; set; }
        public MixedPacket(List<IPacketPart> parts)
        {
            this.Parts = parts;
        }

        public int CompareTo(IPacketPart other)
        {
            if (other is PrimitivePacket p)
            {
                return CompareTo(new MixedPacket(new List<IPacketPart> { p }));
            }
            var mp = (MixedPacket)other;
            for (int i = 0; i < Math.Min(mp.Parts.Count, this.Parts.Count); i++)
            {
                int result = Parts[i].CompareTo(mp.Parts[i]);
                if (result != 0)
                    return result;
            }
            return this.Parts.Count - mp.Parts.Count;
        }
    }
}

