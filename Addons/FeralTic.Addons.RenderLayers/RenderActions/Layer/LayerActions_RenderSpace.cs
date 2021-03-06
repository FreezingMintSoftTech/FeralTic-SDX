﻿using FeralTic.RenderLayers.LayerFX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.RenderLayers.RenderActions
{
    public static partial class LayerActions
    {
        public static Action<LayerSettings> WithinView(LayerSettings settings)
        {
            Matrix view = settings.View;
            settings.View = Matrix.Identity;
            settings.ViewProjection = settings.Projection;
            Action<LayerSettings> restore = (rs) => { rs.View = view; rs.ViewProjection = view * settings.Projection; };
            return restore;
        }

        public static Action<LayerSettings> WithinProjection(LayerSettings settings)
        {
            Matrix view = settings.View;
            Matrix proj = settings.Projection;
            settings.View = Matrix.Identity; ;
            settings.Projection = Matrix.Identity;
            settings.ViewProjection = Matrix.Identity;
            Action<LayerSettings> restore = (rs) => { rs.View = view; rs.Projection = proj; settings.ViewProjection = view * proj; };
            return restore;
        }

        public static Action<LayerSettings> DepthOnly(LayerSettings settings)
        {
            bool orig = settings.DepthOnly;
            settings.DepthOnly = true;
            Action<LayerSettings> restore = (rs) => { rs.DepthOnly = orig; };
            return restore;
        }

        public static Action<LayerSettings> ViewProjection(LayerSettings settings, Matrix view, Matrix projection)
        {
            settings.View = view;
            settings.Projection = projection;
            settings.ViewProjection = view * projection;
            Action<LayerSettings> restore = (rs) => { rs.View = view; rs.Projection = projection; settings.ViewProjection = view * projection; };
            return restore;
        }

        public static Action<LayerSettings> PixelBillBoard(LayerSettings settings, Matrix transform, bool doublescale)
        {
            Matrix view = settings.View;
            Matrix proj = settings.Projection;
            float f = doublescale ? 2.0f : 1.0f;
            settings.View = Matrix.Identity;
            settings.Projection = Matrix.Scaling(f / settings.RenderWidth, f / settings.RenderHeight, 1.0f) * transform;
            settings.ViewProjection = settings.Projection;
            Action<LayerSettings> restore = (rs) => { rs.View = view; rs.Projection = proj; settings.ViewProjection = view * proj; };
            return restore;
        }
    }
}
