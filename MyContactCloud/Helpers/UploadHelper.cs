﻿using MyContactCloud.Client.Helpers;
using MyContactCloud.Model;

namespace MyContactCloud.Helpers
{
    public static class UploadHelper
    {
        public static readonly string DefaultProfilePicture = ImageHelper.DefaultProfilePicture;
        public static readonly string DefaultContactImage = ImageHelper.DefaultContactImage;
        public static readonly int MaxFileSize = ImageHelper.MaxFileSize;

        public static async Task<ImageUpload> GetImageUploadAsync(IFormFile file)
        {
            using var ms = new MemoryStream();

            await file.CopyToAsync(ms);

            byte[] data = ms.ToArray();

            if (ms.Length > 5 * 1024 * 1024)
            {
                throw new IOException("Images must be less than 5MB");
            }

            ImageUpload upload = new ImageUpload()
            {
                Id = Guid.NewGuid(),
                Data = data,
                Type = file.ContentType
            };

            return upload;
        }
    }
}
