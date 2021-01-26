using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Text;
using System.IO;

namespace functions.shaders {

    class Shader {

        public int VertexArrayObject, VertexBufferObject, Program;
        int VertexShader, FragmentShader;

        public Shader(string VertexShaderPath, string FragmentShaderPath) {

            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(VertexShader, GetShaderSource(VertexShaderPath));
            GL.ShaderSource(FragmentShader, GetShaderSource(FragmentShaderPath));
            GL.CompileShader(VertexShader);
            GL.CompileShader(FragmentShader);

            string info;
            GL.GetShaderInfoLog(VertexShader, out info);
            System.Console.WriteLine(info);

            Program = GL.CreateProgram();
            GL.AttachShader(Program, VertexShader);
            GL.AttachShader(Program, FragmentShader);
            GL.LinkProgram(Program);
        }

        public void Use() {
            GL.UseProgram(Program);
        }

        string GetShaderSource(string path) {
            string src = null;

            using (StreamReader r = new StreamReader(path, Encoding.UTF8)) {
                src = r.ReadToEnd();
            }
            return src;
        }
    }
}