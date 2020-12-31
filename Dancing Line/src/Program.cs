using System;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace perlin_noise_techniques.src {
    class Program {
        static void Main(string[] args) {

            NativeWindowSettings NWS = new NativeWindowSettings() {
                Title = "Hello",
                Size = new Vector2i(1000,720),
                StartFocused = true,
                StartVisible = true,
                APIVersion = new Version(3,2),
                Flags = ContextFlags.ForwardCompatible,
                Profile = ContextProfile.Core,
            };
            GameWindowSettings GWS = new GameWindowSettings();

            new Scene(GWS, NWS);
        }
    }
}
