using ImpressaoPluginBle.Negocio.Impressao.Bluetooth;
using ImpressaoPluginBle.Negocio.Impressao.CodeBar;
using ImpressaoPluginBle.Negocio.Impressao.EscPos;
using ImpressaoPluginBle.Negocio.Impressao.QrCode;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using static ImpressaoPluginBle.Util.EnumeradoresU;

namespace TesteImpressao
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            try
            {
                var dispositivos = GerenciaImpressao.ListarDispositivosPareados();

                ListarDispositivos(dispositivos);
            }
            catch (Exception erro)
            {
                DisplayAlert("Notice", $"{erro.Message}", "OK");
            }
        }

        private async void btnAtualizarDispositivos_Clicked(object sender, EventArgs e)
        {
            var dispositivos = await GerenciaImpressao.ListarDispositivosPareadosAsync();

            ListarDispositivos(dispositivos);
        }

        private void ListarDispositivos(List<IDevice> dispositivos)
        {
            pckDispositivos.Items.Clear();
            foreach (var d in dispositivos.Where(x => !string.IsNullOrEmpty(x.Name)).ToList())
                pckDispositivos.Items.Add(d.Name);
        }

        private async void btnImprimirCodigoDeBarras_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (pckDispositivos.SelectedItem == null)
                {
                    await DisplayAlert("Atenção", "A impressora deve ser informada", "Ok");
                    pckDispositivos.Focus();
                    return;
                }

                string nomeImpressora = pckDispositivos.Items[pckDispositivos.SelectedIndex];
                string codigobarras = "03399827000000053509585612400000000586960333";

                await GerenciaImpressao.ConectarAsync(nomeImpressora, Impressora.i80mm);
                var imgCodigoBarras = await new GerenciaCodigoBarras().GerarImagemAsync(codigobarras);
                await ImpressaoEscPos.Imprimir(imgCodigoBarras);
                await GerenciaImpressao.FecharAsync();
            }
            catch (Exception erro)
            {
                await DisplayAlert("Ops", erro.Message, "Ok");
            }
        }
        
        private async void btnImprimirQrCode_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (pckDispositivos.SelectedItem == null)
                {
                    await DisplayAlert("Atenção", "A impressora deve ser informada", "Ok");
                    pckDispositivos.Focus();
                    return;
                }

                string nomeImpressora = pckDispositivos.Items[pckDispositivos.SelectedIndex];
                await GerenciaImpressao.ConectarAsync(nomeImpressora, Impressora.i80mm);
                
                string url = "https://nfce.fazenda.mg.gov.br/portalnfce/sistema/qrcode.xhtml?p=31200705984378000190650200000006331004051209|2|1|1|3332088E1390487974A8EAB1AC8202AECEE50B8B";

                var imgQrCode = await new GerenciaQrCode().GerarImagemAsync(url, Impressora.i80mm);
                await ImpressaoEscPos.Imprimir(imgQrCode);

                await GerenciaImpressao.FecharAsync();
            }
            catch (Exception erro)
            {
                await DisplayAlert("Ops", erro.Message, "Ok");
            }
        }

        private async void btnImprimirTextoGrande_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (pckDispositivos.SelectedItem == null)
                {
                    await DisplayAlert("Atenção", "A impressora deve ser informada", "Ok");
                    pckDispositivos.Focus();
                    return;
                }

                string impressora = pckDispositivos.Items[pckDispositivos.SelectedIndex];

                await GerenciaImpressao.ConectarAsync(impressora, Impressora.i80mm);

                string textoTestes = "Foi efetuado um sorteio para ver quem escolheria em primeiro lugar um desses caminhos. O primeiro sorteado escolheu, naturalmente, o Primeiro caminho. O segundo sorteado escolheu o Segundo caminho. O terceiro sorteado, sem nenhuma outra opção, aceitou o Terceiro caminho.\n\nEles partiram juntos, no mesmo horário, levando consigo apenas uma mochila contendo alimentos, agasalhos e algumas ferramentas.\n\nO Primeiro, com muita facilidade chegou rapidamente até a montanha, subiu, feliz por acreditar que seria o vencedor e quando se deparou com a Macieira Encantada sorriu de felicidade. O que ele não esperava, porém, é que ela fosse tão inatingível. Como chegar até as maçãs? Elas estavam em galhos muito altos. Não havia como subir. O tronco era muito alto também. Ele não possuía nenhum meio de chegar até lá em cima. Ficou esperando o Segundo chegar para resolverem juntos a questão.\n\nO Segundo enfrentou galhardamente a primeira situação com a qual se deparou, porém logo em seguida apareceu outra, e logo depois mais uma e mais outra, sendo algumas delas um tanto difíceis de superar. Ele acabou ficando cansado, esgotado até ficar doente, e cair prostrado. Quando se deu conta de seu péssimo estado físico, foi obrigado a retroceder e voltou para a aldeia, onde foi internado para cuidados médicos.\n\nO Terceiro teve seu primeiro teste quando acabou sua água e ele chegou a um poço. Quando puxou o balde, arrebentou a corda e ele então, rapidamente, com suas ferramentas e alguns galhos, improvisou uma escada para descer até o poço e retirar a água para saciar sua sede. Resolveu levar a escada consigo e também a corda remendada. Percebeu que estava começando a gostar muito dessa aventura.\n\nDepois de descansar, seguiu viagem e precisou atravessar um rio com uma correnteza fortíssima. Construiu, então, uma pequena jangada e com uma vara de bambu como apoio, conseguiu chegar do outro lado do rio, protegendo assim sua mochila, seus agasalhos e todo o material que levava consigo para o momento que precisasse deles, incluindo a jangada.\n\nEm um outro ponto do caminho ele teve de cortar o mato denso e passar por cima de grossos troncos. Com esses troncos ele fez rodas para facilitar o transporte do seu material, usando também a corda para puxar.\n\nE assim, sucessivamente, a cada nova situação que surgia, como ele não tinha pressa, calmamente, fazendo uso de tudo o que estava aprendendo nessa viagem e do material que, prudentemente guardara, resolvia facilmente a questão.\n\nA viagem foi longa, cheia de situações diferentes, de detalhes, e logo chegou o momento esperado, quando ele se defrontou com a Macieira Encantada. O Primeiro havia se cansado de esperar e também retornara ao povoado.\n\nO encanto da Macieira tomou conta do Terceiro. Ela era tão linda, grande, alta, brilhante. Os raios do sol incidindo nos frutos dourados irradiavam uma luz imensa que o deixou extasiado. Quanto mais olhava para a luz dourada, mais ele se sentia invadir por ela, e percebeu que todo o seu corpo parecia estar também dourado. Nesse momento ele sentiu como se uma onda de sabedoria tomasse conta de seu ser. Com essa sensação maravilhosa ele se deixou ficar, inebriado, durante longo tempo. Depois do impacto ele se pôs a trabalhar e preparou cuidadosamente, seu material, fazendo uso de todos os seus recursos. Transformou a jangada numa grande cesta, para guardar as maçãs dentro, subiu na árvore, pela escada, usou o bambu para empurrar as maçãs mais altas e mais distantes. Tudo isso e mais algumas providências que sua criatividade lhe sugeriu para facilitar seu trabalho, que havia se transformado em prazer.\n\nDepois de encher a cesta com as maçãs, e com a certeza de que poderia voltar ali quando quisesse, por ser a Macieira pródiga, ele agradeceu a Deus por ter chegado, por ter conseguido concluir seu objetivo. Agradeceu principalmente a si mesmo pela coragem e persistência na utilização de todos os seus recursos, como inteligência e criatividade.\n\nVoltou pelo caminho mais fácil, levando consigo os frutos de seu trabalho e de seus esforços, frutos esses colhidos com muita competência e merecimento. Descobriu, entre outras coisas que:\n\ntudo que apareceu em seu caminho foi útil e importante para sua vitória;\ncada uma das situações que ele resolveu, foi de grande aprendizado, não só para aquele momento, mas também para vários outros na sua vida futura;\nquando você faz do seu trabalho um prazer, suas chances de sucesso são muito maiores;\nquando seu objetivo vale a pena, não há nada que o faça desistir no meio do caminho;\na sua vitória poderia beneficiar a vida de muita gente e também servir de exemplo a outras pessoas, a quem ele poderia ensinar tudo o que aprendeu nessa trajetória.\nO resto da história vocês podem imaginar. E como toda história que se preze, viveram felizes para sempre…\n\nEu gostaria de convidar a todos que lerem essa metáfora a fazerem uma reflexão sobre seu conteúdo e acrescentar, de acordo com a sua própria experiência e compreensão do texto, novas descobertas e possíveis benefícios e aprendizado, tanto para si, quanto para outras pessoas.";

                await ImpressaoEscPos.ImprimirTextoAsync(textoTestes);

                await GerenciaImpressao.FecharAsync();
            }
            catch (Exception erro)
            {
                await DisplayAlert("Ops", erro.Message, "Ok");
            }
        }
    }
}
