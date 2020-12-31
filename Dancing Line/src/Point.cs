using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;

using OpenTK.Graphics.OpenGL;

namespace perlin_noise_techniques.src {

    class Point {
        
        float[] vertices;
        Shader shader;
        int vao, vbo;

        public Point(List<float> vertex_list) {
            
            vertices = new float[vertex_list.Count];

            int id = 0;
            foreach(float v in vertex_list) {
                vertices[id] = v;
                id++;
            }
            

            shader = new Shader("src/Shaders/vertexShader.glsl", "src/Shaders/fragmentShader.glsl");
            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length*sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(vao);
            GL.VertexAttribPointer(
                0,
                3,
                VertexAttribPointerType.Float,
                false,
                3 * sizeof(float), 0);

            GL.EnableVertexAttribArray(0);
        }

        public void Render() {
            GL.Enable(EnableCap.ProgramPointSize);

            GL.VertexAttribPointer(0,3,VertexAttribPointerType.Float,false,3 * sizeof(float),0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            try { 
                shader.Use();
            } catch(System.Exception e1) {}
            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Points, 0, vertices.Length/3);
        }
    }
}