using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;

namespace ImpressaoPluginBle.Modelo.Impressao
{
    public class BluetoothLe : IDisposable
    {
        public IBluetoothLE BLE { get; set; }
        public IAdapter Adaptador { get; set; }
        public List<IDevice> Dispositivos { get; set; }
        public Dispositivo Dispositivo { get; set; }
        public Caracteristica CaracteristicaEscrita { get; set; }
        public bool DispositivosProcurados { get; set; }
        public string NomeImpressora { get; set; }

        public BluetoothLe()
        {
            DispositivosProcurados = false;
            BLE = CrossBluetoothLE.Current;
            Adaptador = CrossBluetoothLE.Current.Adapter;
            Dispositivos = new List<IDevice>();
        }

        public void Dispose()
        {
            try
            {
                if (Dispositivo != null)
                {
                    if (Dispositivo.ObjDispositivo != null)
                    {
                        try
                        {
                            bool desconectado = false;

                            do
                            {
                                Adaptador.DisconnectDeviceAsync(Dispositivo.ObjDispositivo).Wait();
                                desconectado = Dispositivo.ObjDispositivo.State == Plugin.BLE.Abstractions.DeviceState.Disconnected;

                            } while (!desconectado);
                        }
                        catch { }
                    }
                }

                if (Dispositivos != null)
                {
                    foreach (var d in Dispositivos)
                    {
                        if (d != null)
                            d.Dispose();
                    }
                }

                if (Dispositivo != null)
                    Dispositivo.Dispose();

                if (CaracteristicaEscrita != null)
                    CaracteristicaEscrita.Dispose();

                Dispositivos = null;
                Dispositivo = null;
                CaracteristicaEscrita = null;
                DispositivosProcurados = false;
                NomeImpressora = string.Empty;
            }
            catch { }
        }
    }
}
