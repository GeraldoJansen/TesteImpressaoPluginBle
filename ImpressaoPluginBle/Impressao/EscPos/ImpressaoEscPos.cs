using ImpressaoPluginBle.Negocio.Impressao.Bluetooth;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static ImpressaoPluginBle.Util.EnumeradoresU;

namespace ImpressaoPluginBle.Negocio.Impressao.EscPos
{
    public class ImpressaoEscPos
    {
        private static Impressora Impressora { get; set; } = Impressora.i80mm;

        /// <summary>
        /// Imprimi listas de textos cada item em uma linha
        /// </summary>
        /// <param name="textos">Lista de textos para imprimir</param>
        /// <returns></returns>
        public static async Task ImprimirTextosAsync(List<string> textos)
        {
            try
            {
                await VerificarBluetooth();

                // Iniciando um tipo de espaçamento
                await FonteNormal();
                await EspacoPadrao();
                await Esquerda();

                foreach (var t in textos)
                {
                    await Escrever(t);
                    await Quebra(1);
                }

                await Encerrar();
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        /// <summary>
        /// Imprimi listas de textos cada item em uma linha
        /// </summary>
        /// <param name="texto">Texto para imprimir</param>
        /// <returns></returns>
        public static async Task ImprimirTextoAsync(string texto)
        {
            try
            {
                await VerificarBluetooth();

                // Iniciando um tipo de espaçamento
                await FonteNormal();
                await EspacoPadrao();
                await Esquerda();
                await Escrever(texto);
                await Encerrar();
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public static async Task<int> ImprimirLinha()
        {
            int ret = -1;

            try
            {
                await VerificarBluetooth();

                await FonteNormal();
                ret = await Traco();
                await Encerrar();
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        public static async Task<int> ImprimirQuebra(int quantidadeQuebra)
        {
            int ret = -1;

            try
            {
                await VerificarBluetooth();

                await FonteNormal();
                ret = await Quebra(quantidadeQuebra);
                await Encerrar();
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        public static async Task<int> ImprimirLinhaPontilhada()
        {
            int ret = -1;

            try
            {
                await VerificarBluetooth();

                await FonteNormal();
                ret = await TracoLinhaPontilhada();
                await Encerrar();
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        #region Métodos de apoio para executar na impressora

        private static async Task VerificarBluetooth()
        {
            if (GerenciaImpressao.BluetoothLe == null)
                throw new Exception("A conexão precisa ser aberta para iniciar o uso da impressora");

            if (!GerenciaImpressao.Conectado())
                throw new Exception("Não foi possível conectar através do bluetooth");

            await Task.Delay(0);
        }

        private static async Task<int> PrepararEspaco(string valor, int qtdePermitida)
        {
            string valorRetorno = valor.ToUpper();
            
            return await Espaco((qtdePermitida - valor.Length));
        }

        private static async Task<int> FonteNormal()
        {
            int ret = -1;

            try
            {
                string comando = (((char)0x1B) + string.Empty) + (((char)0x21) + string.Empty) + (((char)0x00) + string.Empty);
                await GerenciaImpressao.EscreverAsync(comando);
                ret = 1;
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        private static async Task<int> FonteGrande()
        {
            int ret = -1;

            try
            {
                string comando = (((char)0x1B) + string.Empty) + (((char)0x21) + string.Empty) + (((char)0x30) + string.Empty) + (((char)0x00) + string.Empty);
                await GerenciaImpressao.EscreverAsync(comando);
                ret = 1;
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        private static async Task<int> ConfigurarIntensidade(int intencidade)
        {
            int ret = -1;

            try
            {
                //0x1b, 0x21, 0x05, 0x00
                string comando = string.Empty + ((char)0x1f) + ((char)0x1b) + ((char)0x1f) + ((char)0x41) + ((char)0x04) + ((char)0x06) + ((byte)intencidade);
                //string comando = string.Empty + new byte[] { 0x1f, 0x1b, 0x1f, 0x41, 0x04, 0x06, 0x07, 0x05 };
                //return await DependencyService.Get<IImpressao>().EnviarComando(comando);
                //port.Write(TEXT_SIZE_MEDIO, 0, TEXT_SIZE_MEDIO.Length);
                await GerenciaImpressao.EscreverAsync(comando);
                ret = 1;
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        private static async Task<int> FontePequena()
        {
            int ret = -1;

            try
            {
                //0x1b, 0x21, 0x05, 0x00
                string comando = string.Empty + ((char)0x1B) + ((char)0x21) + ((char)0x05) + ((char)0x00);

                await GerenciaImpressao.EscreverAsync(comando);
                ret = 1;
            }
            catch (Exception erro)
            {
                throw erro;
            }
            //port.Write(TEXT_SIZE_MEDIO, 0, TEXT_SIZE_MEDIO.Length);

            return ret;
        }

        //public static async Task<int> ImprimirQrCode(string texto, int tamanho)
        //{
        //    int resp = -1;

        //    try
        //    {
        //        //return await DependencyService.Get<IImpressao>().EnviarComando(comando);
        //        //ImpressaoEscPos.PrepararComunicacaoPorta();
        //        resp = await PrepararImprimirQrCode(texto, tamanho);
        //        //ImpressaoEscPos.Encerrar();
        //    }
        //    catch (Exception erro)
        //    {
        //        throw erro;
        //    }

        //    return resp;
        //}

        private static async Task<int> EspacoPadrao()
        {
            int ret = -1;

            try
            {
                string comando = (((char)0x1B) + string.Empty) + (((char)0x32) + string.Empty) + (((char)0x0D) + string.Empty);

                await GerenciaImpressao.EscreverAsync(comando);
                ret = 1;
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        private static async Task<int> Escrever(string texto)
        {
            int ret = -1;

            try
            {
                await GerenciaImpressao.EscreverAsync(texto);
                ret = 1;
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        private static async Task<int> Escrever(byte[] dados)
        {
            int ret = -1;

            try
            {
                await GerenciaImpressao.EscreverAsync(dados);
                
                ret = 1;
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        #region Código comentado
        //private static async Task<int> Escrever(byte[] buffer, int offset, int count)
        //{
        //    int ret = -1;

        //    try
        //    {
        //        //https://stackoverflow.com/questions/65246453/how-to-split-and-send-data-20-bytes-for-ble-in-xamarin-forms
        //        //await GerenciaImpressao.EscreverAsync(buffer);
        //        //await BluetoothService.ImprimirBytes(buffer, offset, count);

        //        byte[] chunk = new byte[count];
        //        Array.Copy(buffer, offset, chunk, 0, count);


        //        //byte[] senddata = Encoding.ASCII.GetBytes("Hi1290004847846767627723676");
        //        //int start = 0;
        //        //while (start < senddata.Length)
        //        //{
        //        //    int chunkLength = Math.Min(20, senddata.Length - start);
        //        //    byte[] chunk = new byte[chunkLength];
        //        //    Array.Copy(senddata, start, chunk, 0, chunkLength);
        //        //    await writeBuf.WriteAsync(chunk);
        //        //    start += 20;
        //        //}


        //        await GerenciaImpressao.EscreverAsync(chunk);
        //        await Encerrar();

        //        ret = 1;
        //    }
        //    catch (Exception erro)
        //    {
        //        throw erro;
        //    }

        //    return ret;
        //}
        
        //private static async Task<List<byte[]>> PrepararArrayEmBlocos(byte[] dados)
        //{
        //    //int blockSize = 1024; // Tamanho do bloco em bytes
        //    //int totalBlocks = (int)Math.Ceiling((double)dados.Length / blockSize);

        //    List<byte[]> byteBlocks = new List<byte[]>();

        //    //for (int i = 0; i < totalBlocks; i++)
        //    //{
        //    //    int offset = i * blockSize;
        //    //    int remainingBytes = dados.Length - offset;
        //    //    int currentBlockSize = Math.Min(blockSize, remainingBytes);

        //    //    byte[] block = new byte[currentBlockSize];
        //    //    Array.Copy(dados, offset, block, 0, currentBlockSize);
        //    //    byteBlocks.Add(block);
        //    //}

        //    //int sleep_time = 100;//100;
        //    //int block_size = 1024;// 512; // Tamanho maior conjuntos de 8
        //    //int num_blocks = (dados.Length / block_size) + (dados.Length % block_size == 0 ? 0 : 1);

        //    //for (int count = 0; count < num_blocks; count++)
        //    //{
        //    //    if (dados.Length - (count * block_size) > block_size)
        //    //    {
        //    //        int currentBlockSize = block_size;
        //    //        int offset = count * block_size;

        //    //        byte[] block = new byte[currentBlockSize];
        //    //        Array.Copy(dados, offset, block, 0, currentBlockSize);
        //    //        byteBlocks.Add(block);
        //    //        //await Escrever(dados, count * block_size, block_size);
        //    //        await Escrever(block);
        //    //    }
        //    //    else
        //    //    {
        //    //        int currentBlockSize = dados.Length - (count * block_size) - 1;
        //    //        int offset = count * block_size;

        //    //        byte[] block = new byte[currentBlockSize];
        //    //        Array.Copy(dados, offset, block, 0, currentBlockSize);
        //    //        byteBlocks.Add(block);
        //    //        //await Escrever(dados, count * block_size, dados.Length - (count * block_size) - 1);
        //    //        await Escrever(block);
        //    //    }

        //    //    await Task.Delay(sleep_time);
        //    //    //System.Threading.Thread.Sleep(sleep_time);
        //    //}

        //    //await Task.Delay(0);

        //    //int sleepTime = 100;
        //    //int blockSize = 1024;
        //    //int numBlocks = (dados.Length / blockSize) + (dados.Length % blockSize == 0 ? 0 : 1);

        //    //for (int count = 0; count < numBlocks; count++)
        //    //{
        //    //    int currentBlockSize;
        //    //    int offset = count * blockSize;

        //    //    if (dados.Length - offset > blockSize)
        //    //    {
        //    //        currentBlockSize = blockSize;
        //    //    }
        //    //    else
        //    //    {
        //    //        currentBlockSize = dados.Length - offset;
        //    //    }

        //    //    byte[] block = new byte[currentBlockSize];
        //    //    Array.Copy(dados, offset, block, 0, currentBlockSize);
        //    //    byteBlocks.Add(block);

        //    //    await Escrever(block);

        //    //    await Task.Delay(sleepTime);
        //    //}

        //    //int blockSize = 1024; // Tamanho do bloco em bytes
        //    //int num_blocks = (dados.Length / blockSize) + (dados.Length % blockSize == 0 ? 0 : 1);

        //    //for (int offset = 0; offset < dados.Length; offset += blockSize)
        //    //{
        //    //    int remainingBytes = num_blocks - offset;
        //    //    int currentBlockSize = Math.Min(blockSize, remainingBytes);

        //    //    byte[] block = new byte[currentBlockSize];
        //    //    Array.Copy(dados, offset, block, 0, currentBlockSize);

        //    //    await Escrever(block);
        //    //    await Task.Delay(10); // Aguarde um pequeno intervalo para permitir que a impressora processe os dados
        //    //}

        //    //byte[] data = dados;/* Seus dados */;
        //    //int blockSize = 1000; // Tamanho do bloco em bytes

        //    //for (int offset = 0; offset < data.Length; offset += blockSize)
        //    //{
        //    //    int remainingBytes = data.Length - offset;
        //    //    int currentBlockSize = Math.Min(blockSize, remainingBytes);

        //    //    byte[] block = new byte[currentBlockSize];
        //    //    Array.Copy(data, offset, block, 0, currentBlockSize);

        //    //    // Enviar o bloco para o dispositivo BLE
        //    //    await Escrever(block);
        //    //    //await characteristic.WriteAsync(block);
        //    //}

        //    // Obtenha as dimensões da imagem em bytes
        //    //int imageSizeInBytes = 20;

        //    // Calcule o tamanho do bloco com base nas dimensões da imagem
        //    //nt blockSize = imageSizeInBytes; // Tamanho do bloco igual ao tamanho total da imagem em bytes
        //    // Se necessário, ajuste o tamanho do bloco para atender aos requisitos do dispositivo BLE

        //    //// Envie os blocos de bytes
        //    //for (int offset = 0; offset < dados.Length; offset += blockSize)
        //    //{
        //    //    int remainingBytes = dados.Length - offset;
        //    //    int currentBlockSize = Math.Min(blockSize, remainingBytes);

        //    //    byte[] block = new byte[currentBlockSize];
        //    //    Array.Copy(dados, offset, block, 0, currentBlockSize);

        //    //    // Enviar o bloco para o dispositivo BLE
        //    //    await Escrever(block);
        //    //}

        //    //int blockSize = 1000; // Tamanho do bloco em bytes
        //    //int numBlocks = (dados.Length / blockSize) + (dados.Length % blockSize == 0 ? 0 : 1);

        //    //for (int count = 0; count < numBlocks; count++)
        //    //{
        //    //    int offset = count * blockSize;
        //    //    int currentBlockSize = Math.Min(blockSize, dados.Length - offset);

        //    //    byte[] block = new byte[currentBlockSize];
        //    //    Array.Copy(dados, offset, block, 0, currentBlockSize);

        //    //    // Enviar o bloco para a impressora térmica usando o plugin BLE
        //    //    await Escrever(block);
        //    //}


        //    //// Simulating a large data payload
        //    //byte[] data = dados; // 1 MB of data

        //    //// Splitting the data into smaller chunks
        //    //int chunkSize = 20; // Adjust this according to your needs
        //    //List<byte[]> chunks = new List<byte[]>();

        //    //for (int i = 0; i < data.Length; i += chunkSize)
        //    //{
        //    //    byte[] chunk = new byte[Math.Min(chunkSize, data.Length - i)];
        //    //    Array.Copy(data, i, chunk, 0, chunk.Length);
        //    //    chunks.Add(chunk);
        //    //}

        //    //GerenciaImpressao.BluetoothLe.CaracteristicaEscrita.ObjCaracteristica.WriteType = CharacteristicWriteType.WithoutResponse;

        //    //// Sending the data in chunks
        //    //foreach (byte[] chunk in chunks)
        //    //{
        //    //    await Escrever(chunk);
        //    //    await Task.Delay(100); // Adjust the delay between chunks if needed
        //    //}

        //    //await Encerrar();

        //    //GerenciaImpressao.BluetoothLe.CaracteristicaEscrita.ObjCaracteristica.WriteType = CharacteristicWriteType.Default;

        //    // Convert the image to bytes
        //    //byte[] imageBytes = dados;//await ConvertImageToBytes(imageSource);

        //    //// Set the maximum packet size for your device and characteristic
        //    //int maxPacketSize = 128; // Adjust this according to your device's specifications

        //    //// Sending the image bytes in larger packets
        //    //for (int i = 0; i < imageBytes.Length; i += maxPacketSize)
        //    //{
        //    //    int packetSize = Math.Min(maxPacketSize, imageBytes.Length - i);
        //    //    byte[] packet = new byte[packetSize];
        //    //    Array.Copy(imageBytes, i, packet, 0, packetSize);
        //    //    await Escrever(packet);
        //    //    //await characteristic.WriteAsync(packet);
        //    //}

        //    //byte[] data = dados; // 1 MB of data

        //    // Increase the chunk size to send more data at once
        //    //int chunkSize = 512; // Adjust this to a larger value based on your device capabilities
        //    //int numBlocks = (dados.Length / chunkSize) + (dados.Length % chunkSize == 0 ? 0 : 1);
        //    //GerenciaImpressao.BluetoothLe.CaracteristicaEscrita.ObjCaracteristica.WriteType = CharacteristicWriteType.WithoutResponse;

        //    int sleep_time = 100;//100;
        //    int block_size = 20;// 512; // Tamanho maior conjuntos de 8
        //    int num_blocks = (dados.Length / block_size) + (dados.Length % block_size == 0 ? 0 : 1);

        //    // Splitting the data into larger chunks
        //    List<byte[]> chunks = new List<byte[]>();

        //    chunks = await SplitDataIntoChunksAsync(dados, block_size);

        //    //for (int count = 0; count < num_blocks; count++)
        //    //{
        //    //    int currentBlockSize;
        //    //    int offset = count * block_size;

        //    //    if (dados.Length - offset > block_size)
        //    //        currentBlockSize = block_size;
        //    //    else
        //    //        currentBlockSize = dados.Length - offset - 1;

        //    //    //if (dados.Length - (count * block_size) > block_size)
        //    //    //    await Escrever(dados, count * block_size, block_size);
        //    //    //else
        //    //    //    await Escrever(dados, count * block_size, dados.Length - (count * block_size) - 1);

        //    //    byte[] chunk = new byte[currentBlockSize];//Math.Min(chunkSize, data.Length - i)];
        //    //    Array.Copy(dados, offset, chunk, 0, currentBlockSize);
        //    //    chunks.Add(chunk);

        //    //    //await Task.Delay(sleep_time);
        //    //}

        //    //for (int cont = 0; cont < numBlocks; cont++)
        //    //{
        //    //    int currentBlockSize;
        //    //    int offset = cont * chunkSize;

        //    //    if (dados.Length - offset > chunkSize)
        //    //    {
        //    //        currentBlockSize = chunkSize;
        //    //    }
        //    //    else
        //    //    {
        //    //        currentBlockSize = dados.Length - offset;
        //    //    }

        //    //    byte[] chunk = new byte[currentBlockSize];//Math.Min(chunkSize, data.Length - i)];
        //    //    Array.Copy(data, offset, chunk, 0, currentBlockSize);
        //    //    chunks.Add(chunk);
        //    //}

        //    // Sending the data in larger chunks
        //    foreach (byte[] chunk in chunks)
        //    {
        //        await Escrever(chunk);
        //        await Task.Delay(0); // Adjust the delay between chunks if needed
        //    }

        //    //Falha
        //    //await GerenciaImpressao.BluetoothLe.CaracteristicaEscrita.ObjCaracteristica.StartUpdatesAsync();

        //    // GerenciaImpressao.BluetoothLe.CaracteristicaEscrita.ObjCaracteristica.WriteType = CharacteristicWriteType.WithResponse;

        //    await Encerrar();

        //    return byteBlocks;
        //}

        //public static void TestandoQrCode(string data)
        //{
        //    // Gerar o código QR
        //    QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //    QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
        //    QRCode qrCode = new QRCode(qrCodeData);
        //    Bitmap qrCodeImage = qrCode.GetGraphic(20); // Ajuste o tamanho do código QR conforme necessário

        //    // Converter a imagem do código QR em dados imprimíveis
        //    byte[] imageData = ConvertBitmapToByteArray(qrCodeImage)
        //}

        //private byte[] ConvertBitmapToByteArray(Bitmap bitmap)
        //{
        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        bitmap.Save(memoryStream, ImageFormat.Bmp);
        //        return memoryStream.ToArray();
        //    }
        //}

        //private static async Task<List<byte[]>> SplitDataIntoChunks2Async(byte[] data, int chunkSize)
        //{
        //    List<byte[]> chunks = new List<byte[]>();

        //    for (int i = 0; i < data.Length; i += chunkSize)
        //    {
        //        byte[] chunk = new byte[Math.Min(chunkSize, data.Length - i)];
        //        Array.Copy(data, i, chunk, 0, chunk.Length);
        //        chunks.Add(chunk);
        //        Console.WriteLine(BitConverter.ToString(chunk));
        //    }

        //    await Task.Delay(0);

        //    return chunks;
        //}

        //private static async Task<List<byte[]>> SplitDataIntoChunksAsync(byte[] data, int chunkSize)
        //{
        //    List<byte[]> chunks = new List<byte[]>();

        //    byte[] senddata = data;
        //    int start = 0;
        //    while (start < senddata.Length)
        //    {
        //        int chunkLength = Math.Min(chunkSize, senddata.Length - start);
        //        byte[] chunk = new byte[chunkLength];
        //        Array.Copy(senddata, start, chunk, 0, chunkLength);
        //        //await writeBuf.WriteAsync(chunk);
        //        chunks.Add(chunk);
        //        start += chunkSize;
        //    }

        //    await Task.Delay(0);

        //    return chunks;
        //}

        //public static async Task SendDataAsync(byte[] data)
        //{
        //    int blocos = 20;
        //    byte[] sendString = data;//put your bytes here
        //    List<byte[]> sendChucks = new List<byte[]>();

        //    if (sendString.Length > blocos)
        //    {
        //        int totalChucks = (sendString.Length / blocos) + 1;
        //        byte[] foundChunk = new byte[blocos];
        //        int i;
        //        for (i = 0; (i < sendString.Length) && (sendString.Length - i > blocos); i += blocos)
        //        {
        //            Array.Copy(sendString, i, foundChunk, 0, blocos);
        //            sendChucks.Add(foundChunk);
        //        }
        //        if (sendString.Length - i > 0)
        //        {
        //            byte[] lastChunk = new byte[(sendString.Length - i) + 1];
        //            Array.Copy(sendString, i, lastChunk, 0, ((sendString.Length - i)));
        //            sendChucks.Add(lastChunk);
        //        }
        //    }
        //    else
        //    {
        //        sendChucks.Add(sendString);
        //    }

        //    foreach(var bytes in sendChucks)
        //        await GerenciaImpressao.EscreverAsync(bytes);

        //}

        //private static async Task<List<byte[]>> PrepararArrayEmBlocos(byte[] dados)
        //{

        //    int blockSize = 4000;//20;
        //    List<byte[]> byteBlocks = new List<byte[]>();

        //    for (int i = 0; i < dados.Length; i += blockSize)
        //    {
        //        int remainingBytes = dados.Length - i;
        //        int currentBlockSize = Math.Min(blockSize, remainingBytes);

        //        byte[] block = new byte[currentBlockSize];
        //        Array.Copy(dados, i, block, 0, currentBlockSize);
        //        byteBlocks.Add(block);
        //    }

        //    await Task.Delay(0);

        //    return byteBlocks;
        //}

        //private static async Task<List<byte[]>> PrepararArrayEmBlocos(byte[] dados)
        //{

        //    int blockSize = 1024;//20;
        //    List<byte[]> byteBlocks = new List<byte[]>();

        //    for (int i = 0; i < dados.Length; i += blockSize)
        //    {
        //        int remainingBytes = dados.Length - i;
        //        int currentBlockSize = Math.Min(blockSize, remainingBytes);

        //        byte[] block = new byte[currentBlockSize];
        //        Array.Copy(dados, i, block, 0, currentBlockSize);
        //        byteBlocks.Add(block);
        //    }

        //    int sleep_time = 100;//100;
        //    int num_blocks = (dados.Length / blockSize) + (dados.Length % blockSize == 0 ? 0 : 1);

        //    for (int count = 0; count < num_blocks; count++)
        //    {
        //        if (dados.Length - (count * blockSize) > blockSize)
        //            await Escrever(dados, count * blockSize, blockSize);
        //        else
        //            await Escrever(dados, count * blockSize, dados.Length - (count * blockSize) - 1);

        //        if (dados.Length - (count * blockSize) > blockSize)
        //        {
        //            await Escrever(dados, count * blockSize, blockSize);

        //            int remainingBytes = dados.Length - (count * blockSize);//dados.Length - count;
        //            int currentBlockSize = Math.Min(blockSize, remainingBytes);

        //            byte[] block = new byte[currentBlockSize];
        //            Array.Copy(dados, count, block, 0, currentBlockSize);
        //            byteBlocks.Add(block);
        //        }
        //        else
        //        {
        //            await Escrever(dados, count * blockSize, dados.Length - (count * blockSize) - 1);

        //            int remainingBytes = count * blockSize;//dados.Length - count;
        //            int currentBlockSize = Math.Min(blockSize, remainingBytes);

        //            byte[] block = new byte[currentBlockSize];
        //            Array.Copy(dados, count * blockSize, block, 0, currentBlockSize);
        //            byteBlocks.Add(block);
        //        }

        //        //System.Threading.Thread.Sleep(sleep_time);
        //    }

        //    await Task.Delay(0);

        //    return byteBlocks;
        //}

        //public static async Task<int> EscreverImagem(byte[] img)
        //{
        //    int ret = -1;

        //    try
        //    {
        //        var imgBlocos = await PrepararArrayEmBlocos(img);
        //        //await SendDataAsync(img);

        //        //foreach (byte[] block in imgBlocos)
        //        //{
        //        //    await Escrever(block);
        //        //    //await printerCharacteristic.WriteAsync(block);
        //        //    await Task.Delay(100); // Aguardar um curto intervalo antes de enviar o próximo bloco
        //        //}

        //        ret = 1;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //    return ret;
        //}
        #endregion;

        private static async Task<int> Esquerda()
        {
            int ret = -1;

            try
            {
                string comando = (((char)0x1B) + string.Empty) + (((char)0x61) + string.Empty) + (((char)0x00) + string.Empty);

                await GerenciaImpressao.EscreverAsync(comando);
                ret = 1;
                //return await DependencyService.Get<IImpressao>().EnviarComando(comando);
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        private static async Task<int> Centralizar()
        {
            int ret = -1;

            try
            {
                string comando = (((char)0x1B) + string.Empty) + (((char)0x61) + string.Empty) + (((char)0x01) + string.Empty);

                await GerenciaImpressao.EscreverAsync(comando);
                ret = 1;
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        private static async Task<int> Direita()
        {
            int ret = -1;

            try
            {
                string comando = (((char)0x1B) + string.Empty) + (((char)0x61) + string.Empty) + (((char)0x02) + string.Empty);

                await GerenciaImpressao.EscreverAsync(comando);
                ret = 1;
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        private static async Task<int> Negritar(bool negrito)
        {
            int ret = -1;

            try
            {
                string comando = string.Empty;

                comando += (((char)0x1B) + string.Empty) + (((char)0x45) + string.Empty);

                if (negrito)
                    comando += (((char)0x01) + string.Empty);
                else
                    comando += (((char)0x00) + string.Empty);

                await GerenciaImpressao.EscreverAsync(comando);
                ret = 1;
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        //public static async Task<int> Tabular()
        //{
        //    await Espaco(6);
        //    //port.Write(TAB1, 0, TAB1.Length);
        //}

        private static async Task<int> Encerrar()
        {
            int ret = -1;

            try
            {
                string comando = "\r\n";//((char)0x0A) + string.Empty;

                await GerenciaImpressao.EscreverAsync(comando);
                ret = 1;
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        private static async Task<int> Quebra(int qtdeQuebras)
        {
            int ret = -1;

            try
            {
                string comando = string.Empty;

                for (int i = 0; i < qtdeQuebras; i++)
                    comando += ((char)0x0A);

                await GerenciaImpressao.EscreverAsync(comando);
                ret = 1;
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        private static async Task<int> Espaco(int qtdeQuebras)
        {
            int ret = -1;

            try
            {
                string comando = string.Empty;

                for (int i = 1; i <= qtdeQuebras; i++)
                    comando += " ";

                if (!string.IsNullOrEmpty(comando))
                {
                    await GerenciaImpressao.EscreverAsync(comando);
                    ret = 1;
                }
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        //public static async Task<int> CodeBar128(string valor)
        //{
        //    string comando = ((char)0x1B) + "@" +
        //    /* Altura da barra */
        //    ((char)0x1D) + ((char)0x68) + ((char)0x4F) +//3F) +
        //    /* Largura da barra */
        //    ((char)0x1D) + ((char)0x77) + ((char)0x02) +
        //    /* Comando de impressão */
        //    ((char)0x1D) + ((char)0x6B) + ((char)0x49) + ((char)(valor.Length + 2)) + "{B" + valor +
        //    //"\n\n\n" +
        //    //"\n" +
        //    ((char)0x00);

        //    return await DependencyService.Get<IImpressao>().EnviarComando(comando);
        //}

        private static async Task<int> CodeBar128(string valor)
        {
            int ret = -1;

            try
            {
                var sb = new StringBuilder(valor.Length);
                foreach (var letra in valor)
                    if (char.IsDigit(letra)) sb.Append(letra);

                string chaveDocumento = sb.ToString();

                if (!string.IsNullOrEmpty(chaveDocumento))
                {
                    string codbarras = ((char)0x1B) +
                        "@" +
                    /* Altura da barra */
                    ((char)0x1D) + ((char)0x68) + ((char)0x61) +//3F) +
                    /* Largura da barra */
                    ((char)0x1D) + ((char)0x77) + ((char)0x02) + //((char)0x01) +
                    /* font hri character */
                    ((char)0x1D) + ((char)0x66) + ((char)0x61) +// Aumenta o tamanho da fonte do número abaixo do codigo de barra
                                                                // 0x1D   0x48      0x0                     // If print code informed
                    ((char)0x1D) + ((char)0x48) + ((char)0x02) + // ((char)0x02) = Imprimi a chave abaixo do codigo de barra
                    /* Comando de impressão */
                    ((char)0x1D) + ((char)0x6B) + ((char)0x49) + ((char)(chaveDocumento.Length // valor.Length
                    + 2
                    )) + "{C" + chaveDocumento + //valor +// "  " +
                    ((char)0x00);

                    await GerenciaImpressao.EscreverAsync(codbarras);
                    ret = 1;
                }
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        private static async Task<int> Tabular()
        {
            return await Espaco(6);
        }

        private static async Task<int> Traco()
        {
            int ret = -1;

            try
            {
                string comando = string.Empty;

                if (Impressora == Impressora.i80mm)
                    comando = "________________________________________________\n";
                else
                    comando = "________________________________\n";

                await GerenciaImpressao.EscreverAsync(comando);

                ret = 1;
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        private static async Task<int> TracoLinhaPontilhada()
        {
            int ret = -1;

            try
            {
                string comando = string.Empty;

                if (Impressora == Impressora.i80mm)
                    comando = "................................................\n";
                else
                    comando = "................................\n";

                await GerenciaImpressao.EscreverAsync(comando);

                ret = 1;
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        private static async Task<SKBitmap> ReduzirImagem(SKBitmap originalBitmap, int qualidadeImagem = 100)
        {
            SKBitmap imagemComprimida;

            try
            {
                // Crie um novo SKBitmap com as mesmas dimensões da imagem original
                using (SKBitmap resizedBitmap = new SKBitmap(originalBitmap.Width, originalBitmap.Height))
                {
                    // Copie os pixels da imagem original para o novo SKBitmap
                    originalBitmap.CopyTo(resizedBitmap);

                    // Crie um novo SKImage a partir do SKBitmap redimensionado
                    using (SKImage resizedImage = SKImage.FromBitmap(resizedBitmap))
                    {
                        // Encode a imagem redimensionada para o formato PNG com as opções de compressão
                        using (SKData compressedData = resizedImage.Encode(SKEncodedImageFormat.Png, qualidadeImagem))
                        {
                            //compressedBytes = compressedData.ToArray();
                            var imgComprimida = SKImage.FromEncodedData(compressedData);
                            imagemComprimida = SKBitmap.FromImage(imgComprimida);

                        }
                    }
                }

                await Task.Delay(0);
            }
            catch (Exception)
            {
                throw;
            }

            return imagemComprimida;
        }

        private static async Task<SKBitmap> ComprimirImagem(SKBitmap imagemOriginal, int qualidade)
        {
            // Crie um objeto SKImageInfo com as dimensões da imagem original
            SKImageInfo info = new SKImageInfo(imagemOriginal.Width, imagemOriginal.Height);

            // Crie um objeto SKBitmap para a imagem comprimida
            SKBitmap imagemComprimida = new SKBitmap(info);

            // Crie um objeto SKImage para a imagem original
            using (SKImage imagem = SKImage.FromBitmap(imagemOriginal))
            {
                // Crie um objeto SKData para armazenar a imagem comprimida
                using (SKData compressedData = imagem.Encode(SKEncodedImageFormat.Png, qualidade))
                {
                    var imgComprimida = SKImage.FromEncodedData(compressedData);
                    imagemComprimida = SKBitmap.FromImage(imgComprimida);
                    //// Decodifique a imagem comprimida para o objeto SKBitmap
                    //compressedData.ToBitmap(imagemComprimida, SKBitmapAllocFlags.ZeroPixels);
                    await Task.Delay(0);
                    // Retorne a nova imagem comprimida
                    return imagemComprimida;
                }
            }
        }

        private static async Task<byte[]> ComprimirBytesDeImagem1(SKBitmap imagemOriginal, int qualidade)
        {
            // Obtenha as informações da imagem original
            SKImageInfo info = imagemOriginal.Info;

            // Crie um objeto SKBitmap para a nova imagem reduzida
            SKBitmap imagemReduzida = new SKBitmap(info.Width, info.Height);

            // Copie o conteúdo do objeto SKBitmap original para o novo objeto SKBitmap
            imagemOriginal.CopyTo(imagemReduzida);

            // Crie um objeto SKImageInfo para a nova imagem comprimida
            SKImageInfo novaInfo = new SKImageInfo(info.Width, info.Height, info.ColorType, SKAlphaType.Premul);

            // Crie um objeto SKBitmap para a nova imagem comprimida
            using (SKBitmap novaImagem = new SKBitmap(novaInfo))
            {
                // Copie o conteúdo do objeto SKBitmap original para o novo objeto SKBitmap
                imagemOriginal.CopyTo(novaImagem);

                // Comprima a nova imagem com a qualidade desejada
                using (SKData compressedData = novaImagem.Encode(SKEncodedImageFormat.Png, qualidade))
                {
                    // Carregue a nova imagem comprimida como um novo objeto SKBitmap
                    SKBitmap imagemComprimida = SKBitmap.Decode(compressedData);

                    await Task.Delay(0);

                    // Retorne a nova imagem comprimida
                    return compressedData.ToArray();
                }
            }
        }

        private static async Task<SKBitmap> ComprimirBytesDeImagem2(SKBitmap imagemOriginal, int qualidade)
        {
            // Obtenha as informações da imagem original
            SKImageInfo info = imagemOriginal.Info;

            // Crie um objeto SKBitmap para a nova imagem reduzida
            SKBitmap imagemReduzida = new SKBitmap(info.Width, info.Height);

            // Copie o conteúdo do objeto SKBitmap original para o novo objeto SKBitmap
            imagemOriginal.CopyTo(imagemReduzida);

            // Crie um objeto SKImageInfo para a nova imagem comprimida
            SKImageInfo novaInfo = new SKImageInfo(info.Width, info.Height, info.ColorType, SKAlphaType.Premul);

            // Crie um objeto SKBitmap para a nova imagem comprimida
            using (SKBitmap novaImagem = new SKBitmap(novaInfo))
            {
                // Copie o conteúdo do objeto SKBitmap original para o novo objeto SKBitmap
                imagemOriginal.CopyTo(novaImagem);

                // Comprima a nova imagem com a qualidade desejada
                using (SKData compressedData = novaImagem.Encode(SKEncodedImageFormat.Png, qualidade))
                {
                    // Carregue a nova imagem comprimida como um novo objeto SKBitmap
                    SKBitmap imagemComprimida = SKBitmap.Decode(compressedData);

                    await Task.Delay(0);

                    // Retorne a nova imagem comprimida
                    return imagemComprimida;
                }
            }
        }

        private static SKBitmap RedimensionarImagem(SKBitmap imagemOriginal, int qualidade, int larguraMaxima, int alturaMaxima)
        {
            // Obtenha as informações da imagem original
            SKImageInfo info = imagemOriginal.Info;

            // Calcule as dimensões redimensionadas com base nas dimensões máximas
            float ratio = (float)larguraMaxima / (float)alturaMaxima;
            int novaLargura, novaAltura;
            if (info.Width > info.Height)
            {
                novaLargura = larguraMaxima;
                novaAltura = (int)(larguraMaxima / ratio);
            }
            else
            {
                novaLargura = (int)(alturaMaxima * ratio);
                novaAltura = alturaMaxima;
            }

            // Crie um objeto SKImageInfo para a nova imagem redimensionada
            SKImageInfo novaInfo = new SKImageInfo(novaLargura, novaAltura, info.ColorType, info.AlphaType);

            // Crie um objeto SKBitmap para a nova imagem redimensionada
            using (SKBitmap novaImagem = new SKBitmap(novaInfo))
            {
                // Redimensione a imagem original para a nova dimensão
                using (SKCanvas canvas = new SKCanvas(novaImagem))
                {
                    canvas.DrawBitmap(imagemOriginal, SKRect.Create(novaInfo.Width, novaInfo.Height));
                }

                // Comprima a nova imagem com a qualidade desejada
                using (SKData compressedData = novaImagem.Encode(SKEncodedImageFormat.Jpeg, qualidade))
                {
                    // Carregue a nova imagem comprimida como um novo objeto SKBitmap
                    SKBitmap imagemReduzida = SKBitmap.Decode(compressedData);

                    // Retorne a nova imagem reduzida
                    return imagemReduzida;
                }
            }
        }

        private static async Task<string> ConverterBytesParaStringHexadecimal(byte[] bytes)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in bytes)
            {
                stringBuilder.Append("\\x" + b.ToString("X2"));
            }

            await Task.Delay(0);

            return stringBuilder.ToString();
        }

        //public char[] ConverterBytesParaCharArrayEscPos(byte[] bytes)
        //{
        //    // Converter os bytes para uma string usando a codificação correta
        //    string texto = Encoding.GetEncoding("IBM437").GetString(bytes);

        //    // Converter a string em um array de caracteres
        //    char[] caracteres = texto.ToCharArray();

        //    return caracteres;
        //}
        
        private static async Task<string> ConverterBytesParaCharArrayEscPos(byte[] bytes)
        {
            await Task.Delay(0);
            // Converter os bytes para uma string usando a codificação correta
            return Encoding.GetEncoding("IBM437").GetString(bytes);
        }

        private static async Task<string> ConverterBytesParaAsciiString(byte[] bytes)
        {
            string asciiString = Encoding.ASCII.GetString(bytes);
            await Task.Delay(0);
            return asciiString;
        }

        #region metodos para Imprimir Imagem

        public static async Task<int> Imprimir(SKBitmap bmp)
        {
            int ret = -1;

            try
            {
                //await Escrever(new byte[] { 0x00, 0x00, 0x00 }, 0, new byte[] { 0x00, 0x00, 0x00 }.Length);

                byte[] img = null;
                //byte[] prnImg = null;
                //var imgCompressa = await ReduzirImagem(bmp, 50);
                //var novaImg = await ComprimirBytesDeImagem2(bmp, 10);
                //var imgCompressa = await ComprimirImagem(bmp, 50);
                //var bytesImgCompressa = await ComprimirBytesDeImagem1(bmp, 50);
                //var bytesImg = await GerarSKBitmap(bytesImgCompressa);
                img = GerarGSv0(bmp);
                //img = GerarGSv0(imgCompressa);
                //prnImg = new byte[] { };

                //var velocidadeImpressao1 = new byte[] { 0x1B, 0x37, 0x30 };
                //var velocidadeImpressao2 = new byte[] { 0x1B, 0x37, 0x31 };
                //var velocidadeImpressao3 = new byte[] { 0x1B, 0x37, 0x32 };
                //var velocidadeImpressao4 = new byte[] { 0x1B, 0x37, 0x33 };
                //var velocidadeImpressao5 = new byte[] { 0x1B, 0x37, 0x34 };

                //await Escrever(velocidadeImpressao5);

                // Comando para inicializa a impressora
                //prnImg = AddElementos(new byte[] { }, new byte[] { 0x1b, 0x40 });
                await Escrever(new byte[] { 0x1b, 0x40 });

                // Ajusta Temperatura ==> Ultimo dígito define a temperatura
                //prnImg = AddElementos(prnImg, new byte[] { 0x1f, 0x1b, 0x1f, 0x41, 0x04, 0x06, 0x07, 0x05 });
                await Escrever(new byte[] { 0x1f, 0x1b, 0x1f, 0x41, 0x04, 0x06, 0x07, 0x05 });
                //prnImg = AddElementos(prnImg, img);
                await Escrever(img);
                //prnImg = AddElementos(prnImg, new byte[] { 0x0A });
                await Escrever(new byte[] { 0x0A });

                //int sleep_time = 100;//100;
                //int block_size = 1024;// 512; // Tamanho maior conjuntos de 8
                //int num_blocks = (prnImg.Length / block_size) + (prnImg.Length % block_size == 0 ? 0 : 1);

                //string hex = await ConverterBytesParaAsciiString(prnImg);//BitConverter.ToString(prnImg);
                //await Escrever(hex);
                //await Escrever(prnImg);

                //await EscreverImagem(prnImg);

                //for (int count = 0; count < num_blocks; count++)
                //{
                //    if (prnImg.Length - (count * block_size) > block_size)
                //        await Escrever(prnImg, count * block_size, block_size);
                //    else
                //        await Escrever(prnImg, count * block_size, prnImg.Length - (count * block_size) - 1);

                //    await Task.Delay(sleep_time);
                //    //System.Threading.Thread.Sleep(sleep_time);
                //}

                //await Escrever(new byte[] { 0x00, 0x00, 0x00 }, 0, new byte[] { 0x00, 0x00, 0x00 }.Length);

                ret = 1;
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }
        
        public static async Task<int> ImprimirBkp(SKBitmap bmp)
        {
            int ret = -1;

            try
            {
                //await Escrever(new byte[] { 0x00, 0x00, 0x00 }, 0, new byte[] { 0x00, 0x00, 0x00 }.Length);

                byte[] img = null;
                byte[] prnImg = null;
                //var imgCompressa = await ReduzirImagem(bmp, 50);
                //var novaImg = await ComprimirBytesDeImagem2(bmp, 10);
                //var imgCompressa = await ComprimirImagem(bmp, 50);
                //var bytesImgCompressa = await ComprimirBytesDeImagem1(bmp, 50);
                //var bytesImg = await GerarSKBitmap(bytesImgCompressa);
                img = GerarGSv0(bmp);
                //img = GerarGSv0(imgCompressa);
                prnImg = new byte[] { };
                
                // Comando para inicializa a impressora
                prnImg = AddElementos(new byte[] { }, new byte[] { 0x1b, 0x40 });

                // Ajusta Temperatura ==> Ultimo dígito define a temperatura
                prnImg = AddElementos(prnImg, new byte[] { 0x1f, 0x1b, 0x1f, 0x41, 0x04, 0x06, 0x07, 0x05 });
                prnImg = AddElementos(prnImg, img);
                prnImg = AddElementos(prnImg, new byte[] { 0x0A });

                int sleep_time = 100;//100;
                int block_size = 1024;// 512; // Tamanho maior conjuntos de 8
                int num_blocks = (prnImg.Length / block_size) + (prnImg.Length % block_size == 0 ? 0 : 1);

                //string hex = await ConverterBytesParaAsciiString(prnImg);//BitConverter.ToString(prnImg);
                //await Escrever(hex);
                await Escrever(prnImg);

                //await EscreverImagem(prnImg);

                //for (int count = 0; count < num_blocks; count++)
                //{
                //    if (prnImg.Length - (count * block_size) > block_size)
                //        await Escrever(prnImg, count * block_size, block_size);
                //    else
                //        await Escrever(prnImg, count * block_size, prnImg.Length - (count * block_size) - 1);

                //    await Task.Delay(sleep_time);
                //    //System.Threading.Thread.Sleep(sleep_time);
                //}

                //await Escrever(new byte[] { 0x00, 0x00, 0x00 }, 0, new byte[] { 0x00, 0x00, 0x00 }.Length);

                ret = 1;
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return ret;
        }

        private static byte[] AddElementos(byte[] ori, byte[] elements)
        {
            var myArray = new byte[ori.Length + elements.Length];
            ori.CopyTo(myArray, 0);
            elements.CopyTo(myArray, ori.Length);

            return myArray;
        }

        private static byte[] GerarGSv0(SKBitmap bmp)
        {
            SKColor clr = new SKColor();
            Int64 column = 0;

            int lines_per_command = 1;

            //Cabeçalho para cada linha
            byte[] cabecalho = new byte[] { 0x1d, 0x76, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00 };

            //determina o número de cabeçalhos que serão inseridos nos comandos
            int num_cabecalho = (bmp.Height / lines_per_command) + (bmp.Height % lines_per_command == 0 ? 0 : 1);

            //determina o número de colunas de 8 bits da imagem
            int num_columns = (bmp.Width % 8 == 0 ? bmp.Width : bmp.Width + (8 - (bmp.Width % 8))) / 8;

            //Cria a matriz para guardar a imagem em formato HEX
            byte[] prnImg = new byte[(bmp.Height * num_columns) + (num_cabecalho * cabecalho.Length)];

            //Pega a imagem linha a linha
            for (int lines = 0; lines < bmp.Height; lines++)
            {
                //Cria o byte de formação da imagem
                byte column_byte = 0x00;

                if (lines == 0 || (lines % lines_per_command == 0))
                {
                    cabecalho[4] = (byte)num_columns;
                    cabecalho[5] = (byte)(num_columns >> 8);
                    int numBytes = (((lines / lines_per_command) + 1) < num_cabecalho) ? lines_per_command :
                            (bmp.Height % lines_per_command == 0 ?
                            lines_per_command : bmp.Height % lines_per_command);
                    cabecalho[6] = (byte)numBytes;
                    cabecalho[7] = (byte)(numBytes >> 8);

                    cabecalho.CopyTo(prnImg, (lines * num_columns) + ((lines / lines_per_command) * cabecalho.Length));
                    column += cabecalho.Length;
                }

                //Pega bit a bit das colunas da imagem 
                for (int columns_bits = 0; columns_bits < num_columns * 8; columns_bits++)
                {
                    if (columns_bits < bmp.Width)
                    {
                        clr = bmp.GetPixel(columns_bits, lines);

                        //Ajusta o byte para a impressora
                        if (clr.Red == 255 && clr.Green == 255 && clr.Blue == 255)
                        {
                            column_byte = (byte)((int)column_byte << 1 | 0x00);
                        }
                        else
                        {
                            column_byte = (byte)((int)column_byte << 1 | 0x01);
                        }
                    }
                    else
                    {
                        column_byte = (byte)((int)column_byte << 1 | 0x00);
                    }

                    //Quando formado o byte, insere a informação na matriz
                    if (columns_bits > 0 && (columns_bits + 1) % 8 == 0)
                    {
                        prnImg[column] = column_byte;
                        column++;
                    }
                }
            }

            return prnImg;
        }

        #endregion;

        #endregion;
    }
}
