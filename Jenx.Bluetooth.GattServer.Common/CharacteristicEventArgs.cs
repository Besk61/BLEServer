using System;

namespace Jenx.Bluetooth.GattServer.Common
{
    public class CharacteristicEventArgs : EventArgs
    {
        private string v1;
        private string v2;

        public CharacteristicEventArgs(Guid characteristicId, object value)
        {
            Characteristic = characteristicId;
            Value = value;
            //Value = "bora";
        }

        public CharacteristicEventArgs(string v1, string v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public Guid Characteristic { get; set; }

        public object Value { get; set; }
    }
}