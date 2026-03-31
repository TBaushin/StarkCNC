using StarkCNC.Utilities.Tests.Dto;

namespace StarkCNC.Utilities.Tests;

public class CryptographyTest
{
    [Fact]
    public void EncryptTest()
    {
        // Arrange
        var data = new { Name = "HelloWorld!", Age = 50, Password = "Qwerty12345!" };

        // Act
        var result = StarkCNC.Utilities.Cryptography.Encrypt(data);

        // Assert
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task EncryptAsyncTest()
    {
        // Arrange
        var data = new { Name = "HelloWorld!", Age = 50, Password = "Qwerty12345!" };

        // Act
        var result = await StarkCNC.Utilities.Cryptography.EncryptAsync(data).ConfigureAwait(true);

        // Assert
        Assert.NotEmpty(result);
    }

    [Fact]
    public void CanDecryptEncryptedTest()
    {
        // Arrange
        var data = new UserDto() { Name = "HelloWorld!", Age = 50, Password = "Qwerty12345!" };
        var encrypted = StarkCNC.Utilities.Cryptography.Encrypt(data);

        // Act
        var result = StarkCNC.Utilities.Cryptography.Decrypt<UserDto>(encrypted);

        // Assert
        Assert.Equivalent(data, result);
    }

    [Fact]
    public async Task CanDecryptEncryptedAsyncTest()
    {
        // Arrange
        var data = new UserDto() { Name = "HelloWorld!", Age = 50, Password = "Qwerty12345!" };
        var encrypted = await StarkCNC.Utilities.Cryptography.EncryptAsync(data).ConfigureAwait(true);

        // Act
        var result = await StarkCNC.Utilities.Cryptography.DecryptAsync<UserDto>(encrypted).ConfigureAwait(true);

        // Assert
        Assert.Equivalent(data, result);
    }
}
