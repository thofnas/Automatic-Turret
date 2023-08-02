using System;
using static EasingEquations.Constants;

namespace EasingEquations
{
    public static class Back
    {
        /// <summary>
        ///     Creates a smooth animation or interpolation effect that starts slowly and accelerates over time.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <returns>The correct value.</returns>
        public static float EaseIn(float t) => C3 * t * t * t - C1 * t * t;

        /// <summary>
        ///     Creates a smooth animation or interpolation effect that starts quickly and decelerates towards the end.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <returns>The correct value.</returns>
        public static float EaseOut(float t) => 1 + C3 * MathF.Pow(t - 1, 3) + C1 * MathF.Pow(t - 1, 2);

        /// <summary>
        ///     Creates a smooth animation or interpolation effect that starts slowly, accelerates in the middle, and then
        ///     decelerates towards the end.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <returns>The correct value.</returns>
        public static float EaseInOut(float t) => t < 0.5
            ? MathF.Pow(2 * t, 2) * ((C2 + 1) * 2 * t - C2) / 2
            : (MathF.Pow(2 * t - 2, 2) * ((C2 + 1) * (t * 2 - 2) + C2) + 2) / 2;
    }
}
