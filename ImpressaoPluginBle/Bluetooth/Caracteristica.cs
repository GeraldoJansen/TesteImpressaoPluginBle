using Plugin.BLE.Abstractions.Contracts;
using System;

namespace ImpressaoPluginBle.Modelo.Impressao
{
    public class Caracteristica : IDisposable
    {
        public ICharacteristic ObjCaracteristica { get; set; }

        public bool Leitura
        {
            get
            {
                if (ObjCaracteristica == null)
                    return false;

                return ObjCaracteristica.CanRead;
            }
        }

        public bool Escrita
        {
            get
            {
                if (ObjCaracteristica == null)
                    return false;

                return ObjCaracteristica.CanWrite;
            }
        }

        public bool Atualiza
        {
            get
            {
                if (ObjCaracteristica == null)
                    return false;

                return ObjCaracteristica.CanUpdate;
            }
        }
        
        public bool LeituraEscrita
        {
            get
            {
                if (ObjCaracteristica == null)
                    return false;

                return ObjCaracteristica.CanWrite && ObjCaracteristica.CanRead;
            }
        }
        
        public bool Todas
        {
            get
            {
                if (ObjCaracteristica == null)
                    return false;

                return ObjCaracteristica.CanWrite && ObjCaracteristica.CanRead && ObjCaracteristica.CanUpdate;
            }
        }

        public void Dispose()
        {
            try
            {
                ObjCaracteristica = null;
            }
            catch { }
        }
    }
}
