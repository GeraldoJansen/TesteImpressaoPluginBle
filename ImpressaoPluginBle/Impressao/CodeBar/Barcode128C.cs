using SkiaSharp;
using System;

namespace ImpressaoPluginBle.Negocio.Impressao.Codebar
{
    public class Barcode128C
    {
        // 576 largura 100 altura
        private const int larguraImgBarcode = 576;
        private const int alturaImgBarcode = 100;

        SKImageInfo imagemInfo = new SKImageInfo(larguraImgBarcode, alturaImgBarcode);
        // SKImageInfo imagemInfoCan = new SKImageInfo(canhotoLanscapeViewWidth, canhotoLanscapeViewHeight);
        //SKImageInfo imagemInfoCompleto = new SKImageInfo(lanscapeViewWidth + canhotoLanscapeViewWidth, canhotoLanscapeViewHeight);
        SKSurface superficieBoleto;
        //SKSurface superficieCanhoto;
        //SKSurface superficieCompleta;
        SKCanvas gBoleto = null;
        
        public Barcode128C()
        {
            superficieBoleto = SKSurface.Create(imagemInfo);

            //Cria ambiente de desenho
            gBoleto = superficieBoleto.Canvas; //Graphics.FromImage(bmpBoleto);
            //Pinta o fundo de branco
            gBoleto.Clear(SKColors.White);// Color.White);
        }

        private static int[,] CODE128Bars = {{2, 1, 2, 2, 2, 2},
                                             {2, 2, 2, 1, 2, 2},
                                             {2, 2, 2, 2, 2, 1},
                                             {1, 2, 1, 2, 2, 3},
                                             {1, 2, 1, 3, 2, 2},
                                             {1, 3, 1, 2, 2, 2},
                                             {1, 2, 2, 2, 1, 3},
                                             {1, 2, 2, 3, 1, 2},
                                             {1, 3, 2, 2, 1, 2},
                                             {2, 2, 1, 2, 1, 3},
                                             {2, 2, 1, 3, 1, 2},
                                             {2, 3, 1, 2, 1, 2},
                                             {1, 1, 2, 2, 3, 2},
                                             {1, 2, 2, 1, 3, 2},
                                             {1, 2, 2, 2, 3, 1},
                                             {1, 1, 3, 2, 2, 2},
                                             {1, 2, 3, 1, 2, 2},
                                             {1, 2, 3, 2, 2, 1},
                                             {2, 2, 3, 2, 1, 1},
                                             {2, 2, 1, 1, 3, 2},
                                             {2, 2, 1, 2, 3, 1},
                                             {2, 1, 3, 2, 1, 2},
                                             {2, 2, 3, 1, 1, 2},
                                             {3, 1, 2, 1, 3, 1},
                                             {3, 1, 1, 2, 2, 2},
                                             {3, 2, 1, 1, 2, 2},
                                             {3, 2, 1, 2, 2, 1},
                                             {3, 1, 2, 2, 1, 2},
                                             {3, 2, 2, 1, 1, 2},
                                             {3, 2, 2, 2, 1, 1},
                                             {2, 1, 2, 1, 2, 3},
                                             {2, 1, 2, 3, 2, 1},
                                             {2, 3, 2, 1, 2, 1},
                                             {1, 1, 1, 3, 2, 3},
                                             {1, 3, 1, 1, 2, 3},
                                             {1, 3, 1, 3, 2, 1},
                                             {1, 1, 2, 3, 1, 3},
                                             {1, 3, 2, 1, 1, 3},
                                             {1, 3, 2, 3, 1, 1},
                                             {2, 1, 1, 3, 1, 3},
                                             {2, 3, 1, 1, 1, 3},
                                             {2, 3, 1, 3, 1, 1},
                                             {1, 1, 2, 1, 3, 3},
                                             {1, 1, 2, 3, 3, 1},
                                             {1, 3, 2, 1, 3, 1},
                                             {1, 1, 3, 1, 2, 3},
                                             {1, 1, 3, 3, 2, 1},
                                             {1, 3, 3, 1, 2, 1},
                                             {3, 1, 3, 1, 2, 1},
                                             {2, 1, 1, 3, 3, 1},
                                             {2, 3, 1, 1, 3, 1},
                                             {2, 1, 3, 1, 1, 3},
                                             {2, 1, 3, 3, 1, 1},
                                             {2, 1, 3, 1, 3, 1},
                                             {3, 1, 1, 1, 2, 3},
                                             {3, 1, 1, 3, 2, 1},
                                             {3, 3, 1, 1, 2, 1},
                                             {3, 1, 2, 1, 1, 3},
                                             {3, 1, 2, 3, 1, 1},
                                             {3, 3, 2, 1, 1, 1},
                                             {3, 1, 4, 1, 1, 1},
                                             {2, 2, 1, 4, 1, 1},
                                             {4, 3, 1, 1, 1, 1},
                                             {1, 1, 1, 2, 2, 4},
                                             {1, 1, 1, 4, 2, 2},
                                             {1, 2, 1, 1, 2, 4},
                                             {1, 2, 1, 4, 2, 1},
                                             {1, 4, 1, 1, 2, 2},
                                             {1, 4, 1, 2, 2, 1},
                                             {1, 1, 2, 2, 1, 4},
                                             {1, 1, 2, 4, 1, 2},
                                             {1, 2, 2, 1, 1, 4},
                                             {1, 2, 2, 4, 1, 1},
                                             {1, 4, 2, 1, 1, 2},
                                             {1, 4, 2, 2, 1, 1},
                                             {2, 4, 1, 2, 1, 1},
                                             {2, 2, 1, 1, 1, 4},
                                             {4, 1, 3, 1, 1, 1},
                                             {2, 4, 1, 1, 1, 2},
                                             {1, 3, 4, 1, 1, 1},
                                             {1, 1, 1, 2, 4, 2},
                                             {1, 2, 1, 1, 4, 2},
                                             {1, 2, 1, 2, 4, 1},
                                             {1, 1, 4, 2, 1, 2},
                                             {1, 2, 4, 1, 1, 2},
                                             {1, 2, 4, 2, 1, 1},
                                             {4, 1, 1, 2, 1, 2},
                                             {4, 2, 1, 1, 1, 2},
                                             {4, 2, 1, 2, 1, 1},
                                             {2, 1, 2, 1, 4, 1},
                                             {2, 1, 4, 1, 2, 1},
                                             {4, 1, 2, 1, 2, 1},
                                             {1, 1, 1, 1, 4, 3},
                                             {1, 1, 1, 3, 4, 1},
                                             {1, 3, 1, 1, 4, 1},
                                             {1, 1, 4, 1, 1, 3},
                                             {1, 1, 4, 3, 1, 1},
                                             {4, 1, 1, 1, 1, 3},
                                             {4, 1, 1, 3, 1, 1},
                                             {1, 1, 3, 1, 4, 1},
                                             {1, 1, 4, 1, 3, 1},
                                             {3, 1, 1, 1, 4, 1},
                                             {4, 1, 1, 1, 3, 1},
                                             {2, 1, 1, 4, 1, 2},
                                             {2, 1, 1, 2, 1, 4},
                                             {2, 1, 1, 2, 3, 2},
                                             {2, 3, 3, 1, 1, 1},
                                             {2, 1, 1, 1, 3, 3}};

        private static int[] Code128StopPattern = new int[] { 2, 3, 3, 1, 1, 1, 2 };

        private static string[] Code128CChars = new string[] { "00", "01", "02", "03", "04", "05",
                                                               "06", "07", "08", "09", "10", "11", "12", "13", "14", "15",
                                                               "16", "17", "18", "19", "20", "21", "22", "23", "24", "25",
                                                               "26", "27", "28", "29", "30", "31", "32", "33", "34", "35",
                                                               "36", "37", "38", "39", "40", "41", "42", "43", "44", "45",
                                                               "46", "47", "48", "49", "50", "51", "52", "53", "54", "55",
                                                               "56", "57", "58", "59", "60", "61", "62", "63", "64", "65",
                                                               "66", "67", "68", "69", "70", "71", "72", "73", "74", "75",
                                                               "76", "77", "78", "79", "80", "81", "82", "83", "84", "85",
                                                               "86", "87", "88", "89", "90", "91", "92", "93", "94", "95",
                                                               "96", "97", "98", "99", "CodeB", "CodeA", "FNC1", "SCodeA",
                                                               "SCodeB", "SCodeC", "Stop", "RStop"};

        public static int[] Code128CtoBITInt(string dado, int narrow_bar)
        {
            int pos_count = 0;
            float checkSum = 0;

            if (dado.Length % 2 != 0)
            {
                return null;
            }

            //Tamanho em bits será o tamanho do dado + StartC(11 barras) + CheckSum + Code128StopPattern (13 Barras)
            int[] img = new int[(((dado.Length / 2) + 2) * 11 + 13) * narrow_bar];

            var pos = Array.IndexOf(Code128CChars, "SCodeC");
            checkSum += pos;

            for (int i = 0; i < CODE128Bars.GetLength(1); i++)
            {
                var bar_size = CODE128Bars[pos, i];
                for (int j = 0; j < bar_size; j++)
                {
                    for (int k = 0; k < narrow_bar; k++)
                    {
                        if (i % 2 == 0)
                        {
                            img[pos_count] = 1;
                        }
                        else
                        {
                            img[pos_count] = 0;
                        }

                        pos_count++;
                    }
                }
            }

            for (int d = 0; d < dado.Length; d += 2)
            {
                string dPar = dado.Substring(d, 2);
                pos = Array.IndexOf(Code128CChars, dPar);
                checkSum += (pos * ((d / 2) + 1));
                for (int i = 0; i < CODE128Bars.GetLength(1); i++)
                {
                    for (int j = 0; j < CODE128Bars[pos, i]; j++)
                    {
                        for (int k = 0; k < narrow_bar; k++)
                        {
                            if (i % 2 == 0)
                            {
                                img[pos_count] = 1;
                            }
                            else
                            {
                                img[pos_count] = 0;
                            }
                            pos_count++;
                        }
                    }
                }
            }

            pos = (int)checkSum % 103;
            for (int i = 0; i < CODE128Bars.GetLength(1); i++)
            {
                var bar_size = CODE128Bars[pos, i];
                for (int j = 0; j < bar_size; j++)
                {
                    for (int k = 0; k < narrow_bar; k++)
                    {
                        if (i % 2 == 0)
                        {
                            img[pos_count] = 1;
                        }
                        else
                        {
                            img[pos_count] = 0;
                        }
                        pos_count++;
                    }
                }
            }

            for (int i = 0; i < Code128StopPattern.Length; i++)
            {
                for (int j = 0; j < Code128StopPattern[i]; j++)
                {
                    for (int k = 0; k < narrow_bar; k++)
                    {
                        if (i % 2 == 0)
                        {
                            img[pos_count] = 1;
                        }
                        else
                        {
                            img[pos_count] = 0;
                        }

                        pos_count++;
                    }
                }
            }

            return img;
        }

        SKPaint lineCodeBar = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Black,
            StrokeWidth = 1
        };

        private void DrawBarcode(string dado, float pos_X, float pos_Y, int altura, int narrow_bar)//, int prop)
        {
            int[] codigoBarras = Code128CtoBITInt(dado, narrow_bar);

            for (int x = 0; x < codigoBarras.Length; x++)
            {
                if (codigoBarras[x] == 1)
                    gBoleto.DrawLine(pos_X + x, pos_Y, pos_X + x, pos_Y + altura, lineCodeBar);
            }
        }

        public SKBitmap Gerar(string dado)// 576 largura 100 altura
        {
            try
            {
                DrawBarcode(dado, 0, 0, alturaImgBarcode, 2);

                using (SKImage image = superficieBoleto.Snapshot())
                using (var bitmap = SKBitmap.FromImage(image))//.Decode("test.jpg"))
                {
                    return SKBitmap.FromImage(image);
                }
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }
    }
}
