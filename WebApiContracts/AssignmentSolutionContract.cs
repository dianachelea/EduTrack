using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContracts
{
    public class AssignmentSolutionContract
    {
        public string Solution_title { get; set; }
        public string Solution { get; set; }
        public string FileName { get; set; }
    }
}
