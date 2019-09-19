using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManager3
{
    public class Analytic : IComparable<Analytic>
    {
        public DateTime Date { get; set; }
        public decimal CalculatedAnalytic { get; set; }

        public int CompareTo(Analytic other)
        {
            return this.CalculatedAnalytic.CompareTo(other.CalculatedAnalytic);
        }
    }
}
