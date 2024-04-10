using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGL
{
	public static unsafe class Program
	{
		public static void Main()
		{
			// GLFW initialization
			if (!GLFW.Init())
			{
				return;
			}

			// Create window
			var window = CreateWindow(Constants.WIDTH, Constants.HEIGHT);
			if (window is null)
			{
				GLFW.Terminate();
				return;
			}

			// Effectively enables VSYNC by setting to 1.
			GLFW.SwapInterval(1);

			// Generate vertex buffer object (VBO), bind it and copy vertex data to the VBO
			GL.GenBuffers(1, out uint VBO);
			GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
			GL.BufferData(BufferTarget.ArrayBuffer, Constants.Vertices.Length * sizeof(float), Constants.Vertices, BufferUsageHint.StaticDraw);

			// Specify vertex attribute pointers
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			// Load and compile vertex and fragment shaders, create shader program, and use it
			var vertexShader = File.ReadAllText(@".\Shaders\Vertex.shader");
			var fragmentShader = File.ReadAllText(@".\Shaders\Fragment.shader");
			var shader = Shader.CreateShader(vertexShader, fragmentShader);
			GL.UseProgram(shader);

			// Set resolution uniform in the shader
			var resolution = new Vector2(Constants.WIDTH, Constants.HEIGHT);
			GL.Uniform2(GL.GetUniformLocation(shader, "resolution"), resolution);

			while (!GLFW.WindowShouldClose(window))
			{
				// Set time uniform in the shader
				GL.Uniform1(GL.GetUniformLocation(shader, "time"), (float)GLFW.GetTime());

				// Draw triangles using the shader program
				GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

				// Swap buffers and poll events
				GLFW.SwapBuffers(window);
				GLFW.PollEvents();
			}

			// Delete the shader program and terminate GLFW
			GL.DeleteProgram(shader);
			GLFW.Terminate();
		}

		private static Window* CreateWindow(int width, int height)
		{
			// Create window, make the OpenGL context current on the thread, and import graphics functions
			var window = GLFW.CreateWindow(width, height, Constants.TITLE, null, null);
			GLFW.MakeContextCurrent(window);

			// Initialize OpenGL bindings
			GL.LoadBindings(new GLFWBindingsContext());

			return window;
		}
	}
}