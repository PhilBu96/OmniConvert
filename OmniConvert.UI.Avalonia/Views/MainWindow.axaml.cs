using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System;
using System.IO;
using System.Linq;
using OmniConvert.Core;

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

            // Zielformat ermitteln
            var fmt = (FormatComboBox.SelectedItem as ComboBoxItem)?
                          .Content?.ToString()?.ToLowerInvariant();
            if (fmt != "png" && fmt != "jpg")
            {
                StatusText.Text = "Wähle bitte png oder jpg.";
                return;
            }

            // Save File Picker Optionen
            var saveOptions = new FilePickerSaveOptions
            {
                Title = "Zieldatei speichern als",
                DefaultExtension = "." + fmt,
                //InitialFileName = Path.GetFileNameWithoutExtension(input) + "." + fmt,
                SuggestedFileName = Path.GetFileNameWithoutExtension(input) + "." + fmt,
                ShowOverwritePrompt = true,
                FileTypeChoices = new[]
                {
                    new FilePickerFileType(fmt.ToUpperInvariant())
                    {
                        Patterns = new[] { "*." + fmt }
                    }
                }
            };

            // Zeige den Save-Dialog an
            var saveResult = await this.StorageProvider.SaveFilePickerAsync(saveOptions);
            var targetFile = saveResult as IStorageFile;
            var outPath = targetFile?.TryGetLocalPath();
            if (outPath is null)
            {
                StatusText.Text = "Speichern abgebrochen oder nicht möglich.";
                return;
            }

            try
            {
                // Core-Converter verwenden
                IConverter converter = new JpgPngConverter();
                await converter.ConvertAsync(input, outPath);
                StatusText.Text = $"Erstellt: {outPath}";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Fehler: {ex.Message}";
            }
        }


    }
}
