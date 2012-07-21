using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Poisson
{
    class Camera
    {
        public Matrix Projection { get; set; }
        float scale;

        public Camera(float scale)
        {
            this.scale = scale;
            this.Projection = Matrix.CreateScale(0.5f, 0.5f, 1f);
        }

        public Vector2 ScreenToWorld(Vector2 screenPos)
        {
            return screenPos / this.scale;
        }

        public Vector2 WorldToScreen(Vector2 worldPos)
        {
            return worldPos * this.scale;
        }
    }
}
