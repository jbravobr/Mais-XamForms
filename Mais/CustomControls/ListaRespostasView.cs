using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mais
{
    public class ListaRespostasView : ContentView
    {
        public int repostaRespondindaId { get; set; }

        RespostaViewModel model;

        public ListaRespostasView(List<Resposta> respostas, int respondida = 0)
        {
            try
            {
                this.BindingContext = model = new RespostaViewModel(respostas);
                
                var listViewRespostas = new ListView
                {
                    SeparatorVisibility = SeparatorVisibility.None,
                    BackgroundColor = Color.Transparent
                };
                listViewRespostas.ItemTemplate = respondida == 1 ? 
                new DataTemplate(typeof(ListaRespostasEnqueteRespondidaViewCell)) : 
                new DataTemplate(typeof(ListaRespostasViewCell));
                listViewRespostas.HasUnevenRows = true;
                listViewRespostas.SetBinding(ListView.ItemsSourceProperty, "Respostas");
                listViewRespostas.ItemTapped += (sender, e) =>
                {
                    this.TrataClique(e.Item);
                    ((ListView)sender).SelectedItem = null; 
                };
                
                var mainLayout = new StackLayout
                {
                    Children = { listViewRespostas },
                    HeightRequest = Acr.DeviceInfo.DeviceInfo.Instance.ScreenHeight * 3
                };
                
                this.Content = new ScrollView{ Content = mainLayout, Orientation = ScrollOrientation.Vertical };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TrataClique(object e)
        {
            try
            {
                var respostaSelecionada = (Resposta)e;
                
                if (respostaSelecionada.Respondida)
                    return;
                
                var item = model.Respostas.FirstOrDefault(c => c.Id == respostaSelecionada.Id);
                
                if (item != null && !model.Respostas.Any(r => r.Respondida))
                {
                    model.Respostas.FirstOrDefault(c => c.Id == respostaSelecionada.Id).Respondida = true;
                    MessagingCenter.Send<ListaRespostasView,List<Resposta>>(this, "Respondido", this.model.Respostas.ToList());
                }
                else if (item != null && model.Respostas.Any(r => r.Respondida) && !item.Respondida)
                {
                    model.Respostas.First(r => r.Respondida).Respondida = false;
                    model.Respostas.FirstOrDefault(r => r.Id == respostaSelecionada.Id).Respondida = true;
                    MessagingCenter.Send<ListaRespostasView,List<Resposta>>(this, "Respondido", this.model.Respostas.ToList());
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


