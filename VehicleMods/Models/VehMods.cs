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

            var contentBytes = new byte[reader.BaseStream.Length];

            reader.Read(contentBytes, 0, contentBytes.Length);

            instance.MagicBytes = new byte[2];
            Array.Copy(contentBytes, 0, instance.MagicBytes, 0, 2);
            
            instance.Version = BitConverter.ToInt16(contentBytes, 2);

            var index = 4;

            VehicleModKit modKit;

            do
            {
                modKit = VehicleModKit.Deserialize(contentBytes, ref index);

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
