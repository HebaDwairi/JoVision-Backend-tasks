using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.IO;

[ApiController]
[Route("api/[controller]")]
public class UpdateController : ControllerBase
{
    public class FileData
    {
        public IFormFile Img { get; set; } 
        public string Owner { get; set; } = string.Empty;
    }

    public class MetaData{
        public string Owner { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public DateTime LastModificationTime {get; set;} = DateTime.Now;
    }
    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] FileData fileData )
    {
        if (fileData.Img == null || fileData.Img.Length == 0)
        {
            return BadRequest("No image uploaded.");
        }
        if (string.IsNullOrWhiteSpace(fileData.Owner))
        {
            return BadRequest("No Image owner specified");
        }

        // create json obj and update modification time
        string imageName = Path.GetFileNameWithoutExtension(fileData.Img.FileName);
        var imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", imageName);
        if(!Directory.Exists(imageFolderPath)){
            return BadRequest("the image you want to update doesn't exist");
        }

        string jsonData = System.IO.File.ReadAllText(imageFolderPath + "/metaData.json");
        MetaData jsonDataObj = JsonSerializer.Deserialize<MetaData>(jsonData);
        if(fileData.Owner != jsonDataObj.Owner){
            return BadRequest("the owner name you provided is incorrect");
        }

        var imagePath = Path.Combine(imageFolderPath, imageName + ".jpg");
        using (var stream = new FileStream(imagePath, FileMode.Create))
        {
            await fileData.Img.CopyToAsync(stream);
        }

        MetaData updatedMetaDataObj =  new MetaData{
            Owner = jsonDataObj.Owner,
            CreationTime = jsonDataObj.CreationTime,
            LastModificationTime = DateTime.Now,
        };
        string updatedMetaData = JsonSerializer.Serialize(updatedMetaDataObj);
        using(var stream = new FileStream(imageFolderPath + "/metaData.json", FileMode.Create))
        using(var writer = new StreamWriter(stream)){
            await writer.WriteAsync(updatedMetaData);
        }

        return Ok(new { response = "updated image" });
    }
}


