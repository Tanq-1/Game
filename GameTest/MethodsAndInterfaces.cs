using System;
using Raylib_cs;

namespace GameTest
{
    public interface ITextureLoading
    {
        abstract void Load();
        abstract void Unload();
    }
}
