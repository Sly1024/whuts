## WHUTS
Which hypercube unfoldings tile space? (See http://whuts.org)

I got excited when I saw Matt Parker's video and started coding. It took me a while and this is probably not the best way to do it but anyway...

## The Idea
The basic idea is that if a 3D tiling exists, it must repeat in all 3 directions. So there is a "box" that when carved out of the tiled 3D space, will contain N number of pieces (8*N little cubes) and that arrangement repeats in all directions. So I try to find the box size and the arrangements by brute force.

* The box must contain an integer number of pieces, so its volume must be divisible by 8 (e.g. 4x2x2)
* When I place a piece and it sticks out in one direction I just wrap it around to the other side. So placing means I calculate the position of each little cube then do an integer remainder on each coordinate with the size of the box. (See P3D.cs, override of % operator)
* I keep track of which little cube is occupied in the 3D box. This is a boolean array and I map the 3D coordinates to a 1D array as efficiently as possible. Since we don't use huge boxes (max size is 4x4x4) I can allocate more space and use sizes that are power of 2 to make the calulation more efficient by using bit shift instead of multiplication. (See TileBits.cs).
* Because of the wrapping, the possible offsets I need to check is just the size of the current box, the possible rotations are the 24 isomorphic rotations.

## The Algorithm
* The `GenerateBoxes` function generates all the possible box sizes that are valid and sorts them in ascending order of the maximum of the dimensions.
* The `GenerateRotations` functoin pre-generates the 24 possible isomorphic rotations of the current piece.
* We place the first piece with offset (0,0,0) rotation index 0 - no need to try and rotate it, because we'll try other box sizes that are rotations of the current box (e.g. 4x2x2 vs. 2x4x2).
* First I generate all possible combinations, then I filter out the ones that clash with the already placed pieces.
* The recursive `TryPlaceAll` function tries placing all working "combinations" of (offset + rotation).
* After successfully placing a piece, it calls itself with the filtered combinations list and the fist index that is worth trying.

## The Performance
* I usually code in JavaScript/TypeScript, but I thought this one might need the extra performance, so I wrote it in C# (no, I won't go C++ for this).
* I limited the max search box size to 4x4x4 and all but one pieces (260/261) actually fit in one of these small boxes. For the one piece that doesn't fit, I tried running the algorithm with bigger boxes, but I just couldn't wait until it finishes.
* With these restrictions it runs in 12 seconds (for all 260 pieces) on one thread on my 8 year old second gen Core i3 CPU.
