using TransportRegister.Server.Models;
namespace TransportRegister.Server.DTOs.PersonDTOs
{
    public class PersonDtoTransformer
    {
        public static PersonDto TransformToDto(Person person)
        {
            return person switch
            {
                Driver driver => new DriverDto
                {
                    AddressDto = TransformToDto(driver.Address),
                    PersonId = driver.PersonId,
                    FirstName = driver.FirstName,
                    LastName = driver.LastName,
                    BirthNumber = driver.BirthNumber,
                },

                Owner owner => new OwnerDto
                {
                    PersonId = owner.PersonId,
                    FirstName = owner.FirstName,
                    LastName = owner.LastName,
                    BirthNumber = owner.BirthNumber,
                },
                _ => null
            };
        }

        public static AddressDto TransformToDto(Address adress)
        {
            return new AddressDto
            {
                Street = adress.Street,
                City = adress.City,
                State = adress.State,
                Country = adress.Country,
                HouseNumber = adress.HouseNumber,
                PostalCode = adress.PostalCode
            };

        }

        // potrebuje dva konvertre???
        public static Person TransformToEntity(PersonDto dto)
        {
            return dto switch
            {
                DriverDto driverDto => new Driver
                {
                    Address = TransformToEntity(driverDto.AddressDto),
                    PersonId = driverDto.PersonId,
                    FirstName = driverDto.FirstName,
                    LastName = driverDto.LastName,
                    BirthNumber = driverDto.BirthNumber,
                },

                OwnerDto ownerDto => new Owner
                {
                    Address = TransformToEntity(ownerDto.AddressDto),
                    PersonId = ownerDto.PersonId,
                    FirstName = ownerDto.FirstName,
                    LastName = ownerDto.LastName,
                    BirthNumber = ownerDto.BirthNumber,
                },
                _ => null
            };
        }

        public static Address TransformToEntity(AddressDto dto)
        {
            return new Address
            {
                Street = dto.Street,
                City = dto.City,
                State = dto.State,
                Country = dto.Country,
                HouseNumber = dto.HouseNumber,
                PostalCode = dto.PostalCode
            };
        }


    }
}
