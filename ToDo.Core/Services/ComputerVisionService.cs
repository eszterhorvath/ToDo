using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Rest;
using Microsoft.Win32.SafeHandles;
using Xamarin.Essentials;

namespace ToDo.Core.Services
{
    public class ComputerVisionService : IComputerVisionService
    {
        private readonly string _subscriptionKey = "c3a2e38a72404c0291722a5ef6d3c764";
        private readonly string _endpoint = "https://westcentralus.api.cognitive.microsoft.com/";

        private readonly ComputerVisionClient _client;
        private (FileResult photo, ImageAnalysis analysis) _imageAnalysis;

        public ComputerVisionService()
        {
            _client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_subscriptionKey))
            {
                Endpoint = _endpoint
            };
        }

        public async Task<string> RecognizeText(FileResult photo)
        {
            // Read text from stream
            HttpOperationHeaderResponse<ReadInStreamHeaders> response;
            using (var photoStream = await photo.OpenReadAsync())
            {
                response = await _client.ReadInStreamWithHttpMessagesAsync(photoStream);
            }

            // After the request, get the operation location (operation ID)
            string operationLocation = response.Headers.OperationLocation;

            // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            ReadOperationResult results;
            do
            {
                results = await _client.GetReadResultAsync(Guid.Parse(operationId));
            } while (results.Status == OperationStatusCodes.Running ||
                     results.Status == OperationStatusCodes.NotStarted);


            var text = "";
            var textResults = results.AnalyzeResult.ReadResults;
            foreach (ReadResult page in textResults)
            {
                foreach (Line line in page.Lines)
                {
                    text += line.Text + "\n";
                }
            }

            return text;
        }

        public async Task<string> GetImageSummary(FileResult photo)
        {
            var imageAnalysis = await GetImageAnalysis(photo);

            var text = "";
            foreach (var caption in imageAnalysis.Description.Captions)
            {
                text += $"{caption.Text}\n";
            }

            return text;
        }

        public async Task<List<DetectedBrand>> RecognizeBrands(FileResult photo)
        {
            var imageAnalysis = await GetImageAnalysis(photo);

            return imageAnalysis.Brands.ToList();
        }

        public async Task<List<FaceDescription>> RecognizeFaces(FileResult photo)
        {
            var imageAnalysis = await GetImageAnalysis(photo);

            return imageAnalysis.Faces.ToList();
        }

        public async Task<List<DetectedObject>> RecognizeObjects(FileResult photo)
        {
            var imageAnalysis = await GetImageAnalysis(photo);

            return imageAnalysis.Objects.ToList();
        }

        public async Task<List<ImageTag>> RecognizeTags(FileResult photo)
        {
            var imageAnalysis = await GetImageAnalysis(photo);

            return imageAnalysis.Tags.ToList();
        }

        private async Task<ImageAnalysis> GetImageAnalysis(FileResult photo)
        {
            if (_imageAnalysis.photo != null && photo.Equals(_imageAnalysis.photo))
            {
                return _imageAnalysis.analysis;
            }

            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
                VisualFeatureTypes.Objects
            };

            ImageAnalysis analysis;
            using (var photoStream = await photo.OpenReadAsync())
            {
                analysis = await _client.AnalyzeImageInStreamAsync(photoStream, features);
            }

            _imageAnalysis = (photo, analysis);
            return analysis;
        }
    }
}
