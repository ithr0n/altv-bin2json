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
        public byte ModNumTotal { get; set; }
        public ushort Index { get; set; }

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

        public static VehicleModKit Deserialize(BinaryReader reader)
        {
            if (reader.BaseStream.Position >= reader.BaseStream.Length)
            {
                return null;
            }

            var instance = new VehicleModKit();

            instance.Index = BitConverter.ToUInt16(reader.ReadBytes(2));

            var modKitLength = reader.ReadBytes(2);

            if (modKitLength.Length > 0)
            {
                var modKitNameStringLength = BitConverter.ToInt16(modKitLength);

                instance.ModKitName = Encoding.UTF8.GetString(reader.ReadBytes(modKitNameStringLength));

                instance.ModNumTotal = reader.ReadByte();

                for (var i = 0; i < instance.ModNumTotal; i++)
                {
                    var mod = VehicleMod.Deserialize(reader);

                    instance.Mods.Add(mod);
                }
            }

            return instance;
        }
    }
}
