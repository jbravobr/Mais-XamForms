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

        public async Task<List<Amigo>> AdicionarAmigo(List<Contact> amigos)
        {
            var listaAmigos = new List<Amigo>();

            foreach (var amigo in amigos)
            {
                listaAmigos.Add(new Amigo{ Nome = amigo.FirstName, NroTelefone = amigo.Phones.First().Number });
            }

            return listaAmigos;

//			var db = new Repositorio<Amigo>();
//
//			if (await db.InserirTodos(listaAmigos))
//				Acr.UserDialogs.UserDialogs.Instance.ShowSuccess("Contatos importados/atualizados!");
//			else
//				Acr.UserDialogs.UserDialogs.Instance.ShowError("Erro ao importar/atualizar contatos.");
        }
    }
}

