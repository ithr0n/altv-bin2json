using System;
using System.Collections.Generic;
using System.IO;

namespace VehicleMods.Models
{
    public class VehMods
    {
        public VehMods()
        {
            ModKits = new List<VehicleModKit>();
        }

        public byte[] MagicBytes { get; set; }
        public short Version { get; set; }
        public List<VehicleModKit> ModKits { get; set; }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(MagicBytes);
            writer.Write(Version);

            foreach (var modKit in ModKits)
            {
                modKit.Serialize(writer);
            }
        }

        public static VehMods Deserialize(BinaryReader reader)
        {
            var instance = new VehMods();

            instance.MagicBytes = reader.ReadBytes(2);
            
            instance.Version = BitConverter.ToInt16(reader.ReadBytes(2));

            VehicleModKit modKit;

            do
            {
                modKit = VehicleModKit.Deserialize(reader);

                if (modKit != null)
                {
                    instance.ModKits.Add(modKit);
                }
            }
            while (modKit != null);

            return instance;
        }
    }
}
