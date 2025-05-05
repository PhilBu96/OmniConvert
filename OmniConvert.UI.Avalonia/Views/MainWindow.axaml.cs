using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Linq;
using Image = SixLabors.ImageSharp.Image;

namespace OmniConvert.UI.Avalonia.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        private async void Browse_Click(object? sender, RoutedEventArgs e)
        {
            var opts = new FilePickerOpenOptions
            {
                AllowMultiple = false,
                Title = "Select Image",
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Images")
                    {
                        Patterns = new[] { "*.png", "*.jpg" }
                    }
                }
            };
            var sel = await this.StorageProvider.OpenFilePickerAsync(opts);
            var file = sel.FirstOrDefault() as IStorageFile;
            if (file is not null && file.TryGetLocalPath() is string path)
                InputFilePath.Text = path;
        }

        private async void Convert_Click(object? sender, RoutedEventArgs e)
        {
            var input = InputFilePath.Text;
            if (string.IsNullOrWhiteSpace(input) || !File.Exists(input))
            {
                StatusText.Text = "Bitte erst eine gültige Datei auswählen.";
                return;
            }
            var fmt = (FormatComboBox.SelectedItem as ComboBoxItem)?
                          .Content?.ToString()?.ToLowerInvariant();
            if (fmt != "png" && fmt != "jpg")
            {
                StatusText.Text = "Wähle bitte png oder jpg.";
                return;
            }
            try
            {
                var outPath = Path.ChangeExtension(input, fmt);
                using var img = await Image.LoadAsync(input);
                await img.SaveAsync(outPath);
                StatusText.Text = $"Erstellt: {outPath}";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Fehler: {ex.Message}";
            }
        }
    }
}
