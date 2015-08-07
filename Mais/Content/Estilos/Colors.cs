using System;
using Xamarin.Forms;

namespace Mais
{
    public static class Colors
    {

        // General
        public static readonly Color BackgroundColor = Color.FromHex("#003d69");

        // Navigation
        public static readonly Color NavigationBarColor = Color.FromHex("4A90E2");
        public static readonly Color NavigationBarTextColor = Color.White;

        // Buttons and Input Fields
        public static readonly Color ButtonBackgroundColor = Color.FromHex("A4C7F0");
        public static readonly Color ButtonTextColor = Color.White;
        public static readonly Color EntryBackgroundColor = Color.FromHex("77ABE9");
        public static readonly Color EntryTextColor = Color.White;
        public static readonly Color EntryPlaceholderTextColor = Color.White;
        public static readonly Color SeparatorColor = Color.White;

        // Cells
        public static readonly Color CellSeparatorColor = Color.FromHex("979797");

        // Cor padrão da marca Mais.
        public static Color _defaultColorFromHex
        {
            get
            { 
                return Color.FromHex("#4182d0");
            }
        }

        public static Color _defaultColorFromHexLighter
        {
            get
            {
                return Color.FromHex("#7AAFEE");
            }
        }

        public static Color _loginBackgroundColorFromHex
        {
            get
            {
                return Color.FromHex("#404040");
            }
        }

        public static Color _defaultColorDarkerFromHex
        {
            get
            {
                return Color.FromHex("#4182d0");
            }
        }
    }
}

