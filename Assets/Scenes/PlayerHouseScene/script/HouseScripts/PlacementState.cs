using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDataBaseSO database;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;

    // Constructor for initializing the PlacementState with required dependencies.
    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectsDataBaseSO database,
                          GridData floorData,
                          GridData furnitureData,
                          ObjectPlacer objectPlacer)
    {
        // Initialize fields with provided parameters.
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        // Find the index of the selected object in the database using its ID.
        selectedObjectIndex = database.objectData.FindIndex(data => data.ID == ID);

        // If the object is found, start showing the placement preview.
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(
                       database.objectData[selectedObjectIndex].Prefab,
                       database.objectData[selectedObjectIndex].Size);
        }
        else
        {
            // If the object is not found, throw an exception.
            throw new System.Exception($"No object with ID {iD}");
        }
    }

    // Ends the current placement state, stopping the preview.
    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    // Called when an action (like clicking) is performed at a specific grid position.
    public void OnAction(Vector3Int gridPos)
    {
        // Check if the object can be placed at the specified grid position.
        bool placementValidity = CheckPlacementValidity(gridPos, selectedObjectIndex);
        if (!placementValidity)
            return;  // If placement is not valid, exit the method.

        // Place the object in the world using the object placer.
        int index = objectPlacer.PlaceObject(
            database.objectData[selectedObjectIndex].Prefab,
            grid.CellToWorld(gridPos));

        // Determine whether the object is floor data or furniture data.
        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ?
            floorData : furnitureData;

        // Record the placed object's position, size, ID, and index in the grid data.
        selectedData.AddObjectAt(gridPos,
                                 database.objectData[selectedObjectIndex].Size,
                                 database.objectData[selectedObjectIndex].ID,
                                 index);

        // Update the preview position and make it invalid (since the object is placed).
        previewSystem.UpdatePosition(grid.CellToWorld(gridPos), false);
    }

    // Updates the placement state as the player moves the cursor on the grid.
    public void UpdateState(Vector3Int gridPos)
    {
        // Check if the object can be placed at the new grid position.
        bool placementValidity = CheckPlacementValidity(gridPos, selectedObjectIndex);

        // Update the preview system's position and validity based on the new grid position.
        previewSystem.UpdatePosition(grid.CellToWorld(gridPos), placementValidity);
    }

    // Checks if the object can be placed at the specified grid position.
    private bool CheckPlacementValidity(Vector3Int gridPos, int selectedObjectIndex)
    {
        // Determine whether the object is floor data or furniture data.
        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ?
            floorData : furnitureData;

        // Check if the object can be placed at the given grid position based on its size.
        return selectedData.CanPlaceObjectAt(gridPos, database.objectData[selectedObjectIndex].Size);
    }
}
