using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationWPF.Models
{
	public partial class ABONEMENTS
	{
		public ABONEMENTS()
		{
			this.ABONEMENT_REQUEST = new HashSet<ABONEMENT_REQUEST>();
			this.SALEOFABONEMENTS = new HashSet<SALEOFABONEMENTS>();
		}
		[Key]
		public int ID_ABONEMENT { get; set; }
		public int ID_SHEDULE { get; set; }
		public decimal PRICE_ABONEMENT { get; set; }
		public int AMOUNT_OF_VISITS { get; set; }
		public byte[] IMG_ABONEMENT { get; set; }
		public string TYPE_OF_ABONEMENT { get; set; }
		public string TYPE_OF_EXERCISE { get; set; }
		public string ADDITIONAL_SERVICE { get; set; }
		[ForeignKey("ID_SHEDULE")]
		public virtual SHEDULE_TABLE SHEDULE_TABLE { get; set; }
		public virtual ICollection<ABONEMENT_REQUEST> ABONEMENT_REQUEST { get; set; }
		public virtual ICollection<SALEOFABONEMENTS> SALEOFABONEMENTS { get; set; }
	}
}
