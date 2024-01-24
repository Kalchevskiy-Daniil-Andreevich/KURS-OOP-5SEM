using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationWPF.Models
{
	public partial class CLIENTS
	{
		public CLIENTS()
		{
			this.ABONEMENT_REQUEST = new HashSet<ABONEMENT_REQUEST>();
			this.SALEOFABONEMENTS = new HashSet<SALEOFABONEMENTS>();
		}
		[Key]
		public int ID_CLIENT { get; set; }

		public int ID_LOGPAS { get; set; }
		public string FIRSTNAME_CLIENT { get; set; }
		public string LASTNAME_CLIENT { get; set; }
		public string MIDDLENAME_CLIENT { get; set; }
		public int AGE_CLIENT { get; set; }
		public string GENDER_CLIENT { get; set; }
		public string PHONENAME_CLIENT { get; set; }

		public virtual ICollection<ABONEMENT_REQUEST> ABONEMENT_REQUEST { get; set; }
		[ForeignKey("ID_LOGPAS")]
		public virtual LOGPAS LOGPAS { get; set; }
		public virtual ICollection<SALEOFABONEMENTS> SALEOFABONEMENTS { get; set; }
	}
}
