using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

namespace OmniConvert.Core
{
    public class WebPConverter : IConverter
    {
        public async Task ConvertAsync(string inputPath, string outputPath)
        {
            using var image = await Image.LoadAsync(inputPath);
            await image.SaveAsync(outputPath, new WebpEncoder());
        }
    }
}