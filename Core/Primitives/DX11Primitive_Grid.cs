﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX;
using SharpDX.Direct3D11;

using FeralTic.DX11.Resources;
using SharpDX.Direct3D;

namespace FeralTic.DX11.Geometry
{
    public partial class PrimitivesManager
    {
        public DX11IndexedGeometry Grid(Grid settings)
        {
            GridBuilder builder = new GridBuilder();
            ListGeometryAppender appender = new ListGeometryAppender();
            PrimitiveInfo info = builder.GetPrimitiveInfo(settings);
            builder.Construct(settings, appender.AppendVertex, appender.AppendIndex);
            return FromAppender(settings, appender, info);
        }
    }
}
