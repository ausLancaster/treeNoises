﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain
{
    // farly standary dunes with hills

    public class SeussNoise0 : INoiseProvider
    {

        float turbulenceFreq = 1/80.0f;
        float turbulenceAmp = 120.0f; //80.0f;
        float sinAmplitude = 5.0f;
        float sinFreq = 1 / 16.0f;
        float cutoffRatio = 0.2f;

        float hillMax = 30.0f;
        float hillFrequency = 1 / 120.0f;

        /*
                float turbulenceFreq = 1 / 80.0f;
        float turbulenceAmp = 80.0f;
        float sinAmplitude = 1.5f;
        float sinFreq = 1 / 8.0f;
        float cutoffRatio = 0.2f;
        */

        public float GetValue(float x, float y)
        {

            // create dunes
            var x0 = 2 * Mathf.PerlinNoise(x * turbulenceFreq + 10000, y * turbulenceFreq + 20000) - 1;
            var y0 = 2 * Mathf.PerlinNoise(x * turbulenceFreq + 30000, y * turbulenceFreq + 40000) - 1;
            var result = sinAmplitude * (Mathf.Sin((x + turbulenceAmp * x0) * sinFreq) + Mathf.Sin((y + turbulenceAmp * x0) * sinFreq));
            if (result < sinAmplitude * cutoffRatio) result = sinAmplitude * cutoffRatio;

            // add large hills
            var hill = Mathf.PerlinNoise(x * hillFrequency + 10000, y * hillFrequency + 20000);
            result += hill * hillMax;
            return result;

        }
    }

}