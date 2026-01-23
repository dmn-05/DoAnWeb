using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCar_Model.Models;
using MiniCar_Model.Models.ViewModels;

namespace MiniCar_Model.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class StatisticController : Controller
	{
		private readonly ApplicationDbContext _context;

		public StatisticController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult RevenueByDay(DateTime? fromDate, DateTime? toDate)
		{
			var query = _context.Bills
					.Where(b =>
							b.StatusBill == "Completed" &&
							b.PaymentDate != null &&
							b.TotalPrice != null);

			if (fromDate.HasValue)
			{
				query = query.Where(b => b.PaymentDate!.Value.Date >= fromDate.Value.Date);
			}

			if (toDate.HasValue)
			{
				query = query.Where(b => b.PaymentDate!.Value.Date <= toDate.Value.Date);
			}

			var data = query
					.AsEnumerable()
					.GroupBy(b => b.PaymentDate!.Value.Date)
					.Select(g => new RevenueStatisticViewModel
					{
						Label = g.Key.ToString("dd/MM/yyyy"),
						Revenue = g.Sum(x => x.TotalPrice!.Value),
						OrderCount = g.Count()
					})
					.OrderBy(x => x.Label)
					.ToList();

			var model = new RevenueFilterViewModel
			{
				FromDate = fromDate,
				ToDate = toDate,
				Data = data
			};

			return View(model);
		}
		public IActionResult OrderCountByDay(DateTime? FromDate, DateTime? ToDate)
		{
			var query = _context.Bills
					.Where(b =>
							b.StatusBill == "Completed" &&
							b.PaymentDate != null);

			if (FromDate.HasValue)
				query = query.Where(b => b.PaymentDate!.Value.Date >= FromDate.Value.Date);

			if (ToDate.HasValue)
				query = query.Where(b => b.PaymentDate!.Value.Date <= ToDate.Value.Date);

			var data = query
					.Select(b => new
					{
						Date = b.PaymentDate!.Value.Date
					})
					.AsEnumerable()
					.GroupBy(x => x.Date)
					.Select(g => new RevenueStatisticViewModel
					{
						Label = g.Key.ToString("dd/MM/yyyy"),
						OrderCount = g.Count()
					})
					.OrderBy(x => DateTime.ParseExact(x.Label, "dd/MM/yyyy", null))
					.ToList();

			var model = new RevenueFilterViewModel
			{
				FromDate = FromDate,
				ToDate = ToDate,
				Data = data
			};

			return View(model);
		}

	}
}
