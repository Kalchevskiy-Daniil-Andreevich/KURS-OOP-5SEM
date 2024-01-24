using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationWPF.Models
{
	public partial class ABONEMENT_REQUEST
	{
		[Key]
		public int ID_REQUEST { get; set; }
		public int ID_CLIENT { get; set; }
		public int ID_ABONEMENT { get; set; }
		public string TYPE_OF_TRAINING { get; set; }
		public string TYPE_OF_SERVICE { get; set; }
		public Nullable<System.DateTime> DATA_REQUEST { get; set; }

		[ForeignKey("ID_ABONEMENT")]
		public virtual ABONEMENTS ABONEMENTS { get; set; }
		[ForeignKey("ID_CLIENT")]
		public virtual CLIENTS CLIENTS { get; set; }
	}
}
