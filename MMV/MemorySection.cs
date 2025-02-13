
using Raylib_cs;

namespace memoryMapViewer
{
    public class MemorySection

    {
        private static int counter;

        private static readonly Color[] colors =

 [
    new Color(0xF2, 0xC0, 0xA2), // #F2C0A2
    new Color(0xE9, 0x84, 0x72), // #E98472
    new Color(0xD8, 0x23, 0x23), // #D82323
    new Color(0x98, 0x18, 0x3C), // #98183C
    new Color(0x1F, 0xCB, 0x23), // #1FCB23
    new Color(0x12, 0x6D, 0x30), // #126D30
    new Color(0x26, 0xDD, 0xDD), // #26DDDD
    new Color(0x18, 0x67, 0xA0), // #1867A0
    new Color(0x93, 0x42, 0x26), // #934226
    new Color(0x6C, 0x25, 0x1E), // #6C251E
    new Color(0xF7, 0xE2, 0x6C), // #F7E26C
    new Color(0xED, 0xB3, 0x29), // #EDB329
    new Color(0xE7, 0x6D, 0x14), // #E76D14
    new Color(0xF2, 0xF2, 0xF9), // #F2F2F9
    new Color(0x6A, 0x5F, 0xA0), // #6A5FA0
    new Color(0x16, 0x14, 0x23)  // #161423
 ];

        public MemorySection()
        {
            counter++;
            var c = colors[counter % colors.Length];
            Col = new Color(c.R, c.G, c.B, (byte)110);
        }
      
        public int Address { get; set; }
      
        public int Bank { get; set; }
        public string Label { get; set; } = "";
    
        public int Size { get; set; }

        public Color Col { get; init; }
    }
    public static class MemorySectionExtensions
    {
        public static async Task<MemorySection[]> LoadData(IEnumerable<string> lines, Action<int, int> callback)
        {
          return await Task.Run(() =>
            {
                int mode = 0;
                var tempSections = new List<MemorySection>();

                int counter = 0;
                int allCount = lines.Count();
                foreach (var line in lines)
                {
                    counter++;
                   if (counter % 1000 == 0)
                    callback(counter, allCount);

                    if (line.StartsWith('[')) // Faster than `Contains`
                    {
                        mode = line switch
                        {
                            "[labels]" => 1,
                            "[definitions]" => 2,
                            _ => 0,
                        };
                        continue;
                    }

                    var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (split.Length != 2) continue;

                    if (mode == 1)
                    {
                        if (split[1].Contains("__local_")) continue;

                        // Use Span<char> for efficient slicing
                        var span = split[0].AsSpan();
                        var bHex = span[..2];
                        var aHex = span.Slice(2, 4);

                        var section = new MemorySection
                        {
                            Bank = byte.Parse(bHex, System.Globalization.NumberStyles.HexNumber),
                            Address = ushort.Parse(aHex, System.Globalization.NumberStyles.HexNumber),
                            Label = split[1],
                            Size = 1
                        };

                        tempSections.Add(section);
                       
                    }
                    else if (mode == 2)
                    {

                        if (tempSections.FirstOrDefault(x => $"_sizeof_{x.Label}" == split[1]) is { } item)
                        {
                            item.Size = int.Parse(split[0], System.Globalization.NumberStyles.HexNumber);
                        }
                       
                    }
                }

                tempSections.Sort((a, b) => a.Bank != b.Bank ? a.Bank.CompareTo(b.Bank) : a.Address.CompareTo(b.Address));

                return tempSections.ToArray();
            });
        }

    }
}