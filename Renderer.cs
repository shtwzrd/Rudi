using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Rudi {

  public class Renderer {

    VertexBufferObject vbo;
    Texture realTexture;
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

    Mesh quad = new Mesh(Mesh.DefaultMesh.Block);
    
    static float counter;

    static readonly string vShaderSource = @"
      #version 110

      uniform mat4 model;
      uniform mat4 view;
      uniform mat4 screen;

      attribute vec3 position;
      attribute vec2 textureCoordIn;

      varying vec2 textureCoord;

      void main() {
        textureCoord = textureCoordIn;
        gl_Position = screen * view * model * vec4(position, 1.0);
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

    private void LoadScene(VertexBufferObject vbo)
    {
      //Place-holder map
      int[,] map = {
        { 1, 1, 1, 1, 1, 0, 1, 1, 0, 0 }, 
        { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 }, 
        { 1, 0, 0, 1, 1, 1, 1, 1, 1, 1 }, 
        { 1, 1, 1, 1, 0, 0, 1, 1, 0, 0 }, 
        { 0, 1, 1, 1, 1, 0, 1, 1, 0, 0 }, 
        { 1, 1, 1, 0, 1, 0, 1, 1, 0, 0 }, 
        { 1, 1, 1, 1, 1, 1, 1, 0, 1, 0 }, 
        { 1, 1, 0, 1, 1, 1, 1, 0, 0, 0 }, 
        { 1, 1, 1, 1, 0, 1, 1, 0, 0, 1 }, 
        { 0, 1, 1, 1, 1, 0, 1, 1, 0, 0 }, 
        { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
      };

      for(int x = 0; x < 10; x++)
      {
        for(int y = 0; y < 10; y++)
        {

        }
      }

    }                                 

    public Renderer()
    {
      counter = 30;
      this.vbo = new VertexBufferObject(BufferUsageHint.StaticDraw, quad);
      this.vbo.Update(quad);
      this.vbo.Bind();
      GL.Enable(EnableCap.DepthTest);
      GL.DepthFunc(DepthFunction.Less);

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
                             3,
                             VertexAttribPointerType.Float,
                             false,
                             TexturedVertex.SizeInBytes,
                             0);

      GL.VertexAttribPointer(shaderAttribTexCoord,
                             2,
                             VertexAttribPointerType.Float,
                             false,
                          sizeof(float) * 5,
                          Vector3.SizeInBytes);

      realTexture = new Texture("content/hello1.tif", Texture.TextureParameterState.LinearClamp); 
      GL.Uniform1(this.shaderUniformSampler, 0); 
      GL.BindVertexArray(0);
    }

    public void Render()
    {
      this.vbo.Update(quad);
      this.vbo.Bind();
      GL.BindVertexArray(this.vao); 
      
      GL.BindTexture(TextureTarget.Texture2D, realTexture.Handle);
      
      GL.ActiveTexture(TextureUnit.Texture0);

      //Transforms
      Matrix4 rotY;
      Matrix4.CreateRotationZ(counter, out model);
      Matrix4.CreateRotationY(counter, out rotY);
      model = Matrix4.Mult(model, rotY);

      GL.UniformMatrix4(this.shaderUniformModel, false, ref model);
      GL.UniformMatrix4(this.shaderUniformView, false, ref view); 
      GL.UniformMatrix4(this.shaderUniformScreen, false, ref screen); 
      

      view = Matrix4.Identity;

      screen = Matrix4.CreateOrthographic(10, 5, -100f, 100f);

      GL.DrawArrays(BeginMode.TriangleStrip,
                    0,
                    this.quad.Vertices.Length);

      counter += 0.01f;
      GL.BindVertexArray(0);
    }
  }
}
