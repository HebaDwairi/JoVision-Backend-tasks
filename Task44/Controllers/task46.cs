using Microsoft.AspNetCore.Mvc;
using System;


[ApiController]
[Route("api/[controller]")]
public class Birthdate46Controller : ControllerBase
{
    private int calculateAge(int y, int m, int d){
        var bdate = new DateTime(y, m, d);
        var current = DateTime.Now;
        TimeSpan differance = current.Subtract(bdate);
        int age = differance.Days/365;
        return age;
    }
    [HttpPost]
    public IActionResult Post( [FromBody] Person person)
    {
        if(person.year == 0 || person.month == 0 || person.day == 0){
           var noAge = new { greeting = "Hello "+ person.name +" I can’t calculate your age without knowing your birthdate!"};
           return Ok(noAge);
        }
        var age = calculateAge(person.year,person.month,person.day);
        var greeting = new { greeting = "Hello " + person.name + " your age is "+ age};
        return Ok(greeting);
    }
}

[Route("api/[controller]")]
public class Greeter46Controller : ControllerBase
{
    [HttpPost]
    public IActionResult Post( [FromBody] Person person )
    {
        var greeting = new { greeting = "Hello " + person.name };
        return Ok(greeting);
    }
}

public class Person {
    public string name { get; set; }
    public int day { get; set; }
    public int month { get; set; }
    public int year { get; set; }
}