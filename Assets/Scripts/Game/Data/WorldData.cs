using System;

namespace Game.Data
{
    [Serializable]
    public class WorldData
    {
        public WorldData(string initialLevel)
        {
            PositionOnLevel = new PositionOnLevel(initialLevel);
        }

        public PositionOnLevel PositionOnLevel;

    }
}