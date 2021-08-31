using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TilePlacementScript : MonoBehaviour
{
    public Tile[] tilesToPlace;
    private bool containingTiles;
    public Tilemap groundMap;
    private GameObject isThis;
    private PaletteData currentData;
    public GameObject canvasSelect;

    void Start()
    {
        containingTiles = false;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //RaycastHit2D mouseCollision = Physics2D.Raycast(worldPos, Vector2.zero);

            Vector3Int cellPos = groundMap.WorldToCell(worldPos);
            //Debug.Log(groundMap.HasTile(cellPos));
            //Debug.Log(groundMap.GetTile(cellPos));

            if (tilesToPlace != null && currentData.usesLeft > 0 &&
                EventSystem.current.currentSelectedGameObject == null)
            {
                if (currentData.paletteSlot != 12)
                {
                    switch (tilesToPlace.Length)
                    {
                        case 1:
                            if (groundMap.HasTile(cellPos) == false)
                            {
                                groundMap.SetTile(cellPos, tilesToPlace[0]);
                                isThis.GetComponent<PaletteData>().usesLeft--;

                            }
                            break;

                        default:
                            int numberOfEmpty = 0;

                            for (int i = 0; i < (tilesToPlace.Length / 3); i++)
                            {
                                for (int j = -1; j < 2; j++)
                                {
                                    if (groundMap.HasTile(new Vector3Int(cellPos.x + j, cellPos.y - i, cellPos.z)) == false)
                                    {
                                        numberOfEmpty++;
                                    }
                                }
                            }

                            if (numberOfEmpty == tilesToPlace.Length)
                            {
                                int placeIndex = 0;
                                for (int i = 0; i < (tilesToPlace.Length / 3); i++)
                                {
                                    for (int j = -1; j < 2; j++)
                                    {
                                        groundMap.SetTile(new Vector3Int(cellPos.x + j, cellPos.y - i, cellPos.z), tilesToPlace[placeIndex]);
                                        placeIndex++;
                                    }
                                }

                                currentData.usesLeft--;
                            }
                            break;
                    }
                }

                else if (currentData.paletteSlot == 12)
                {
                    if (groundMap.HasTile(cellPos))
                    {
                        groundMap.SetTile(cellPos, null);
                        currentData.usesLeft--;
                    }
                }
                
            }
            //if (mouseCollision.collider != null)
            //{
            //    Debug.Log(mouseCollision.collider.gameObject.name);
            //}
        }

        else if (Input.GetMouseButtonDown(1))
        {
            tilesToPlace = null;
            canvasSelect.GetComponent<TextMeshProUGUI>().text = "Currently Selected: Empty";
        }

        if (isThis != null) isThis.transform.GetChild(1).GetComponent<Text>().text = currentData.usesLeft.ToString();
    }

    public void ClickAction()
    {
        isThis = EventSystem.current.currentSelectedGameObject;
        currentData = isThis.GetComponent<PaletteData>();
        tilesToPlace = currentData.tileAssets;
        containingTiles = true;

        canvasSelect.GetComponent<TextMeshProUGUI>().text = "Currently Selected: " + currentData.tileName;
    }
}