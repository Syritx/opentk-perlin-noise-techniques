using System;
using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using functions.shaders;

namespace functions {

    abstract class Wave {

        public float[] vertices;
        public Shader shader;
        public Vector3 color;
        public int resolution = 50000, vao, vbo;

        public virtual void Render() {}
    }

    class NoiseWave : Wave {

        float offset = 0;

        float tx = 1000;
            float ty = 2421;

        public NoiseWave() {
            shader = new Shader("shaders/glsl/vertexShader.glsl", "shaders/glsl/fragmentShader.glsl");
            color = new Vector3(0,1,0);

            int r = resolution/2;
            List<float> list = new List<float>();

            for (int x = -r; x < r; x++) {
                float rx = (float)x/r;
                float freq = 10;
                float height = (float)Math.Sin(rx*freq);

                list.Add(rx);
                list.Add(height/5);
                list.Add(0);
            }
            vertices = new float[list.Count];
            for (int i = 0; i < list.Count; i++) {
                vertices[i] = list[i];
            }

            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(vao);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 3*sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            shader.Use();
            int colorLocation = GL.GetUniformLocation(shader.Program, "color");
            Console.WriteLine(colorLocation + " location");
            GL.Uniform3(colorLocation, color);
        }

        float noise(int o, float p, float l, float x, float off) {
            float n = 0;
            float freq = 2f;
            float ampl = 2;

            for (int i = 0; i < o; i++) {
                n += PerlinNoise.Noise(x*freq+off)*ampl;
                freq *= l;
                ampl *= p;
            }

            return n;
        }

        public override void Render()
        {

            int r = resolution/2;
            List<float> list = new List<float>();

            for (int x = -r; x < r; x++) {
                float rx = (float)x/r;

                float relx = PerlinNoise.Noise(tx/r+offset);
                float rely = PerlinNoise.Noise(ty/r+offset);

                float height = noise(15, rx, 2, relx, offset);
                float nx = noise(15, height, 2, rely, offset);

                float a = nx*2;
                float b = height;

                tx += rx;
                ty += rx;

                list.Add(a/9);
                list.Add(b/1.5f);
                list.Add(0);
            }
            vertices = new float[list.Count];
            for (int i = 0; i < list.Count; i++) {
                vertices[i] = list[i];
            }

            offset+=.01f;

            base.Render();

            shader.Use();
            GL.BindVertexArray(vao);

            int colorLocation = GL.GetUniformLocation(shader.Program, "color");
            Console.WriteLine(colorLocation + " location");
            GL.Uniform3(colorLocation, color);

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.DrawArrays(PrimitiveType.Points, 0, vertices.Length);
        }
    }

    class PerlinNoise {

        public static float Noise(float x)
        {
            var X = (int)Math.Floor(x) & 0xff;
            x -= (int)Math.Floor(x);
            var u = Fade(x);
            return Lerp(u, Grad(perm[X], x), Grad(perm[X+1], x-1)) * 2;
        }

        static float Fade(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        static float Lerp(float t, float a, float b)
        {
            return a + t * (b - a);
        }

        static float Grad(int hash, float x)
        {
            return (hash & 1) == 0 ? x : -x;
        }

        static int[] perm = {
            151,160,137,91,90,15,
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
            190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
            129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
            49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,
            151
        };
    }
}

