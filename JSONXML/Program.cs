using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace JSONXML
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string path = Console.ReadLine();

            FileAbout fileInfo = new FileAbout(path);

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            using (FileStream fs = new FileStream(fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf('.')) + ".json", FileMode.Create))
            {
                await JsonSerializer.SerializeAsync<FileAbout>(fs, fileInfo, options);
                Console.WriteLine("Data has been saved to file");
            }
        }
    }
}