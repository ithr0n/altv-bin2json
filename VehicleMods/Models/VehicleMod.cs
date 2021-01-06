using System;
using System.Collections.Generic;
using System.IO;

namespace VehicleMods.Models
{
    public class VehicleMod
    {
        public VehicleMod()
        {
            ModIndex = new List<short>();
        }

        public short ModType { get; set; }
        public short ModNum { get; set; }
        public List<short> ModIndex { get; set; }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Convert.ToByte(ModType & 0x00FF));

            writer.Write(Convert.ToByte(ModNum & 0x00FF));

            foreach (var modIndex in ModIndex)
            {
                writer.Write(modIndex);
            }
        }

        public static VehicleMod Deserialize(byte[] buffer, ref int indexPosition)
        {
            if (indexPosition >= buffer.Length)
            {
                return null;
            }

            var instance = new VehicleMod();

            var modType = new byte[2];
            Array.Copy(buffer, indexPosition, modType, 0, 1);
            instance.ModType = BitConverter.ToInt16(modType, 0);
            indexPosition += 1;

            var modNum = new byte[2];
            Array.Copy(buffer, indexPosition, modNum, 0, 1);
            instance.ModNum = BitConverter.ToInt16(modNum, 0);
            indexPosition += 1;

            for (var ii = 0; ii < instance.ModNum; ii++)
            {
                instance.ModIndex.Add(BitConverter.ToInt16(buffer, indexPosition));
                indexPosition += 2;
            }

            return instance;
        }
    }
}
