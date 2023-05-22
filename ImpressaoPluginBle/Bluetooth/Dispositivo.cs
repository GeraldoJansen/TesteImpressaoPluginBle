using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;

namespace ImpressaoPluginBle.Modelo.Impressao
{
    public class Dispositivo : IDisposable
    {
        public IDevice ObjDispositivo { get; set; }
        public List<Servico> Servicos { get; set; }

        public Dispositivo()
        {
            Servicos = new List<Servico>();
        }

        public void Dispose()
        {
            try
            {
                if (Servicos != null)
                {
                    foreach (var sv in Servicos)
                        sv.Dispose();
                }

                if (ObjDispositivo != null)
                    ObjDispositivo?.Dispose();

                Servicos = null;
                ObjDispositivo = null;
            }
            catch { }
        }
    }
}
