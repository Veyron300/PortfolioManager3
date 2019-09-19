using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PortfolioManager3.WPFUtility
{
    public class ListViewSelectionChangedBehavior
    {
        public static readonly DependencyProperty CommandProperty =
           DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ListViewSelectionChangedBehavior), new UIPropertyMetadata(null, OnCommandChanged));


        public static void SetCommand(ListView listView, ICommand command)
        {
            listView.SetValue(CommandProperty, command);
        }

        public static void OnCommandChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            ListView listView = (ListView)depObj;
            listView.SelectionChanged += ListView_SelectionChanged;

        }

        private static void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            ICommand command = (ICommand)listView.GetValue(CommandProperty);
            command.Execute(null);

            if (listView.SelectedItem != null)
                listView.ScrollIntoView(listView.SelectedItem);


            e.Handled = true;
        }
    }
}
