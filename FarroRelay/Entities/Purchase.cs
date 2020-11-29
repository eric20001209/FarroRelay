using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarroRelay.Entities
{
	public class Purchase
	{
		public int Id{ get; set; }
		public int Branch_Id{ get; set; }
		public int? Supplier_Id{ get; set; }
		public double PO_Number{ get; set; }
		public string Inv_Number{ get; set; }
		public int Type{ get; set; }
		public DateTime Date_Create{ get; set; }
		public DateTime? Date_Invoiced{ get; set; }
		public int Status{ get; set; }
		public byte Payment_Status{ get; set; }
		public decimal Total_Amount{ get; set; }
	}
}
