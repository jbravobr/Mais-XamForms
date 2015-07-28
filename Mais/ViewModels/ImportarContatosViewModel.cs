using System;
using System.Collections.ObjectModel;
using System.Linq;
using Contacts.Plugin.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mais
{
    public class ImportarContatosViewModel
    {
        private ObservableCollection<Amigo> Amigos { get; set; }

        public ImportarContatosViewModel()
        {
            this.Amigos = new ObservableCollection<Amigo>();
        }
            
    }
}

