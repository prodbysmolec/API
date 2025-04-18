using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Client.Core.Services.Auth;
// Basic implementation - for production use a secure platform-specific implementation
public class SecureStorageImplementation : ISecureStorage
{
    private readonly string _storageFilePath;
    private readonly Dictionary<string, string> _secureData = new();
    private readonly byte[] _encryptionKey;
    
    public SecureStorageImplementation(string appName)
    {
        var dataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
            appName);
            
        if (!Directory.Exists(dataFolder))
        {
            Directory.CreateDirectory(dataFolder);
        }
        
        _storageFilePath = Path.Combine(dataFolder, "securestorage.bin");
        
        // In a real implementation, use a proper key management solution
        // This is just a simple example, not secure for production
        _encryptionKey = Encoding.UTF8.GetBytes("YourSecretKey1234567890123456"); // 32 bytes for AES-256
        
        LoadDataAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }

    public Task<string> GetAsync(string key)
    {
        _secureData.TryGetValue(key, out var value);
        return Task.FromResult(value ?? string.Empty);
    }

    public async Task SetAsync(string key, string value)
    {
        _secureData[key] = value;
        await SaveDataAsync();
    }

    public async Task RemoveAsync(string key)
    {
        if (_secureData.ContainsKey(key))
        {
            _secureData.Remove(key);
            await SaveDataAsync();
        }
    }

    public Task<bool> ContainsKeyAsync(string key)
    {
        return Task.FromResult(_secureData.ContainsKey(key));
    }
    
    private async Task LoadDataAsync()
    {
        if (!File.Exists(_storageFilePath))
            return;

        try
        {
            var encryptedData = await File.ReadAllBytesAsync(_storageFilePath);
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
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading secure storage: {ex.Message}");
        }
    }

    private async Task SaveDataAsync()
    {
        try
        {
            var json = JsonSerializer.Serialize(_secureData);
            var dataToEncrypt = Encoding.UTF8.GetBytes(json);
            var encryptedData = EncryptData(dataToEncrypt);

            await File.WriteAllBytesAsync(_storageFilePath, encryptedData);
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
        
        var iv = new byte[16]; // AES block size is 16 bytes
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
