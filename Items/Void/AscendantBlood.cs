namespace ROI.Items.Void
{
    internal abstract class AscendantBlood : VoidItem
    {
        //TODO: every crit strike counts as a 2% increase in damage
        //or  - being around town npcs counts as 2% increase in damage
        //or  - get bonuses during boss fights
        protected override int Affinity => 20;

        public override void SetDefaults()
        {

        }
    }
}