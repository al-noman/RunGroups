using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using RunGroups.Configurations;
using RunGroups.Services.Interfaces;

namespace RunGroups.Services;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;
    
    public PhotoService(IOptions<CloudinaryConfig> config)
    {
        _cloudinary = new Cloudinary(
            new Account(
                cloud: config.Value.CloudName, 
                apiKey: config.Value.ApiKey, 
                apiSecret: config.Value.ApiSecret
                )
            );
    }
    
    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
    {
        if (file.Length <= 0) return new ImageUploadResult();
        
        var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
        };
        return await _cloudinary.UploadAsync(uploadParams);
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicUrl)
    {
        var publicId = publicUrl.Split('/').Last().Split('.')[0];
        var deleteParams = new DeletionParams(publicId);
        return await _cloudinary.DestroyAsync(deleteParams);
    }
}