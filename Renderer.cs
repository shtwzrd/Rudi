using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OaqGraphics {

  public class Renderer {

    uint vbo;
    uint ebo;
    uint vao;
    int vshaderHandle, fshaderHandle, shaderProgramHandle;

    Matrix4 model;  
    Matrix4 view;
    Matrix4 screen;

    int shaderAttribPosition;
    int shaderAttribTexCoord;
    int shaderUniformSampler;

    int shaderUniformModel;
    int shaderUniformView;
    int shaderUniformScreen;
    
    //Hardcoded Shit
    static float counter;

    //Quad
    float[] vertData = {
      //Position
      //  X     Y
     -0.5f, 0.5f, 0.0f, 0.0f,
      0.5f, 0.5f, 1.0f, 0.0f,
      0.5f,-0.5f, 1.0f, 1.0f,
     -0.5f,-0.5f, 0.0f, 1.0f,
    };

    //EBO Indices for the Quad
    uint[] elements = {
      0, 1, 2,
      2, 3, 0
    };

    static readonly string vShaderSource = @"
      #version 110

      uniform mat4 model;
      uniform mat4 view;
      uniform mat4 screen;

      attribute vec2 position;
      attribute vec2 textureCoordIn;

      varying vec2 textureCoord;

      void main() {
        textureCoord = textureCoordIn;
        gl_Position = screen * view * model * vec4(position, 0.0, 1.0);
      }
      ";
    

    static readonly string fShaderSource  = @"
      #version 110

      uniform sampler2D textureSample;
      
      varying vec2 textureCoord;

      void main() {
        gl_FragColor = texture2D(textureSample, textureCoord);
      }
      ";

    public Renderer()
    {
      counter = 0;
    }

    public void Render()
    {
      GL.GenBuffers(1, out this.vbo);
      GL.BindBuffer(BufferTarget.ArrayBuffer, this.vbo);

      GL.BufferData(BufferTarget.ArrayBuffer,
                    new IntPtr(vertData.Length * sizeof(float)),
                    vertData,
                    BufferUsageHint.StaticDraw);

      GL.GenBuffers(1, out ebo);
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ebo);

      GL.BufferData(BufferTarget.ElementArrayBuffer,
                    new IntPtr(elements.Length * sizeof(uint)),
                    elements,
                    BufferUsageHint.StaticDraw);
      
      this.vshaderHandle = GL.CreateShader(ShaderType.VertexShader);
      GL.ShaderSource(this.vshaderHandle, vShaderSource);
      GL.CompileShader(this.vshaderHandle);
      Console.WriteLine(GL.GetShaderInfoLog(this.vshaderHandle));

      this.fshaderHandle = GL.CreateShader(ShaderType.FragmentShader);
      GL.ShaderSource(this.fshaderHandle, fShaderSource);
      GL.CompileShader(this.fshaderHandle);
      Console.WriteLine(GL.GetShaderInfoLog(this.fshaderHandle));

      this.shaderProgramHandle = GL.CreateProgram();
      GL.AttachShader(this.shaderProgramHandle, this.vshaderHandle);
      GL.AttachShader(this.shaderProgramHandle, this.fshaderHandle);
      GL.LinkProgram(this.shaderProgramHandle);
      Console.WriteLine(GL.GetProgramInfoLog(this.shaderProgramHandle));
      GL.UseProgram(this.shaderProgramHandle);
      

      this.shaderAttribPosition = GL.GetAttribLocation(this.shaderProgramHandle,
                                                       "position");

      this.shaderAttribTexCoord = GL.GetAttribLocation(this.shaderProgramHandle,
                                                       "textureCoordIn");

      this.shaderUniformSampler =  GL.GetUniformLocation(this.shaderProgramHandle,
                                     "texture");

      this.shaderUniformModel = GL.GetUniformLocation(this.shaderProgramHandle,
                                     "model");

      this.shaderUniformView = GL.GetUniformLocation(this.shaderProgramHandle,
                                     "view"); 

      this.shaderUniformScreen = GL.GetUniformLocation(this.shaderProgramHandle,
                                     "screen"); 

      GL.GenVertexArrays(1, out this.vao);
      GL.BindVertexArray(this.vao);

      GL.EnableVertexAttribArray(shaderAttribPosition);
      GL.EnableVertexAttribArray(shaderAttribTexCoord);

      GL.VertexAttribPointer(shaderAttribPosition,
                             2,
                             VertexAttribPointerType.Float,
                             false,
                             4 * sizeof(float),
                             0);

      GL.VertexAttribPointer(shaderAttribTexCoord,
                             2,
                             VertexAttribPointerType.Float,
                             false,
                             4 * sizeof(float),
                             2 * sizeof(float));

      
      Texture realTexture = new Texture("content/hello1.tif");
      GL.BindTexture(TextureTarget.Texture2D, realTexture.Handle);
      
      GL.ActiveTexture(TextureUnit.Texture0);
      GL.Uniform1(this.shaderUniformSampler, 0);

      //Fake affine transform stuff

      Matrix4.CreateRotationZ(counter, out model);

      GL.UniformMatrix4(this.shaderUniformModel, false, ref model);
      GL.UniformMatrix4(this.shaderUniformView, false, ref view); 
      GL.UniformMatrix4(this.shaderUniformScreen, false, ref screen); 

      Console.WriteLine("model: " + model + " view: " + view);
      
      view = Matrix4.LookAt(new Vector3(1.2f, 1.2f, 1.2f),
                     new Vector3(0.0f, 0.0f, 0.0f),
                     new Vector3(0.0f, 0.0f, 1.0f));

      screen = Matrix4.Perspective(45.0f, 800.0f / 600.0f, 0.5f, 40.0f);

      GL.DrawElements(BeginMode.Triangles,
          this.elements.Length,
          DrawElementsType.UnsignedInt,
          this.elements);
      
      counter += 0.01f;
    }
  }
}
