using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace Busticket.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            if (string.IsNullOrEmpty(cloudName) ||
                string.IsNullOrEmpty(apiKey) ||
                string.IsNullOrEmpty(apiSecret))
            {
                throw new Exception("❌ Cloudinary no está configurado correctamente en appsettings.json");
            }

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account) { Api = { Secure = true } };
        }

        public async Task<string> SubirImagenAsync(IFormFile imagen)
        {
            if (imagen == null || imagen.Length == 0)
                throw new Exception("❌ No se recibió ningún archivo.");

            try
            {
                using var stream = imagen.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(imagen.FileName, stream)
                 
                };

                var resultado = await _cloudinary.UploadAsync(uploadParams);

           
                Console.WriteLine($"StatusCode: {resultado.StatusCode}");
                Console.WriteLine($"SecureUrl: {resultado.SecureUrl?.AbsoluteUri}");
                Console.WriteLine($"Url: {resultado.Url?.AbsoluteUri}");
                if (resultado.Error != null)
                    Console.WriteLine($"Error: {resultado.Error.Message}");

              
                var url = resultado.SecureUrl?.ToString() ?? resultado.Url?.ToString();
                if (string.IsNullOrEmpty(url))
                    throw new Exception("❌ Cloudinary no devolvió una URL válida.");

                return url;
            }
            catch (Exception ex)
            {
                throw new Exception($"❌ Error subiendo imagen a Cloudinary: {ex.Message}");
            }
        }
    }
}
