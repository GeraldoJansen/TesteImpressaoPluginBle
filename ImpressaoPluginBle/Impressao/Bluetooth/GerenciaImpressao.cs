using ImpressaoPluginBle.Modelo.Impressao;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImpressaoPluginBle.Util.EnumeradoresU;

namespace ImpressaoPluginBle.Negocio.Impressao.Bluetooth
{
    public class GerenciaImpressao
    {
        public static BluetoothLe BluetoothLe { get; set; }
        public static Impressora TipoImpressora { get; set; } = Impressora.i80mm;

        public static void PrepararObjetos()
        {
            if (BluetoothLe == null)
                BluetoothLe = new BluetoothLe();
            BluetoothLe.Adaptador.ScanMode = ScanMode.Balanced;
            BluetoothLe.Adaptador.ScanTimeout = 20000;
        }

        public static async Task<List<IDevice>> ListarDispositivosAsync()
        {
            PrepararObjetos();
            VerificarBluetoothLigado();

            if (BluetoothLe.Dispositivos == null)
                BluetoothLe.Dispositivos = new List<IDevice>();

            BluetoothLe.Dispositivos.Clear();
            BluetoothLe.Adaptador.DeviceDiscovered += (g, a) =>
            {
                BluetoothLe.Dispositivos.Add(a.Device);
            };

            // Verificar se algum dispositivo foi encontrado
            if (!BluetoothLe.BLE.Adapter.IsScanning)
            {
                await BluetoothLe.Adaptador.StartScanningForDevicesAsync();
            }
            else
                throw new Exception("Não foi possível encontrar os dispositivos bluetooth");

            await BluetoothLe.Adaptador.StopScanningForDevicesAsync();

            BluetoothLe.DispositivosProcurados = true;

            return BluetoothLe.Dispositivos;
        }

        public static List<IDevice> ListarDispositivosPareados()
        {
            PrepararObjetos();
            VerificarBluetoothLigado();

            if (BluetoothLe.Dispositivos == null)
                BluetoothLe.Dispositivos = new List<IDevice>();

            BluetoothLe.Dispositivos.Clear();

            var pareados = BluetoothLe.Adaptador.GetSystemConnectedOrPairedDevices();
            BluetoothLe.Dispositivos = pareados.ToList();

            if (BluetoothLe.Dispositivos.Count > 0)
                BluetoothLe.DispositivosProcurados = true;

            return BluetoothLe.Dispositivos;
        }

        public static async Task<List<IDevice>> ListarDispositivosPareadosAsync() => await Task.Run(() => ListarDispositivosPareados());

        public static async Task<bool> ConectarAsync(string nomeImpressora, Impressora tipoImpressora)
        {
            bool encontrou = false;

            try
            {
                TipoImpressora = tipoImpressora;
                PrepararObjetos();

                if (!BluetoothLe.DispositivosProcurados)
                    await ListarDispositivosPareadosAsync();
                else
                    VerificarBluetoothLigado();

                var lstDispositivos = BluetoothLe.Dispositivos.Where(x => $"{x.Name}".ToUpper().Equals(nomeImpressora.ToUpper())).ToList();

                if (lstDispositivos == null)
                    throw new Exception("Nenhum dispositivo encontrado");
                else if (lstDispositivos.Count == 0)
                    throw new Exception("Nenhum dispositivo encontrado");

                foreach (var dp in lstDispositivos)
                {
                    if (dp != null)
                    {
                        BluetoothLe.NomeImpressora = dp.Name;
                        BluetoothLe.Dispositivo = new Dispositivo() { ObjDispositivo = dp };
                        var parameters = new ConnectParameters(forceBleTransport: true);
                        await BluetoothLe.Adaptador.ConnectToDeviceAsync(BluetoothLe.Dispositivo.ObjDispositivo, parameters/*, cancellationToken.Token*/);
                        //device = await Adaptador.ConnectToKnownDeviceAsync(guid);

                        if (BluetoothLe.Dispositivo.ObjDispositivo != null)
                        {
                            if (BluetoothLe.Dispositivo.ObjDispositivo.State == DeviceState.Connected)
                            {
                                var servs = await BluetoothLe.Dispositivo.ObjDispositivo.GetServicesAsync();

                                if (servs != null)
                                {
                                    BluetoothLe.Dispositivo.Servicos = servs.Select(x => new Servico() { ObjServico = x }).ToList();

                                    foreach (var sv in BluetoothLe.Dispositivo.Servicos)
                                    {
                                        var chars = await sv.ObjServico.GetCharacteristicsAsync();

                                        if (chars != null)
                                            sv.Caracteristicas = chars.Select(x => new Caracteristica() { ObjCaracteristica = x }).ToList();
                                    }
                                }
                            }
                        }
                    }
                }

                encontrou = CapturarCaracteristica(BluetoothLe.Dispositivo);
            }
            catch (Exception erro)
            {
                if (erro.Message.ToUpper().Contains("gattcallback error".ToUpper()))
                    throw new Exception($"Não foi possível estabelecer conexão com o aparelho. {erro.Message}");
                else
                    throw erro;
            }

            return encontrou;
        }

        public static async Task<bool> EscreverAsync(byte[] dados)
        {
            bool resposta = false;

            try
            {
                PrepararObjetos();
                
                if (dados == null)
                    return resposta;

                VerificarBluetoothLigado();
                VerificarSeDispositivoConectado();

                var partesDadosBytes = await QuebrarBytesEmBlocosAsync(dados, 20);

                foreach (byte[] dadoBytes in partesDadosBytes)
                {
                    resposta = await BluetoothLe.CaracteristicaEscrita.ObjCaracteristica.WriteAsync(dadoBytes);
                    await Task.Delay(0); //Padrao de espera 100 Adjust the delay between chunks if needed
                }
                //var c = (IDevice)BluetoothLe.Dispositivo.ObjDispositivo.NativeDevice;
                
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return resposta;
        }

        public static async Task<bool> EscreverAsync(string texto)
        {
            bool resposta = false;

            try
            {
                if (string.IsNullOrEmpty(texto))
                    return resposta;

                var dados = Encoding.UTF8.GetBytes(texto);
                await EscreverAsync(dados);
            }
            catch (Exception erro)
            {
                throw erro;
            }

            return resposta;
        }

        public static void Fechar()
        {
            try
            {
                PrepararObjetos();
                VerificarBluetoothLigado();
                VerificarSeDispositivoConectado();

                BluetoothLe.Dispose();
            }
            catch (Exception erro)
            {
                //throw erro;
            }
        }

        public static async Task FecharAsync() => await Task.Run(() => Fechar());
        
        public static bool Conectado()
        {
            bool resposta = false;

            try
            {
                PrepararObjetos();
                VerificarBluetoothLigado();
                VerificarSeDispositivoConectado();

                resposta = true;
            }
            catch (Exception erro)
            {
                //throw erro;
            }

            return resposta;
        }

        public static async Task<bool> ConectadoAsync() => await Task.Run(() => Conectado());

        private static bool CapturarCaracteristica(Dispositivo dispositivo)
        {
            bool encontrou = false;

            try
            {
                List<Caracteristica> caracteristicas = new List<Caracteristica>();

                foreach (var servico in dispositivo.Servicos)
                {
                    caracteristicas.AddRange(servico.Caracteristicas);
                }

                var cEscrita = caracteristicas.Where(x => x.Escrita).ToList();
                var cTodas = caracteristicas.Where(x => x.Todas).ToList();

                if (cTodas.Count > 0)
                    BluetoothLe.CaracteristicaEscrita = cTodas.FirstOrDefault();
                else
                if (cEscrita.Count > 0)
                    BluetoothLe.CaracteristicaEscrita = cEscrita.FirstOrDefault();

                if (BluetoothLe.CaracteristicaEscrita != null)
                {
                    encontrou = true;
                }

                caracteristicas = null;
                cEscrita = null;
                cTodas = null;
            }
            catch (Exception erro)
            {

            }

            return encontrou;
        }

        public static void VerificarBluetoothLigado()
        {
            if (BluetoothLe.BLE.State == BluetoothState.Off)
                throw new Exception("Bluetooth não conectado");
        }

        public static bool VerificarSeDispositivoConectado()
        {
            bool resposta = false;
            string descricao = "Nenhum dispositivo conectado";
            bool falha = false;

            if (BluetoothLe.Adaptador == null)
                falha = true;
            else if (BluetoothLe.Adaptador.ConnectedDevices == null)
                falha = true;
            else if (BluetoothLe.Adaptador.ConnectedDevices.Count == 0)
                falha = true;
            else if (BluetoothLe.Dispositivo == null)
                falha = true;
            else if (BluetoothLe.Dispositivo.ObjDispositivo == null)
                falha = true;
            else if (BluetoothLe.Dispositivo.ObjDispositivo.State != DeviceState.Connected)
                falha = true;

            if (falha)
                throw new Exception(descricao);

            resposta = true;

            return resposta;
        }

        public static void ListarDadosDoDispositivo()
        {
            try
            {
                PrepararObjetos();
                VerificarBluetoothLigado();
                VerificarSeDispositivoConectado();
            }
            catch (Exception erro)
            {
                Debug.WriteLine(erro.Message);
                return;
            }

            var disp = BluetoothLe.Dispositivo.ObjDispositivo;
            var servs = BluetoothLe.Dispositivo.Servicos;

            Debug.WriteLine("= Dispositivo ===============================================================");
            Debug.WriteLine($"Dispositivo => [ID: {disp.Id}, Name: {disp.Name}, NativeDevice: {disp.NativeDevice}, State: {disp.State}, Rssi: {disp.Rssi}]");

            Debug.WriteLine("= Servicos ===============================================================");

            foreach (var ser in servs)
            {
                Debug.WriteLine($"Id: {ser.ObjServico.Id}, Name: {ser.ObjServico.Name}, IsPrimary: {ser.ObjServico.IsPrimary}, Device.Name: {ser.ObjServico.Device.Name}");

                Debug.WriteLine("= Caracteristicas ===============================================================");

                foreach (var carac in ser.Caracteristicas)
                {
                    Debug.WriteLine($"Id: {carac.ObjCaracteristica.Id}, Name: {carac.ObjCaracteristica.Name}, Uuid: {carac.ObjCaracteristica.Uuid}, Properties: {carac.ObjCaracteristica.Properties}, StringValue: {carac.ObjCaracteristica.StringValue}, CanRead: {carac.ObjCaracteristica.CanRead}, CanWrite: {carac.ObjCaracteristica.CanWrite}, CanUpdate: {carac.ObjCaracteristica.CanUpdate}");
                }

                Debug.WriteLine("================================================================");
            }

            Debug.WriteLine("================================================================");
        }

        private static async Task<List<byte[]>> QuebrarBytesEmBlocosAsync(byte[] dados, int chunkSize)
        {
            List<byte[]> chunks = new List<byte[]>();

            int start = 0;
            while (start < dados.Length)
            {
                int chunkLength = Math.Min(chunkSize, dados.Length - start);
                byte[] chunk = new byte[chunkLength];
                Array.Copy(dados, start, chunk, 0, chunkLength);
                chunks.Add(chunk);
                start += chunkSize;
            }

            await Task.Delay(0);

            return chunks;
        }
    }
}
