using AllRiskSolutions_Desafio.Domain.ExternalServices;
using BCrypt.Net;
using static BCrypt.Net.BCrypt;

namespace AllRiskSolutions_Desafio.Infrastructure.Services;

public class EncryptorBcrypt : IEncryptor
{
    public Task<string> EncryptAsync(string value, string salt)
    {
        return Task.FromResult(HashPassword(value, salt, false, HashType.SHA256));
    }

    public Task<bool> CompareAsync(string text, string encryptedText)
    {
        return Task.FromResult(Verify(text, encryptedText));
    }

    public Task<string> GenerateSaltAsync(int rounds)
    {
        return Task.Run(() => GenerateSalt(rounds));
    }
}