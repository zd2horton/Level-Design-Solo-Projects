using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PaletteData : MonoBehaviour
{
    public int paletteSlot, usesLeft;
    public Tile[] tileAssets;
    public string tileName;

    private void Start()
    {
        if (paletteSlot != 12)
        {
            usesLeft = 5;
        }

        switch (paletteSlot)
        {
            case 11:
                usesLeft = 4;
                break;

            case 12:
                usesLeft = 25;
                break;
        }
    }
}