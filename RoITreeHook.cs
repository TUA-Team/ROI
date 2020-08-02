namespace ROI
{
    /// <summary>
    /// All tree hook related thing added by ROI
    /// TODO: Add PostDrawTreeTrunk and PostDrawTreeBranch
    /// </summary>
    /*
    internal class ROITreeHookLoader
    {

        private static IDictionary<int, ModTree> trees;

        public static void Load()
        {
            trees = (IDictionary<int, ModTree>)typeof(TileLoader).GetField("trees", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

            IL.Terraria.Main.DrawTiles += DrawTileIL;
        }

        public static void DrawTileIL(ILContext il)
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
            if (trees.ContainsKey(type) && trees[type] is ITreeHook tree)
            {
                tree.PostDrawTreeTop(sb, position, sourceRectangle, origin);
            }
        }
    }

    */
}
