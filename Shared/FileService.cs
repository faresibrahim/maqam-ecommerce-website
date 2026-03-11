namespace MusicShoppingCartMvcUI.Shared
{

    public interface IFileService
    {
        void DeleteFile(string fileName);
        Task<string> SaveFile(IFormFile file, string[] allowedExtensions);
    }


    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }


        public async Task<string> SaveFile(IFormFile file, string[] allowedExtensions)
        {
            // Get the physical path of the wwwroot folder (this is where static files live)
            var wwwPath = _environment.WebRootPath;

            // Build the full path to the "images" folder inside wwwroot
            var path = Path.Combine(wwwPath, "images");

            // If the images folder does not exist yet, create it
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Extract the file extension from the uploaded file (e.g., .jpg, .png)
            var extension = Path.GetExtension(file.FileName);

            // Validate that the uploaded file extension is allowed
            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException($"Only {string.Join(",", allowedExtensions)} files allowed");
            }

            // Generate a unique file name using GUID to avoid overwriting existing files
            string fileName = $"{Guid.NewGuid()}{extension}";

            // Combine the folder path with the new file name to get the full save path
            string fileNameWithPath = Path.Combine(path, fileName);

            // Create a file stream that will write the uploaded file to disk
            using var stream = new FileStream(fileNameWithPath, FileMode.Create);

            // Copy the uploaded file into the stream (saves the file to the server)
            await file.CopyToAsync(stream);

            // Return the generated file name (usually stored in the database)
            return fileName;
        }


        public void DeleteFile(string fileName)
        {
            var wwwPath = _environment.WebRootPath;
            var fileNameWithPath = Path.Combine(wwwPath, "images\\", fileName);
            if (!File.Exists(fileNameWithPath))
                throw new FileNotFoundException(fileName);
            File.Delete(fileNameWithPath);
        }

      
    }
}
