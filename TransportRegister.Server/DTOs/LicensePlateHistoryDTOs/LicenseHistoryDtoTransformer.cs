using TransportRegister.Server.Models;

namespace TransportRegister.Server.DTOs.LicensePlateHistoryDTOs
{
    public class LicenseHistoryDtoTransformer
    {
        public static List<LicensePlateHistory> ConvertDtoToLicensePlateHistory(List<LicensePlateHistoryDto> dtos)
        {
            var result = new List<LicensePlateHistory>();
            foreach (var dto in dtos)
            {
                var history = new LicensePlateHistory
                {
                    LicensePlateHistoryId = dto.LicensePlateHistoryId,
                    LicensePlate = dto.LicensePlate,
                    ChangedOn = dto.ChangedOn,
                    VehicleId = dto.VehicleId
                };
                result.Add(history);
            }
            return result;
        }

    }
}
