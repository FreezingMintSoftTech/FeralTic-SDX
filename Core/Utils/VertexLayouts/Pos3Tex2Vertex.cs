﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using SharpDX;
using SharpDX.Direct3D11;

namespace FeralTic.DX11.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Pos3Tex2Vertex
    {
        public Vector3 Position;
        public Vector2 TexCoords;

        private static InputElement[] layout;

        public static InputElement[] Layout
        {
            get
            {
                if (layout == null)
                {
                    layout = new InputElement[]
                    {
                        new InputElement("POSITION",0,SharpDX.DXGI.Format.R32G32B32_Float,0, 0),
                        new InputElement("TEXCOORD",0,SharpDX.DXGI.Format.R32G32_Float,12,0),
                    };
                }
                return layout;
            }
        }

        public static int VertexSize
        {
            get { return Marshal.SizeOf(typeof(Pos3Tex2Vertex)); }
        }
    }
}
