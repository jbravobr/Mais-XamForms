using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using SQLite.Net.Async;

namespace Mais
{
    public interface IBaseRepositorio<T>
    {
        Task<bool> Inserir(T entidade);

        Task<bool> InserirTodos(List<T> listaEntidade);

        Task<bool> Deletar(T entidade);

        Task<List<T>> ProcurarPorColecao(Expression<Func<T, bool>> predicado);

        Task<T> ProcurarPorFiltro(Expression<Func<T,bool>> filtro);

        Task<List<T>> RetornarTodos();

        Task<T> RetornarPorId(int pkId);

        Task<bool> Atualizar(T entidade);

        Task<bool>ExisteRegistro();

        Task<FacebookInfos> ExisteRegistroFacebook();

        Task<bool> ExisteEnquetePublica();

        Task<bool> ExisteEnqueteInteresse();

        Task<bool> ExisteBanner();

        Task<bool> InserirAsync(T entidade);
    }
}

