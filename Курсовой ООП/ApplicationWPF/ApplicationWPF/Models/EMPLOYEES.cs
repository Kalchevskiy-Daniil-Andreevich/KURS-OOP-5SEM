using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationWPF.Models
{
	public partial class EMPLOYEES
	{
		[Key]
		public int ID_EMPLOYEE { get; set; }
		public int ID_LOGPAS { get; set; }
		public string FIRSTNAME_EMPLOYEE { get; set; }
		public string LASTNAME_EMPLOYEE { get; set; }
		public string PHONENAME_EMPLOYEE { get; set; }
		public int SALARY_EMPLOYEE { get; set; }
		[ForeignKey("ID_LOGPAS")]
		public virtual LOGPAS LOGPAS { get; set; }
	}
}
