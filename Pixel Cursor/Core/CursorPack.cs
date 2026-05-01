using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class CursorPack
{
    public string Name { get; set; }
    public List<CursorEntry> Entries { get; set; } = new();

    public static CursorPack LoadDefault(GraphicsDevice graphics)
    {
        var pack = new CursorPack { Name = "Default Pack" };
        pack.Entries.Add(new CursorEntry { Id = "arrow", Name = "Arrow" });
        pack.Entries.Add(new CursorEntry { Id = "hand", Name = "Pointer" });
        pack.Entries.Add(new CursorEntry { Id = "text", Name = "Text" });
        pack.Entries.Add(new CursorEntry { Id = "move", Name = "Move" });
        return pack;
    }
}