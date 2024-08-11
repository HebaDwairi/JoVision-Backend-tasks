using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CreateController : ControllerBase
{
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
        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        var imageName = Path.GetRandomFileName() ;
        var imgFolderPath = Path.Combine(uploadsPath,imageName);
        var imgPath = Path.Combine(imgFolderPath,imageName+".jpg");
        var metaDataPath = Path.Combine(imgFolderPath,"metaData.json");
        //var directory = Path.GetDirectoryName(imgFolderPath);
          Directory.CreateDirectory(imgFolderPath);

        MetaData jsonObj = new MetaData{
            Owner = fileData.Owner,
        };
        string jsonObjString = JsonSerializer.Serialize(jsonObj);
        // Save the file
        using (var stream = new FileStream(imgPath, FileMode.Create))
        {
            await fileData.Img.CopyToAsync(stream);
        }
        using (var stream = new FileStream(metaDataPath, FileMode.Create))
        using (var writer = new StreamWriter(stream))
        {
            await writer.WriteAsync(jsonObjString);
         }

        return Ok(new { response = "image uploaded" });
    }
}

public class FileData
{
    public IFormFile Img { get; set; }
    public string Owner { get; set; }
}

public class MetaData{
    public string Owner { get; set; }
}