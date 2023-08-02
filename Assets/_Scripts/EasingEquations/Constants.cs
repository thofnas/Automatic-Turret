using System;

namespace EasingEquations
{
    public static class Constants
    {
        public const float TOLERANCE = 0.0001f;
        public const float C1 = 1.70158f;
        public const float C2 = C1 * 1.525f;
        public const float C3 = C1 + 1f;
        public const float C4 = 2f * MathF.PI / 3f;
        public const float C5 = 2f * MathF.PI / 4.5f;
    }
}
