﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using SharpDX;
using SharpDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public class ConstantBuffer
    {
        private DxDevice device;

        public ConstantBuffer(DxDevice device, int size)
        {
            this.device = device;
            size = ((size + 15) / 16) * 16;
            
            BufferDescription bd = new BufferDescription()
            {
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = Math.Max(size,16),
                Usage = ResourceUsage.Dynamic
            };

            this.Buffer = new SharpDX.Direct3D11.Buffer(device.Device, bd);
        }

        public IntPtr Map(DeviceContext ctx)
        {      
            DataBox db = ctx.MapSubresource(this.Buffer,0, MapMode.WriteDiscard, MapFlags.None);
            return db.DataPointer;
        }

        public void Unmap(DeviceContext ctx)
        {
            ctx.UnmapSubresource(this.Buffer, 0);
        }

        public SharpDX.Direct3D11.Buffer Buffer { get; protected set; }

        public void Dispose()
        {
            if (this.Buffer != null) { this.Buffer.Dispose(); }
        }

    }
}
