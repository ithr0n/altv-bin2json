using System;
using System.Collections.Generic;
using System.IO;

namespace VehicleMods.Models
{
    public class VehicleMod
    {
        public VehicleMod()
        {
            ModIndex = new List<ushort>();
        }

        public byte ModType { get; set; }
        public byte ModNum { get; set; }
        public List<ushort> ModIndex { get; set; }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Convert.ToByte(ModType & 0x00FF));

            writer.Write(Convert.ToByte(ModNum & 0x00FF));

            foreach (var modIndex in ModIndex)
            {
                writer.Write(modIndex);
            }
        }

        public static VehicleMod Deserialize(BinaryReader reader)
        {
            if (reader.BaseStream.Position >= reader.BaseStream.Length)
            {
                return null;
            }

            var instance = new VehicleMod();

            instance.ModType = reader.ReadByte();

            instance.ModNum = reader.ReadByte();

            for (var ii = 0; ii < instance.ModNum; ii++)
            {
                instance.ModIndex.Add(BitConverter.ToUInt16(reader.ReadBytes(2)));
            }

            return instance;
        }
    }
}
