using Raylib_cs;
using System.Text.Json.Serialization;

namespace memoryMapViewer
{
    public class LabeledSection<T>
    {
        public List<T> LayoutList { get; set; } = [];
        public string Name { get; set; } = "";
        [JsonConverter(typeof(ColorHexConverter))]
        public Color Color { get; set; }
}

   

}