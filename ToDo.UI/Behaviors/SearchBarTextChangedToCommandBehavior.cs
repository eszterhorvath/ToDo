using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ToDo.UI.Behaviors
{
    class SearchBarTextChangedToCommandBehavior : BehaviorBase<SearchBar>
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(SearchBarTextChangedToCommandBehavior), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnAttachedTo(SearchBar bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Command != null)
            {
                if (Command.CanExecute(e.NewTextValue))
                {
                    Command.Execute(e.NewTextValue);
                }
            }
        }

        protected override void OnDetachingFrom(SearchBar bindable)
        {
            bindable.TextChanged -= OnTextChanged;

            base.OnDetachingFrom(bindable);
        }
    }
}
