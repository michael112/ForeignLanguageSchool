using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class PasswordSaltAndHash
{

    private String PasswordSalt;
    private String PasswordHash;

	public PasswordSaltAndHash( String PasswordRaw )
	{
        this.PasswordSalt = HashGen.CreateSalt(16);
        this.PasswordHash = HashGen.CreateSHAHash(PasswordRaw, this.PasswordSalt);
	}

    public String getPasswordSalt()
    {
        return this.PasswordSalt;
    }
    public String getPasswordHash()
    {
        return this.PasswordHash;
    }
}