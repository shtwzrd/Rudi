using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using OpenTK;

namespace Rudi {

  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack=1)]
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
        return Marshal.SizeOf(new TexturedVertex());
      }
    }


  }
}
