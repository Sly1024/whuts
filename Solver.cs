using System.Collections;
using System;
using System.Collections.Generic;
namespace whuts
{
    public class Solver
    {

        P3D[] octocube;
        List<P3D[]> rotations;

        List<P3D> boxes;
        // bool[,,] tiling;
        TileBits tiling;
        P3D currentBox;
        int currentCount;

        (P3D offset, int rotation)[] currentPieces;

        public Solver(P3D[] piece) => octocube = piece;

        public void FindFit()
        {
            GenerateBoxes(4, 4, 4);
            Console.WriteLine("found {0} boxes", boxes.Count);
            GenerateRotations();
            Console.WriteLine("generated {0} rotations", rotations.Count);

            foreach (var box in boxes)
            {
                Console.WriteLine("Trying box " + box);
                if (TryFitBox(box))
                {
                    //Console.WriteLine("Fit in box " + box);
                    foreach (var piece in currentPieces)
                    {
                        Console.WriteLine(" " + piece.offset + " r:" + piece.rotation);
                    }
                    break;
                }
            }
        }
        private bool TryFitBox(P3D box)
        {
            currentBox = box;
            currentCount = (box.x * box.y * box.z) / 8;
            currentPieces = new (P3D offset, int rotation)[currentCount];

            // need a 3d array to store bits (occupied/empty)
            // tiling = new bool[box.x, box.y, box.z];
            tiling = new TileBits(box);

            // put first piece in with no rotation, no offset
            if (TryPlace((0, 0, 0), 0) == null) return false;
            currentPieces[0] = ((0, 0, 0), 0);

            return TryPlaceAll(1);
        }

        bool TryPlaceAll(int idx, int minRot = 0, int minX = 0, int minY = 0, int minZ = 0)
        {
            if (idx == currentCount) return true;

            // all rotations
            for (var rot = minRot; rot < rotations.Count; rot++)
            {
                // all offsets
                for (var offx = rot == minRot ? minX : 0; offx < currentBox.x; offx++)
                {
                    for (var offy = rot == minRot && offx == minX ? minY : 0; offy < currentBox.y; offy++)
                    {
                        for (var offz = rot == minRot && offx == minX && offy == minY ? minZ : 0; offz < currentBox.z; offz++)
                        {
                            if (offx == 0 && offy == 0 && offz == 0) continue;
                            var pos = TryPlace((offx, offy, offz), rot);
                            if (pos != null)
                            {
                                // place successful
                                if (TryPlaceAll(idx + 1, rot, offx, offy, offz))
                                {
                                    currentPieces[idx] = ((offx, offy, offz), rot);
                                    return true;
                                }
                                UnPlace(pos);
                            }
                        }
                    }
                }
            }
            return false;
        }

        P3D[] TryPlace(P3D offset, int rotation)
        {
            var rotated = rotations[rotation];
            var positions = new P3D[8];
            var i = 0;
            for (; i < 8; i++)
            {
                var p = positions[i] = (rotated[i] + offset) % currentBox;
                if (tiling[p]) break;
                tiling[p] = true;
            }
            if (i < 8)
            {
                while (--i >= 0) tiling[positions[i]] = false;
                return null;
            }
            return positions;
        }

        void UnPlace(P3D[] pos)
        {
            for (var i = 0; i < 8; i++)
            {
                tiling[pos[i]] = false;
            }
        }

        void GenerateBoxes(int maxSize1, int maxSize2, int maxSize3)
        {
            boxes = new List<P3D>();

            for (var s1 = 2; s1 <= maxSize1; s1++)
            {
                for (var s2 = 2; s2 <= maxSize1; s2++)
                {
                    for (var s3 = 2; s3 <= maxSize1; s3++)
                    {
                        int prod = s1 * s2 * s3;
                        if (prod % 8 == 0)
                        {
                            boxes.Add((s1, s2, s3));
                        }
                    }
                }
            }
        }

        void GenerateRotations()
        {
            rotations = new List<P3D[]>();

            var matrix = new P3D[] { (1, 0, 0), (0, 1, 0), (0, 0, 1) };

            void AddReflections(P3D[] matrix)
            {
                rotations.Add(GenerateRotation((matrix[0], matrix[1], matrix[2])));
                rotations.Add(GenerateRotation((-matrix[0], -matrix[1], matrix[2])));
                rotations.Add(GenerateRotation((matrix[0], -matrix[1], -matrix[2])));
                rotations.Add(GenerateRotation((-matrix[0], matrix[1], -matrix[2])));
            }

            foreach (var permut in EvenPermutations)
            {
                AddReflections(PermuteRows(matrix, permut));
            }

            foreach (var permut in OddPermutations)
            {
                var mat2 = PermuteRows(matrix, permut);
                mat2[0] = -mat2[0];
                AddReflections(mat2);
            }
        }



        P3D[] GenerateRotation((P3D row1, P3D row2, P3D row3) matrix)
        {
            var rotated = new P3D[8];

            for (var i = 0; i < 8; i++)
            {
                rotated[i] = (DotProduct(matrix.row1, octocube[i]), DotProduct(matrix.row2, octocube[i]), DotProduct(matrix.row3, octocube[i]));
            }

            return rotated;
        }

        int DotProduct(P3D v1, P3D v2) => v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;

        int[][] EvenPermutations = new int[][] { new int[] { 0, 1, 2 }, new int[] { 1, 2, 0 }, new int[] { 2, 0, 1 } };
        int[][] OddPermutations = new int[][] { new int[] { 0, 2, 1 }, new int[] { 2, 1, 0 }, new int[] { 1, 0, 2 } };

        P3D[] PermuteRows(P3D[] rows, int[] permut)
        {
            return new P3D[] {
                rows[permut[0]],
                rows[permut[1]],
                rows[permut[2]]
            };
        }
    }
}