
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class WallTypesHelper
{
    public static HashSet<int> wallTop = new HashSet<int>
    {
        0b1111,
        0b0110,
        0b0011,
        0b0010,
        0b1010,
        0b1100,
        0b1110,
        0b1011,
        0b0111
    };
    public static HashSet<int> wallSideLeft = new HashSet<int>
    {
        0b0100
    };
    public static HashSet<int> wallSideRight = new HashSet<int>
    {
        0b0001
    };
    public static HashSet<int> wallBottm = new HashSet<int>
    {
        0b1000
    };
    public static HashSet<int> wallInnerCornerDownLeft = new HashSet<int>
    {
        0b11110001,
        0b11100000,
        0b11110000,
        0b11100001,
        0b10100000,
        0b10110001,
        0b10100001,
        0b10010000,
        0b00110001,
        0b10110000,
        0b00100001,
        0b10010001
    };
    public static HashSet<int> wallInnerCornerDownRight = new HashSet<int>
    {
        0b11000111,
        0b11000011,
        0b10000011,
        0b10000111,
        0b10000010,
        0b11000110,
        0b11000010,
        0b10000100,
        0b01000110,
        0b10000110,
        0b11000100,
        0b01000010
    };

    public static HashSet<int> wallInnerCornerUpLeft = new HashSet<int>
    {
        0b00111000,
        0b00111100,
        0b01101100,
        0b01111100,
        0b01111000,
        0b00101000,
        0b01101000
    };

    public static HashSet<int> wallInnerCornerUpRight = new HashSet<int>
    {
        0b00001111,
        0b00011111,
        0b00011110,
        0b00001110,
        0b00011011,
        0b00001010
    };



    public static HashSet<int> wallDiagonalCornerDownLeft = new HashSet<int>
    {
        0b01000000,
        0b10100000
    };
    public static HashSet<int> wallDiagonalCornerDownRight = new HashSet<int>
    {
        0b00000001
    };
    public static HashSet<int> wallDiagonalCornerUpLeft = new HashSet<int>
    {
        0b00010000,
        0b01010000,
    };
    public static HashSet<int> wallDiagonalCornerUpRight = new HashSet<int>
    {
        0b00000100,
        0b00000101
    };
    public static HashSet<int> wallFull = new HashSet<int>
    {
        0b1101,
        0b0101,
        0b1001
    };
    public static HashSet<int> wallFullEightDirections = new HashSet<int>
    {
        // 0b11100100,
        // 0b10010011,
        // 0b01110100,
        // 0b00010111,
        // 0b00010110,
        // 0b00110100,
        // 0b00010101,
        // 0b01010100,
        // 0b00010010,
        // 0b00100100,
        // 0b00010011,
        // 0b01100100,
        // 0b10010111,
        // 0b11110100,
        // 0b10010110,
        // 0b10110100,
        // 0b11100101,
        // 0b11010011,
        // 0b11110101,
        // 0b11010111,
        // 0b11010111,
        // 0b11110101,
        // 0b01110101,
        // 0b01010111,
        // 0b01100101,
        // 0b01010011,
        // 0b01010010,
        // 0b00100101,
        // 0b00110101,
        // 0b01010110,
        // 0b11010101,
        // 0b11010100,
        // 0b10010101
        // 0b11111001,
        // 0011111111,
        // 0b11001111,
        // 0b11111110,
        // 0b00111111,
        // 0b11001111,
        // 0b11111011,
        // 0b11101111,
        // 0b10111111,
        // 0b10101111
    };
    public static HashSet<int> wallBottmEightDirections = new HashSet<int>
    {
    };

    public static HashSet<int> wallFullyExposedTop = new HashSet<int>
    {
        0b00111110,
        0b01111110,
        0b01111111,
        0b01111001,
        0b01110011,
        0b00111111,
        0b01101011,
        0b01101111,
        0b00101110,
        0b00111010,
        0b00101010,
        0b01111011
    };
    public static HashSet<int> wallFullyExposedBottom = new HashSet<int>
    {
        0b11100011,
        0b11110011,
        0b11110111,
        0b11100010,
        0b10100011,
        0b10100010,
        0b11100111,
        0b10110111,
        0b11110110
    };
    public static HashSet<int> wallFullyExposedRight = new HashSet<int>
    {
        0b10001111,
        0b10011111,
        0b11011111,
        0b10001011,
        0b10001110,
        0b11010111,
        0b11001111,
        0b10011110,
        0b10011011
    };
    public static HashSet<int> wallFullyExposedLeft = new HashSet<int>
    {
        0b11111000,
        0b11111100,
        0b11111101,
        0b11011001,
        0b10111101,
        0b11101000,
        0b11111001,
        0b11100101,
        0b11101001
    };

    public static HashSet<int> wallFullyExposedTopCorners = new HashSet<int>
    {
        0b00010100
    };
    
    public static HashSet<int> wallFullyExposedBottomCorners = new HashSet<int>
    {
        0b01000001
    };

    public static HashSet<int> wallFullyExposed = new HashSet<int>
    {
        0b11111111,
        0b11101111,
        0b10111111,
        0b10101111,
        0b11111011,
        0b11111110,
        0b11111010,
        0b11101011,
        0b11101110,
        0b10111110,
        0b10101110,
        0b10101010,
        0b10111011
    };

    public static HashSet<int> wallExposedSides1 = new HashSet<int>
    {
        0b00110110,
        0b00110111,
        0b01100011,
        0b01100111,
        0b00110011,
        0b01110111,
        0b00100110
    };
    public static HashSet<int> wallExposedSides2 = new HashSet<int>
    {
        0b11011101,
        0b11011000,
        0b11001000,
        0b11001101,
        0b10011001,
        0b11011100,
        0b10001101,
        0b10011101
    };

    public static HashSet<int> wallCurveTop1 = new HashSet<int>
    {
        0b00010111,
        0b00010110,
    };
    public static HashSet<int> wallCurveTop2 = new HashSet<int>
    {
        0b01111010
    };
    public static HashSet<int> wallCurveBottom1 = new HashSet<int>
    {
        0b01000111,
        0b01000011,
    };
    public static HashSet<int> wallCurveBottom2 = new HashSet<int>
    {
        0b01100001,
        0b01110001,
        0b00110001,
    };
    public static HashSet<int> wallCurveLeft1 = new HashSet<int>
    {
        0b11010000,
        0b11010001,
    };
    public static HashSet<int> wallCurveLeft2 = new HashSet<int>
    {

    };
    public static HashSet<int> wallCurveRight1 = new HashSet<int>
    {
        0b11000101,
        0b10000101
    };
    public static HashSet<int> wallCurveRight2 = new HashSet<int>
    {
        
    };
    public static HashSet<int> cornerBottomRight = new HashSet<int>
    {
        0b01000100,
        0b01000101,
    };

     public static HashSet<int> cornerBottomLeft = new HashSet<int>
    {
        0b01010001,
        0b00010001,
    };
     public static HashSet<int> cornerTopRight = new HashSet<int>
    {
        0b00010101
    };
     public static HashSet<int> cornerTopLeft = new HashSet<int>
    {
        0b01010100
    };


}
