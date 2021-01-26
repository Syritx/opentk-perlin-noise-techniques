using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace functions {
    class Scene : GameWindow {

        public static Vector2 ScreenSize;
        Wave wave;

        float elapsed = 0;

        public Scene(GameWindowSettings GWS, NativeWindowSettings NWS) : base(GWS, NWS) {
            Run();
        }

        protected override void OnRenderFrame(FrameEventArgs args) {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            wave.Render();
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e) {
            base.OnResize(e);
        }

        protected override void OnLoad() {
            base.OnLoad();
            wave = new NoiseWave();

            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0,0,0,1.0f);
        }
    }
}