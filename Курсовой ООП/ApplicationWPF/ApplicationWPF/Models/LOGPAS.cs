using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ApplicationWPF.Models
{
	public partial class LOGPAS
	{
		public LOGPAS()
		{
			this.CLIENTS = new HashSet<CLIENTS>();
			this.EMPLOYEES = new HashSet<EMPLOYEES>();
		}

		[Key]
		public int ID_lOGPAS { get; set; }
		public string LOGIN_VALUE { get; set; }
		public string PASSWORD_VALUE { get; set; }
		public string USER_TYPE { get; set; }

		public virtual ICollection<CLIENTS> CLIENTS { get; set; }
		public virtual ICollection<EMPLOYEES> EMPLOYEES { get; set; }
	}
}
