using System;
using OpenTK;

namespace Rudi {
  
  public class Block {

    public Texture Texture {
      get;
      set;
    }

    public Mesh Mesh {
      get;
      set;
    }

    public Vector3 Position {
      get;
      set;
    }

    public Block(Texture t, Mesh m, Vector3 worldPos)
    {
      this.Texture = t;
      this.Mesh = m;
      this.Position = worldPos;
    }

  }
}
