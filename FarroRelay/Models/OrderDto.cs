using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarroRelay.Models
{
	public class OrderDto
	{
		public int Id { get; set; }
		public string Branch{ get; set; }
		public string Supplier { get; set; }
		public string PO_Number { get; set; }
		public string Inv_Number { get; set; }
		public DateTime Date_Create { get; set; }
		public DateTime? Date_Invoiced { get; set; }
		public string Status { get; set; }
		public decimal Total_Amount { get; set; }
	}
}
