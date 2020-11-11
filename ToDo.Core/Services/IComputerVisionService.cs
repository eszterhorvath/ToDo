using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Xamarin.Essentials;

namespace ToDo.Core.Services
{
    public interface IComputerVisionService
    {
        Task<string> RecognizeText(FileResult photo);
        Task<string> GetImageSummary(FileResult photo);
        Task<List<DetectedObject>> RecognizeObjects(FileResult photo);
        Task<List<FaceDescription>> RecognizeFaces(FileResult photo);
        Task<List<DetectedBrand>> RecognizeBrands(FileResult photo);
        Task<List<ImageTag>> RecognizeTags(FileResult photo);
    }
}
