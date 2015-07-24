using System;
using System.Collections.ObjectModel;

using PropertyChanged;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Mais
{
	[ImplementPropertyChanged]
	public class RespostaViewModel
	{
		public ObservableCollection<Resposta> Respostas { get; set; }

		public RespostaViewModel(List<Resposta> respostas)
		{
			this.Respostas = new ObservableCollection<Resposta>(respostas);
		}

		public async Task<bool> GravarResposta(Resposta resposta)
		{
			if (resposta != null)
			{
				var db = new Repositorio<Resposta>();

				if (await db.Inserir(resposta))
				{
					var dbEnquete = new Repositorio<Enquete>();
					var enquete = (await dbEnquete.RetornarTodos()).First(e => e.PerguntaId == resposta.PerguntaId);
					enquete.EnqueteRespondida = true;
					return await dbEnquete.Atualizar(enquete);
				}

				return await Task.FromResult<bool>(false);
			}

			return await Task.FromResult<bool>(false);
		}
	}
}

