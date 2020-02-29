using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;

namespace ROI
{
    /// <summary>
    /// All tree management hooks added by ROI
    /// </summary>
    // TODO: Add PostDrawTreeTrunk and PostDrawTreeBranch
    internal static class TreeHooking
    {
        private static IDictionary<int, ITreeHook> trees;

        public static void Load()
        {
            trees = (typeof(TileLoader).GetField("trees", BindingFlags.NonPublic | BindingFlags.Static)
                    .GetValue(null) as Dictionary<int, ModTree>)
                .Where(x => x.Value is ITreeHook)
                .Select(x => new { K = x.Key, V = x.Value as ITreeHook })
                .ToDictionary(x => x.K, x => x.V);

            IL.Terraria.Main.DrawTiles += DrawTileIL;
        }

        private static void DrawTileIL(ILContext il)
        {
            var c = new ILCursor(il);

            int storeForLater = 0;

            if (c.TryGotoNext(
                    i => i.MatchCall(out _),
                    i => i.MatchLdfld(out _),
                    i => i.MatchLdloc(out _),
                    i => i.MatchNop(),
                    i => i.MatchNop(),
                    i => i.MatchLdloc(out _),
                    i => i.MatchNop(),
                    i => i.MatchNop(),
                    i => i.MatchLdloca(out _),
                    i => i.MatchNop(),
                    i => i.MatchNop(),
                    i => i.MatchLdloca(out _),
                    i => i.MatchNop(),
                    i => i.MatchNop(),
                    i => i.MatchLdloca(out _),
                    i => i.MatchNop(),
                    i => i.MatchNop(),
                    i => i.MatchLdloca(out _),
                    i => i.MatchNop(),
                    i => i.MatchNop(),
                    i => i.MatchLdloca(out _),
                    i => i.MatchNop(),
                    i => i.MatchNop(),
                    i => i.MatchCall(out _),
                    i => i.MatchStloc(out _),
                    i => i.MatchNop(),
                    i => i.MatchNop()))
            {
                c.Index += 2;
                c.Emit(OpCodes.Dup);
                c.EmitDelegate<Action<int>>(i =>
                {
                    storeForLater = i;
                });
            }

            if (c.TryGotoNext(
                    i => i.MatchCallvirt(typeof(SpriteBatch), "Draw"),
                    i => i.MatchBr(out _), i => i.MatchLdloc(out _),
                    i => i.MatchNop(),
                    i => i.MatchNop(),
                    i => i.MatchCallvirt(out _),
                    i => i.MatchLdcI4(out _),
                    i => i.MatchBle(out _)))
            {
                c.Remove();
                c.EmitDelegate<Action<SpriteBatch, Texture2D, Vector2, Rectangle?, Color, float, Vector2, float, SpriteEffects, float>>((spritebatch, texture, position, sourceRectangle, color, rotation, origin, scale, effect, layerDept) =>
                {
                    spritebatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effect, layerDept);

                    PostDrawTreeTop(storeForLater, spritebatch, position, sourceRectangle, origin);
                    //Get tree type?
                });
            }
        }

        public static void PostDrawTreeTop(int type, SpriteBatch sb, Vector2 position, Rectangle? sourceRectangle, Vector2 origin)
        {
            if (trees.TryGetValue(type, out var tree))
            {
                tree.PostDrawTreeTop(sb, position, sourceRectangle, origin);
            }
        }
    }
}