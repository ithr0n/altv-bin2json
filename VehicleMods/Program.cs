using AltVehiclesBin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AltVehiclesBin
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
            using var reader = new StreamReader(File.OpenRead("vehmods.json"));

            var content = reader.ReadToEnd();

            var data = JsonConvert.DeserializeObject<VehMods>(content);

            using var writer = new BinaryWriter(File.Create($"{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}_vehmods.bin"));

            data.Serialize(writer);
        }

        static void TransformBinaryToJson()
        {
            using var reader = new BinaryReader(File.OpenRead("vehmods.bin"));

            var vehmods = VehMods.Deserialize(reader);

            var json = JsonConvert.SerializeObject(vehmods, Formatting.Indented);

            File.WriteAllText("vehmods.json", json);
        }
    }
}
