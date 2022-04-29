using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorsFirstGenerator : SimpleRandomWalkGenerator
{
    [SerializeField] private int corridorLength = 14, corridorCount = 5;
    [SerializeField] [Range(0.1f, 1)] private float roomPercent = .8f;

    protected override void RunProceduralGeneration()
    {
        RunCorridorsFirst();
    }

    private void RunCorridorsFirst() {
        var floorPositions = new HashSet<Vector2Int>();
        var potentialRoomPositions = new HashSet<Vector2Int>();
        CreateCorridors(floorPositions, potentialRoomPositions);
        var roomPositions = CreateRooms(potentialRoomPositions);
        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);
        CreateRoomsAtDeadEnds(deadEnds, roomPositions);
        floorPositions.UnionWith(roomPositions);
        tilemapVisualizer.Visualize(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions) {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);
        for (int i = 0; i < corridorCount; i++) {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions) {
        var roomPositions = new HashSet<Vector2Int>();
        int roomCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);
        List<Vector2Int> roomToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomCount).ToList();
        foreach (var roomPosition in roomToCreate) {
            var newRoom = RunRandomWalk(simpleRandomWalkData, roomPosition);
            roomPositions.UnionWith(newRoom);
        }
        return roomPositions;
    }

    private void CreateRoomsAtDeadEnds(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomPositions) {
        foreach (var position in deadEnds) {
            if (roomPositions.Contains(position) == false) {
                var newRoom = RunRandomWalk(simpleRandomWalkData, position);
                roomPositions.UnionWith(newRoom);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions) {
        var deadEnds = new List<Vector2Int>();
        foreach (var position in floorPositions) {
            int neighbourCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList) {
                if (floorPositions.Contains(position + direction)) {
                    neighbourCount++;
                }
            }
            if (neighbourCount == 1) {
                deadEnds.Add(position);
            }
        }
        return deadEnds;
    }
}
