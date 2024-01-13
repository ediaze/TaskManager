using TaskManager.Infrastructure.Helpers;

namespace TaskManager.UnitTests
{
    public class PasswordHasherTests
    {
        [Fact]
        public void CreatePasswordHash_GeneratesValidHashAndSalt()
        {
            // Arrange
            var password = "TestPassword123!";

            // Act
            PasswordHasher.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            // Assert
            Assert.NotNull(passwordHash);
            Assert.Equal(64, passwordHash.Length); // HMACSHA512 produces a 64-byte hash
            Assert.NotNull(passwordSalt);
            Assert.Equal(128, passwordSalt.Length); // HMACSHA512 has a 128-byte key
        }

        [Fact]
        public void VerifyPasswordHash_ReturnsTrueForValidPassword()
        {
            // Arrange
            var password = "TestPassword123!";
            PasswordHasher.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            // Act
            var isValid = PasswordHasher.VerifyPasswordHash(password, passwordHash, passwordSalt);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void VerifyPasswordHash_ReturnsFalseForInvalidPassword()
        {
            // Arrange
            var password = "TestPassword123!";
            var wrongPassword = "WrongPassword123!";
            PasswordHasher.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            // Act
            var isValid = PasswordHasher.VerifyPasswordHash(wrongPassword, passwordHash, passwordSalt);

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ValidatePassword_ReturnsFalseForInvalidInput(string password)
        {
            // Act
            var isValid = PasswordHasher.ValidatePassword(password, out var errorMessage);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(errorMessage);
        }
    }
}