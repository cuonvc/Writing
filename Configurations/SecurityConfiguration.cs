using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Writing.Entities;

namespace Writing.Configurations; 

public class SecurityConfiguration {

    public string encodePassword(string rawPassword, byte[] salt) {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: rawPassword,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100,
            numBytesRequested: 256 / 8
        ));
    }
}