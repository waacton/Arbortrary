namespace Wacton.Arbortrary
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    internal static class Seed
    {
        public static (int value, string source) Get(int? seed, string text, string filepath)
        {
            if (seed.HasValue) return (seed.Value, "argument");
            if (!string.IsNullOrEmpty(text)) return (FromText(text), $"text \"{text}\"");
            if (!string.IsNullOrEmpty(filepath)) return (FromFile(filepath), $"file {Path.GetFileName(filepath)}");
            throw new InvalidOperationException("No seed available");
        }

        private static int FromText(string text)
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
            return BitConverter.ToInt32(hash);
        }

        private static int FromFile(string filepath)
        {
            using var sha = SHA256.Create();
            using var stream = File.OpenRead(filepath);
            var hash = sha.ComputeHash(stream);
            return BitConverter.ToInt32(hash);
        }
    }
}
