using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop.Application.DTOs;

namespace Web_Shop.Application.Validation
{
    public class AddUpdateCustomerDTOValidator : AbstractValidator<AddUpdateCustomerDTO>
    {
        public AddUpdateCustomerDTOValidator()
        {
            RuleFor(request => request.Name).Length(3, 32).WithMessage("Pole 'Imię' należy podać w zakresie {MinLength} - {MaxLength} znaków");
            RuleFor(request => request.Email).EmailAddress();
            RuleFor(request => request.Password)
                //.NotEmpty()
                .MinimumLength(8).WithMessage("Minimalna długość pola 'Hasło' to {MinLength} znaków")
                .Matches("[A-Z]").WithMessage("'Hasło' musi zawierać co najmniej jedną wielką literę.")
                .Matches("[a-z]").WithMessage("'Hasło' musi zawierać co najmniej jedną małą literę.")
                .Matches(@"\d").WithMessage("'Hasło' musi zawierać co najmniej jedną cyfrę.")
                .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage("'Hasło' musi zawierać co najmniej jeden znak specjalny.")
                .Matches("^[^£# “”]*$").WithMessage("'Hasło' nie może zawierać znaków £ # “” ani spacji.")
                .When(request => request.IsPasswordUpdate);
            // .Must(pass => !blacklistedWords.Any(word => pass.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0))
            //     .WithMessage("'Hasło' zawiera niedozwoloną frazę.");
        }
    }
}
