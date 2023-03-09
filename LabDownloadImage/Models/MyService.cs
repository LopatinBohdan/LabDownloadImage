using Azure.Storage.Blobs;
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
			CreateContainer();
		}

        public void CreateContainer()
		{
			//Directory.CreateDirectory(path);
			container = new BlobContainerClient(connString, path);
		}

		public async Task AddImage(string imgName)
		{
			fileName= Path.GetFileName(imgName);
			string filePath=Path.Combine(path, fileName);
			//await File.WriteAllBytesAsync(filePath, imageName);
			blob = container.GetBlobClient(fileName);
			
			//using FileStream fs=File.OpenRead(filePath);
			await blob.UploadAsync(imgName);
			//fs.Close();
		}
	}
}
