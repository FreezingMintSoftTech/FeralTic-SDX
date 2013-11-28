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
    public partial class DX11PrimitivesManager
    {
        public DX11IndexedGeometry Grid(Grid settings)
        {
            Vector2 size = settings.Size;
            int resX = settings.ResolutionX;
            int resY = settings.ResolutionY;

            DX11IndexedGeometry geom = new DX11IndexedGeometry(this.device);
            geom.Tag = settings;
            geom.PrimitiveType = "Grid";

            DataStream ds = new DataStream(resX * resY * Pos4Norm3Tex2Vertex.VertexSize, true, true);
            ds.Position = 0;

            float sx = 0.5f * size.X;
            float sy = 0.5f * size.Y;

            float ix = (sx / Convert.ToSingle(resX - 1)) * 2.0f;
            float iy = (sy / Convert.ToSingle(resY - 1)) * 2.0f;


            float y = -sy;


            Pos4Norm3Tex2Vertex v = new Pos4Norm3Tex2Vertex();
            v.Normals = new Vector3(0, 0, -1.0f);

            for (int i = 0; i < resY; i++)
            {
                float x = -sx;
                for (int j = 0; j < resX; j++)
                {
                    v.Position = new Vector4(x, y, 0.0f, 1.0f);
                    v.TexCoords.X = Convert.ToSingle(Map((float)j, 0.0f, resX - 1, 0.0f, 1.0f));
                    v.TexCoords.Y = Convert.ToSingle(Map((float)i, 0.0f, resY - 1, 1.0f, 0.0f));
                    x += ix;

                    ds.Write<Pos4Norm3Tex2Vertex>(v);
                }
                y += iy;
            }

            ds.Position = 0;

            List<int> indlist = new List<int>();
            for (int j = 0; j < resY - 1; j++)
            {
                int rowlow = (j * resX);
                int rowup = ((j + 1) * resX);
                for (int i = 0; i < resX - 1; i++)
                {

                    int col = i * (resX - 1);

                    indlist.Add(0 + rowlow + i);
                    indlist.Add(0 + rowup + i);
                    indlist.Add(1 + rowlow + i);

                    indlist.Add(1 + rowlow + i);
                    indlist.Add(0 + rowup + i);
                    indlist.Add(1 + rowup + i);
                }
            }

            geom.VertexBuffer = DX11VertexBuffer.CreateImmutable(device, resX * resY, Pos4Norm3Tex2Vertex.VertexSize, ds);
            geom.IndexBuffer = DX11IndexBuffer.CreateImmutable(device, indlist.ToArray());
            geom.InputLayout = Pos4Norm3Tex2Vertex.Layout;
            geom.Topology = PrimitiveTopology.TriangleList;

            geom.HasBoundingBox = true;
            geom.BoundingBox = new BoundingBox(new Vector3(-sx, -sy, 0.0f), new Vector3(sx, sy, 0.0f));

            ds.Dispose();

            return geom;
        }
    }
}
