using System;
using System.IO;
using System.Text;

namespace VehicleModels.Models
{
    public class VehicleModel
    {
        public VehicleModel()
        {
            ModKitsTemp = new ushort[2];
        }

        public uint Hash { get; set; }
        public string ModelName { get; set; }

        public byte Type { get; set; }
        public byte WheelsCount { get; set; }
        public bool HasArmoredWindows { get; set; }
        public byte PrimaryColor { get; set; }
        public byte SecondaryColor { get; set; }
        public byte PearlColor { get; set; }
        public byte WheelsColor { get; set; }
        public byte InteriorColor { get; set; }
        public byte DashboardColor { get; set; }
        public ushort[] ModKitsTemp { get; set; }
        public ushort Extras { get; set; }
        public ushort DefaultExtras { get; set; }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Hash);
            writer.Write((byte)ModelName.Length);

            writer.Write(Encoding.UTF8.GetBytes(ModelName));
            writer.Write(Type);
            writer.Write(WheelsCount);
            writer.Write(HasArmoredWindows);
            writer.Write(PrimaryColor);
            writer.Write(SecondaryColor);
            writer.Write(PearlColor);
            writer.Write(WheelsColor);
            writer.Write(InteriorColor);
            writer.Write(DashboardColor);

            writer.Write(ModKitsTemp[0]);
            writer.Write(ModKitsTemp[1]);

            writer.Write(Extras);
            writer.Write(DefaultExtras);
        }

        public static VehicleModel Deserialize(BinaryReader reader)
        {
            if (reader.BaseStream.Position >= reader.BaseStream.Length)
            {
                return null;
            }

            var instance = new VehicleModel();

            instance.Hash = BitConverter.ToUInt32(reader.ReadBytes(4));

            var modKitLength = reader.ReadByte();

            if (modKitLength > 0)
            {
                instance.ModelName = Encoding.UTF8.GetString(reader.ReadBytes(modKitLength));

                instance.Type = reader.ReadByte();
                instance.WheelsCount = reader.ReadByte();
                instance.HasArmoredWindows = BitConverter.ToBoolean(reader.ReadBytes(1));
                instance.PrimaryColor = reader.ReadByte();
                instance.SecondaryColor = reader.ReadByte();
                instance.PearlColor = reader.ReadByte();
                instance.WheelsColor = reader.ReadByte();
                instance.InteriorColor = reader.ReadByte();
                instance.DashboardColor = reader.ReadByte();

                instance.ModKitsTemp = new ushort[2];
                instance.ModKitsTemp[0] = BitConverter.ToUInt16(reader.ReadBytes(2));
                instance.ModKitsTemp[1] = BitConverter.ToUInt16(reader.ReadBytes(2));

                instance.Extras = BitConverter.ToUInt16(reader.ReadBytes(2));
                instance.DefaultExtras = BitConverter.ToUInt16(reader.ReadBytes(2));

                return instance;
            }

            return null;
        }
    }
}
