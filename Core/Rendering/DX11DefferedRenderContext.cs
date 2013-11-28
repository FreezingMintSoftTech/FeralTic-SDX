﻿using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11
{
    public class DX11DefferedRenderContext : DX11RenderContext
    {
        public DX11DefferedRenderContext(DxDevice device)
        {
            this.Device = device;
            this.Context = new DeviceContext2(device.Device);
            this.RenderTargetStack = new DX11RenderTargetStack(this);
            this.RenderStateStack = new DX11RenderStateStack(this);
        }

        public CommandList CommandList { get; private set; }

        public void Finish(bool restore = false)
        {
            this.CommandList = this.Context.FinishCommandList(restore);
        }

        public void Execute(DX11RenderContext context, bool restore = false)
        {
            context.Context.ExecuteCommandList(this.CommandList, restore);
            this.CommandList.Dispose();
        }
    }
}
