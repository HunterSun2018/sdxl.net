using System.Drawing;
using System.Drawing.Imaging;
using StableDiffusion.NET.Helper.Images.Colors;
using StableDiffusion.NET.Helper.Images;
using SkiaSharp;

public static class ImageConversion
{
    public static unsafe SKBitmap ToBitmap(this RefImage<ColorRGB> image)
    {
        SKBitmap bitmap = new SKBitmap(image.Width, image.Height);
        IntPtr pixels = bitmap.GetPixels();
        byte* ptr = (byte*)pixels.ToPointer();
        var info = bitmap.Info;

        foreach (ReadOnlyRefEnumerable<ColorRGB> row in image.Rows)
        {

            for (int i = 0; i < row.Length; i++)
            {
                ColorRGB srcColor = row[i];

                *ptr++ = srcColor.R;   // red
                *ptr++ = srcColor.G;   // green
                *ptr++ = srcColor.B;   // blue
                *ptr++ = 0xFF;         // alpha
            }
        }

        return bitmap;
    }

    public static byte[] ToPng(this IImage image)
    {
        using SKBitmap bitmap = ToBitmap(image.AsRefImage<ColorRGB>());

        MemoryStream wStream = new MemoryStream();
        using (SKManagedWStream wstream = new SKManagedWStream(wStream))
        {
            bitmap.Encode(wstream, SKEncodedImageFormat.Png, 100);
            byte[] bytes = wStream.ToArray();

            return bytes;
        }
    }

    
    public static async Task<byte[]> test()
    {
        string url = "http://120.133.36.29:8081/1712550685.png";
        HttpClient httpClient = new HttpClient();

        try
        {
            using (Stream stream = await httpClient.GetStreamAsync(url))
            using (MemoryStream memStream = new MemoryStream())
            {
                await stream.CopyToAsync(memStream);
                memStream.Seek(0, SeekOrigin.Begin);

                SKBitmap webBitmap = SKBitmap.Decode(memStream);
                // canvasView.InvalidateSurface();

                using (MemoryStream wStream = new MemoryStream())
                using (SKManagedWStream wstream = new SKManagedWStream(wStream))
                {
                    webBitmap.Encode(wstream, SKEncodedImageFormat.Png, 100);
                    byte[] bytes = wStream.ToArray();

                    return bytes;
                }
            };
        }
        catch
        {
            throw;
        }
    }
}