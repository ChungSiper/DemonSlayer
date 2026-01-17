using uk.vroad.api.map;

namespace uk.vroad.ucm
{
    public class RoofWrapper
    {
        public readonly IOutline outline;

        public RoofWrapper(IOutline ol)
        {
            outline = ol;
        }
    }
}