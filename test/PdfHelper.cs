﻿using System.Collections.Generic;
using System.IO;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PowerPointToPDF
{
    public sealed class PdfHelper
    {
        private PdfHelper()
        {
        }

        public static PdfHelper Instance { get; } = new PdfHelper();

        internal void SaveImagesAsPdf(List<string> imageFileNames, string pdfFileName, int width = 600, bool deleteImages = false)
        {
            using (var document = new PdfDocument())
            {
                foreach (var imageFileName in imageFileNames)
                {
                    PdfPage page = document.AddPage();
                    using (XImage img = XImage.FromFile(imageFileName))
                    {
                        // Calculate new height to keep image ratio
                        var height = (int)(width / (double)img.PixelWidth * img.PixelHeight);

                        // Change PDF Page size to match image
                        page.Width = width;
                        page.Height = height;

                        XGraphics gfx = XGraphics.FromPdfPage(page);
                        gfx.DrawImage(img, 0, 0, width, height);
                    }
                }

                document.Save(pdfFileName);
            }

            if (deleteImages)
            {
                foreach (var imageFileName in imageFileNames)
                {
                    File.Delete(imageFileName);
                }
            }
        }
    }
}