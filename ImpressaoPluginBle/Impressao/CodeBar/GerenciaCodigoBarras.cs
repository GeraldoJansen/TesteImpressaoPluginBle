using SkiaSharp;
using System;
using System.Threading.Tasks;
using ImpressaoPluginBle.Negocio.Impressao.Codebar;

namespace ImpressaoPluginBle.Negocio.Impressao.CodeBar
{
    public class GerenciaCodigoBarras
    {
        public GerenciaCodigoBarras() { }

        public async Task<SKBitmap> GerarImagemAsync(string codigoBarras)
        {
            SKBitmap imgCodigoBarras = null;

            try
            {
                await Task.Run(() => {
                    imgCodigoBarras = new Barcode128C().Gerar(codigoBarras);//"03399827000000053509585612400000000586960101");
                });
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return imgCodigoBarras;
        }
    }
}