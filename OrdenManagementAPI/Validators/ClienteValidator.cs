using FluentValidation;
using OrderManagementAPI.DTOs;

namespace OrderManagementAPI.Validators
{
    public class ClienteValidator : AbstractValidator<ClienteDto>
    {
        public ClienteValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es requerido")
                .Length(3, 100).WithMessage("El nombre debe tener entre 3 y 100 caracteres");

            RuleFor(x => x.Identidad)
                .NotEmpty().WithMessage("La identidad es requerida")
                // Formato ####-####-#####
                .Matches(@"^\d{4}-\d{4}-\d{5}$").WithMessage("El formato de identidad debe ser 0000-0000-00000");
        }
    }
}

