using System.Security.Cryptography;

namespace TaskManager.Application.Helpers
{
    public static class PasswordHasher
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            ArgumentNullException.ThrowIfNull(password);
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            }

            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            ArgumentNullException.ThrowIfNull(password);
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            }
            if (passwordHash.Length != 64)
            {
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(passwordHash));
            }
            if (passwordSalt.Length != 128)
            {
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", nameof(passwordSalt));
            }

            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int index = 0; index < computedHash.Length; index++)
            {
                if (computedHash[index] != passwordHash[index])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ValidatePassword(string password, out string errorMessage)
        {
            var validations = new List<KeyValuePair<bool, string>>
            {
                new (string.IsNullOrWhiteSpace(password), "Password cannot be empty."),
                new (password.Length < 8 || password.Length > 100, "Password must be between 8 and 100 characters."),
                new (!password.Any(char.IsLower), "Password must contain at least one lowercase letter."),
                new (!password.Any(char.IsUpper), "Password must contain at least one uppercase letter."),
                new (!password.Any(char.IsDigit), "Password must contain at least one number."),
                new (!password.Any(ch => !char.IsLetterOrDigit(ch)), "Password must contain at least one special character.")
            };

            var firstIssue = validations.FirstOrDefault(x => x.Key);

            errorMessage = (firstIssue.Key) ? firstIssue.Value : string.Empty;
            return string.IsNullOrEmpty(errorMessage);
        }
    }
}
