using OpenTK.Graphics.ES30;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
#if __ANDROID__
using Android.Content.Res;
#endif
namespace Engine.Shared.Graphics
{
    /// <summary> The shader that is used to draw sprites </summary>
    public class Shader : IDisposable
    {
        /// <summary> The attribute location for Position </summary>
        public const Int32 POSITION_ATTRIBUTE = 0;
        /// <summary> The attribute location for Position </summary>
        public const Int32 UV_ATTRIBUTE = 1;
        /// <summary> The attribute location for Position </summary>
        public const Int32 COLOUR_ATTRIBUTE = 2;
        /// <summary> The uniform location for the WVP matrix </summary>
        public const Int32 WVP_UNIFORM = 0;
        /// <summary> The uniform location for the texture </summary>
        public const Int32 TEXTURE_UNIFORM = 1;
        /// <summary> The uniforms attributes found in the shader </summary>
        protected Dictionary<Int32, Int32> _Uniforms = new Dictionary<Int32, Int32>();

        /// <summary> The uniforms attributes found in the shader </summary>
        public Dictionary<Int32, Int32> Uniforms => _Uniforms;
        /// <summary> The vertex shader </summary>
        public String VertexShader { get; }
        /// <summary> The fragment shader </summary>
        public String FragmentShader { get; }
        /// <summary> The OpenGL program for the shader </summary>
        public Int32 Program { get; protected set; }

        public Shader(String vertexShaderLocation, String fragmentShaderLocation)
        {
#if __ANDROID__
            AssetManager assets = Android.GameActivity.Instance.Assets;
            using (StreamReader sr = new StreamReader(assets.Open(vertexShaderLocation)))
            {
                VertexShader = sr.ReadToEnd();
            }
            using (StreamReader sr = new StreamReader(assets.Open(fragmentShaderLocation)))
            {
                FragmentShader = sr.ReadToEnd();
            }
#elif __IOS__
            VertexShader = File.ReadAllText(vertexShaderLocation);
            FragmentShader = File.ReadAllText(fragmentShaderLocation);
#endif
        }

        /// <summary> Compiles the shader </summary>
        /// <returns>True if successful</returns>
        public System.Boolean Compile()
        {
            Program = GL.CreateProgram();
#if __ANDROID__
            Int32 vertexShader = GL.CreateShader(All.VertexShader);
#elif __IOS__
            Int32 vertexShader = GL.CreateShader(ShaderType.VertexShader);
#endif
            if (!CompileShader(vertexShader, VertexShader)) return false;

#if __ANDROID__
            Int32 fragmentShader = GL.CreateShader(All.FragmentShader);
#elif __IOS__
            Int32 fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
#endif
            if (!CompileShader(fragmentShader, FragmentShader)) return false;

            GL.AttachShader(Program, vertexShader);
            GL.AttachShader(Program, fragmentShader);
            BindAttributes();

            GL.LinkProgram(Program);
            Int32 status = 0;
#if __ANDROID__
            GL.GetProgram(Program, All.LinkStatus, out status);
#elif __IOS__
           GL.GetProgram(Program, ProgramParameter.LinkStatus, out status);
#endif
            if (status == 0)
            {
                return false;
            }

            GetUniformLocations();

            GL.DetachShader(Program, vertexShader);
            GL.DeleteShader(vertexShader);
            GL.DetachShader(Program, fragmentShader);
            GL.DeleteShader(fragmentShader);

            return true;
        }

        /// <summary> Gets the uniform locations for the shaders </summary>
        protected virtual void GetUniformLocations()
        {
            AddUniformLocation(WVP_UNIFORM, "u_WVPMatrix");
            AddUniformLocation(TEXTURE_UNIFORM, "uTexture");
        }

        /// <summary> Adds the uniform location to the dictionary </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        protected void AddUniformLocation(Int32 id, String name)
        {
#if __ANDROID__
            _Uniforms.Add(id, GL.GetUniformLocation(Program, new StringBuilder(name)));
#elif __IOS__
            _Uniforms.Add(id, GL.GetUniformLocation(Program, name));
#endif
        }

        /// <summary> Compiles the given shader </summary>
        /// <param name="shader"></param>
        /// <param name="shaderText"></param>
        /// <returns></returns>
        private System.Boolean CompileShader(Int32 shader, String shaderText)
        {
            Int32 length = shaderText.Length;
            GL.ShaderSource(shader, 1, new String[] { shaderText }, new Int32[] { length });
            GL.CompileShader(shader);
            Int32 status;
#if __ANDROID__
            GL.GetShader(shader, All.CompileStatus, out status);
#elif __IOS__
            GL.GetShader(shader, ShaderParameter.CompileStatus, out status);
#endif
            if (status != 1)
            {
                GL.DeleteShader(shader);
                return false;
            }
            return true;
        }

        /// <summary> Binds the attributes to their locations in OpenGL </summary>
        protected virtual void BindAttributes()
        {
            GL.BindAttribLocation(Program, POSITION_ATTRIBUTE, "a_Position");
            GL.BindAttribLocation(Program, UV_ATTRIBUTE, "a_TexCoord");
            GL.BindAttribLocation(Program, COLOUR_ATTRIBUTE, "a_Colour");
        }

        /// <summary> Disposes of the shader </summary>
        public void Dispose()
        {
            GL.DeleteProgram(Program);
        }
    }
}