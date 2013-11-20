﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Direct3D11;

using Buffer = SharpDX.Direct3D11.Buffer;

namespace FeralTic.DX11.Resources
{
    public unsafe class DX11IndexBuffer : IDX11Resource, IDisposable
    {
        private DX11Device device;

        public Buffer Buffer { get; protected set; }

        public int IndicesCount { get; protected set; }
        private SharpDX.DXGI.Format format;

        protected DX11IndexBuffer(DX11Device device, int indicescount, BufferDescription desc, DataStream initial = null, bool largeformat = true)
        {
            this.device = device;
            this.IndicesCount = indicescount;
            this.format = largeformat ? SharpDX.DXGI.Format.R32_UInt : SharpDX.DXGI.Format.R16_UInt;


            if (initial != null)
            {
                initial.Position = 0;
                this.Buffer = new SharpDX.Direct3D11.Buffer(device.Device, initial, desc);
            }
            else
            {
                this.Buffer = new SharpDX.Direct3D11.Buffer(device.Device, desc);
            }
        }

        protected DX11IndexBuffer(DX11Device device, int indicescount, BufferDescription desc, IntPtr ptr, bool largeformat = true)
        {
            this.device = device;
            this.IndicesCount = indicescount;
            this.format = largeformat ? SharpDX.DXGI.Format.R32_UInt : SharpDX.DXGI.Format.R16_UInt;

            this.Buffer = new SharpDX.Direct3D11.Buffer(device.Device, ptr, desc);
        }

        public static DX11IndexBuffer CreateImmutable(DX11Device device, int indicescount, DataStream initial = null, bool largeformat = true)
        {
            int indexsize = largeformat ? 4 : 2;
            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = indexsize * indicescount,
                Usage = ResourceUsage.Immutable
            };

            return new DX11IndexBuffer(device, indicescount, bd, initial,largeformat);
        }

        public static DX11IndexBuffer CreateImmutable(DX11Device device, short[] initial)
        {
            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = initial.Length * 2,
                Usage = ResourceUsage.Immutable
            };
            DX11IndexBuffer result;
            fixed (short* ptr = &initial[0])
            {
                result = new DX11IndexBuffer(device,initial.Length,bd,new IntPtr(ptr),false);
            }
            return result;
        }

        public static DX11IndexBuffer CreateImmutable(DX11Device device, int[] initial)
        {
            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = initial.Length * 4,
                Usage = ResourceUsage.Immutable
            };
            DX11IndexBuffer result;
            fixed (int* ptr = &initial[0])
            {
                result = new DX11IndexBuffer(device, initial.Length, bd, new IntPtr(ptr), true);
            }
            return result;
        }

        public void Bind(DX11RenderContext context)
        {
            context.Context.InputAssembler.SetIndexBuffer(this.Buffer, this.format, 0);
        }

        public void Dispose()
        {
            if (this.Buffer != null) { this.Buffer.Dispose(); }  
        }
    }
}
