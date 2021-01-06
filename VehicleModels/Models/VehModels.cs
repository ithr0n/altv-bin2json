using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VehicleModels.Models
{
    public class VehModels
    {
        public VehModels()
        {
            Models = new List<VehicleModel>();
        }

        public byte[] MagicBytes { get; set; }
        public ushort Version { get; set; }
        public List<VehicleModel> Models { get; set; }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(MagicBytes);
            writer.Write(Version);
            
            foreach (var vehicleModel in Models)
            {
                vehicleModel.Serialize(writer);
            }
        }

        public static VehModels Deserialize(BinaryReader reader)
        {
            var instance = new VehModels();

            instance.MagicBytes = reader.ReadBytes(2);

            instance.Version = BitConverter.ToUInt16(reader.ReadBytes(2));
            
            VehicleModel vehicleModel;

            do
            {
                vehicleModel = VehicleModel.Deserialize(reader);

                if (vehicleModel != null)
                {
                    instance.Models.Add(vehicleModel);
                }
            }
            while (vehicleModel != null);

            return instance;
        }
    }
}
