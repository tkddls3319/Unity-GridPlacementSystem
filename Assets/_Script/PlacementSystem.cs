using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

public class PlacementSystem : MonoBehaviour
{

    [SerializeField]
    GameObject mouseIndicator;

    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;
    private int selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;

    GridData floorData, furnitureData;

    List<GameObject> placeGameObjects = new();

    [SerializeField]
    PreviewSystem preview;

    Vector3Int lastDetectedPosition = Vector3Int.zero;
    private void Start()
    {
        StopPlacement();
        floorData = new ();
        furnitureData = new();
        //gridVisualization.SetActive(false);
        //floorData = new();
        //furnitureData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);

        if(selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        preview.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab, database.objectsData[selectedObjectIndex].Size);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;

        //StopPlacement();
        //gridVisualization.SetActive(true);
        //buildingState = new PlacementState(ID,
        //                                   grid,
        //                                   preview,
        //                                   database,
        //                                   floorData,
        //                                   furnitureData,
        //                                   objectPlacer,
        //                                   soundFeedback);
        //inputManager.OnClicked += PlaceStructure;
        //inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        //StopPlacement();
        //gridVisualization.SetActive(true);
        //buildingState = new RemovingState(grid, preview, floorData, furnitureData, objectPlacer, soundFeedback);
        //inputManager.OnClicked += PlaceStructure;
        //inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if(placementValidity == false)    
            return;
       

        GameObject newObject = GameObject.Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        placeGameObjects.Add(newObject);
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, placeGameObjects.Count - 1);
        //buildingState.OnAction(gridPosition);
        preview.UpdatePosition(grid.CellToWorld(gridPosition), false);

    }
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        return selectedData.CanPlaceObejctAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    //private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    //{
    //    GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? 
    //        floorData : 
    //        furnitureData;

    //    return selectedData.CanPlaceObejctAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    //}

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        preview.StopShowingPreview();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        //soundFeedback.PlaySound(SoundType.Click);
        //if (buildingState == null)
        //    return;
        //gridVisualization.SetActive(false);
        //buildingState.EndState();
        //inputManager.OnClicked -= PlaceStructure;
        //inputManager.OnExit -= StopPlacement;
        //lastDetectedPosition = Vector3Int.zero;
        //buildingState = null;
    }

    private void Update()
    {
        if (selectedObjectIndex < 0)
            return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if(lastDetectedPosition != gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

            mouseIndicator.transform.position = mousePosition;
            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
            lastDetectedPosition = gridPosition;
        }

        //if (buildingState == null)
        //    return;
        //Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        //Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        //if (lastDetectedPosition != gridPosition)
        //{
        //    buildingState.UpdateState(gridPosition);
        //    lastDetectedPosition = gridPosition;
        //}

    }
}
