﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using SharpDX;

using FeralTic.DX11.Resources;
using FeralTic.DX11.Utils;
using SharpDX.Direct3D;


namespace FeralTic.DX11.Geometry
{
    public partial class PrimitivesManager
    {
        public DX11IndexedGeometry Isocahedron(Isocahedron settings)
        {
            IsocahedronBuilder builder = new IsocahedronBuilder();
            ListGeometryAppender appender = new ListGeometryAppender();
            PrimitiveInfo info = builder.GetPrimitiveInfo(settings);

            builder.Construct(settings, appender.AppendVertex, appender.AppendIndex);
            return FromAppender(settings, appender, info);
        }
    }
}
