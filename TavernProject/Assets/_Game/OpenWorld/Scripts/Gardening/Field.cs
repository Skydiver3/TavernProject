using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour, IInteractable
{
    //player: in trigger (stay) with tool selected
    //spawn marker on tile closest to interaction transform of player
    //  get tile in matrix closest to interaction transform

    //field is divided into a matrix of tiles
    //
    public float tileWidth = 0.5f;
    public GameObject markerPrefab;
    public Material freeMaterial;
    public Material usedMaterial;
    public GameObject plantBedPrefab;
    private GameObject marker;
    private bool canPlant = false;
    private string interactionText = "Plough";

    private List<int[]> usedTiles = new List<int[]>();
    private List<GameObject> planted = new List<GameObject>();
    private int[] selectedTile;
    private PlantBed selectedBed;


    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        //select tile
        selectedTile = GetTileAt(other.transform.position);
        MoveMarkerToTile(selectedTile);

        UpdateSelection();
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        HideMarker();
        canPlant = false;
        selectedBed = null;
    }

    private void UpdateSelection()
    {
        //check if selected tile is interactable
        MeshRenderer renderer = marker.GetComponent<MeshRenderer>();
        if (GetTileUsed(selectedTile))
        {
            canPlant = false;
            renderer.material = usedMaterial;
            selectedBed = GetPlantBed(GetIndexOfTile(selectedTile));
        }
        else
        {
            canPlant = true;
            renderer.material = freeMaterial;
            selectedBed = null;
        }
    }

    private bool GetTileUsed(int[] coords)
    {
        for (int i = 0; i < usedTiles.Count; i++)
        {
            if (usedTiles[i][0] == coords[0] && usedTiles[i][1] == coords[1])
            {
                return true;
            }
        }
        return false;
    }
    private int GetIndexOfTile(int[] tile)
    {
        int index = -1;
        for (int i = 0; i < usedTiles.Count; i++)
        {
            if (usedTiles[i][0] == tile[0] && usedTiles[i][1] == tile[1])
            {
                index = i;
                break;
            }
        }
        return index;
    }

    private bool TryPlantAt(int[] coords)
    {
        int index = GetIndexOfTile(coords);
        if (index == -1)
        {
            //tile not used yet
            //spawn plant bed there
            planted.Add(SpawnPlantBedAt(coords));
            //add tile to used
            usedTiles.Add(coords);
            return true;
        }
        else
        {
            //tile used already
            //do not plant
            return false;
        }        
    }

    private PlantBed GetPlantBed(int index)
    {
        PlantBed bed = planted[index].GetComponent<PlantBed>();
        return bed;
    }

    private GameObject SpawnPlantBedAt(int[] tile)
    {
        GameObject newBed = Instantiate(plantBedPrefab, GetTileSpawnPos(tile), Quaternion.identity, transform);
        return newBed;
    }

    private void HideMarker()
    {
        marker.SetActive(false);
    }

    private void MoveMarkerToTile(int[] tileCoords)
    {
        Vector3 newMarkerPos = GetTileSpawnPos(tileCoords);

        if (marker == null)
        {
            marker = Instantiate(markerPrefab, transform);
        }

        marker.SetActive(true);
        marker.transform.position = newMarkerPos;
    }


    private Vector3 GetTileSpawnPos(int[] tileCoord)
    {
        Vector3 relativeSpawnPos = new Vector3(tileCoord[0] * tileWidth, 0, tileCoord[1] * tileWidth);
        Vector3 spawnPos = transform.position + relativeSpawnPos;
        return spawnPos;
    }

    private int[] GetTileAt(Vector3 playerPos)
    {
        //get 2d coordinates
        //get player pos relative to plane
        //player pos on plane = playerPos-planePos
        Vector3 localPos = playerPos - transform.position;
        Vector2 posOnPlane = new Vector2(localPos.x, localPos.z);
        return GetTileOnPlane(posOnPlane);
    }

    private int[] GetTileOnPlane(Vector2 pos)
    {
        //if player at 0.7|3.1, that is tile 2|7
        int tileX = (int)(pos.x / tileWidth);
        int tileY = (int)(pos.y / tileWidth);
        int[] coordinates  = { tileX, tileY};

        return coordinates;
    }

    public bool GetInteractive()
    {
        bool interactive = canPlant || selectedBed.GetInteractive();
        return interactive;
    }

    public void Interact()
    {
        print("interact");
        //plough, interact with plant bed
        if (!TryPlantAt(selectedTile))
        {
            selectedBed.Interact();
        }
    }

    public string GetInteractionText()
    {
        UpdateSelection();

        string text = "[Field interaction] error";
        if (canPlant) text = interactionText;
        if (selectedBed) text = selectedBed.GetInteractionText();
        return text;
    }


}
