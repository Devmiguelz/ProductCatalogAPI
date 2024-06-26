using FluentValidation;
using ProductCatalogAPI.Application.DTO.Product;
using ProductCatalogAPI.Application.DTO.ProductImage;

namespace ProductCatalogAPI.Application.Validators
{
    public class ProductUpdateDtoValidator: AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .Must(id => id != Guid.Empty).WithMessage("Id must be a valid non-empty GUID.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Description).MaximumLength(250).WithMessage("Description cannot be longer than 250 characters.");
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.");

            RuleForEach(x => x.Images).SetValidator(new VehicleImageUpdateDtoValidator());
        }
    }

    public class VehicleImageUpdateDtoValidator : AbstractValidator<ProductImageUpdateDto>
    {
        private const int MaxFileSizeInMB = 5;
        private static readonly string[] AllowedExtensions = { ".png", ".jpg", ".jpeg" };

        public VehicleImageUpdateDtoValidator()
        {
            RuleFor(x => x)
                .Must(x => x.Id.HasValue || x.File != null)
                .WithMessage("Either 'Id' or 'File' must be provided.");

            When(x => x.File != null, () =>
            {
                RuleFor(x => x.File)
                    .Must(file => file!.Length <= MaxFileSizeInMB * 1024 * 1024)
                    .WithMessage($"The file exceeds the maximum allowed size of {MaxFileSizeInMB} MB.")
                    .Must(file => AllowedExtensions.Contains(Path.GetExtension(file!.FileName).ToLower()))
                    .WithMessage("Only image files in PNG, JPG, or JPEG formats are allowed.");
            });
        }
    }
}
