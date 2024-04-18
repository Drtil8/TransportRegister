using System.Text.Json;
using System.Text.Json.Serialization;
using TransportRegister.Server.DTOs.VehicleDTOs;

namespace TransportRegister.Server.DTOs._Convertors
{
    public class VehicleDtoConverter : JsonConverter<VehicleDto>
    {
        public override VehicleDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader);
            JsonElement vehicleTypeProp = jsonDocument.RootElement.GetProperty("vehicleType");

            if (vehicleTypeProp.ValueKind == JsonValueKind.Undefined)
            {
                throw new JsonException("vehicleType is required.");
            }

            string vehicleType = vehicleTypeProp.GetString();
            string vehicleJson = jsonDocument.RootElement.GetRawText();

            return vehicleType switch
            {
                "Car" => JsonSerializer.Deserialize<CarDto>(vehicleJson, options),
                "Motorcycle" => JsonSerializer.Deserialize<MotorcycleDto>(vehicleJson, options),
                "Bus" => JsonSerializer.Deserialize<BusDto>(vehicleJson, options),
                "Truck" => JsonSerializer.Deserialize<TruckDto>(vehicleJson, options),
                _ => throw new JsonException($"Unsupported vehicle type: {vehicleType}")
            };
        }

        public override void Write(Utf8JsonWriter writer, VehicleDto value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }

}
