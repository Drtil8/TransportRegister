using System.Text.Json;
using System.Text.Json.Serialization;
namespace TransportRegister.Server.DTOs.PersonDTOs
{
    public class PersonDtoConverter : JsonConverter<PersonDto>
    {
        public override PersonDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader);
            JsonElement personTypeProp = jsonDocument.RootElement.GetProperty("personType");

            if (personTypeProp.ValueKind == JsonValueKind.Undefined)
            {
                throw new JsonException("personType is required.");
            }

            string personType = personTypeProp.GetString();
            string personJson = jsonDocument.RootElement.GetRawText();

            return personType switch
            {
                "Person" => JsonSerializer.Deserialize<PersonDto>(personJson, options),
                "Driver" => JsonSerializer.Deserialize<DriverDto>(personJson, options),
                "Owner" => JsonSerializer.Deserialize<OwnerDto>(personJson, options),
                _ => throw new JsonException($"Unsupported person type: {personType}")
            };
        }

        public override void Write(Utf8JsonWriter writer, PersonDto value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }

    }
}
