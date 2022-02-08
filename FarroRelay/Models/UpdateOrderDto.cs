﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarroRelay.Models
{
	public class UpdateOrderDto
	{
		public byte Status{ get; set; }
		public int Order_Status { get; set; }
		public string Inv_Number { get; set; }
		public decimal Amount_Paid { get; set; }
	}
}
