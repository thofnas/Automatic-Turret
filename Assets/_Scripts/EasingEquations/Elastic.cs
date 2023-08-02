using System;
using static EasingEquations.Constants;

namespace EasingEquations
{
    public static class Elastic
    {
        /// <summary>
        ///     Creates a smooth animation or interpolation effect that starts slowly and accelerates over time.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <returns>The correct value.</returns>
        public static float EaseIn(float t) => t == 0
            ? 0
            : Math.Abs(t - 1) < TOLERANCE
                ? 1
                : -MathF.Pow(2, 10 * t - 10) * MathF.Sin((t * 10 - 10.75f) * C4);

        /// <summary>
        ///     Creates a smooth animation or interpolation effect that starts quickly and decelerates towards the end.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <returns>The correct value.</returns>
        public static float EaseOut(float t) => t == 0
            ? 0
            : Math.Abs(t - 1) < TOLERANCE
                ? 1
                : MathF.Pow(2, -10 * t) * MathF.Sin((t * 10 - 0.75f) * C4) + 1;

        /// <summary>
        ///     Creates a smooth animation or interpolation effect that starts slowly, accelerates in the middle, and then
        ///     decelerates towards the end.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <returns>The correct value.</returns>
        public static float EaseInOut(float t) => t == 0
            ? 0
            : Math.Abs(t - 1) < TOLERANCE
                ? 1
                : t < 0.5
                    ? -(MathF.Pow(2, 20 * t - 10) * MathF.Sin((20 * t - 11.125f) * C5)) / 2
                    : MathF.Pow(2, -20 * t + 10) * MathF.Sin((20 * t - 11.125f) * C5) / 2 + 1;
    }
}
