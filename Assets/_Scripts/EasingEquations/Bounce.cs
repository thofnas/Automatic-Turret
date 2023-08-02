namespace EasingEquations
{
    public static class Bounce
    {
        /// <summary>
        ///     Creates a smooth animation or interpolation effect that starts slowly and accelerates over time.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <returns>The correct value.</returns>
        public static float EaseIn(float t) => 1 - EaseOut(1 - t);

        /// <summary>
        ///     Creates a smooth animation or interpolation effect that starts quickly and decelerates towards the end.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <returns>The correct value.</returns>
        public static float EaseOut(float t)
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;

            switch (t)
            {
                case < 1 / d1:
                    return n1 * t * t;
                case < 2 / d1:
                    return n1 * (t -= 1.5f / d1) * t + 0.75f;
                case < 2.5f / d1:
                    return n1 * (t -= 2.25f / d1) * t + 0.9375f;
                default:
                    return n1 * (t -= 2.625f / d1) * t + 0.984375f;
            }
        }

        /// <summary>
        ///     Creates a smooth animation or interpolation effect that starts slowly, accelerates in the middle, and then
        ///     decelerates towards the end.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <returns>The correct value.</returns>
        public static float EaseInOut(float t) => t < 0.5
            ? (1 - EaseOut(1 - 2 * t)) / 2
            : (1 + EaseOut(2 * t - 1)) / 2;
    }
}
