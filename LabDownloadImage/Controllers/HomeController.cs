using LabDownloadImage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace LabDownloadImage.Controllers
{
	public class HomeController : Controller
	{
		MyService myService=new MyService();
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}
		public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Add(string fileName)
		{
			var items = Request.Form.Files;
			if (items.Count>0)
			{
				string folderPath=$@"{Directory.GetCurrentDirectory()}\wwwroot\temp";
				//string folderPath = Server.MapPath("~/temp");
				Directory.CreateDirectory(folderPath);
				string filePath=Path.Combine(folderPath, items[0].Name);

				using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
				{
					items[0].CopyTo(fs);
				}
				await myService.AddImage(filePath);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}