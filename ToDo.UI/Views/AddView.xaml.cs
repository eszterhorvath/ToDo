using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Rest;
using MvvmCross.Core.Parse.StringDictionary;
using MvvmCross.Forms.Views;
using ToDo.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace ToDo.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddView : MvxContentPage<AddViewModel>
    {
        public AddView()
        {
            InitializeComponent();
        }

        private async void CameraButton_OnClicked(object sender, EventArgs e)
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            var photoStream = await photo.OpenReadAsync();

            PhotoImage.Source = ImageSource.FromStream(() => photoStream);

            string subscriptionKey = "c3a2e38a72404c0291722a5ef6d3c764";
            string endpoint = "https://westcentralus.api.cognitive.microsoft.com/";

            var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(subscriptionKey))
            {
                Endpoint = endpoint
            };

            photoStream = await photo.OpenReadAsync();

            // Read text from stream
            var response = await client.ReadInStreamWithHttpMessagesAsync(photoStream);
            // After the request, get the operation location (operation ID)
            string operationLocation = response.Headers.OperationLocation;

            // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            ReadOperationResult results;
            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while (results.Status == OperationStatusCodes.Running ||
                    results.Status == OperationStatusCodes.NotStarted);


            var text = "";
            var textResults = results.AnalyzeResult.ReadResults;
            foreach (ReadResult page in textResults)
            {
                foreach (Line line in page.Lines)
                {
                    text += line.Text + " ";
                }
            }

            DescriptionEntry.Text = text;
        }
    }
}