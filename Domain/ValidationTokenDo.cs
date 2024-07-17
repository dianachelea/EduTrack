using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
	public class ValidationTokenDo
	{
		public string userEmail { get; set; }
		public string token { get; set; }
		public DateTime expirationDate { get; set; }
	}
}
