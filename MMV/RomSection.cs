using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace memoryMapViewer
{
    public class RomSection
    {
        public string Label { get; set; } = "";

        [JsonConverter(typeof(HexIntConverter<byte>))]
        public byte BankStart { get; set; }

        [JsonConverter(typeof(HexIntConverter<byte>))]
        public byte BankEnd { get; set; }

        [JsonConverter(typeof(HexIntConverter<ushort>))]
        public ushort SectionStart { get; set; }

        [JsonConverter(typeof(HexIntConverter<ushort>))]
        public ushort SectionEnd { get; set; }

        [JsonConverter(typeof(ColorHexConverter))]
        public Color BlendColor { get; set; }
    }



}