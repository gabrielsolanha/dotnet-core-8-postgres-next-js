﻿namespace AplicacaoWeb.Service.Interfaces
{
    public interface IS3Service
    {
        Task<string> ImageWebpToS3Async(IFormFile image);
    }
}
