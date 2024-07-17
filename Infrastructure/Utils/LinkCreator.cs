using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
	public class LinkCreator: ILinkCreator
	{
		private readonly IConfiguration _configuration;

		public LinkCreator(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string CreateLink(string path)
		{
			return _configuration.GetSection("JwtSettings:Audience").Value + path;
		}
	}
}
