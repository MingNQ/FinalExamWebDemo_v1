using FinalExamDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalExamDemo.ViewComponents
{
	public class RenderViewComponent : ViewComponent
	{
		private List<Category> Categories = new List<Category>();
		private readonly NewShopContext _context;

		public RenderViewComponent(NewShopContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			Categories = _context.Categories.ToList();

			return View("RenderCategories", Categories);
		}
	}
}
