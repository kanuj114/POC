using FluentValidation;
using TokenAuthentication.DTO;
namespace TokenAuthentication.Validation
{
    public class RegisterDTOValidator:AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidator()
        {
           RuleFor(RegisterDTO => RegisterDTO.FullName).NotNull().MaximumLength(20); 
           RuleFor(RegisterDTO=>RegisterDTO.Email).NotNull().MaximumLength(50);  
           // Now we have to define the rule to validate the RegisterDTO.  
        }
        
    }
}