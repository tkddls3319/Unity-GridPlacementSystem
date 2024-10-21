using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

public class PlacementSystem : MonoBehaviour
{

    [SerializeField]
    GameObject mouseIndicator, cellIndicator;

    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;
    private int selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;

    private void Start()
    {
        StopPlacement();
        //gridVisualization.SetActive(false);
        //floorData = new();
        //furnitureData = new();
    }

    public void StartPlacement(int ID)
    {
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);

        if(selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
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
        GameObject newObject = GameObject.Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);

        //buildingState.OnAction(gridPosition);

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
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;

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
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

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
