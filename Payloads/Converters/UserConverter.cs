using System.Security.Cryptography;
using Writing.Configurations;
using Writing.Entities;
using Writing.Enumerates;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;

namespace Writing.Payloads.Converters; 

public class UserConverter {

    private readonly SecurityConfiguration securityConfiguration;

    public UserConverter(SecurityConfiguration configuration) {
        securityConfiguration = configuration;
    }

    public User regRequestToEntity(RegisterRequest request) {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        return new User {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = securityConfiguration.encodePassword(request.Password, salt),
            Salt = salt
        };
    }

    public UserDTO entityToDto(User entity) {
        return new UserDTO {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            About = entity.About,
            Gender = entity.Gender,
            DateOfBirth = entity.DateOfBirth,
            AvatarPhoto = entity.AvatarPhoto,
            CoverPhoto = entity.CoverPhoto
        };
    }

    public void updateToEntity(User entity, UserUpdateRequest request) {
        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.About = request.About;
        Gender[] genders = (Gender[]) Enum.GetValues(typeof(Gender));
        if (genders.ToList().Select(gender => gender.ToString()).ToList().Contains(request.Gender)) {
            entity.Gender = request.Gender;
        }
        entity.DateOfBirth = request.DateOfBirth;
    }
}