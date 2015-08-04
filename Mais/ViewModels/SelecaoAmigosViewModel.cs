using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace Mais
{
    public class SelecaoAmigosViewModel
    {
        public ObservableCollection<Amigo> Amigos { get; set; }

        public SelecaoAmigosViewModel()
        {
        }

        public async Task RetornarAmigos()
        {
            var db = new Repositorio<Amigo>();
            var amigos = (await db.RetornarTodos()).Distinct();

            this.Amigos = new ObservableCollection<Amigo>(amigos.Distinct().OrderBy(x => x.Nome));
        }
    }
}

