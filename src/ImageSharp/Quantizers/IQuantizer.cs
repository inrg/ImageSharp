﻿// <copyright file="IQuantizer.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Quantizers
{
    using System;

    using ImageSharp.Dithering;

    /// <summary>
    /// Provides methods for allowing quantization of images pixels.
    /// </summary>
    /// <typeparam name="TColor">The pixel format.</typeparam>
    public interface IQuantizer<TColor> : IQuantizer
        where TColor : struct, IPackedPixel, IEquatable<TColor>
    {
        /// <summary>
        /// Quantize an image and return the resulting output pixels.
        /// </summary>
        /// <param name="image">The image to quantize.</param>
        /// <param name="maxColors">The maximum number of colors to return.</param>
        /// <returns>
        /// A <see cref="T:QuantizedImage"/> representing a quantized version of the image pixels.
        /// </returns>
        QuantizedImage<TColor> Quantize(ImageBase<TColor> image, int maxColors);
    }

    /// <summary>
    /// Provides methods for allowing dithering of quantized image pixels.
    /// </summary>
    /// <typeparam name="TColor">The pixel format.</typeparam>
    public interface IDitheredQuantizer<TColor> : IQuantizer<TColor>
        where TColor : struct, IPackedPixel, IEquatable<TColor>
    {
        /// <summary>
        /// Gets or sets a value indicating whether to apply dithering to the output image.
        /// </summary>
        bool Dither { get; set; }

        /// <summary>
        /// Gets or sets the dithering algorithm to apply to the output image.
        /// </summary>
        IErrorDiffusion DitherType { get; set; }
    }

    /// <summary>
    /// Provides methods for allowing quantization of images pixels.
    /// </summary>
    public interface IQuantizer
    {
    }
}
