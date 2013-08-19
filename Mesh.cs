using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Rudi {

  public class Mesh {

    public static Dictionary<string, Mesh> MeshCache =
      new Dictionary<string, Mesh>();

    /// <summary>
    /// The X, Y and Z components of the Vertex position in space
    /// </summary>
    public TexturedVertex[] Vertices {
      get;
      private set;
    }

    public uint[] Indices {
      get;
      private set;
    }

    public int Handle {
      get;
      set;
    }

    public Mesh(string filename)
    {
      //TODO
    }

    

    public Mesh(DefaultMesh type)
    {
      switch(type) {
        case DefaultMesh.Quad :
          this.Vertices = new TexturedVertex[] {

            new TexturedVertex(new Vector3(-0.5f, 0.5f, 0.0f),
                new Vector2(1.0f, 1.0f)),

                new TexturedVertex(new Vector3(0.5f, 0.5f, 0.0f),
                    new Vector2(1.0f, 0.0f)),

                new TexturedVertex(new Vector3(0.5f, -0.5f, 0.0f),
                    new Vector2(0.0f, 1.0f)),

                new TexturedVertex(new Vector3(0.5f, -0.5f, 0.0f),
                    new Vector2(1.0f, 0.0f)), 

                new TexturedVertex(new Vector3(-0.5f, -0.5f, 0.0f),
                    new Vector2(0.0f, 1.0f)), 

                new TexturedVertex(new Vector3(-0.5f, 0.5f, 0.0f),
                    new Vector2(1.0f, 1.0f)), 
          };
          break;
        case DefaultMesh.Block :
          this.Vertices = CreateBlock();
          break;
      }
    }
    private TexturedVertex[] CreateBlock() {
      TexturedVertex[] mesh;
      mesh = new TexturedVertex[36];
      mesh[0].Position = new Vector3(-1.0f,-1.0f,-1.0f);
      mesh[1].Position = new Vector3( 1.0f,-1.0f,-1.0f); 
      mesh[2].Position = new Vector3(-1.0f,-1.0f, 1.0f);      
      mesh[3].Position = new Vector3( 1.0f,-1.0f,-1.0f);      
      mesh[4].Position = new Vector3( 1.0f,-1.0f, 1.0f);      
      mesh[5].Position = new Vector3(-1.0f,-1.0f, 1.0f);      

      mesh[6].Position = new Vector3(-1.0f, 1.0f,-1.0f);    
      mesh[7].Position = new Vector3(-1.0f, 1.0f, 1.0f);    
      mesh[8].Position = new Vector3( 1.0f, 1.0f,-1.0f);    
      mesh[9].Position = new Vector3( 1.0f, 1.0f,-1.0f);    
      mesh[10].Position = new Vector3(-1.0f, 1.0f, 1.0f);    
      mesh[11].Position = new Vector3( 1.0f, 1.0f, 1.0f);    

      mesh[12].Position = new Vector3(-1.0f,-1.0f, 1.0f);    
      mesh[13].Position = new Vector3( 1.0f,-1.0f, 1.0f);    
      mesh[14].Position = new Vector3(-1.0f, 1.0f, 1.0f);    
      mesh[15].Position = new Vector3( 1.0f,-1.0f, 1.0f);    
      mesh[16].Position = new Vector3( 1.0f, 1.0f, 1.0f);    
      mesh[17].Position = new Vector3(-1.0f, 1.0f, 1.0f);    

      mesh[18].Position = new Vector3(-1.0f,-1.0f,-1.0f);    
      mesh[19].Position = new Vector3(-1.0f, 1.0f,-1.0f);    
      mesh[20].Position = new Vector3( 1.0f,-1.0f,-1.0f);    
      mesh[21].Position = new Vector3( 1.0f,-1.0f,-1.0f);    
      mesh[22].Position = new Vector3(-1.0f, 1.0f,-1.0f);    
      mesh[23].Position = new Vector3( 1.0f, 1.0f,-1.0f);    

      mesh[24].Position = new Vector3(-1.0f,-1.0f, 1.0f);    
      mesh[25].Position = new Vector3(-1.0f, 1.0f,-1.0f);    
      mesh[26].Position = new Vector3(-1.0f,-1.0f,-1.0f);    
      mesh[27].Position = new Vector3(-1.0f,-1.0f, 1.0f);    
      mesh[28].Position = new Vector3(-1.0f, 1.0f, 1.0f);    
      mesh[29].Position = new Vector3(-1.0f, 1.0f,-1.0f);    

      mesh[30].Position = new Vector3( 1.0f,-1.0f, 1.0f);    
      mesh[31].Position = new Vector3( 1.0f,-1.0f,-1.0f);    
      mesh[32].Position = new Vector3( 1.0f, 1.0f,-1.0f);    
      mesh[33].Position = new Vector3( 1.0f,-1.0f, 1.0f);    
      mesh[34].Position = new Vector3( 1.0f, 1.0f,-1.0f);    
      mesh[35].Position = new Vector3( 1.0f, 1.0f, 1.0f);    
                                                           
      mesh[0].TextureCoordinate   = new Vector2(0.0f, 0.0f);
      mesh[1].TextureCoordinate   = new Vector2(1.0f, 0.0f);
      mesh[2].TextureCoordinate   = new Vector2(0.0f, 1.0f);
      mesh[3].TextureCoordinate   = new Vector2(1.0f, 0.0f);
      mesh[4].TextureCoordinate   = new Vector2(1.0f, 1.0f);
      mesh[5].TextureCoordinate   = new Vector2(0.0f, 1.0f);

      mesh[6].TextureCoordinate   = new Vector2(0.0f, 0.0f);
      mesh[7].TextureCoordinate   = new Vector2(0.0f, 1.0f);
      mesh[8].TextureCoordinate   = new Vector2(1.0f, 0.0f);
      mesh[9].TextureCoordinate   = new Vector2(1.0f, 0.0f);
      mesh[10].TextureCoordinate  = new Vector2(0.0f, 1.0f);
      mesh[11].TextureCoordinate  = new Vector2(1.0f, 1.0f);

      mesh[12].TextureCoordinate  = new Vector2(1.0f, 0.0f);
      mesh[13].TextureCoordinate  = new Vector2(0.0f, 0.0f);
      mesh[14].TextureCoordinate  = new Vector2(1.0f, 1.0f);
      mesh[15].TextureCoordinate  = new Vector2(0.0f, 0.0f);
      mesh[16].TextureCoordinate  = new Vector2(0.0f, 1.0f);
      mesh[17].TextureCoordinate  = new Vector2(1.0f, 1.0f);

      mesh[18].TextureCoordinate  = new Vector2(0.0f, 0.0f);
      mesh[19].TextureCoordinate  = new Vector2(0.0f, 1.0f);
      mesh[20].TextureCoordinate  = new Vector2(1.0f, 0.0f);
      mesh[21].TextureCoordinate  = new Vector2(1.0f, 0.0f);
      mesh[22].TextureCoordinate  = new Vector2(0.0f, 1.0f);
      mesh[23].TextureCoordinate  = new Vector2(1.0f, 1.0f);

      mesh[24].TextureCoordinate  = new Vector2(0.0f, 1.0f);
      mesh[25].TextureCoordinate  = new Vector2(1.0f, 0.0f);
      mesh[26].TextureCoordinate  = new Vector2(0.0f, 0.0f);
      mesh[27].TextureCoordinate  = new Vector2(0.0f, 1.0f);
      mesh[28].TextureCoordinate  = new Vector2(1.0f, 1.0f);
      mesh[29].TextureCoordinate  = new Vector2(1.0f, 0.0f);

      mesh[30].TextureCoordinate  = new Vector2(1.0f, 1.0f);
      mesh[31].TextureCoordinate  = new Vector2(1.0f, 0.0f);
      mesh[32].TextureCoordinate  = new Vector2(0.0f, 0.0f);
      mesh[33].TextureCoordinate  = new Vector2(1.0f, 1.0f);
      mesh[34].TextureCoordinate  = new Vector2(0.0f, 0.0f);
      mesh[35].TextureCoordinate  = new Vector2(0.0f, 1.0f);

      return mesh;
    }

    public enum DefaultMesh {
      Quad,
      Block
    };

  }
}
