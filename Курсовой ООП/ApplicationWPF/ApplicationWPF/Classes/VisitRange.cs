using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationWPF.Classes
{
    class VisitRange
    {
		public int MinAmountOfVisits { get; set; }
		public int MaxAmountOfVisits { get; set; }

		public VisitRange(int minAmountOfVisits, int maxAmountOfVisits)
		{
			MinAmountOfVisits = minAmountOfVisits;
			MaxAmountOfVisits = maxAmountOfVisits;
		}
	}
}
