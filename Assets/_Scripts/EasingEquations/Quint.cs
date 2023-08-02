﻿using System;

namespace EasingEquations
{
    public static class Quint
    {
        /// <summary>
        ///     Creates a smooth animation or interpolation effect that starts slowly and accelerates over time.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <returns>The correct value.</returns>
        public static float EaseIn(float t) => t * t * t * t * t;

        /// <summary>
        ///     Creates a smooth animation or interpolation effect that starts quickly and decelerates towards the end.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <returns>The correct value.</returns>
        public static float EaseOut(float t) => 1 - MathF.Pow(1 - t, 5);

        /// <summary>
        ///     Creates a smooth animation or interpolation effect that starts slowly, accelerates in the middle, and then
        ///     decelerates towards the end.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <returns>The correct value.</returns>
        public static float EaseInOut(float t) => t < 0.5
            ? 16 * t * t * t * t * t
            : 1 - MathF.Pow(-2 * t + 2, 5) / 2;
    }
}