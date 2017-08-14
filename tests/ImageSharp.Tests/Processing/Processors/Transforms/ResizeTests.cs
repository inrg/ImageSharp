﻿// <copyright file="ResizeTests.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Tests.Processing.Processors.Transforms
{
    using ImageSharp.PixelFormats;
    using ImageSharp.Processing;
    using SixLabors.Primitives;
    using Xunit;

    public class ResizeTests : FileTestBase
    {
        public static readonly string[] ResizeFiles = { TestImages.Png.CalliphoraPartial };

        public static readonly TheoryData<string, IResampler> ReSamplers =
            new TheoryData<string, IResampler>
            {
                { "Bicubic", new BicubicResampler() },
                { "Triangle", new TriangleResampler() },
                { "NearestNeighbor", new NearestNeighborResampler() },
                { "Box", new BoxResampler() },
                { "Lanczos3", new Lanczos3Resampler() },
                { "Lanczos5", new Lanczos5Resampler() },
                { "MitchellNetravali", new MitchellNetravaliResampler() },
                { "Lanczos8", new Lanczos8Resampler() },
                { "Hermite", new HermiteResampler() },
                { "Spline", new SplineResampler() },
                { "Robidoux", new RobidouxResampler() },
                { "RobidouxSharp", new RobidouxSharpResampler() },
                { "Welch", new WelchResampler() }
            };

        [Theory]
        [WithFile(TestImages.Gif.Giphy, DefaultPixelType)]
        public void ResizeShouldApplyToAllFrames<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2, true));
                image.DebugSave(provider, extension: Extensions.Gif);
            }
        }

        [Theory]
        [WithTestPatternImages(nameof(ReSamplers), 100, 100, DefaultPixelType)]
        [WithFileCollection(nameof(ResizeFiles), nameof(ReSamplers), DefaultPixelType)]
        public void ImageShouldResize<TPixel>(TestImageProvider<TPixel> provider, string name, IResampler sampler)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2, sampler, true));
                image.DebugSave(provider, name);
            }
        }

        [Theory]
        [WithFileCollection(nameof(ResizeFiles), nameof(ReSamplers), DefaultPixelType)]
        public void ImageShouldResizeFromSourceRectangle<TPixel>(TestImageProvider<TPixel> provider, string name, IResampler sampler)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                var sourceRectangle = new Rectangle(image.Width / 8, image.Height / 8, image.Width / 4, image.Height / 4);
                var destRectangle = new Rectangle(image.Width / 4, image.Height / 4, image.Width / 2, image.Height / 2);

                image.Mutate(x => x.Resize(image.Width, image.Height, sampler, sourceRectangle, destRectangle, false));
                image.DebugSave(provider, name);
            }
        }

        [Theory]
        [WithFileCollection(nameof(ResizeFiles), nameof(ReSamplers), DefaultPixelType)]
        public void ImageShouldResizeWidthAndKeepAspect<TPixel>(TestImageProvider<TPixel> provider, string name, IResampler sampler)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                image.Mutate(x => x.Resize(image.Width / 3, 0, sampler, false));
                image.DebugSave(provider, name);
            }
        }

        [Theory]
        [WithFileCollection(nameof(ResizeFiles), nameof(ReSamplers), DefaultPixelType)]
        public void ImageShouldResizeHeightAndKeepAspect<TPixel>(TestImageProvider<TPixel> provider, string name, IResampler sampler)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                image.Mutate(x => x.Resize(0, image.Height / 3, sampler, false));
                image.DebugSave(provider, name);
            }
        }

        [Theory]
        [WithFileCollection(nameof(ResizeFiles), nameof(ReSamplers), DefaultPixelType)]
        public void ImageShouldResizeWithCropWidthMode<TPixel>(TestImageProvider<TPixel> provider, string name, IResampler sampler)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                var options = new ResizeOptions
                {
                    Sampler = sampler,
                    Size = new Size(image.Width / 2, image.Height)
                };

                image.Mutate(x => x.Resize(options));
                image.DebugSave(provider, name);
            }
        }

        [Theory]
        [WithFileCollection(nameof(ResizeFiles), nameof(ReSamplers), DefaultPixelType)]
        public void ImageShouldResizeWithCropHeightMode<TPixel>(TestImageProvider<TPixel> provider, string name, IResampler sampler)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                var options = new ResizeOptions
                {
                    Sampler = sampler,
                    Size = new Size(image.Width, image.Height / 2)
                };

                image.Mutate(x => x.Resize(options));
                image.DebugSave(provider, name);
            }
        }

        [Theory]
        [WithFileCollection(nameof(ResizeFiles), nameof(ReSamplers), DefaultPixelType)]
        public void ImageShouldResizeWithPadMode<TPixel>(TestImageProvider<TPixel> provider, string name, IResampler sampler)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                var options = new ResizeOptions
                {
                    Sampler = sampler,
                    Size = new Size(image.Width + 200, image.Height),
                    Mode = ResizeMode.Pad
                };

                image.Mutate(x => x.Resize(options));
                image.DebugSave(provider, name);
            }
        }

        [Theory]
        [WithFileCollection(nameof(ResizeFiles), nameof(ReSamplers), DefaultPixelType)]
        public void ImageShouldResizeWithBoxPadMode<TPixel>(TestImageProvider<TPixel> provider, string name, IResampler sampler)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                var options = new ResizeOptions
                {
                    Sampler = sampler,
                    Size = new Size(image.Width + 200, image.Height + 200),
                    Mode = ResizeMode.BoxPad
                };

                image.Mutate(x => x.Resize(options));
                image.DebugSave(provider, name);
            }
        }

        [Theory]
        [WithFileCollection(nameof(ResizeFiles), nameof(ReSamplers), DefaultPixelType)]
        public void ImageShouldResizeWithMaxMode<TPixel>(TestImageProvider<TPixel> provider, string name, IResampler sampler)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                var options = new ResizeOptions
                {
                    Sampler = sampler,
                    Size = new Size(300, 300),
                    Mode = ResizeMode.Max
                };

                image.Mutate(x => x.Resize(options));
                image.DebugSave(provider, name);
            }
        }

        [Theory]
        [WithFileCollection(nameof(ResizeFiles), nameof(ReSamplers), DefaultPixelType)]
        public void ImageShouldResizeWithMinMode<TPixel>(TestImageProvider<TPixel> provider, string name, IResampler sampler)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                var options = new ResizeOptions
                {
                    Sampler = sampler,
                    Size = new Size((int)MathF.Round(image.Width * .75F), (int)MathF.Round(image.Height * .95F)),
                    Mode = ResizeMode.Min
                };

                image.Mutate(x => x.Resize(options));
                image.DebugSave(provider, name);
            }
        }

        [Theory]
        [WithFileCollection(nameof(ResizeFiles), nameof(ReSamplers), DefaultPixelType)]
        public void ImageShouldResizeWithStretchMode<TPixel>(TestImageProvider<TPixel> provider, string name, IResampler sampler)
            where TPixel : struct, IPixel<TPixel>
        {
            using (Image<TPixel> image = provider.GetImage())
            {
                var options = new ResizeOptions
                {
                    Sampler = sampler,
                    Size = new Size(image.Width / 2, image.Height),
                    Mode = ResizeMode.Stretch
                };

                image.Mutate(x => x.Resize(options));
                image.DebugSave(provider, name);
            }
        }

        [Theory]
        [InlineData(-2, 0)]
        [InlineData(-1, 0)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        [InlineData(2, 0)]
        public static void BicubicWindowOscillatesCorrectly(float x, float expected)
        {
            var sampler = new BicubicResampler();
            float result = sampler.GetValue(x);

            Assert.Equal(result, expected);
        }

        [Theory]
        [InlineData(-2, 0)]
        [InlineData(-1, 0)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        [InlineData(2, 0)]
        public static void TriangleWindowOscillatesCorrectly(float x, float expected)
        {
            var sampler = new TriangleResampler();
            float result = sampler.GetValue(x);

            Assert.Equal(result, expected);
        }

        [Theory]
        [InlineData(-2, 0)]
        [InlineData(-1, 0)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        [InlineData(2, 0)]
        public static void Lanczos3WindowOscillatesCorrectly(float x, float expected)
        {
            var sampler = new Lanczos3Resampler();
            float result = sampler.GetValue(x);

            Assert.Equal(result, expected);
        }

        [Theory]
        [InlineData(-4, 0)]
        [InlineData(-2, 0)]
        [InlineData(0, 1)]
        [InlineData(2, 0)]
        [InlineData(4, 0)]
        public static void Lanczos5WindowOscillatesCorrectly(float x, float expected)
        {
            var sampler = new Lanczos5Resampler();
            float result = sampler.GetValue(x);

            Assert.Equal(result, expected);
        }
    }
}