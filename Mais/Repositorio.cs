using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;

using SQLite.Net.Async;
using SQLiteNetExtensionsAsync.Extensions;
using Xamarin.Forms;
using Xamarin;

namespace Mais
{
    public class Repositorio<T> : IBaseRepositorio<T> where T : class, new()
    {
        private readonly SQLiteAsyncConnection conn;
        private readonly AsyncLock Mutex = new AsyncLock();

        public Repositorio()
        {
            this.conn = DependencyService.Get<ISQLite>().GetConnection();
            CriarBase();
        }

        private async Task CriarBase()
        {
            try
            {
                using (await Mutex.LockAsync().ConfigureAwait(false))
                {
                    await conn.CreateTableAsync<Usuario>();
                    await conn.CreateTableAsync<Categoria>();
                    await conn.CreateTableAsync<Pergunta>();
                    await conn.CreateTableAsync<Resposta>();
                    await conn.CreateTableAsync<Enquete>();
                    await conn.CreateTableAsync<SubCategoria>();
                    await conn.CreateTableAsync<ControleSession>();
                    await conn.CreateTableAsync<Amigo>();
                    await conn.CreateTableAsync<Banner>();
                    await conn.CreateTableAsync<FacebookInfos>();
                }
            }
            catch (Exception ex)
            {
                Insights.Report(ex);
            }
        }

        ///<summary>
        /// Insere uma nova entidade T no Banco de Dados
        /// </summary>
        public async Task<bool> InserirAsync(T entity)
        {
            try
            {
                using (await Mutex.LockAsync().ConfigureAwait(false))
                {
                    try
                    {
                        await this.conn.InsertAsync(entity);
                        return await Task.FromResult<bool>(true);
                    }
                    catch (Exception ex)
                    {
                        return await Task.FromResult<bool>(false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Inserir(T entity)
        {
            try
            {
                using (await Mutex.LockAsync().ConfigureAwait(false))
                {
                    try
                    {
                        await this.conn.InsertOrReplaceWithChildrenAsync(entity, recursive : true);
                        return await Task.FromResult<bool>(true);
                    }
                    catch (Exception ex)
                    {
                        return await Task.FromResult<bool>(false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insere uma nova coleção da entidade T no Banco de Dados.
        /// </summary>
        /// <param name="listaEntity">Lista entity.</param>
        public async Task<bool> InserirTodos(List<T> listaEntity)
        {
            try
            {
                using (await Mutex.LockAsync().ConfigureAwait(false))
                {
                    try
                    {
                        await this.conn.InsertOrReplaceAllWithChildrenAsync(listaEntity, recursive: true);
                        return await Task.FromResult<bool>(true);
                    }
                    catch (Exception)
                    {
                        return await Task.FromResult<bool>(false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete uma determinada entidade T do Banco de Dados
        /// </summary>
        public async Task<bool> Deletar(T entity)
        {
            try
            {
                using (await Mutex.LockAsync().ConfigureAwait(false))
                {
                    await this.conn.DeleteAsync(entity, recursive: false);
                    return await Task.FromResult(true);
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(false);

            }
        }

        /// <summary>
        /// Retorna uma coleção de Entidades T de acordo com um predicado
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> ProcurarPorColecao(Expression<Func<T, bool>> predicado)
        {
            try
            {
                using (await Mutex.LockAsync().ConfigureAwait(false))
                {
                    return await this.conn.GetAllWithChildrenAsync<T>(predicado, recursive: true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> ProcurarPorFiltro(Expression<Func<T,bool>> expr)
        {
            try
            {
                return await this.conn.GetAsync(expr);
				
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retorna um único registro da entidade T
        /// </summary>
        /// <param name = "pkId">Chave primária para busca</param>
        /// <returns></returns>
        public async Task<T> RetornarPorId(int pkId)
        {
            try
            {
                return await this.conn.GetWithChildrenAsync<T>(pkId, recursive: true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retorna todas as Entidades T
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> RetornarTodos()
        {
            try
            {
                using (await Mutex.LockAsync().ConfigureAwait(false))
                {
                    return await this.conn.GetAllWithChildrenAsync<T>(null, recursive: true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Atualizar uma Entidade T
        /// </summary>
        /// <param name="entidade"></param>
        public async Task<bool> Atualizar(T entidade)
        {
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                try
                {
                    await this.conn.UpdateWithChildrenAsync(entidade);
                    return await Task.FromResult<bool>(true);
                }
                catch (Exception ex)
                {
                    return await Task.FromResult<bool>(false);
                }
            }
        }

        public async Task<bool> ExisteRegistro()
        {
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                try
                {
                    return await this.conn.Table<T>().CountAsync() > 0;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<bool> ExisteEnquetePublica()
        {
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                try
                {
                    return await conn.Table<Enquete>().Where(x => x.Tipo == EnumTipoEnquete.Publica).CountAsync() > 0;
                }
                catch (Exception ex)
                {
                    return await Task.FromResult(false);
                }
            }
        }

        public async Task<bool> ExisteEnqueteInteresse()
        {
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                try
                {
                    return await conn.Table<Enquete>().Where(x => x.Tipo == EnumTipoEnquete.Interesse).CountAsync() > 0;
                }
                catch (Exception ex)
                {
                    return await Task.FromResult(false);
                }
            }
        }

        public async Task<bool> ExisteMensagem()
        {
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                try
                {
                    return await conn.Table<Enquete>().Where(x => x.Tipo == EnumTipoEnquete.Mensagem).CountAsync() > 0;
                }
                catch (Exception ex)
                {
                    return await Task.FromResult(false);
                }
            }
        }

        public async Task<bool> ExistePerguntas()
        {
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                try
                {
                    return await conn.Table<Pergunta>().FirstOrDefaultAsync() != null;
                }
                catch (Exception ex)
                {
                    return await Task.FromResult(false);
                }
            }
        }

        public async Task<bool> ExisteBanner()
        {
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                try
                {
                    return await this.conn.Table<Banner>().FirstOrDefaultAsync() != null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<bool> ExisteCategoria()
        {
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                try
                {
                    return await this.conn.Table<Categoria>().FirstOrDefaultAsync() != null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}

