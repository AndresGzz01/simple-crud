using Konscious.Security.Cryptography;

using Microsoft.Extensions.Options;

using simple_crud.Api.Models;

using System.Security.Cryptography;

namespace simple_crud.Api.Infrastructure;

public class Argon2Hasher : IPasswordHasher
{
    private readonly Argon2Options _options;

    public Argon2Hasher(IOptionsSnapshot<Argon2Options> options)
    {
        _options = options.Value;
    }

    public string HashPassword(string password)
    {
        // Generar salt aleatoria
        byte[] salt = new byte[_options.SaltSize];
        RandomNumberGenerator.Fill(salt);

        // Configurar Argon2id
        var argon2 = new Argon2id(System.Text.Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            MemorySize = _options.MemorySize,
            Iterations = _options.Iterations,
            DegreeOfParallelism = _options.Parallelism
        };

        // Generar hash
        byte[] hashBytes = argon2.GetBytes(32); // tamaño típico: 256 bits

        // Combinar salt + hash en un solo string Base64
        byte[] result = new byte[salt.Length + hashBytes.Length];
        Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
        Buffer.BlockCopy(hashBytes, 0, result, salt.Length, hashBytes.Length);

        return Convert.ToBase64String(result);
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        byte[] hashBytes = Convert.FromBase64String(hashedPassword);

        // Extraer salt y hash
        byte[] salt = new byte[_options.SaltSize];
        byte[] originalHash = new byte[hashBytes.Length - _options.SaltSize];
        Buffer.BlockCopy(hashBytes, 0, salt, 0, _options.SaltSize);
        Buffer.BlockCopy(hashBytes, _options.SaltSize, originalHash, 0, originalHash.Length);

        // Recalcular hash con la misma salt
        var argon2 = new Argon2id(System.Text.Encoding.UTF8.GetBytes(providedPassword))
        {
            Salt = salt,
            MemorySize = _options.MemorySize,
            Iterations = _options.Iterations,
            DegreeOfParallelism = _options.Parallelism
        };

        byte[] newHash = argon2.GetBytes(originalHash.Length);

        // Comparar hashes de forma segura
        return CryptographicOperations.FixedTimeEquals(originalHash, newHash);
    }
}
