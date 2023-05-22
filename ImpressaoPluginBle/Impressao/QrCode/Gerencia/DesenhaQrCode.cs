using SkiaSharp;
using System;
using static ImpressaoPluginBle.Util.EnumeradoresU;

namespace ImpressaoPluginBle.Negocio.Impressao.QrCode
{
    public class DesenhaQrCode
    {
        public DesenhaQrCode() { }

        public static SKBitmap DesenharImagem(string url, Impressora impressora)
        {
            try
            {
                int alinhamentoAltura = 40;
                // Dimensão da imagem com base na cabeça de impressão
                int folhaLargura = impressora == Impressora.i80mm ? 576 : 384;//1460
                int folhaAltura = (folhaLargura / 2);// / 3;

                if (impressora == Impressora.i58mm)
                    folhaAltura += alinhamentoAltura + 40;
                else
                    folhaAltura -= alinhamentoAltura - 20;

                SKImageInfo imagemInfo = new SKImageInfo(folhaLargura, folhaAltura);

                using (SKSurface superficieFolha = SKSurface.Create(imagemInfo))
                {
                    using (SKCanvas canvasFolha = superficieFolha.Canvas)
                    {
                        // Cria ambiente de desenho
                        canvasFolha.Clear(SKColors.White);

                        using (var generator = new QRCodeGenerator())
                        {
                            // Gerador do QrCode
                            using (var qr = generator.CreateQrCode(url, ECCLevel.L))
                            {
                                int altura = imagemInfo.Height + alinhamentoAltura;
                                int largura = altura;//(imagemInfo.Width / 2);
                                int posX = imagemInfo.Rect.MidX - (largura / 2);
                                int posY = -(alinhamentoAltura / 2);

                                posX += 8;
                                posY += 10;
                                altura -= 20;
                                largura = altura;

                                var canvas = superficieFolha.Canvas;
                                canvas.Render(qr, posX, posY, largura, altura, SKColors.White);
                            }
                        }

                        canvasFolha.Save();
                        canvasFolha.Restore();

                        using (SKImage image = superficieFolha.Snapshot())
                            return SKBitmap.FromImage(image);
                    }
                }
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }
    }
}
