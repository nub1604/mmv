using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Raylib_cs;

public class ColorHexConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        
        string ? hex = reader.GetString();
        if (hex is null || !Regex.IsMatch(hex, "#([0-9A-Fa-f]{6})([0-9A-Fa-f]{2})?$") || hex[0] != '#') // Expecting #RRGGBBAA
            return new Color(255, 255, 255);

        byte r = Convert.ToByte(hex.Substring(1, 2), 16);
        byte g = Convert.ToByte(hex.Substring(3, 2), 16);
        byte b = Convert.ToByte(hex.Substring(5, 2), 16);
        byte a = 255;
        if (hex.Length == 9)
        a= Convert.ToByte(hex.Substring(7, 2), 16);
        return new Color(r, g, b, a);
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        string hex = $"#{value.R:X2}{value.G:X2}{value.B:X2}{value.A:X2}";
        writer.WriteStringValue(hex);
    }
}
