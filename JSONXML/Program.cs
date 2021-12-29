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
            Console.WriteLine("Inter full path with file name.");
            string path = Console.ReadLine();

            FileWordsInfoModel fileInfo = new FileWordsInfoModel(path);

            Console.WriteLine();

            try
            {
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true
                };

                string name = fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf('.'));

                using (FileStream fs = new FileStream(name + ".json", FileMode.Create))
                {
                    await JsonSerializer.SerializeAsync(fs, fileInfo, options);
                    Console.WriteLine("Data has been saved to file");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }
    }
}