﻿using System;

namespace Voxels {
    public class AmbientOcclusion {
        /// <summary>
        /// Calculate simple ambient for voxel based on surrounding voxels.
        /// https://0fps.net/2013/07/03/ambient-occlusion-for-minecraft-like-worlds/
        /// </summary>
        /// <param name="voxelData">voxel data set.</param>
        /// <param name="p">The voxel being rendered.</param>
        /// <param name="left">The voxel to the left side.</param>
        /// <param name="right">The voxel to the right side.</param>
        /// <param name="up">The voxel above.</param>
        /// <returns>An integer from 0-3 representing the occlusion case.</returns>
        public static int CalculateAO(VoxelData voxelData, XYZ p, XYZ left, XYZ right, XYZ up) {
            var side1 = Math.Sign(voxelData[p + left + up].Index); // 0 or 1
            var side2 = Math.Sign(voxelData[p + right + up].Index); // 0 or 1
            var corner = Math.Sign(voxelData[p + left + right + up].Index); // 0 or 1

            if (side1 == 1 && side2 == 1) {
                return 0;
            }
            return 3 - (side1 + side2 + corner);
        }

        public static float AOToOcclusion(int ao) {
            return new[] { 0.5f, 0.75f, 0.8f, 1.0f }[ao];
        }

        public static Color CombineColorOcclusion(Color color, float occlusion) {
            float h, s, v;
            color.ToHSV(out h, out s, out v);

            if (color.A == 0) {
                return new Color(0, 0, 0, 1 - occlusion);
            }
            else {
                return new Color(Color.FromHSV(h, s, v * occlusion), color.A);
            }
        }
    }
}
