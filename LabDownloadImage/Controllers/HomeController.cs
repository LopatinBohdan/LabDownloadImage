using LabDownloadImage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.Cosmos;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace LabDownloadImage.Controllers
{
	public class HomeController : Controller
	{
		MyService myService=new MyService();
		MyContext db;
		static string endPoint = "https://lopatincompvision.cognitiveservices.azure.com/";
        static string key = "18145f58fb0d44e5a01e60da9cadbdf5";
		//static int count = 0;

		ComputerVisionClient cVisionClient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
		{
			Endpoint = endPoint
		};


		public HomeController(MyContext context)
		{
			db = context;
		}
		public async Task<IActionResult> Index()
		{
			//ViewBag.Images = await myService.DownloadImage();
            return View();
		}
		[HttpPost]
		public async Task<IActionResult> Add(IFormFile fileName)
		{
			string folderPath=$@"{Directory.GetCurrentDirectory()}\wwwroot\temp";
			Directory.CreateDirectory(folderPath);
			string filePath=Path.Combine(folderPath, fileName.FileName);

			using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
			{
				fileName.CopyTo(fs);
			}

            await myService.AddImage(filePath);
			string imageLink= await myService.UploadImage(fileName);
			ViewBag.Link=imageLink;

			/////////////////////////////////////////////////////
			List<VisualFeatureTypes?> featureTypes = Enum.GetValues(typeof(VisualFeatureTypes)).OfType<VisualFeatureTypes?>().ToList();

			ImageAnalysis analysis = await cVisionClient.AnalyzeImageAsync(imageLink, featureTypes);

			if (analysis.Adult.IsAdultContent)
			{
				ViewBag.Link = null;

            }
			string imgTitle = "";
			foreach (var item in analysis.Categories)
			{
				imgTitle += item.Name;
				foreach (var subitem in analysis.Brands)
				{
					imgTitle += subitem.Name;
				}
			}
			//count++;
			Image temp=new Image();

            temp.Id = Guid.NewGuid().ToString();
			temp.Title = imgTitle;
			temp.Path = imageLink;
            db.Add(temp);
			db.SaveChanges();

			return View();
        }
        [HttpPost]
        public async Task<IActionResult> ImageSearch(string searchString)
		{
			List<Image> images = new List<Image>();
			List<Image> dbImages= db.Images.ToList();
			foreach (Image item in dbImages)
			{
				if (item.Title!.ToLower().Contains(searchString))
				{
                    images.Add(item);
                }
			}
			if (images.Count > 0)
			{
				ViewBag.Imgs=images.ToList();
			}
			return View("ImageSearch", images);	
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