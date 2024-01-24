using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationWPF.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SHEDULE_TABLE
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public SHEDULE_TABLE()
		{
			this.ABONEMENTS = new HashSet<ABONEMENTS>();
		}
		[Key]
		public int ID_SHEDULE { get; set; }
		public string MONDAY { get; set; }
		public string TUESDAY { get; set; }
		public string WEDNRSDAY { get; set; }
		public string THURSDAY { get; set; }
		public string FRIDAY { get; set; }
		public string SATURDAY { get; set; }
		public string SUNDAY { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<ABONEMENTS> ABONEMENTS { get; set; }
	}
}
