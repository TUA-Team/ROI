using ROI.Spawning.Wasteland;

namespace ROI.Spawning
{
    public abstract class SpawnCondition
    {
        public static readonly WastelandSpawnCondition wasteland = new WastelandSpawnCondition();
        public static UnsafeWastelandDirtWall unsafeWastelandDirtWall = new UnsafeWastelandDirtWall();
        public static unsafeWastelandRockWall unsafeWastelandRockWall = new unsafeWastelandRockWall();


        public abstract bool Active(int x, int y);


        public abstract float SpawnChance { get; }
    }
}