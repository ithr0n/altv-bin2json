using Newtonsoft.Json;
using System;
using System.IO;
using VehicleModels.Models;

namespace VehicleModels
{
    class Program
    {
        static void Main(string[] args)
        {
            TransformBinaryToJson();
            TransformJsonToBinary();
        }

        static void TransformJsonToBinary()
        {
            using var reader = new StreamReader(File.OpenRead("vehmodels.json"));

            var content = reader.ReadToEnd();

            var data = JsonConvert.DeserializeObject<VehModels>(content);

            using var writer = new BinaryWriter(File.Create($"{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}_vehmodels.bin"));

            data.Serialize(writer);
        }

        static void TransformBinaryToJson()
        {
            using var reader = new BinaryReader(File.OpenRead("vehmodels.bin"));

            var vehmods = VehModels.Deserialize(reader);

            var json = JsonConvert.SerializeObject(vehmods, Formatting.Indented);

            File.WriteAllText("vehmodels.json", json);
        }
    }
}
