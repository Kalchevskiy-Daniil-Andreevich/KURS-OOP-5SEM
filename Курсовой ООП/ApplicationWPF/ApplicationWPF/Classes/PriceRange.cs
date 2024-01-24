using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationWPF.Classes
{
    class PriceRange
    {
		public int MinPrice { get; set; }
		public int MaxPrice { get; set; }

		public string DisplayName => $"{MinPrice} - {MaxPrice}";

		public PriceRange(int minPrice, int maxPrice)
		{
			MinPrice = minPrice;
			MaxPrice = maxPrice;
		}
	}
}
