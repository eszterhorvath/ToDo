using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynamicData.Annotations;
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
    }
}