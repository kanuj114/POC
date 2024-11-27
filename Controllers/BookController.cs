using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TokenAuthentication.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace TokenAuthentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Private End-Point
    public class BookController : ControllerBase
    {
        //private readonly ILogger<BookController> _logger;
        private readonly IValidator<RegisterDTO> _validator;

        public BookController(IValidator<RegisterDTO> validator)
        {
            _validator= validator;
        }

        [HttpPost("Create")] // Attribute Routing
        public IActionResult CreateAccount(RegisterDTO registerDTO)
        {
           var res =  _validator.Validate(registerDTO); // As soon as DTO is validated, then we have to call certain program
           // to hash your password

           if(!res.IsValid)
           {
                return BadRequest(res.Errors);
           }
           else 
           {
                // At this point, we have to call this service for register function. Now before sending the data
                // to database table, we need to apply hashing + SALT for password column
                return Ok("Record is not saved in backend");
           }
    }
}
}