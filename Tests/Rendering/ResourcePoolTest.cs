﻿using FeralTic.DX11.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.Tests
{
    [TestClass()]
    public class ResourcePoolTest : RenderDeviceTestBase
    {
        [TestMethod()]
        public void TestSimilarTarget()
        {
            DX11RenderTarget2D rt1 = this.Device.ResourcePool.LockRenderTarget(100, 100, SharpDX.DXGI.Format.R8G8B8A8_UNorm);

            this.Device.ResourcePool.Unlock(rt1);

            DX11RenderTarget2D rt2 = this.Device.ResourcePool.LockRenderTarget(100, 100, SharpDX.DXGI.Format.R8G8B8A8_UNorm);

            Assert.AreEqual(rt1.Texture.NativePointer, rt2.Texture.NativePointer);
        }

        [TestMethod()]
        public void TestTwoTarget()
        {
            DX11RenderTarget2D rt1 = this.Device.ResourcePool.LockRenderTarget(100, 100, SharpDX.DXGI.Format.R8G8B8A8_UNorm);

            this.Device.ResourcePool.Unlock(rt1);

            DX11RenderTarget2D rt2 = this.Device.ResourcePool.LockRenderTarget(120, 100, SharpDX.DXGI.Format.R8G8B8A8_UNorm);

            Assert.AreNotEqual(rt1.Texture.NativePointer, rt2.Texture.NativePointer);
        }

        [TestMethod()]
        public void TestSimilarBuffer()
        {
            DX11StructuredBuffer sb1 = this.Device.ResourcePool.LockStructuredBuffer(16, 16);
            this.Device.ResourcePool.Unlock(sb1);

            DX11StructuredBuffer sb2 = this.Device.ResourcePool.LockStructuredBuffer(16, 16);

            Assert.AreEqual(sb1.Buffer.NativePointer, sb2.Buffer.NativePointer);
        }

        [TestMethod()]
        public void TestSimilarBufferStride()
        {
            DX11StructuredBuffer sb1 = this.Device.ResourcePool.LockStructuredBuffer(16, 16);
            this.Device.ResourcePool.Unlock(sb1);

            DX11StructuredBuffer sb2 = this.Device.ResourcePool.LockStructuredBuffer<Vector4>(16);

            Assert.AreEqual(sb1.Buffer.NativePointer, sb2.Buffer.NativePointer);
        }

        [TestMethod()]
        public void TestTwoBuffers()
        {
            DX11StructuredBuffer sb1 = this.Device.ResourcePool.LockStructuredBuffer(16, 16);
            this.Device.ResourcePool.Unlock(sb1);

            DX11StructuredBuffer sb2 = this.Device.ResourcePool.LockStructuredBuffer(16, 25);

            Assert.AreNotEqual(sb1.Buffer.NativePointer, sb2.Buffer.NativePointer);
        }

        [TestMethod()]
        public void TestBuffersWithFlags()
        {
            DX11StructuredBuffer sb1 = this.Device.ResourcePool.LockStructuredBuffer(16, 16, eDX11BufferMode.Default);
            this.Device.ResourcePool.Unlock(sb1);

            DX11StructuredBuffer sb2 = this.Device.ResourcePool.LockStructuredBuffer(16, 16, eDX11BufferMode.Append);
            this.Device.ResourcePool.Unlock(sb2);

            DX11StructuredBuffer sb3 = this.Device.ResourcePool.LockStructuredBuffer(16, 16, eDX11BufferMode.Counter);
            this.Device.ResourcePool.Unlock(sb2);

            Assert.AreNotEqual(sb1.Buffer.NativePointer, sb2.Buffer.NativePointer);
            Assert.AreNotEqual(sb2.Buffer.NativePointer, sb3.Buffer.NativePointer);
        }
    }
}