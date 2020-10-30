using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ToDo.UI.Behaviors
{
    public class ListViewItemSelectedToCommandBehavior : BehaviorBase<ListView>
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(ListViewItemSelectedToCommandBehavior), null);

        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.ItemSelected += OnItemSelected;
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (Command != null)
            {
                if (Command.CanExecute(e.SelectedItem))
                {
                    Command.Execute(e.SelectedItem);
                }
            }
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            bindable.ItemSelected -= OnItemSelected;

            base.OnDetachingFrom(bindable);
        }
    }
}
