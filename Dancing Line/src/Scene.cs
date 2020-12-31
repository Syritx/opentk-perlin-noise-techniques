using System;
using System.Collections.Generic;

using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace perlin_noise_techniques.src {

    class Scene : GameWindow {

        float s = 0;

        Point point;

        public Scene(GameWindowSettings GWS, NativeWindowSettings NWS) : base(GWS, NWS) {
            Run();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            point.Render();

            List<float> vertices = new List<float>();
            float x = 1, y = 1, tx = 0, ty = 0;

            for (int i = -2000; i < 2000; i++) {

                float p = (float)Noise.noise(x, y, s)*.1f;
                float n = (float)Noise.noise(y, x, s);

                x = (float)Noise.noise(n, p+tx/247824, s+p*n)*1.5f;
                y = (float)Noise.noise(p+ty, n/247824, s-p*n)*1.5f;

                tx += .01f;
                ty += .01f;

                vertices.Add(x);
                vertices.Add(y);
                vertices.Add(0);
            }
            s += .01f;
            point = new Point(vertices);

            SwapBuffers();
        }

        protected override void OnLoad()
        {
            List<float> vertices = new List<float>();
            float x = 1, y = 1, tx = 0, ty = 10000;
            for (int i = -800; i < 800; i++) {
                x = (float)Noise.noise(tx, ty, 0)*.1f;

                tx+=.01f;
                ty+=.01f;

                vertices.Add((float)i/800);
                vertices.Add(x);
                vertices.Add(0);
            }

            base.OnLoad();
            GL.ClearColor(0,0,0,0);
            point = new Point(vertices);
        }
    }
}