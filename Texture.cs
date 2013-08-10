using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace OaqGraphics {

  /// <summary>
  /// Represents a texture stored on the graphics card and encapsulates its
  /// state information.
  /// </summary>
  public class Texture : IDisposable, IEquatable<Texture> {

    public int Handle {
      get;
      private set;
    }

    public string Name {
      get;
      private set;
    }

    public int Width {
      get;
      private set;
    }

    public int Height {
      get;
      private set;
    }

    public TextureParameterState TextureParameters {
      get;
      private set;
    }

    public Texture(string filename, TextureParameterState parameters =
        TextureParameterState.Default)
    {
      this.Handle = GL.GenTexture(); 
      this.Name = filename;
      this.TextureParameters = parameters;
      Load();
    }

    public void Load()
    {
      try
      {
        Bitmap image = new Bitmap(this.Name);
        BitmapData pixels = image.LockBits(
            new Rectangle(0, 0, image.Width, image.Height),
            ImageLockMode.ReadOnly,
            System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        this.Width = pixels.Width;
        this.Height = pixels.Height;

        GL.BindTexture(TextureTarget.Texture2D, this.Handle);
        GL.TexImage2D(TextureTarget.Texture2D,
            0,
            PixelInternalFormat.Rgba,
            this.Width,
            this.Height,
            0,
            OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
            PixelType.UnsignedByte,
            pixels.Scan0); 

        SetParameters();
        GL.BindTexture(TextureTarget.Texture2D, 0);

        image.UnlockBits(pixels);
        image.Dispose();
      }
      catch (FileNotFoundException)
      {
        Console.WriteLine("File not found.");
      }
    }

    void SetParameters()
    {
      switch(this.TextureParameters)
      {
        case TextureParameterState.Default:
        case TextureParameterState.NearestClamp:
          GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
          GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);

          GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
          GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
          break;

        case TextureParameterState.NearestRepeat:
          GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
          GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);

          GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
          GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
          break;

        case TextureParameterState.LinearClamp:
          GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
          GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);

          GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
          GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
          break;

        case TextureParameterState.LinearRepeat:
          GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
          GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);

          GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
          GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
          break;
      }
    }

    public void Dispose()
    {
      GL.DeleteTexture(this.Handle);
    }

    public bool Equals(Texture that)
    {
      return (this.Handle == that.Handle && this.Name == that.Name);
    }

    /// <summary>
    /// Simplified enum of Texture Parameters
    ///"Nearest" and "Linear" refer to min and mag filtering,
    ///"Clamp" and "Repeat" refer to Clamp to Edge and (non-mirrored) repeat.
    ///Many possible state combinations are neglected as I have no intention of
    ///ever using them.
    /// </summary>
    public enum TextureParameterState {
      Default,        //NearestClamp is the Default
      NearestClamp,   
      NearestRepeat,  
      LinearClamp,
      LinearRepeat
    };

  }
}

