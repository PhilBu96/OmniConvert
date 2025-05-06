using System.Threading.Tasks;
using SixLabors.ImageSharp;
using static System.Net.Mime.MediaTypeNames;

namespace OmniConvert.Core
{
    /// <summary>
    /// Konvertiert JPG ↔ PNG mit ImageSharp.
    /// </summary>
    public class JpgPngConverter : IConverter
    {
        public async Task ConvertAsync(string inputPath, string outputPath)
        {
            // Lädt Bild (JPG oder PNG) und speichert in neuem Format
            using var image = await SixLabors.ImageSharp.Image.LoadAsync(inputPath);
            await image.SaveAsync(outputPath);
        }
    }
}
