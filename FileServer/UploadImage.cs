namespace InventAutoApi.FileServer
{
    public class UploadImage
    {
        private readonly string _imagesFolderPath;

        public UploadImage(string imagesFolderPath)
        {
            _imagesFolderPath = imagesFolderPath;
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Image file is null or empty");

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(_imagesFolderPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return Path.Combine("Images", uniqueFileName);
        }

        public bool DeleteImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                throw new ArgumentException("Image Path is Null or Empty");

            var fullPath = Path.Combine(_imagesFolderPath, imagePath.Split("\\")[1]);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }

            return false;
        }

    }
}
