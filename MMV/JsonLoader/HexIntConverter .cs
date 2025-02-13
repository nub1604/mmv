using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


   public class HexIntConverter<T> : JsonConverter<T> where T : struct, IConvertible 
{
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string hexString = reader.GetString()!;
        var value = Convert.ToUInt64(hexString, 16); // Hexadezimal in Ganzzahl umwandeln

        return (T)Convert.ChangeType(value, typeof(T)); // In den gewünschten Typ konvertieren
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        ulong num = Convert.ToUInt64(value); // Wert in `ulong` umwandeln für Formatierung
        writer.WriteStringValue($"0x{num:X}"); // Hexadezimal als String mit "0x" ausgeben
    }
}

