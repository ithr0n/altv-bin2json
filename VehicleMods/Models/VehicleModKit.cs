using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VehicleMods.Models
{
    public class VehicleModKit
    {
        public VehicleModKit()
        {
            Mods = new List<VehicleMod>();
        }

        public string ModKitName { get; set; }
        public short ModNumTotal { get; set; }
        public short Index { get; set; }

        public List<VehicleMod> Mods { get; set; }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Index);

            writer.Write((ushort)ModKitName.Length);
            writer.Write(Encoding.UTF8.GetBytes(ModKitName));

            writer.Write(Convert.ToByte(ModNumTotal & 0x00FF));

            foreach (var mod in Mods)
            {
                mod.Serialize(writer);
            }
        }

        public static VehicleModKit Deserialize(byte[] buffer, ref int indexPosition)
        {
            if (indexPosition >= buffer.Length)
            {
                return null;
            }

            var instance = new VehicleModKit();

            instance.Index = BitConverter.ToInt16(buffer, indexPosition);
            indexPosition += 2;

            var modKitLength = new byte[2];
            Array.Copy(buffer, indexPosition, modKitLength, 0, 2);
            indexPosition += 2;

            if (modKitLength.Length > 0)
            {
                var modKitNameStringLength = BitConverter.ToInt16(modKitLength);

                instance.ModKitName = Encoding.UTF8.GetString(buffer, indexPosition, modKitNameStringLength);
                indexPosition += modKitNameStringLength;

                var modNumTotal = new byte[2];
                Array.Copy(buffer, indexPosition, modNumTotal, 0, 1);
                instance.ModNumTotal = BitConverter.ToInt16(modNumTotal, 0);
                indexPosition += 1;

                for (var i = 0; i < instance.ModNumTotal; i++)
                {
                    var mod = VehicleMod.Deserialize(buffer, ref indexPosition);

                    instance.Mods.Add(mod);
                }
            }

            return instance;
        }
    }
}
