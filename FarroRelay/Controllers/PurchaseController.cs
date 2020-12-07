using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarroRelay.Data;
using FarroRelay.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace FarroRelay.Controllers
{
	[Route("api")]
	[ApiController]
	public class PurchaseController : ControllerBase
	{
		private readonly farroRelayContext _context;
		public PurchaseController(farroRelayContext context)
		{
			_context = context;
		}

		[HttpGet("orders/{po_number}")]
		public async Task<IActionResult> getOrder(double po_number)
		{
			var order =  await _context.Purchase.Where(p => p.PO_Number == po_number)
							.Join(_context.Branch, p=>p.Branch_Id, b=>b.Id, (p,b) => new { b.Name, p.Id,p.Supplier_Id, p.PO_Number, p.Inv_Number, p.Date_Create, p.Date_Invoiced,p.Type, p.Status, p.Payment_Status, p.Total_Amount})
							.Join(_context.Card, p=>p.Supplier_Id, c=>c.Id, (p,c) => new { c.Company, p.Id, p.Name, p.Supplier_Id, p.PO_Number, p.Inv_Number, p.Date_Create, p.Date_Invoiced,p.Type, p.Status,p.Payment_Status, p.Total_Amount })
							.Join(_context.EnumTable.Where(e=>e.Class == "purchase_order_status"), p=>p.Status, e=>e.Id,
							(p,e)=>new OrderDto
							{
								Id = p.Id,
								Branch = p.Name,
								Supplier_Id = p.Supplier_Id,
								Supplier = p.Company,
								PO_Number = p.PO_Number.ToString(),
								Inv_Number = p.Inv_Number,
								Date_Create = p.Date_Create,
								Date_Invoiced = p.Date_Invoiced,
								Status = (p.Type == 4 && p.Payment_Status == 1) ? "open bill" : ((p.Type == 4 && p.Payment_Status == 2) ? "closed bill" : e.Name ) ,
								Total_Amount = p.Total_Amount
							})
							.FirstOrDefaultAsync();
			return Ok(order);
		}

		

		[HttpGet("orders")]
		public async Task<IActionResult> getOders([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] string po)
		{
			Filter myfilter = new Filter
			{
				From = from,
				To = to,
				PO_Number = po
			};

			var orders = await getOrdersByFilter(myfilter);
			return Ok(orders);
		}

		private async Task<List<OrderDto>> getOrdersByFilter(Filter myfilter)
		{
			//var orders = await (from p in _context.Purchase
			//					join b in _context.Branch on p.Branch_Id equals b.Id into pb
			//					from b in pb.DefaultIfEmpty()
			//					join c in _context.Card on p.Supplier_Id equals c.Id into pbc
			//					from c in pbc.DefaultIfEmpty()
			//					where
			//					myfilter.From != null ? p.Date_Create >= myfilter.From : true
			//					&& myfilter.To != null ? p.Date_Create <= myfilter.To : true
			//					&& myfilter.PO_Number != null ? p.PO_Number.ToString().Contains(myfilter.PO_Number) : true

			//					select new OrderDto
			//					{ 
			//						Branch = b.Name,
			//						Supplier = c.Company,
			//						PO_Number = p.PO_Number,
			//						Inv_Number = p.Inv_Number,
			//						Date_Create = p.Date_Create,
			//						Date_Invoiced = p.Date_Invoiced,
			//						Status = p.Status,
			//						Total_Amount = p.Total_Amount

			//					}).ToListAsync();

			var orders = await _context.Purchase.Where(p =>
													(myfilter.From != null ? p.Date_Create >= myfilter.From : true)
													&&
													(myfilter.To != null ? p.Date_Create <= myfilter.To : true)
													&& 
													(myfilter.PO_Number != null ? p.PO_Number.ToString().Contains(myfilter.PO_Number) : true))
													.Join(_context.Branch, p => p.Branch_Id, b => b.Id, (p, b) => new { b.Name, p.Id,p.Supplier_Id, p.PO_Number, p.Inv_Number, p.Date_Create, p.Date_Invoiced, p.Type, p.Status, p.Payment_Status,p.Total_Amount })
													.Join(_context.Card, p => p.Supplier_Id, c => c.Id, (p, c) => new { p.Name, c.Company, p.Id, p.Supplier_Id, p.PO_Number, p.Inv_Number, p.Date_Create, p.Date_Invoiced,p.Type, p.Status,p.Payment_Status, p.Total_Amount })
													.Join(_context.EnumTable.Where(e => e.Class == "purchase_order_status"), p => p.Status, e => e.Id,
													(p, e)=>new OrderDto
													{
														Id = p.Id,
														Branch = p.Name,
														Supplier_Id = p.Supplier_Id,
														Supplier = p.Company,
														PO_Number = p.PO_Number.ToString(),
														Inv_Number = p.Inv_Number,
														Date_Create = p.Date_Create,
														Date_Invoiced = p.Date_Invoiced,
														Status = (p.Type == 4 && p.Payment_Status == 1) ? "open bill" : ((p.Type == 4 && p.Payment_Status == 2) ? "closed bill" : e.Name),
														Total_Amount = p.Total_Amount
													})
													.ToListAsync();

			return orders;
		}

		[HttpPatch("updatePurchase/{id}")]
		public async Task<IActionResult> updatePurchase(int id, [FromBody] JsonPatchDocument<UpdateOrderDto> patchDoc)
		{
			try
			{

				if (patchDoc == null)
					return BadRequest();

				var orderToUpdate = await _context.Purchase.FirstOrDefaultAsync(p => p.Id == id);
				if (orderToUpdate == null)
					return NotFound("This purchase order does not exist!");

				var purchaseToPatch = new UpdateOrderDto
				{
					Status = orderToUpdate.Payment_Status,
					Inv_Number = orderToUpdate.Inv_Number
				};

				patchDoc.ApplyTo(purchaseToPatch, ModelState);

				if (!ModelState.IsValid)
					return BadRequest(ModelState);
				orderToUpdate.Payment_Status = purchaseToPatch.Status ;
				orderToUpdate.Inv_Number = purchaseToPatch.Inv_Number;

				await _context.SaveChangesAsync();

				return Ok("Purchase updated!");
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
