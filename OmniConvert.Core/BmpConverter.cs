using System.Threading.Tasks;
using SixLabors.ImageSharp;

namespace OmniConvert.Core
{
    public class BmpConverter : IConverter
    {
        public async Task ConvertAsync(string inputPath, string outputPath)
        {
            using var image = await Image.LoadAsync(inputPath);
            await image.SaveAsync(outputPath);
        }
    }
}