using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationWPF.Classes
{
    class AbonementFilter
    {
        public AbonementFilter(string displayName, AbonementSortingType sortingType)
        {
            DisplayName = displayName;
            SortingType = sortingType;
        }

        public string DisplayName { get; }

        public AbonementSortingType SortingType { get; }
        public override string ToString()
        {
            return DisplayName;
        }
    }
}
