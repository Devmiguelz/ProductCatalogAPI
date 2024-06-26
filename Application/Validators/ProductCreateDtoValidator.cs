using FluentValidation;
using ProductCatalogAPI.Application.DTO.Product;
using ProductCatalogAPI.Application.DTO.ProductImage;

namespace ProductCatalogAPI.Application.Validators
{
    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Description).MaximumLength(250).WithMessage("Description cannot be longer than 250 characters.");
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.");

            RuleForEach(x => x.Images).SetValidator(new VehicleImageCreateDtoValidator());
        }
    }

    public class VehicleImageCreateDtoValidator : AbstractValidator<ProductImageCreateDto>
    {
        private const int MaxFileSizeInMB = 5;
        private static readonly string[] AllowedExtensions = { ".png", ".jpg", ".jpeg" };

        public VehicleImageCreateDtoValidator()
        {
            RuleFor(file => file.File)
                .NotNull().WithMessage("File cannot be null.")
                .Must(file => file.Length <= MaxFileSizeInMB * 1024 * 1024)
                .WithMessage($"The file exceeds the maximum allowed size of {MaxFileSizeInMB} MB.")
                .Must(file => AllowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                .WithMessage("Only image files in PNG, JPG, or JPEG formats are allowed.");
        }
    }
}
