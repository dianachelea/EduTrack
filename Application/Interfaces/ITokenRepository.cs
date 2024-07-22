using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
	public interface ITokenRepository
	{
		Task<bool> AddToken(ValidationTokenDo token);
		Task<IEnumerable<ValidationTokenDo>> GetToken(string token);
		Task<bool> DeleteToken(string token);
	}
}
