﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace FeralTic.DX11.Resources
{
    public class DX11RenderTextureArray : IDxTexture2D, IDxRenderTarget
    {
        private DxDevice device;

        public RenderTargetView RenderView { get; protected set; }
        public ShaderResourceView ShaderView { get; protected set; }
        public Texture2D Texture { get; protected set; }

        public DX11SliceRenderTarget[] Slices { get; protected set; }

        private Texture2DDescription resourceDesc;

        public int Width
        {
            get { return resourceDesc.Width; }
        }

        public int Height
        {
            get { return resourceDesc.Height; }
        }

        public Format Format
        {
            get { return resourceDesc.Format; }
        }

        public int ElementCount { get { return resourceDesc.ArraySize; } }

        public DX11RenderTextureArray(DxDevice device, int w, int h, int elemcnt, Format format, bool buildslices = true)
        {
            this.device = device;

            var texBufferDesc = new Texture2DDescription
            {
                ArraySize = elemcnt,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = format,
                Height = h,
                Width = w,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1,0),
                Usage = ResourceUsage.Default,
            };

            this.Texture = new Texture2D(device.Device, texBufferDesc);
            this.resourceDesc = this.Texture.Description;

            this.RenderView = new RenderTargetView(device.Device, this.Texture);
            this.ShaderView = new ShaderResourceView(device.Device, this.Texture);

            if (buildslices)
            {
                this.Slices = new DX11SliceRenderTarget[this.ElementCount];
                for (int i = 0; i < this.ElementCount; i++)
                {
                    this.Slices[i] = new DX11SliceRenderTarget(device, this, i);
                }
            }
        }

        public void Dispose()
        {
            if (this.Slices != null)
            {
                foreach (DX11SliceRenderTarget slice in this.Slices)
                {
                    if (slice != null) { slice.Dispose(); }
                }
            }

            this.RenderView.Dispose();
            this.ShaderView.Dispose();
            this.Texture.Dispose();
        }
    }
}
