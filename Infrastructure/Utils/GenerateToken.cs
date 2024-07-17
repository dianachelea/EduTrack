using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
	public class GenerateToken : IGenerateToken
	{
		string IGenerateToken.GenerateToken(int length)
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				byte[] tokenData = new byte[length];
				rng.GetBytes(tokenData);

				return Convert.ToBase64String(tokenData)
							.Replace('+', '-')
							.Replace('/', '_')
							.Replace("=", string.Empty);
			}
		}
	}
}
