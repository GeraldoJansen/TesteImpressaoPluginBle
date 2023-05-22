using SkiaSharp;
using System;
using System.Text;
using System.Threading.Tasks;
using static ImpressaoPluginBle.Util.EnumeradoresU;

namespace ImpressaoPluginBle.Negocio.Impressao.QrCode
{
    public class GerenciaQrCode
    {
        public GerenciaQrCode() { }

        public async Task<SKBitmap> GerarImagemAsync(string url, Impressora impressora)
        {
            try
            {
                await Task.Delay(0);
                return DesenhaQrCode.DesenharImagem(url, impressora);

                //await new GerenciaImpressaoImagem().Imprimir(bmp, BluetoothService);
                //await new TesteImpressao.BoletoTeste.GerenciaImpressaoImagem().ImprimirAsync(bmp, BluetoothService);

                //await Task.Run(async () =>
                //{
                //    SKBitmap bmp = DesenhaQrCode.DesenharImagem(url, impressora);
                //    await new GerenciaImpressaoImagem().ImprimirAsync(bmp, nomeImpressora);
                //});
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public async Task<string> GerarQrCodeAsync(string texto)
        {
            StringBuilder strBuilderImpressao = new StringBuilder();
            string tamanhoModulo = $"{((char)0x1D)}{((char)0x28)}{((char)0x6B)}{((char)0x03)}{((char)0x00)}{((char)0x31)}{((char)0x43)}{((char)0x04)}";
            strBuilderImpressao.Append(tamanhoModulo);

            string nivelCorrecao = $"{((char)0x1D)}{((char)0x28)}{((char)0x6B)}{((char)0x03)}{((char)0x00)}{((char)0x31)}{((char)0x45)}{((char)0x31)}";
            strBuilderImpressao.Append(nivelCorrecao);

            char[] retMenosMais = MaiorMenorByte(texto);
            string byteMenosMais = $"{((char)0x1D)}{((char)0x28)}{((char)0x6B)}{((char)retMenosMais[1])}{((char)retMenosMais[0])}{((char)0x31)}{((char)0x50)}{((char)0x30)}{texto}";
            strBuilderImpressao.Append(byteMenosMais);

            string imprimeQrCode = $"{((char)0x1D)}{((char)0x28)}{((char)0x6B)}{((char)0x03)}{((char)0x00)}{((char)0x31)}{((char)0x51)}{((char)0x30)}";
            strBuilderImpressao.Append(imprimeQrCode);

            await Task.Delay(0);

            return strBuilderImpressao.ToString();
        }

        public async Task<string> GerarQrCode2Async(string texto)
        {
            StringBuilder strBuilderImpressao = new StringBuilder();

            int store_len = texto.Length + 3;
            byte store_pL = (byte)(store_len % 256);
            byte store_pH = (byte)(store_len / 256);

            // QR Code: Select the model
            //              Hex     1D      28      6B      04      00      31      41      n1(x32)     n2(x00) - size of model
            // set n1 [49 x31, model 1] [50 x32, model 2] [51 x33, micro qr code]
            // https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=140
            //byte[] modelQR = new byte[] { 0x1d, 0x28, 0x6b, 0x04, 0x00, 0x31, 0x41, 0x32, 0x00 };
            string modelQR = $"{((char)0x1d)}{((char)0x28)}{((char)0x6b)}{((char)0x04)}{((char)0x00)}{((char)0x31)}{((char)0x41)}{((char)0x32)}{((char)0x00)}";
            strBuilderImpressao.Append(modelQR);

            // QR Code: Set the size of module
            // Hex      1D      28      6B      03      00      31      43      n
            // n depends on the printer
            // https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=141
            //byte[] sizeQR = new byte[] { 0x1d, 0x28, 0x6b, 0x03, 0x00, 0x31, 0x43, 0x03 };
            string sizeQR = $"{((char)0x1d)}{((char)0x28)}{((char)0x6b)}{((char)0x03)}{((char)0x00)}{((char)0x31)}{((char)0x43)}{((char)0x03)}";
            strBuilderImpressao.Append(sizeQR);

            //          Hex     1D      28      6B      03      00      31      45      n
            // Set n for error correction [48 x30 -> 7%] [49 x31-> 15%] [50 x32 -> 25%] [51 x33 -> 30%]
            // https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=142
            //byte[] errorQR = new byte[] { 0x1d, 0x28, 0x6b, 0x03, 0x00, 0x31, 0x45, 0x31 };
            string errorQR = $"{((char)0x1d)}{((char)0x28)}{((char)0x6b)}{((char)0x03)}{((char)0x00)}{((char)0x31)}{((char)0x45)}{((char)0x31)}";
            strBuilderImpressao.Append(errorQR);

            // QR Code: Store the data in the symbol storage area
            // Hex      1D      28      6B      pL      pH      31      50      30      d1...dk
            // https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=143
            //                        1D          28          6B         pL          pH  cn(49->x31) fn(80->x50) m(48->x30) d1…dk
            //byte[] storeQR = new byte[] { 0x1d, 0x28, 0x6b, store_pL, store_pH, 0x31, 0x50, 0x30 };
            string storeQR = $"{((char)0x1d)}{((char)0x28)}{((char)0x6b)}{((char)store_pL)}{((char)store_pH)}{((char)0x31)}{((char)0x50)}{((char)0x30)}{texto}";
            strBuilderImpressao.Append(storeQR);

            //string dados = $"{((char)Encoding.ASCII.GetBytes(qrdata).ToArray())}";

            // QR Code: Print the symbol data in the symbol storage area
            // Hex      1D      28      6B      03      00      31      51      m
            // https://reference.epson-biz.com/modules/ref_escpos/index.php?content_id=144
            //byte[] printQR = new byte[] { 0x1d, 0x28, 0x6b, 0x03, 0x00, 0x31, 0x51, 0x30 };
            string printQR = $"{((char)0x1d)}{((char)0x28)}{((char)0x6b)}{((char)0x03)}{((char)0x00)}{((char)0x31)}{((char)0x51)}{((char)0x30)}";
            strBuilderImpressao.Append(printQR);

            // flush() runs the print job and clears out the print buffer
            //flush();

            // write() simply appends the data to the buffer
            //write(modelQR);

            //write(sizeQR);
            //write(errorQR);
            //write(storeQR);
            //write(Encoding.ASCII.GetBytes(qrdata));
            //write(printQR);

            //flush();

            await Task.Delay(0);

            return strBuilderImpressao.ToString();
        }

        private char[] MaiorMenorByte(string dados)
        {
            char[] retorno = new char[2];

            if (((dados.Length + 3) >> 8) == 0)
            {
                retorno[0] = (char)0;
            }
            else
            {
                retorno[0] = (char)((dados.Length + 3) >> 8);
            }

            if (((dados.Length + 3) & 0x00ff) == 0)
            {
                retorno[1] = (char)0;
            }
            else
            {
                retorno[1] = (char)((dados.Length + 3) & 0x00ff);
            }

            return retorno;
        }
    }
}
