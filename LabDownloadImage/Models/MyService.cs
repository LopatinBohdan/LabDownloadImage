using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Xml;

namespace LabDownloadImage.Models
{
	public class MyService
	{
		private string connString =Environment.GetEnvironmentVariable("AZURE_VALUE");
		private BlobServiceClient blobServiceClient { get; set; }
		private BlobContainerClient container { get; set; }
		private BlobClient blob { get; set; }
		private string path= "home";
        public string fileName { get; set; }

        public MyService()
		{
			blobServiceClient = new BlobServiceClient(connString);
			try
			{
				container=blobServiceClient.CreateBlobContainer(path);
            }
			catch (Exception)
			{
				container = blobServiceClient.GetBlobContainerClient(path);
			}
		}

		public async Task DownloadImage(string fileName)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				await blob.DownloadToAsync(stream);
				byte[] bytes = stream.ToArray();

				await File.WriteAllBytesAsync($"wwwroot/img/{fileName}", bytes);
			}
		}

		public async Task<string> UploadImage(IFormFile file)
		{
			blob=container.GetBlobClient(file.Name);
			using(Stream stream = file.OpenReadStream())
			{
				await blob.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
			}
			string linkName = container.GetBlobs().LastOrDefault().Name;
			string link=Path.Combine(container.Uri.ToString(), linkName);
			return link;
		}

        public async Task AddImage(string file)
        {
        	fileName= Path.GetFileName(file);
        	blob = container.GetBlobClient(fileName);

        	await blob.UploadAsync(file, true);
        }

		public async Task<string> GetLinks(string fileName)
		{
            container = blobServiceClient.GetBlobContainerClient(path);
			BlobItem blobItem = container.GetBlobs().LastOrDefault();
			var pathPic= Path.Combine(container.Uri.ToString(), blobItem.Name);
			return pathPic;
        }
    }
}
