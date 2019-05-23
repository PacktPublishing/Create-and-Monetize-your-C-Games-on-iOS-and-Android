#if __ANDROID__
using Android.Content.Res;
using Android.Graphics;
using Engine.Android;
using System.IO;
using Android.Opengl;
#elif __IOS__
using UIKit;
using CoreGraphics;
#endif
using OpenTK.Graphics.ES30;
using System;
using System.Collections.Generic;
namespace Engine.Shared.Graphics
{
    public class Texture
    {
        /// <summary> The textures that are currently loaded </summary>
        public static Dictionary<String, Texture> LoadedTextures = new Dictionary<String, Texture>();
        /// <summary> The ID of the texture </summary>
        private Int32 _GLID;
        /// <summary> The name of the file </summary>
        private String _Filename;
        /// <summary> The ID of the texture for OpenGL </summary>
        public Int32 GLID => _GLID;
        /// <summary> The width of the texture </summary>
        public Int32 Width { get; private set; }
        /// <summary> The height of the texture </summary>
        public Int32 Height { get; private set; }

        private Texture(String filename)
        {
#if __ANDROID__
            AssetManager assets = GameActivity.Instance.Assets;
            Bitmap bitmap = null;
            using (Stream stream = assets.Open(filename))
            {
                bitmap = BitmapFactory.DecodeStream(stream);
                Width = bitmap.Width;
                Height = bitmap.Height;
            }
            GL.GenTextures(1, out _GLID);
            GL.BindTexture(All.Texture2D, _GLID);

            GL.TexParameter(All.Texture2D, All.TextureWrapS, (Int32)All.Repeat);
            GL.TexParameter(All.Texture2D, All.TextureWrapT, (Int32)All.Repeat);
            GL.TexParameter(All.Texture2D, All.TextureMinFilter, (Int32)All.Linear);
            GL.TexParameter(All.Texture2D, All.TextureMagFilter, (Int32)All.Linear);
            GLUtils.TexImage2D((Int32)All.Texture2D, 0, bitmap, 0);
            GL.GenerateMipmap(All.Texture2D);
            GL.BindTexture(All.Texture2D, 0);
            bitmap.Recycle();
#elif __IOS__
            UIImage image = UIImage.FromFile(filename);
            nint cgWidth = image.CGImage.Width;
            nint cgHeight = image.CGImage.Height;
            Width = (Int32)cgWidth;
            Height = (Int32)cgHeight;
            CGColorSpace colorSpace = CGColorSpace.CreateDeviceRGB();
            Byte[] data = new Byte[Width * Height * 4];
            CGContext context = new CGBitmapContext(data, cgWidth, cgHeight, 8, 4 * Width, colorSpace, CGBitmapFlags.PremultipliedLast | CGBitmapFlags.ByteOrderDefault);
            colorSpace.Dispose();
            context.ClearRect(new CGRect(0, 0, cgWidth, cgHeight));
            context.DrawImage(new CGRect(0, 0, cgWidth, cgHeight), image.CGImage);

            GL.GenTextures(1, out _GLID);
            GL.BindTexture(TextureTarget.Texture2D, _GLID);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (Int32)All.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (Int32)All.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (Int32)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (Int32)TextureMagFilter.Linear);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
            context.Dispose();
            GL.GenerateMipmap(TextureTarget.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, 0);
#endif
            LoadedTextures.Add(filename, this);
            _Filename = filename;
        }

        /// <summary> Creates a texture from the given byte data </summary>
        /// <param name="data"></param>
        private Texture(Byte[] data, Int32 width, Int32 height, String name)
        {
            GL.GenTextures(1, out _GLID);
            Width = width;
            Height = height;
#if __ANDROID__
            GL.BindTexture(All.Texture2D, _GLID);

            GL.TexParameter(All.Texture2D, All.TextureWrapS, (Int32)All.Repeat);
            GL.TexParameter(All.Texture2D, All.TextureWrapT, (Int32)All.Repeat);
            GL.TexParameter(All.Texture2D, All.TextureMinFilter, (Int32)All.Linear);
            GL.TexParameter(All.Texture2D, All.TextureMagFilter, (Int32)All.Linear);

            GL.TexImage2D(All.Texture2D, 0, (Int32)All.Rgba, Width, Height, 0, All.Rgba, All.UnsignedByte, data);
            GL.GenerateMipmap(All.Texture2D);
            GL.BindTexture(All.Texture2D, 0);
#elif __IOS__
            GL.BindTexture(TextureTarget.Texture2D, _GLID);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (Int32)All.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (Int32)All.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (Int32)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (Int32)TextureMagFilter.Linear);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
            GL.GenerateMipmap(TextureTarget.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, 0);
#endif
            LoadedTextures.Add(name, this);
            _Filename = name;
        }

        /// <summary> Generates a white pixel asset </summary>
        public static Texture GetPixel()
        {
            if (LoadedTextures.ContainsKey("Pixel")) return LoadedTextures["Pixel"];
            Byte[] data = new Byte[] { 255, 255, 255, 255 };
            return new Texture(data, 1, 1, "Pixel");
        }

        /// <summary> Disposes of the texture </summary>
        public void Dispose()
        {
            GL.DeleteTextures(1, ref _GLID);
            LoadedTextures.Remove(_Filename);
        }

        /// <summary> Gets the texture - if it does not exist, it will load the texture </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Texture GetTexture(String filename)
        {
            if (LoadedTextures.ContainsKey(filename)) return LoadedTextures[filename];
            return new Texture(filename);
        }
    }
}
