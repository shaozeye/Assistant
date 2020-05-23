using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace UILib.Controls
{
    public class IconButton:Button
    {
        static IconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata(typeof(IconButton)));
        }


        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(IconButton), new PropertyMetadata(default));


        public Visibility IsIconVisual
        {
            get { return (Visibility)GetValue(IsIconVisualProperty); }
            set { SetValue(IsIconVisualProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsIconVisual.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsIconVisualProperty =
            DependencyProperty.Register("IsIconVisual", typeof(Visibility), typeof(IconButton), new PropertyMetadata(Visibility.Visible));



    }
}
