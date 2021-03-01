using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarroRelay.Models
{
	public class Filter
	{
		public string PO_Number { get; set; }
		public string Inv_Number { get; set; }
		public string Keyword { get; set; }
		public DateTime? From { get; set; }
		public DateTime? To { get; set; }
	}
}
