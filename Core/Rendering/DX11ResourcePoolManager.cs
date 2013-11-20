﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.DXGI;

using FeralTic.DX11.Resources;
using System.Runtime.InteropServices;

namespace FeralTic.DX11
{
    public class DX11ResourcePoolManager : IDisposable
    {
        private DX11Device device;

        private DX11RenderTargetPool targetpool;
        private DX11StructuredBufferPool sbufferpool;
        private DX11VolumeTexturePool volumepool;
        private DX11DepthStencilPool depthpool;
        private DX11VertexBufferPool vbopool;

        public DX11ResourcePoolManager(DX11Device device)
        {
            this.device = device;
            this.targetpool = new DX11RenderTargetPool(this.device);
            this.sbufferpool = new DX11StructuredBufferPool(this.device);
            this.volumepool = new DX11VolumeTexturePool(this.device);
            this.depthpool = new DX11DepthStencilPool(this.device);
            this.vbopool = new DX11VertexBufferPool(this.device);
        }

        public void BeginFrame()
        {
        }

        public DX11ResourcePoolEntry<DX11RenderTarget2D> LockRenderTarget(int w, int h, Format format, bool genMM = false, int mmLevels = 1)
        {
            return this.targetpool.Lock(w, h, format, new SampleDescription(1, 0), genMM, mmLevels);
        }

        public DX11ResourcePoolEntry<DX11RenderTarget2D> LockRenderTarget(int w, int h, Format format, SampleDescription sd, bool genMM = false, int mmLevels = 1)
        {
            return this.targetpool.Lock(w, h, format, sd, genMM, mmLevels);
        }

        public DX11ResourcePoolEntry<DX11StructuredBuffer> LockStructuredBuffer(int stride, int numelements, eDX11BufferMode mode = eDX11BufferMode.Default)
        {
            return this.sbufferpool.Lock(stride, numelements, mode);
        }

        public DX11ResourcePoolEntry<DX11StructuredBuffer> LockStructuredBuffer<T>(int numelements, eDX11BufferMode mode = eDX11BufferMode.Default) where T : struct
        {
            return this.sbufferpool.Lock(Marshal.SizeOf(typeof(T)), numelements, mode);
        }

        public DX11ResourcePoolEntry<DX11RenderTexture3D> LockVolume(int w, int h, int d, Format format)
        {
            return this.volumepool.Lock(w, h, d, format);
        }

        public DX11ResourcePoolEntry<DX11DepthStencil> LockDepth(int w, int h, SampleDescription sd, eDepthFormat format)
        {
            return this.depthpool.Lock(w, h, format,sd);
        }

        public DX11ResourcePoolEntry<DX11VertexBuffer> LockVertexBuffer(int verticescount,int vertexsize,bool allowstreamout)
        {
            return this.vbopool.Lock(verticescount, vertexsize, allowstreamout);
        }

        public void Unlock(DX11RenderTarget2D target)
        {
            this.targetpool.UnLock(target);
        }

        public void Unlock(DX11StructuredBuffer target)
        {
            this.sbufferpool.UnLock(target);
        }

        public void Unlock(DX11RenderTexture3D target)
        {
            this.volumepool.UnLock(target);
        }

        public void Unlock(DX11DepthStencil target)
        {
            this.depthpool.UnLock(target);
        }


        public void Unlock(DX11VertexBuffer target)
        {
            this.vbopool.UnLock(target);
        }

        /*public void UnlockStructuredBuffers()
        {
            this.sbufferpool.UnlockAll();
        }*/

        public void ClearUnlocked()
        {
            this.sbufferpool.ClearUnlocked();
            this.targetpool.ClearUnlocked();
            this.volumepool.ClearUnlocked();
            this.vbopool.ClearUnlocked();
            this.depthpool.ClearUnlocked();
        }

        public int RenderTargetCount
        {
            get { return this.targetpool.Count; }
        }

        /*public int BufferCount
        {
            get { return this.sbufferpool.Count; }
        }*/

        public void Dispose()
        {
            this.targetpool.Dispose();
            this.sbufferpool.Dispose();
            this.volumepool.Dispose();
            this.depthpool.Dispose();
            this.vbopool.Dispose();
        }
    }
}