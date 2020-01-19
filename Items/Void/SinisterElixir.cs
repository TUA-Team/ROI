namespace ROI.Items.Void
{
    internal abstract class SinisterElixir : VoidItem
    {
        //TODO: random buff, but there's a possibility you temporarily lose health
        //or  - more invincibility frames, at the cost of health
        //or  - the lower health you are, the more health it gives you, more defense you lose
        protected override int Affinity => throw new System.NotImplementedException();
    }
}