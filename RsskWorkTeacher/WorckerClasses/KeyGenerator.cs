using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RsskWorkTeacher.WorckerClasses
{
    public static class KeyGenerator
    {
        private const string Alphabet =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string GenerateKey(int length = 16)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);

            return string.Create(length, Alphabet, static (span, alphabet) =>
            {
                for (int i = 0; i < span.Length; i++)
                    span[i] = alphabet[RandomNumberGenerator.GetInt32(alphabet.Length)];
            });
        }
    }
}
