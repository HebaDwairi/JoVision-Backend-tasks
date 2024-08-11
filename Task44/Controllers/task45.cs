using Microsoft.AspNetCore.Mvc;
using System;


[ApiController]
[Route("api/[controller]")]
public class BirthdateController : ControllerBase
{
    private int calculateAge(int y, int m, int d){
        var bdate = new DateTime(y, m, d);
        var current = DateTime.Now;
        TimeSpan differance = current.Subtract(bdate);
        int age = differance.Days/365;
        return age;
    }
    [HttpGet]
    public IActionResult Get( string name = "anonymous", int year = 0, int month = 0, int day = 0 )
    {
        if(year == 0 || month == 0 || day == 0){
           var noAge = new { greeting = "Hello "+ name +" I can’t calculate your age without knowing your birthdate!"};
           return Ok(noAge);
        }
        var age = calculateAge(year,month,day);
        var greeting = new { greeting = "Hello " + name + " your age is "+ age};
        return Ok(greeting);
    }
}