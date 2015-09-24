using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class StringGen
{
	public static String GenString()
	{
        String chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random random = new Random();
        return new string(
            Enumerable.Repeat(chars, 8)
                      .Select(s => s[random.Next(s.Length)])
                      .ToArray());
	}
}