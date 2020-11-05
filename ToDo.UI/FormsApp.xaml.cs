using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Forms.Views;
using ToDo.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDo.UI
{
    public partial class FormsApp : Application
    {
        public FormsApp()
        {
            InitializeComponent();

            Resources.Add
            (
                new Style(typeof(Label))
                {
                    Setters =
                    {
                        new Setter()
                        {
                            Property = Label.TextColorProperty,
                            Value = Color.WhiteSmoke
                        }
                    }
                }
            );
            Resources.Add
            (
                new Style(typeof(Button))
                {
                    Setters =
                    {
                        new Setter()
                        {
                            Property = Button.TextColorProperty,
                            Value = Color.WhiteSmoke
                        },
                        new Setter()
                        {
                            Property = Button.BackgroundColorProperty,
                            Value = "#F8485E"
                        }
                    }
                }
            );
            Resources.Add
            (
                new Style(typeof(Entry))
                {
                    Setters =
                    {
                        new Setter()
                        {
                            Property = Entry.TextColorProperty,
                            Value = Device.RuntimePlatform == Device.iOS ? Color.Black : Color.WhiteSmoke
                        }
                    }
                }
            );
            Resources.Add
            (
                new Style(typeof(Picker))
                {
                    Setters =
                    {
                        new Setter()
                        {
                            Property = Picker.TextColorProperty,
                            Value = Device.RuntimePlatform == Device.iOS ? Color.Black : Color.WhiteSmoke
                        }
                    }
                }
            );
            Resources.Add
            (
                new Style(typeof(SearchBar))
                {
                    Setters =
                    {
                        new Setter()
                        {
                            Property = SearchBar.TextColorProperty,
                            Value = Color.WhiteSmoke
                        },
                        new Setter()
                        {
                            Property = SearchBar.BackgroundColorProperty,
                            Value = Color.Gray
                        },
                        new Setter()
                        {
                            Property = SearchBar.PlaceholderColorProperty,
                            Value = Color.DimGray
                        }
                    }
                }
            );
            Resources.Add
            (
                new Style(typeof(ListView))
                {
                    Setters =
                    {
                        new Setter()
                        {
                            Property = ListView.BackgroundColorProperty,
                            Value = Color.Gray
                        }
                    }
                }
            );

            Resources.Add
            (
                new Style(typeof(NavigationPage))
                {
                    Setters =
                    {
                        new Setter()
                        {
                            Property = NavigationPage.BackgroundColorProperty,
                            Value = Color.Gray
                        },
                        new Setter()
                        {
                            Property = NavigationPage.BarBackgroundColorProperty,
                            Value = "#F8485E"
                        },
                        new Setter()
                        {
                            Property = NavigationPage.BarTextColorProperty,
                            Value = Color.WhiteSmoke
                        }
                    }
                }
            );
        }
    }
}