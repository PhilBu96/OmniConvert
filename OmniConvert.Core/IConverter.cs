using System.Threading.Tasks;

namespace OmniConvert.Core
{
    /// <summary>
    /// Wandelt eine Datei von einem Format in ein anderes um.
    /// </summary>
    public interface IConverter
    {
        /// <param name="inputPath">Pfad zur Quelldatei.</param>
        /// <param name="outputPath">Pfad zur Zieldatei.</param>
        Task ConvertAsync(string inputPath, string outputPath);
    }
}
