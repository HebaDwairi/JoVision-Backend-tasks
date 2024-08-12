using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.IO;

[ApiController]
[Route("api/[controller]")]
public class CreateController : ControllerBase
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
    
        var imageName = Path.GetFileNameWithoutExtension(fileData.Img.FileName); ;
        var imgFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads",imageName);
        var imgPath = Path.Combine(imgFolderPath,imageName+".jpg");
        var metaDataPath = Path.Combine(imgFolderPath,"metaData.json");
        Directory.CreateDirectory(imgFolderPath);

        if(System.IO.File.Exists(imgPath)){
            return BadRequest("an image with the same name exists");
        }

        MetaData jsonObj = new MetaData{
            Owner = fileData.Owner,
            CreationTime = DateTime.Now,
            LastModificationTime = DateTime.Now,
        };
        string jsonObjString = JsonSerializer.Serialize(jsonObj);
        using (var stream = new FileStream(imgPath, FileMode.Create))
        {
            await fileData.Img.CopyToAsync(stream);
        }
        using (var stream = new FileStream(metaDataPath, FileMode.Create))
        using (var writer = new StreamWriter(stream))
        {
            await writer.WriteAsync(jsonObjString);
         }

        return Ok(new { response = "created" });
    }
}



[ApiController]
[Route("api/[controller]")]
public class DeleteController : ControllerBase
{
    public class MetaData{
        public string Owner { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public DateTime LastModificationTime {get; set;} = DateTime.Now;
    }
    [HttpGet]
    public IActionResult Get([FromQuery] string ownerName, [FromQuery] string imgName )  {
        if (string.IsNullOrWhiteSpace(ownerName) || string.IsNullOrWhiteSpace(imgName)){
            return BadRequest("image owner or name is not specified");
        }
        var imgDirectory  = Path.Combine(Directory.GetCurrentDirectory(), "uploads", imgName);
        if(!Directory.Exists(imgDirectory)){
            return BadRequest("no image with this name exists");
        }
        
        string metaData = System.IO.File.ReadAllText(imgDirectory + "/metaData.json");
        var metaDataObj = JsonSerializer.Deserialize<MetaData>(metaData);

        if(metaDataObj.Owner != ownerName){
            return BadRequest("image owner name is incorrect");
        }

        Directory.Delete(imgDirectory,true);
        return Ok(new { response = "deleted" });
    }
    
}
