namespace LabDownloadImage.Models
{
    
    public class Image
    {
       // [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Path { get; set; }
    }
}
