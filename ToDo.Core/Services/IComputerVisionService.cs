using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Plugin.Media.Abstractions;
using Xamarin.Essentials;

namespace ToDo.Core.Services
{
    public interface IComputerVisionService
    {
        Task<string> RecognizeText(MediaFile photo);
        Task<string> GetImageSummary(MediaFile photo);
        Task<List<DetectedObject>> RecognizeObjects(MediaFile photo);
        Task<List<FaceDescription>> RecognizeFaces(MediaFile photo);
        Task<List<DetectedBrand>> RecognizeBrands(MediaFile photo);
        Task<List<ImageTag>> RecognizeTags(MediaFile photo);
    }
}
