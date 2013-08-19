using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Rudi {
  
  public class VertexBufferObject {

    public static uint CurrentBoundVBO = 0; 

    public uint Handle {
      get;
      private set;
    }

    public BufferUsageHint BufferType {
      get;
      private set;
    }

    public VertexBufferObject(BufferUsageHint hint, Mesh data)
    {
      this.BufferType = hint;
      this.Update(data);
      this.Handle = 0;
    }

    public void Bind()
    {
      VertexBufferObject.CurrentBoundVBO = this.Handle;
      GL.BindBuffer(BufferTarget.ArrayBuffer, this.Handle);
    }

    public void Update(Mesh data)
    {
      if(this.Handle != 0)
        this.Delete();
      uint vbo;
      GL.GenBuffers(1, out vbo);
      this.Handle = vbo;
      GL.BindBuffer(BufferTarget.ArrayBuffer, this.Handle);

      GL.BufferData(BufferTarget.ArrayBuffer,
                    new IntPtr(TexturedVertex.SizeInBytes * data.Vertices.Length),
                    data.Vertices,
                    this.BufferType);

      GL.BindBuffer(BufferTarget.ArrayBuffer, 0); 
    }

    public void Delete()
    {
      GL.DeleteBuffer(this.Handle);
    }

  }
}
