using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ToDo.Core.Models;
using ToDo.Core.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ToDo.Core.ViewModels
{
    public class AddViewModel : MvxViewModel
    {
        private readonly IToDoService _todoService;
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogService;
        private readonly IComputerVisionService _computerVisionService;

        public Models.ToDo ToDoItem { get; set; }
        public ICommand AddTodoItemCommand { get; }
        public ICommand TakePhotoCommand { get; }

        private Stream photoStream;

        public AddViewModel(IToDoService todoService, IMvxNavigationService navigationService, IUserDialogs userDialogService, IComputerVisionService computerVisionService)
        {
            _todoService = todoService;
            _navigationService = navigationService;
            _userDialogService = userDialogService;
            _computerVisionService = computerVisionService;

            ToDoItem = new Models.ToDo();

            AddTodoItemCommand = new Command(async () => await AddTodo());
            TakePhotoCommand = new Command(async () => await TakePhoto());
        }

        public override void ViewDisappeared()
        {
            photoStream?.Dispose();
        }

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                RaisePropertyChanged();
            }
        }

        private string _recognizedText;
        public string RecognizedText
        {
            get => _recognizedText;
            set
            {
                _recognizedText = value;
                RaisePropertyChanged();
            }
        }

        private string _recognizedSummary;
        public string RecognizedSummary
        {
            get => _recognizedSummary;
            set
            {
                _recognizedSummary = value;
                RaisePropertyChanged();
            }
        }

        private string _recognizedTags;
        public string RecognizedTags
        {
            get => _recognizedTags;
            set
            {
                _recognizedTags = value;
                RaisePropertyChanged();
            }
        }

        private string _recognizedObjects;
        public string RecognizedObjects
        {
            get => _recognizedObjects;
            set
            {
                _recognizedObjects = value;
                RaisePropertyChanged();
            }
        }

        private string _recognizedBrands;
        public string RecognizedBrands
        {
            get => _recognizedBrands;
            set
            {
                _recognizedBrands = value;
                RaisePropertyChanged();
            }
        }

        private string _recognizedFaces;
        public string RecognizedFaces
        {
            get => _recognizedFaces;
            set
            {
                _recognizedFaces = value;
                RaisePropertyChanged();
            }
        }

        internal async Task AddTodo()
        {
            if (ToDoItem.Title == null)
            {
                _userDialogService.Alert("Title cannot be empty!");
                return;
            }

            ToDoItem.State = State.Pending;
            await _todoService.SaveTodoAsync(ToDoItem);

            await _navigationService.Close(this);
            
        }

        internal async Task TakePhoto()
        {
            photoStream?.Dispose();

            var photo = await MediaPicker.CapturePhotoAsync();

            photoStream = await photo.OpenReadAsync();
            ImageSource = ImageSource.FromStream(() => photoStream);

            await RecognizeText(photo);
            await RecognizeSummary(photo);
            await RecognizeTags(photo);
            await RecognizeObjects(photo);
            await RecognizeBrands(photo);
            await RecognizeFaces(photo);
        }

        internal async Task RecognizeText(FileResult photo)
        {
            RecognizedText = await _computerVisionService.RecognizeText(photo);
        }

        internal async Task RecognizeSummary(FileResult photo)
        {
            RecognizedSummary = await _computerVisionService.GetImageSummary(photo);
        }

        internal async Task RecognizeTags(FileResult photo)
        {
            var list = await _computerVisionService.RecognizeTags(photo);

            var text = "";
            foreach (var tag in list)
            {
                text += $"{tag.Name} (with confidence {tag.Confidence}) \n";
            }

            RecognizedTags = text;
        }

        internal async Task RecognizeObjects(FileResult photo)
        {
            var list = await _computerVisionService.RecognizeObjects(photo);

            var text = "";
            foreach (var obj in list)
            {
                text += $"{obj.ObjectProperty} (with confidence {obj.Confidence}) at location {obj.Rectangle.X}, " +
                                                $"{obj.Rectangle.X + obj.Rectangle.W}, {obj.Rectangle.Y}, {obj.Rectangle.Y + obj.Rectangle.H} \n";
            }

            RecognizedObjects = text;
        }

        internal async Task RecognizeBrands(FileResult photo)
        {
            var list = await _computerVisionService.RecognizeBrands(photo);

            var text = "";
            foreach (var brand in list)
            {
                text = $"Logo of {brand.Name} (with confidence {brand.Confidence}) \n";
            }

            RecognizedBrands = text;
        }

        internal async Task RecognizeFaces(FileResult photo)
        {
            var list = await _computerVisionService.RecognizeFaces(photo);

            var text = "";
            foreach (var face in list)
            {
                text += $"A {face.Gender} of age {face.Age} at location {face.FaceRectangle.Left}, " +
                        $"{face.FaceRectangle.Left}, {face.FaceRectangle.Top + face.FaceRectangle.Width}, " +
                        $"{face.FaceRectangle.Top + face.FaceRectangle.Height} \n";
            }

            RecognizedFaces = text;
        }
    }
}
