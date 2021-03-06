﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D11;

namespace FeralTic.DX11.Resources
{
    public interface IDxGeometryDrawer
    {
        void Draw(RenderContext ctx);
    }

    /// <summary>
    /// Drawer for DX11 geometry
    /// </summary>
    /// <typeparam name="T">Geometry type</typeparam>
    public interface IDX11GeometryDrawer<T> : IDxGeometryDrawer where T : IDxGeometry
    {
        /// <summary>
        /// Assigns the geometry to the drawer
        /// </summary>
        /// <param name="geometry"></param>
        void Assign(T geometry);

        /// <summary>
        /// Prepares geometry input assembler
        /// </summary>
        /// <param name="ctx">Device Context</param>
        /// <param name="layout">Input Layout</param>
        void PrepareInputAssembler(RenderContext ctx, InputLayout layout);
    }
}
