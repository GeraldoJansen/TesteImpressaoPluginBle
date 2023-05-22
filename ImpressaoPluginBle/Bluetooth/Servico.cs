using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;

namespace ImpressaoPluginBle.Modelo.Impressao
{
    public class Servico : IDisposable
    {
        public IService ObjServico { get; set; }
        public List<Caracteristica> Caracteristicas { get; set; }
        
        public Servico()
        {
            Caracteristicas = new List<Caracteristica>();
        }

        public void Dispose()
        {
            try
            {
                if (Caracteristicas != null)
                {
                    foreach(var ch in Caracteristicas)
                        ch.Dispose();
                }
                
                if (ObjServico != null)
                    ObjServico?.Dispose();

                Caracteristicas = null;
                ObjServico = null;
            }
            catch { }
        }
    }
}
