using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Text;

namespace perlin_noise_techniques.src {

    class Shader {

        public int program, 
                   vertexShader, 
                   fragmentShader;

        public Shader(string vertexFilePath, string fragmentFilePath) {
            string vertexSource = LoadShaderSource(vertexFilePath);
            string fragmentSource = LoadShaderSource(fragmentFilePath);

            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(vertexShader, vertexSource);
            GL.ShaderSource(fragmentShader, fragmentSource);
            GL.CompileShader(vertexShader);
            GL.CompileShader(fragmentShader);

            string info;
            GL.GetShaderInfoLog(fragmentShader, out info);
            System.Console.WriteLine(info);

            program = GL.CreateProgram();
            GL.AttachShader(program,vertexShader);
            GL.AttachShader(program,fragmentShader);
            GL.LinkProgram(program);
        }

        public void Use() {
            GL.UseProgram(program);
        }

        public static string LoadShaderSource(string filePath) {
            string source = null;

            using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8)) {
                source = reader.ReadToEnd();
            }
            return source;
        }
    }
}