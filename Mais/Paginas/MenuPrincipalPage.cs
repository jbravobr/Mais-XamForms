using System;

using Xamarin.Forms;

namespace Mais
{
    public class MenuPrincipalPage : ContentPage
    {
        public MenuPrincipalPage()
        {
            var imgBox = new Image
            {
                Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemUICaminho("botao_de_baixo.png")),
                Aspect = Aspect.Fill
            };
            var imgBox2 = new Image
            {
                Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemUICaminho("botao_de_baixo.png")),
                Aspect = Aspect.Fill
            };
            var imgBox3 = new Image
            {
                Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemUICaminho("botao_de_baixo.png")),
                Aspect = Aspect.Fill
            };
            var imgBox4 = new Image
            {
                Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemUICaminho("botao_de_baixo.png")),
                Aspect = Aspect.Fill
            };
            var imgBox5 = new Image
            {
                Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemUICaminho("botao_de_baixo.png")),
                Aspect = Aspect.Fill
            };
            var imgBox6 = new Image
            {
                Source = ImageSource.FromResource(RetornaCaminhoImagem.GetImagemUICaminho("botao_de_baixo.png")),
                Aspect = Aspect.Fill
            };

            var lblItem1 = new Label
            {
                Text = "Conteúdo Público",
                Style = Estilos._estiloFonteMenu
                       
            };
            var lblItem2 = new Label
            {
                Text = "Conteúdo Pessoal",
                Style = Estilos._estiloFonteMenu
            };
            var lblItem3 = new Label
            {
                Text = "Criar Enquete",
                Style = Estilos._estiloFonteMenu
            };
            var lblItem4 = new Label
            {
                Text = "Importar Amigos",
                Style = Estilos._estiloFonteMenu
            };
            var lblItem5 = new Label
            {
                Text = "Buscar Conteúdo",
                Style = Estilos._estiloFonteMenu
            };
            var lblItem6 = new Label
            {
                Text = "Editar Cadastro",
                Style = Estilos._estiloFonteMenu
            };


            var box1 = new AbsoluteLayout();

            AbsoluteLayout.SetLayoutFlags(imgBox, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(imgBox, new Rectangle(0.50, 0.15, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(lblItem1, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(lblItem1, new Rectangle(0.50, 0.15, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutFlags(imgBox2, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(imgBox2, new Rectangle(0.50, 0.25, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(lblItem2, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(lblItem2, new Rectangle(0.50, 0.25, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutFlags(imgBox3, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(imgBox3, new Rectangle(0.50, 0.35, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(lblItem3, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(lblItem3, new Rectangle(0.50, 0.35, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutFlags(imgBox4, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(imgBox4, new Rectangle(0.50, 0.45, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(lblItem4, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(lblItem4, new Rectangle(0.50, 0.45, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutFlags(imgBox5, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(imgBox5, new Rectangle(0.50, 0.55, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(lblItem5, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(lblItem5, new Rectangle(0.50, 0.55, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutFlags(imgBox6, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(imgBox6, new Rectangle(0.50, 0.65, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(lblItem6, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(lblItem6, new Rectangle(0.50, 0.65, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            box1.Children.Add(imgBox);
            box1.Children.Add(imgBox2);
            box1.Children.Add(imgBox3);
            box1.Children.Add(imgBox4);
            box1.Children.Add(imgBox5);
            box1.Children.Add(imgBox6);
            box1.Children.Add(lblItem1);
            box1.Children.Add(lblItem2);
            box1.Children.Add(lblItem3);
            box1.Children.Add(lblItem4);
            box1.Children.Add(lblItem5);
            box1.Children.Add(lblItem6);

            this.Content = box1;
        }
    }
}


