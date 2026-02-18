using UnityEngine;

namespace APX.Extra.Misc
{
    public class BlurHelper
    {
#region Public Methods  
        public static Texture2D Blur(Texture2D srcTex, int radius, int iterations, bool disposeSourceTexture)
        {
            Texture2D blurredTex = new Texture2D(srcTex.width, srcTex.height, srcTex.format, false);

            Texture2D tmpTex;
            for (var i = 0; i < iterations; i++)
            {
                if (i > 0)
                {
                    //swap textures
                    tmpTex = srcTex;
                    srcTex = blurredTex;
                    blurredTex = tmpTex;
                }

                OneDimensialBlur(srcTex, blurredTex, radius, true);

                //swap textures
                tmpTex = srcTex;
                srcTex = blurredTex;
                blurredTex = tmpTex;
                OneDimensialBlur(srcTex, blurredTex, radius, false);
            }

            if (disposeSourceTexture)
            {
                Object.Destroy(srcTex);
            }

            return blurredTex;
        }

        public static Texture2D OneDimensialBlur(Texture2D srcTex, int radius, bool horizontal, bool disposeSourceTexture)
        {
            Texture2D blurred = new Texture2D(srcTex.width, srcTex.height, srcTex.format, false);

            OneDimensialBlur(srcTex, blurred, radius, horizontal);
            if (disposeSourceTexture)
            {
                Object.Destroy(srcTex);
            }

            return blurred;
        }

        public static void OneDimensialBlur(Texture2D srcTex, Texture2D destText, int radius, bool horizontal)
        {
            int windowSize = radius * 2 + 1;

            int width = srcTex.width;
            int height = srcTex.height;
            Color pixelColor;
            if (horizontal)
            {
                for (int imgY = 0; imgY < height; ++imgY)
                {
                    float rSum = 0.0f;
                    float gSum = 0.0f;
                    float bSum = 0.0f;

                    for (int imgX = 0; imgX < width; imgX++)
                    {
                        if (imgX == 0)
                        {
                            for (int x = radius * -1; x <= radius; ++x)
                            {
                                if (x <= 0)
                                {
                                    pixelColor = srcTex.GetPixel(0, imgY);
                                    rSum += pixelColor.r;
                                    gSum += pixelColor.g;
                                    bSum += pixelColor.b;
                                }
                                else if (x >= width)
                                {
                                    pixelColor = srcTex.GetPixel(width - 1, imgY);
                                    rSum += pixelColor.r;
                                    gSum += pixelColor.g;
                                    bSum += pixelColor.b;
                                }
                                else
                                {
                                    pixelColor = srcTex.GetPixel(x, imgY);
                                    rSum += pixelColor.r;
                                    gSum += pixelColor.g;
                                    bSum += pixelColor.b;
                                }
                            }
                        }
                        else
                        {
                            Color toExclude = GetPixelWithXCheck(srcTex, imgX - radius - 1, imgY);
                            Color toInclude = GetPixelWithXCheck(srcTex, imgX + radius, imgY);

                            rSum -= toExclude.r;
                            gSum -= toExclude.g;
                            bSum -= toExclude.b;

                            rSum += toInclude.r;
                            gSum += toInclude.g;
                            bSum += toInclude.b;
                        }

                        destText.SetPixel(imgX, imgY, new Color(rSum / windowSize, gSum / windowSize, bSum / windowSize));
                    }
                }
            }
            else
            {
                for (int imgX = 0; imgX < width; imgX++)
                {
                    float rSum = 0.0f;
                    float gSum = 0.0f;
                    float bSum = 0.0f;

                    for (int imgY = 0; imgY < height; ++imgY)
                    {
                        if (imgY == 0)
                        {
                            for (int y = radius * -1; y <= radius; ++y)
                            {
                                pixelColor = GetPixelWithYCheck(srcTex, imgX, y);
                                rSum += pixelColor.r;
                                gSum += pixelColor.g;
                                bSum += pixelColor.b;
                            }
                        }
                        else
                        {
                            var toExclude = GetPixelWithYCheck(srcTex, imgX, imgY - radius - 1);
                            var toInclude = GetPixelWithYCheck(srcTex, imgX, imgY + radius);

                            rSum -= toExclude.r;
                            gSum -= toExclude.g;
                            bSum -= toExclude.b;

                            rSum += toInclude.r;
                            gSum += toInclude.g;
                            bSum += toInclude.b;
                        }

                        destText.SetPixel(imgX, imgY, new Color(rSum / windowSize, gSum / windowSize, bSum / windowSize));
                    }
                }
            }

            destText.Apply();
        }

        private static Color GetPixelWithXCheck(Texture2D tex, int x, int y)
        {
            if (x <= 0) return tex.GetPixel(0, y);
            if (x >= tex.width) return tex.GetPixel(tex.width - 1, y);
            return tex.GetPixel(x, y);
        }

        private static Color GetPixelWithYCheck(Texture2D tex, int x, int y)
        {
            if (y <= 0) return tex.GetPixel(x, 0);
            if (y >= tex.height) return tex.GetPixel(x, tex.height - 1);
            return tex.GetPixel(x, y);
        }
#endregion
    }
}
