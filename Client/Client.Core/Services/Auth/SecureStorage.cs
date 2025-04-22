using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Core.Services.Auth;

public class SecureStorageImplementation : ISecureStorage
{
    private readonly string _storageFilePath;
    private readonly Dictionary<string, string> _secureData = new();
    private readonly byte[] _encryptionKey;
    private bool _isInitialized = false;
    private readonly SemaphoreSlim _initLock = new SemaphoreSlim(1, 1);
    private readonly SemaphoreSlim _fileLock = new SemaphoreSlim(1, 1);

    public SecureStorageImplementation(string appName)
    {
        try
        {
            var dataFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                appName);

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            _storageFilePath = Path.Combine(dataFolder, "securestorage.bin");

            // Ensure key is exactly 32 bytes by using SHA256
            using var sha256 = SHA256.Create();
            _encryptionKey = sha256.ComputeHash(Encoding.UTF8.GetBytes("Your32ByteLongEncryptionKeyHere1"));

            // Don't load data in constructor - defer it to first use
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing secure storage: {ex.Message}");
            // Set defaults to prevent null reference exceptions
            _storageFilePath = string.Empty;
            _encryptionKey = Array.Empty<byte>();
        }
    }

    private async Task InitializeAsync()
    {
        // Use a semaphore to prevent multiple initialization attempts
        await _initLock.WaitAsync();
        try
        {
            if (!_isInitialized)
            {
                await LoadDataAsync();
                _isInitialized = true;
            }
        }
        finally
        {
            _initLock.Release();
        }
    }

    public async Task<string> GetAsync(string key)
    {
        await InitializeAsync();
        _secureData.TryGetValue(key, out var value);
        return value ?? string.Empty;
    }

    public async Task SetAsync(string key, string value)
    {
        await InitializeAsync();
        _secureData[key] = value;
        await SaveDataAsync();
    }

    public async Task RemoveAsync(string key)
    {
        await InitializeAsync();
        if (_secureData.ContainsKey(key))
        {
            _secureData.Remove(key);
            await SaveDataAsync();
        }
    }

    public async Task<bool> ContainsKeyAsync(string key)
    {
        await InitializeAsync();
        return _secureData.ContainsKey(key);
    }

    private async Task LoadDataAsync()
    {
        if (string.IsNullOrEmpty(_storageFilePath) || !File.Exists(_storageFilePath))
            return;

        try
        {
            // Use a timeout to prevent hanging on file operations
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            await _fileLock.WaitAsync(cts.Token);
            try
            {
                byte[] encryptedData;

                using (var fileStream = new FileStream(
                    _storageFilePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    4096,
                    true)) // Use async IO
                {
                    encryptedData = new byte[fileStream.Length];
                    await fileStream.ReadAsync(encryptedData, 0, (int)fileStream.Length, cts.Token);
                }

                if (encryptedData.Length > 0)
                {
                    var decryptedData = DecryptData(encryptedData);
                    var json = Encoding.UTF8.GetString(decryptedData);

                    var data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                    if (data != null)
                    {
                        _secureData.Clear();
                        foreach (var item in data)
                        {
                            _secureData[item.Key] = item.Value;
                        }
                    }
                }
            }
            finally
            {
                _fileLock.Release();
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Loading secure storage data timed out.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading secure storage: {ex.Message}");
            // Continue with empty storage
        }
    }

    private async Task SaveDataAsync()
    {
        if (string.IsNullOrEmpty(_storageFilePath))
            return;

        try
        {
            // Use a timeout to prevent hanging on file operations
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            var json = JsonSerializer.Serialize(_secureData);
            var dataToEncrypt = Encoding.UTF8.GetBytes(json);
            var encryptedData = EncryptData(dataToEncrypt);

            await _fileLock.WaitAsync(cts.Token);
            try
            {
                using (var fileStream = new FileStream(
                    _storageFilePath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None,
                    4096,
                    true)) // Use async IO
                {
                    await fileStream.WriteAsync(encryptedData, 0, encryptedData.Length, cts.Token);
                }
            }
            finally
            {
                _fileLock.Release();
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Saving secure storage data timed out.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving secure storage: {ex.Message}");
        }
    }

    private byte[] EncryptData(byte[] dataToEncrypt)
    {
        using Aes aes = Aes.Create();
        aes.Key = _encryptionKey;
        aes.GenerateIV();

        using var memoryStream = new MemoryStream();
        // Write IV to the beginning of the stream
        memoryStream.Write(aes.IV, 0, aes.IV.Length);

        using (var cryptoStream = new CryptoStream(
            memoryStream,
            aes.CreateEncryptor(),
            CryptoStreamMode.Write))
        {
            cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
            cryptoStream.FlushFinalBlock();
        }

        return memoryStream.ToArray();
    }

    private byte[] DecryptData(byte[] encryptedData)
    {
        using Aes aes = Aes.Create();
        aes.Key = _encryptionKey;

        // Get the IV from the beginning of the encrypted data
        var iv = new byte[16]; // AES block size is 16 bytes
        if (encryptedData.Length < iv.Length)
        {
            throw new InvalidOperationException("Encrypted data is too short to contain an IV.");
        }

        Array.Copy(encryptedData, 0, iv, 0, iv.Length);
        aes.IV = iv;

        using var memoryStream = new MemoryStream();

        using (var cryptoStream = new CryptoStream(
            memoryStream,
            aes.CreateDecryptor(),
            CryptoStreamMode.Write))
        {
            cryptoStream.Write(encryptedData, iv.Length, encryptedData.Length - iv.Length);
            cryptoStream.FlushFinalBlock();
        }

        return memoryStream.ToArray();
    }
}