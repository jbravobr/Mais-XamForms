using System;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace Mais
{
    public static class Estilos
    {
        public static Style _estiloPadraoButton = new Style(typeof(Button))
        {
            Setters =
            {
                new Setter{ Property = Button.BackgroundColorProperty, Value = Colors._defaultColorFromHex },
                new Setter { Property = Button.TextColorProperty, Value = Color.White },
                new Setter{ Property = Button.BorderRadiusProperty, Value = 2 },
                new Setter
                {
                    Property = Button.FontFamilyProperty,
                    Value = Device.OnPlatform(
                        iOS: "Helvetica",
                        Android: "Roboto",
                        WinPhone: "Segoe"
                    )
                },
                new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.Bold },
                new Setter
                {
                    Property = Button.HorizontalOptionsProperty,
                    Value = LayoutOptions.CenterAndExpand
                },
                new Setter
                {
                    Property = Button.WidthRequestProperty,
                    Value = 140
                },
                new Setter
                {
                    Property = Button.VerticalOptionsProperty,
                    Value = LayoutOptions.Center
                },
                new Setter
                {
                    Property = Button.HeightRequestProperty,
                    Value = 50
                }
            }
        };

        public static Style _estiloPadraoButtonBackgroundColorWhite = new Style(typeof(Button))
        {
            Setters =
            {
                new Setter{ Property = Button.BackgroundColorProperty, Value = Color.White },
                new Setter { Property = Button.TextColorProperty, Value = Color.Black },
                new Setter{ Property = Button.BorderRadiusProperty, Value = 2 },
                new Setter
                {
                    Property = Button.FontFamilyProperty,
                    Value = Device.OnPlatform(
                        iOS: "Helvetica",
                        Android: "Roboto",
                        WinPhone: "Segoe"
                    )
                },
                new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.Bold },
                new Setter
                {
                    Property = Button.HorizontalOptionsProperty,
                    Value = LayoutOptions.CenterAndExpand
                },
                new Setter
                {
                    Property = Button.WidthRequestProperty,
                    Value = 140
                },
                new Setter
                {
                    Property = Button.VerticalOptionsProperty,
                    Value = LayoutOptions.Center
                },
                new Setter
                {
                    Property = Button.HeightRequestProperty,
                    Value = 50
                }
            }
        };

        public static Style _estiloPadraoButtonFonteMenor = new Style(typeof(Button))
        {
            Setters =
            {
                new Setter{ Property = Button.BackgroundColorProperty, Value = Colors._defaultColorFromHex },
                new Setter { Property = Button.TextColorProperty, Value = Color.White },
                new Setter{ Property = Button.BorderRadiusProperty, Value = 2 },
                new Setter
                {
                    Property = Button.FontFamilyProperty,
                    Value = Device.OnPlatform(
                        iOS: "Helvetica",
                        Android: "Roboto",
                        WinPhone: "Segoe"
                    )
                },
                new Setter { Property = Button.FontSizeProperty, Value = 14 },
                new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.Bold },
                new Setter
                {
                    Property = Button.HorizontalOptionsProperty,
                    Value = LayoutOptions.CenterAndExpand
                },
                new Setter
                {
                    Property = Button.WidthRequestProperty,
                    Value = 140
                },
                new Setter
                {
                    Property = Button.VerticalOptionsProperty,
                    Value = LayoutOptions.Center
                },
                new Setter
                {
                    Property = Button.HeightRequestProperty,
                    Value = 50
                }
            }
        };

        public static Style _estiloFonteEnquete = new Style(typeof(Label))
        {
            Setters =
            {
                new Setter{ Property = Label.FontSizeProperty, Value = 12 },
                new Setter { Property = Label.TextColorProperty, Value = Color.White },
                //new Setter{ Property = Label.LineBreakModeProperty, Value = LineBreakMode.WordWrap },
                new Setter
                {
                    Property = Label.FontFamilyProperty,
                    Value = Device.OnPlatform(
                        iOS: "Helvetica",
                        Android: "Roboto",
                        WinPhone: "Segoe"
                    )
                }
            }
        };

        public static Style _estiloFonteMenu = new Style(typeof(Label))
        {
            Setters =
            {
                new Setter{ Property = Label.FontSizeProperty, Value = 18 },
                new Setter { Property = Label.TextColorProperty, Value = Color.White },
                new Setter{ Property = Label.LineBreakModeProperty, Value = LineBreakMode.WordWrap },
                new Setter
                {
                    Property = Label.FontFamilyProperty,
                    Value = Device.OnPlatform(
                        iOS: "Helvetica",
                        Android: "Roboto",
                        WinPhone: "Segoe"
                    )
                }
            }
        };

        public static Style _estiloFonteSucessoResposta = new Style(typeof(Label))
        {
            Setters =
            {
                new Setter{ Property = Label.FontSizeProperty, Value = 28 },
                new Setter { Property = Label.TextColorProperty, Value = Color.White },
                new Setter{ Property = Label.LineBreakModeProperty, Value = LineBreakMode.WordWrap },
                new Setter
                {
                    Property = Label.FontFamilyProperty,
                    Value = Device.OnPlatform(
                        iOS: "Helvetica",
                        Android: "Roboto",
                        WinPhone: "Segoe"
                    )
                }
            }
        };

        public static Style _estiloFonteSucessoRespostaQuiz = new Style(typeof(Label))
        {
            Setters =
            {
                new Setter{ Property = Label.FontSizeProperty, Value = 22 },
                new Setter { Property = Label.TextColorProperty, Value = Color.White },
                new Setter{ Property = Label.LineBreakModeProperty, Value = LineBreakMode.WordWrap },
                new Setter
                {
                    Property = Label.FontFamilyProperty,
                    Value = Device.OnPlatform(
                        iOS: "Helvetica",
                        Android: "Roboto",
                        WinPhone: "Segoe"
                    )
                }
            }
        };

        public static Style _estiloFonteSucessoRespostaSucessoVoucher = new Style(typeof(Label))
        {
            Setters =
            {
                new Setter{ Property = Label.FontSizeProperty, Value = 22 },
                new Setter{ Property = Label.VerticalOptionsProperty, Value = LayoutOptions.CenterAndExpand },
                new Setter{ Property = Label.YAlignProperty, Value = TextAlignment.Center },
                new Setter { Property = Label.TextColorProperty, Value = Color.White },
                new Setter{ Property = Label.LineBreakModeProperty, Value = LineBreakMode.WordWrap },
                new Setter
                {
                    Property = Label.FontFamilyProperty,
                    Value = Device.OnPlatform(
                        iOS: "Helvetica",
                        Android: "Roboto",
                        WinPhone: "Segoe"
                    )
                }
            }
        };
		

        public static Style _estiloFonteCategorias = new Style(typeof(Label))
        {
            Setters =
            {
                new Setter{ Property = Label.FontSizeProperty, Value = 18 },
                new Setter { Property = Label.TextColorProperty, Value = Color.Black },
                new Setter
                {
                    Property = Label.FontFamilyProperty,
                    Value = Device.OnPlatform(
                        iOS: "Helvetica",
                        Android: "Roboto",
                        WinPhone: "Segoe"
                    )
                }
            }
        };
    }
}

