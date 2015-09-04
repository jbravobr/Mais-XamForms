using System;

namespace Mais
{
    public interface IShare
    {
        void ShareStatus(string status);

        void ShareLink(string title, string status, string link);
    }
}
