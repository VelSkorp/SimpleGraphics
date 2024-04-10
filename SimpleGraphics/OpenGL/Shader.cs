using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
	public static class Shader
	{
		public static int CreateShader(string vertexShaderSource, string fragmentShaderSource)
		{
			// Compile shaders and create shader program
			var vertexShader = CompileShader(ShaderType.VertexShader, vertexShaderSource);
			var fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentShaderSource);
			var shaderProgram = GL.CreateProgram();

			// Attach shaders to shader program
			GL.AttachShader(shaderProgram, vertexShader);
			GL.AttachShader(shaderProgram, fragmentShader);
			GL.LinkProgram(shaderProgram);
			GL.ValidateProgram(shaderProgram);

			// Delete compiled shaders (no longer needed after linking)
			GL.DeleteShader(vertexShader);
			GL.DeleteShader(fragmentShader);

			return shaderProgram;
		}

		public static int CompileShader(ShaderType shaderType, string shaderSource)
		{
			var shader = GL.CreateShader(shaderType);

			// Set shader source code and compile shader
			GL.ShaderSource(shader, shaderSource);
			GL.CompileShader(shader);

			// Output compilation log to console if not empty
			var infoLogFrag = GL.GetShaderInfoLog(shader);
			if (!string.IsNullOrWhiteSpace(infoLogFrag))
			{
				Console.WriteLine($"{shaderType} shader compile log: {infoLogFrag}");
			}

			return shader;
		}
	}
}