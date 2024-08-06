using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class GreeterController : ControllerBase
{
    [HttpGet]
    public IActionResult Get( string name = "anonymous" )
    {
        var greeting = new { greeting = "Hello " + name};
        return Ok(greeting);
    }
}
