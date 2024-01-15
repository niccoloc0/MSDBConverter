using System;
using System.IO;
using ImageMagick;
class Program
{
    static void Main()
    {
        string toConvertFolderPath = "ToConvert";
        string convertedFolderPath = "Converted";

        if (Directory.Exists(toConvertFolderPath))
        {
            // Count and display the number of files inside the ToConvert folder
            int fileCount = GetFileCount(toConvertFolderPath);
            Console.WriteLine($"Contents of folder ({fileCount} files):");

            // Get all image files in the "bin" folder
            string[] imageExtensions = [".tif", ".tiff", ".jpg", ".jpeg", ".png"];
            string[] imageFiles = GetFilesWithExtensions(toConvertFolderPath, imageExtensions);

            Stampa(imageFiles);

            if (imageFiles.Length > 0)
            {
                // Ensure the "Converted" folder exists
                Directory.CreateDirectory(convertedFolderPath);

                int totalFiles = imageFiles.Length;
                int convertedFiles = 0;

                Console.WriteLine("Converting...");

                foreach (string imageFile in imageFiles)
                {
                    string filename = Path.GetFileName(imageFile);
                    //Console.Write("Converting: " + filename);

                    ConvertToJpg(imageFile, convertedFolderPath);
                    convertedFiles++;

                    // Update the progress bar
                    UpdateProgressBar(convertedFiles, totalFiles);
                }

                Console.WriteLine("\nConversion completed!");
            }
            else
            {
                Console.WriteLine($"No image files found in '{toConvertFolderPath}' folder.");
            }
        }
        else
        {
            Console.WriteLine($"'{toConvertFolderPath}' folder does not exist in the program directory.");
        }
    }

    public static void Stampa(string[] imageFiles)
    {
        foreach (string imageFile in imageFiles)
        {
            // Extract only the filename without the path
            string filename = Path.GetFileName(imageFile);
            Console.WriteLine("- " + filename);
        }
    }

    static int GetFileCount(string folderPath)
    {
        // Include SearchOption.AllDirectories to count files recursively in all subdirectories
        return Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories).Length;
    }

    static string[] GetFilesWithExtensions(string folderPath, string[] extensions)
    {
        return Directory.GetFiles(folderPath, "*.*")
                        .Where(file => extensions.Contains(Path.GetExtension(file).ToLower()))
                        .ToArray();
    }

    static void ConvertToJpg(string imagePath, string convertedFolderPath, double maxSizeInMB = 7.5, int maxDimension = 7500)
    {
        string outputFileName = Path.Combine(convertedFolderPath, Path.GetFileNameWithoutExtension(imagePath) + ".jpg");

        using var image = new MagickImage(imagePath);
        if (image.Width > maxDimension || image.Height > maxDimension)
        {
            image.Resize(maxDimension, maxDimension);
        }

        var metadata = image.GetExifProfile();
        int quality = CalculateCompressionQuality(image, maxSizeInMB);

        image.Quality = quality;
        image.Format = MagickFormat.Jpg;
        image.Write(outputFileName);
    }

    static int CalculateCompressionQuality(MagickImage image, double maxSizeInMB)
    {
        const int maxQuality = 100;
        const int minQuality = 1;
        int quality = maxQuality;

        while (true)
        {
            using var compressedImage = new MagickImage(image);
            compressedImage.Quality = quality;
            compressedImage.Format = MagickFormat.Jpg;

            using var stream = new MemoryStream();
            compressedImage.Write(stream);

            if (stream.Length > maxSizeInMB * 1024 * 1024)
            {
                quality -= 10;
                if (quality < minQuality)
                {
                    quality = minQuality;
                    break;
                }
            }
            else
            {
                break;
            }
        }
        return quality;
    }

    static void UpdateProgressBar(int completed, int total)
    {
        int progressWidth = 50;
        int progress = (int)((double)completed / total * progressWidth);

        Console.CursorLeft = 0;
        Console.Write("[");
        Console.CursorLeft = progressWidth + 1;
        Console.Write("]");
        Console.CursorLeft = 1;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("".PadRight(progress, '='));
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}