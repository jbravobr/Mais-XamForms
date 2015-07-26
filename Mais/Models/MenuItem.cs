using System;
using Xamarin.Forms;

namespace Mais
{
    public class MenuItem
    {
        public string Titulo { get; set; }

        public ImageSource Icone { get; set; }

        public Type TipoPagina { get; set; }

        public Action Enviar { get; set; }

        public Color Color { get; set; }
    }
}

