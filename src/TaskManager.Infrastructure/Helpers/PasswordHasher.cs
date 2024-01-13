using System.Security.Cryptography;

namespace TaskManager.Infrastructure.Helpers
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
            if (string.IsNullOrEmpty(password))
            {
                errorMessage = "Password cannot be null or empty.";
                return false;
            }

            bool hasLower = password.Any(char.IsLower);
            bool hasUpper = password.Any(char.IsUpper);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            if (password.Length < 8 || password.Length > 100)
            {
                errorMessage = "Password must be between 8 and 100 characters.";
            }
            else if (!hasLower)
            {
                errorMessage = "Password must contain at least one lowercase letter.";
            }
            else if (!hasUpper)
            {
                errorMessage = "Password must contain at least one uppercase letter.";
            }
            else if (!hasDigit)
            {
                errorMessage = "Password must contain at least one number.";
            }
            else if (!hasSpecial)
            {
                errorMessage = "Password must contain at least one special character.";
            }
            else
            {
                errorMessage = string.Empty; // All validations passed
                return true;
            }

            return false; // Validation failed
        }
    }
}
