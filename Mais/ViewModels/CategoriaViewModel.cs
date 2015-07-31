using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

using PropertyChanged;
using System.Threading.Tasks;
using Xamarin;

namespace Mais
{
    [ImplementPropertyChanged]
    public class CategoriaViewModel
    {
        public ObservableCollection<Categoria> Categorias { get; set; }

        public CategoriaViewModel()
        {
            try
            {
                MontaCategoriasFake();
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }
        }

        public async Task MontaCategoriasFake()
        {
            try
            {
                var dbCat = new Repositorio<Categoria>();
                
                var lista = await dbCat.RetornarTodos();
                
                this.Categorias = new ObservableCollection<Categoria>(lista.OrderBy(x => x.Nome));
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }
        }

        public async Task<List<Categoria>> GetCategorias()
        {
            try
            {
                var dbCat = new Repositorio<Categoria>();
                var lista = await dbCat.RetornarTodos();
                return lista.OrderBy(x => x.Nome).ToList();
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }

            return new List<Categoria>();
        }
    }
}

