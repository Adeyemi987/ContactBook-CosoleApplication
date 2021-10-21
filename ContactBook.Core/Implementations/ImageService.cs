using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ContactBook.Core.Interfaces;
using ContactBook.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactBook.Core.Implementations
{
    public class ImageService : IImageService
    {
        private readonly IConfiguration _configuration;
        private readonly Cloudinary cloudinary;
        private readonly ImageUploadConfig _options;

        public ImageService(IConfiguration configuration, IOptions<ImageUploadConfig> options)
        {
            _options = options.Value;
            _configuration = configuration;
            cloudinary = new Cloudinary(new Account
                (_options.CloudName,
                _options.APIKey,
                _options.APISecret));
        }
        public async Task<UploadResult> UploadAsync(IFormFile image)
        {
            var pictureSize = Convert.ToInt64(_configuration.GetSection("PhotoSettings:Size").Get<string>());
            if (image.Length > pictureSize)
            {
                throw new ArgumentException("File size exceeded");
            }
            var pictureFormat = false;

            var listOfImageExtensions = _configuration.GetSection("PhotoSettings:Formats").Get<List<string>>();

            foreach (var item in listOfImageExtensions)
            {
                if (image.FileName.EndsWith(item))
                {
                    pictureFormat = true;
                    break;
                }
            }

            if (pictureFormat == false)
            {
                throw new ArgumentException("File format not supported");
            }

            var uploadResult = new ImageUploadResult();

            //fetch the image using image stream
            using (var imageStream = image.OpenReadStream())
            {
                string filename = Guid.NewGuid().ToString() + image.FileName;
                uploadResult = await cloudinary.UploadAsync(new ImageUploadParams()
                {
                    File = new FileDescription(filename, imageStream),
                    Transformation = new Transformation().Crop("thumb").Gravity("face").Width(150)
                });
            }
            return uploadResult;
        }
    }
}
