using System.Security.Cryptography;
using System.Text;

namespace BudgetRequests.Data;

public static class Hash
{
    public static string ComputeHash(this string plainText, byte[] salt)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        var plainTextWithSaltBytes = new byte[plainTextBytes.Length + salt.Length];
        plainTextBytes.CopyTo(plainTextWithSaltBytes, 0);
        salt.CopyTo(plainTextWithSaltBytes, plainTextBytes.Length);
        
        var hashBytes = SHA256.HashData(plainTextWithSaltBytes);

        var hashValue = Convert.ToBase64String(hashBytes);
        return hashValue;
    }

    public static byte[] GenerateSalt(int length = 16)
    {
        var salt = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }
}