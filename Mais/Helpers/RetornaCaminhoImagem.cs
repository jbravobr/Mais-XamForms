using System;

namespace Mais
{
    public static class RetornaCaminhoImagem
    {
        private const string caminhoImagens = "Mais.Content.Imagens.";

        private const string caminhoIamgensUI = "Mais.Content.Imagens.UI.";

        // Retorna como String o caminho absoluto das imagens do projeto PCL.
        public static string GetImagemCaminho(string nomeArquivo)
        {
            return String.Format("{0}{1}", caminhoImagens, nomeArquivo);
        }

        public static string GetImagemUICaminho(string nomeArquivo)
        {
            return String.Format("{0}{1}", caminhoIamgensUI, nomeArquivo);
        }
    }
}

