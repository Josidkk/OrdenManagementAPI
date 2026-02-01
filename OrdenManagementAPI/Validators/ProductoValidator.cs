using FluentValidation;
using OrderManagementAPI.DTOs;

namespace OrderManagementAPI.Validators
{
    public class ProductoValidator : AbstractValidator<ProductoDto>
    {
        public ProductoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es requerido")
                .Length(3, 100).WithMessage("El nombre debe tener entre 3 y 100 caracteres");

            RuleFor(x => x.Descripcion)
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");

            RuleFor(x => x.Precio)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0");

            RuleFor(x => x.Existencia)
                .GreaterThanOrEqualTo(0).WithMessage("La existencia no puede ser negativa");
        }
    }
}
