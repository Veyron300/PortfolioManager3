using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace PortfolioManager3.WPFUtility
{
    /// <summary>
    /// Provides attached properties that allow a ViewModel to bind to a <see cref="IUIElementView"/>
    /// or <see cref="ITextBoxView"/> interface. This allows the viewmodel to set the focus 
    /// on a UIElement and set the selection on a TextBox control.
    /// <example>
    /// <![CDATA[
    /// helpers:UIElementViewBehavior.Enable ="True"
    /// helpers:UIElementViewBehavior.UIElementView ="{Binding SymbolTextBoxView, Mode=OneWayToSource}"
    /// ]]>
    /// </example>
    /// </summary>

    public static class UIElementViewBehavior
    {
        public static readonly DependencyProperty UIElementViewProperty = DependencyProperty.RegisterAttached(
            "UIElementView",
            typeof(IUIElementView),
            typeof(UIElementViewBehavior));

        public static void SetUIElementView(UIElement element, IUIElementView value)
        {
            element.SetValue(UIElementViewProperty, value);
        }

        public static IUIElementView GetUIElementView(UIElement element)
        {
            return (IUIElementView)element.GetValue(UIElementViewProperty);
        }

        public static readonly DependencyProperty EnableProperty = DependencyProperty.RegisterAttached(
            "Enable",
            typeof(bool),
            typeof(UIElementViewBehavior),
            new FrameworkPropertyMetadata(OnEnableChanged));

        public static void SetEnable(UIElement element, bool value)
        {
            element.SetValue(EnableProperty, value);
        }

        public static bool GetEnable(UIElement element)
        {
            return (bool)element.GetValue(EnableProperty);
        }


        private static void OnEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = d as UIElement;
            if (uiElement != null)
            {
                var textBox = d as TextBox;

                if (((bool)e.NewValue) == true)
                {
                    IUIElementView uiElementView = null;
                    if (textBox != null)
                    {
                        uiElementView = new TextBoxView(textBox);
                    }
                    else
                    {
                        uiElementView = new UIElementView(uiElement);
                    }

                    // invoke on dispatcher, otherwise value does not take.
                    Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                    {
                        d.SetCurrentValue(UIElementViewProperty, uiElementView);
                    }));
                }
            }
        }
    }

    public interface IUIElementView
    {
        void Focus();
    }

    public interface ITextBoxView : IUIElementView
    {

        void SelectAll();
    }

    public class UIElementView : IUIElementView
    {
        private readonly UIElement _uiElement;

        public UIElementView(UIElement uiElement)
        {
            _uiElement = uiElement;
        }

        public void Focus()
        {
            if (!_uiElement.Focusable)
            {
                UIElement focusableElement = GetVisualChildren(_uiElement).Where(uiElement => uiElement.Focusable == true).FirstOrDefault();
                if (focusableElement != null)
                {
                    focusableElement.Focus();
                }
            }
            else
            {
                _uiElement.Focus();
            }
        }

        public static IEnumerable<UIElement> GetVisualChildren(DependencyObject depObj)
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is UIElement)
                    {
                        yield return (UIElement)child;
                    }

                    foreach (UIElement childOfChild in GetVisualChildren(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }

    public class TextBoxView : ITextBoxView
    {
        private readonly TextBox _textBox = null;


        public TextBoxView(TextBox textBox)
        {
            _textBox = textBox;
        }

        public void Focus()
        {
            _textBox.Focus();
        }

        public void SelectAll()
        {
            _textBox.SelectAll();
        }
    }
}

