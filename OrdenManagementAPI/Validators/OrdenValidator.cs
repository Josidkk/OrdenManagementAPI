using FluentValidation;
using OrderManagementAPI.DTOs;

namespace OrderManagementAPI.Validators
{
    public class OrdenRequestValidator : AbstractValidator<OrdenRequestDto>
    {
        public OrdenRequestValidator()
        {
            RuleFor(x => x.ClienteId)
                .GreaterThan(0).WithMessage("ClienteId debe ser mayor a 0");

            RuleFor(x => x.Detalles)
                .NotEmpty().WithMessage("La orden debe tener al menos un detalle");

            RuleForEach(x => x.Detalles).SetValidator(new DetalleRequestValidator());
        }
    }

    public class DetalleRequestValidator : AbstractValidator<DetalleRequestDto>
    {
        public DetalleRequestValidator()
        {
            RuleFor(x => x.ProductoId)
                .GreaterThan(0).WithMessage("ProductoId debe ser mayor a 0");

            RuleFor(x => x.Cantidad)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0");
        }
    }
}
