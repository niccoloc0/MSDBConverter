# Nico's MSDBConverter

Have you ever found yourself spending considerable time converting/compressing and individually checking a large batch of images to ensure they meet [MovieStillsDB](https://www.moviestillsdb.com/) standards before uploading? If your answer is YES, then this tool is tailor-made for you!

This tool efficiently converts and compresses multiple images simultaneously to adhere to MovieStillsDB's standards. It resizes images exceeding 7500 pixels in width or height while maintaining the original aspect ratio. Additionally, it compresses images surpassing 7.5 MB without compromising quality and seamlessly converts formats (tiff, tif, jpeg, jpg) to jpg, ensuring they are ready for hassle-free on-site uploads.

## Usage

Create two folders within the path `\bin\Release\net8.0\win-x64\publish`:

-   The first folder must be named 'ToConvert' (it's case-sensitive). This is where you should place the images you want to convert before running the tool.
-   The second folder should be named 'Converted' (also case-sensitive). This is where you'll find the converted images once the tool has completed its process.

After creating these folders, populate the 'ToConvert' folder with the images you wish to convert. Launch PowerShell or Windows Terminal from the same specified folder and execute the following command: `./MSDBConverter.exe` to initiate the tool. Let it perform its task, and in a matter of seconds, all your images will be converted and compressed. Pretty cool, right?"

## Support
Feel free to provide feedback on the tool by sending a private message to [my MovieStillsDB account](https://www.moviestillsdb.com/users/Nico).
