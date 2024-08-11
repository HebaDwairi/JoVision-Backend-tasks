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
        //var directory = Path.GetDirectoryName(imgFolderPath);
          Directory.CreateDirectory(imgFolderPath);

        // Save the file
        using (var stream = new FileStream(imgPath, FileMode.Create))
        {
            await fileData.Img.CopyToAsync(stream);
        }

        return Ok(new { response = "image uploaded" });
    }
}

public class FileData
{
    public IFormFile Img { get; set; }
    public string Owner { get; set; }
}
