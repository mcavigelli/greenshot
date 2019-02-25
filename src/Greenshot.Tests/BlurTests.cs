﻿#region Greenshot GNU General Public License

// Greenshot - a free and open source screenshot tool
// Copyright (C) 2007-2018 Thomas Braun, Jens Klingen, Robin Krom
// 
// For more information see: http://getgreenshot.org/
// The Greenshot project is hosted on GitHub https://github.com/greenshot/greenshot
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 1 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

#endregion

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Greenshot.Gfx;
using Greenshot.Gfx.Experimental;
using Greenshot.Tests.Implementation;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Xunit;

namespace Greenshot.Tests
{
    /// <summary>
    /// This tests if the new blur works
    /// </summary>
    public class BlurTests
    {
        [Fact]
        public void Test_BoxBlurSharpImage()
        {
            using (var bitmapOld = BitmapFactory.CreateEmpty(400, 400, PixelFormat.Format32bppArgb, Color.White))
            {
                using (var graphics = Graphics.FromImage(bitmapOld))
                using (var pen = new SolidBrush(Color.Blue))
                {
                    graphics.FillRectangle(pen, new Rectangle(30, 30, 340, 340));
                    bitmapOld.ApplyOldBoxBlur(10);
                    bitmapOld.Save(@"old.png", ImageFormat.Png);
                }

                using (var tmpStream = new MemoryStream())
                using (var image = new Image<Rgba32>(SixLabors.ImageSharp.Configuration.Default, 400, 400, Rgba32.White))
                {
                    var color = Rgba32.Blue;
                    color.A = 255;
                    var solidBlueBrush = SixLabors.ImageSharp.Processing.Brushes.Solid(color);
                    var g = new GraphicsOptions(false);
                    image.Mutate(c => c
                    .Fill(new GraphicsOptions(false), solidBlueBrush, new SixLabors.Primitives.Rectangle(30, 30, 340, 340))
                    .BoxBlur(10));
                    image.SaveAsPng(tmpStream);

                    tmpStream.Seek(0, SeekOrigin.Begin);
                    using (var bitmapNew = (Bitmap)System.Drawing.Image.FromStream(tmpStream))
                    {
                        bitmapNew.Save(@"new.png", ImageFormat.Png);
                        Assert.True(bitmapOld.IsEqualTo(bitmapNew), "New blur doesn't compare to old.");
                    }
                }

            }
        }


        [Theory]
        [InlineData(PixelFormat.Format24bppRgb)]
        [InlineData(PixelFormat.Format32bppRgb)]
        [InlineData(PixelFormat.Format32bppArgb)]
        public void Test_Blur(PixelFormat pixelFormat)
        {
            using (var bitmapNew = BitmapFactory.CreateEmpty(400, 400, pixelFormat, Color.White))
            using (var bitmapOld = BitmapFactory.CreateEmpty(400, 400, pixelFormat, Color.White))
            {
                using (var graphics = Graphics.FromImage(bitmapNew))
                using (var pen = new SolidBrush(Color.Blue))
                {
                    graphics.FillRectangle(pen, new Rectangle(30, 30, 340, 340));
                    bitmapNew.ApplyBoxBlur(10);
                }
                using (var graphics = Graphics.FromImage(bitmapOld))
                using (var pen = new SolidBrush(Color.Blue))
                {
                    graphics.FillRectangle(pen, new Rectangle(30, 30, 340, 340));
                    bitmapOld.ApplyOldBoxBlur(10);
                }
                bitmapOld.Save(@"old.png", ImageFormat.Png);
                bitmapNew.Save(@"new.png", ImageFormat.Png);

                Assert.True(bitmapOld.IsEqualTo(bitmapNew), "New blur doesn't compare to old.");
            }
        }

        [Theory]
        [InlineData(PixelFormat.Format24bppRgb)]
        [InlineData(PixelFormat.Format32bppRgb)]
        [InlineData(PixelFormat.Format32bppArgb)]
        public void Test_Blur_Span(PixelFormat pixelFormat)
        {
            using (var bitmapNew = BitmapFactory.CreateEmpty(400, 400, pixelFormat, Color.White))
            using (var bitmapOld = BitmapFactory.CreateEmpty(400, 400, pixelFormat, Color.White))
            {
                using (var graphics = Graphics.FromImage(bitmapNew))
                using (var pen = new SolidBrush(Color.Blue))
                {
                    graphics.FillRectangle(pen, new Rectangle(30, 30, 340, 340));
                    bitmapNew.ApplyBoxBlurSpan(10);
                }
                using (var graphics = Graphics.FromImage(bitmapOld))
                using (var pen = new SolidBrush(Color.Blue))
                {
                    graphics.FillRectangle(pen, new Rectangle(30, 30, 340, 340));
                    bitmapOld.ApplyOldBoxBlur(10);
                }
                bitmapOld.Save(@"old.png", ImageFormat.Png);
                bitmapNew.Save(@"new.png", ImageFormat.Png);

                Assert.True(bitmapOld.IsEqualTo(bitmapNew), "New blur doesn't compare to old.");
            }
        }
    }
}