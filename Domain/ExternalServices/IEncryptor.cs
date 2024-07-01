namespace AllRiskSolutions_Desafio.Domain.ExternalServices;

public interface IEncryptor
{
    Task<string> EncryptAsync(string value, string salt);

    Task<bool> CompareAsync(string text, string encryptedText);

    Task<string> GenerateSaltAsync(int rounds);
}