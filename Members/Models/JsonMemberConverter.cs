using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WebApp.Members.Models;

class EnumCollectionJsonValueConverter : JsonConverter<IEnumerable<Role>>
{
    public override IEnumerable<Role> Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) => JsonSerializer
        .Deserialize<IEnumerable<Role>>(reader.GetString()!)
        .ToList();

    public override void Write(
        Utf8JsonWriter writer,
        IEnumerable<Role> roles,
        JsonSerializerOptions options) =>
        writer.WriteRawValue(JsonSerializer.Serialize(roles));
}
