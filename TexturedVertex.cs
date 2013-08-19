using System;
using System.Runtime.InteropServices;
using OpenTK;

namespace Rudi {

  [StructLayout(LayoutKind.Sequential)]
  public struct TexturedVertex {
    
    /// <summary>
    /// The X, Y and Z components of the Vertex position in space
    /// </summary>
    public Vector3 Position; 

    /// <summary>
    /// The coordinates of the corresponding texture 
    /// </summary>
    public Vector2 TextureCoordinate;

    public TexturedVertex(Vector3 position, Vector2 textureCoordinate)
    {
      this.Position = position;
      this.TextureCoordinate = textureCoordinate;
    }

    public static int SizeInBytes {
      get {
        return Vector3.SizeInBytes + Vector2.SizeInBytes;
      }
    }


  }
}
