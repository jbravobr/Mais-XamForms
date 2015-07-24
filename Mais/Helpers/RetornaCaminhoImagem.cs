using System;

namespace Mais
{
    public static class RetornaCaminhoImagem
    {
        private const string caminhoImagens = "Mais.Content.Imagens.";

        // Retorna como String o caminho absoluto das imagens do projeto PCL.
        public static string GetImagemCaminho(string nomeArquivo)
        {
            return String.Format("{0}{1}", caminhoImagens, nomeArquivo);
        }
    }
}

