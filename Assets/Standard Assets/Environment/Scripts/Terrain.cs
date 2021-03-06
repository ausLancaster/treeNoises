﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Geometry;


namespace Terrain {

    public class Terrain
    {
        private float[,] heightMap;
        private float length;
        private float height;
        private int resolution;

        public Terrain(float[,] heightMap, float length, float height, int resolution) {

            this.heightMap = heightMap;
            this.length = length;
            this.height = height;
            this.resolution = resolution;
        }

        public MeshBuilder Generate()
        {
            var mb = new MeshBuilder();


            for (int i = 0; i < heightMap.GetLength(0); i++)
            {
                for (int j = 0; j < heightMap.GetLength(1); j++)
                {
                    float x = i * (length / (resolution-1));
                    float y = j * (length / (resolution-1));

                    mb.vertices.Add(new Vector3(x - length/2, heightMap[i, j], y - length/2));
                }
            }


            for (int i=0; i< heightMap.GetLength(0)-1; i++)
            {
                for (int j=0; j< heightMap.GetLength(1)-1; j++)
                {
                    int x00 = i + j * heightMap.GetLength(0);
                    int x01 = i + (j + 1) * heightMap.GetLength(0);
                    int x10 = i + 1 + j * heightMap.GetLength(0);
                    int x11 = i + 1 + (j + 1) * heightMap.GetLength(0);

                    mb.triangles.Add(x00);
                    mb.triangles.Add(x10);
                    mb.triangles.Add(x11);

                    mb.triangles.Add(x00);
                    mb.triangles.Add(x11);
                    mb.triangles.Add(x01);

                }
            }


            for (int i = 0; i < mb.vertices.Count; i++)
            {
                mb.normals.Add(Vector3.zero);
            }

            for (int i=0; i<mb.triangles.Count; i+=3)
            {
                var ia = mb.triangles[i];
                var ib = mb.triangles[i+1];
                var ic = mb.triangles[i+2];

                var e1 = mb.vertices[ia] - mb.vertices[ib];
                var e2 = mb.vertices[ic] - mb.vertices[ib];
                var no = Vector3.Cross(e1, e2);

                mb.normals[ia] += no;
                mb.normals[ib] += no;
                mb.normals[ic] += no;
            }

            for (int i = 0; i < mb.normals.Count; i++)
            {
                mb.normals[i] = -Vector3.Normalize(mb.normals[i]);
                
            }



            // cull extra rows
            for (int i = heightMap.GetLength(0) - 2; i >= 0; i--)
            {
                for (int j = heightMap.GetLength(1) - 2; j >= 0; j--)
                {
                    if (i == 0 || i == heightMap.GetLength(0) - 2 || j == 0 || j == heightMap.GetLength(1) - 2)
                    {
                        for (int k = 5; k >= 0; k--)
                        {
                            mb.triangles.RemoveAt((i * (heightMap.GetLength(0) - 1) + j)*6 + k);
                        }
                    }
                }
            }



            return mb;
        }
    }


}
