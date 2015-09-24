using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

public class HashGen
{
    public static string CreateSalt(int size)
    {
        // Taken from: http://stackoverflow.com/questions/2138429/hash-and-salt-passwords-in-c-sharp

        //Generate a cryptographic random number.
        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        byte[] buff = new byte[size];
        rng.GetBytes(buff);

        // Return a Base64 string representation of the random number.
        return Convert.ToBase64String(buff);
    }
    public static string CreateSHAHash(string Password, string Salt)
    {
        System.Security.Cryptography.SHA512Managed HashTool = new System.Security.Cryptography.SHA512Managed();
        Byte[] PasswordAsByte = System.Text.Encoding.UTF8.GetBytes(string.Concat(Password, Salt));
        Byte[] EncryptedBytes = HashTool.ComputeHash(PasswordAsByte);
        HashTool.Clear();
        return Convert.ToBase64String(EncryptedBytes);
    }
}