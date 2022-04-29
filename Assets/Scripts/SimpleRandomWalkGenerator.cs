using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomWalkGenerator : AbstractDungeonGenerator
{
    [SerializeField] protected SimpleRandomWalkSO simpleRandomWalkData;

    protected override void RunProceduralGeneration() {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(simpleRandomWalkData, startPosition);
        tilemapVisualizer.Visualize(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO simpleRandomWalkData, Vector2Int position) {
        var currentPosition = position;
        var floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < simpleRandomWalkData.iterations; i++) {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, simpleRandomWalkData.walkLength);
            floorPositions.UnionWith(path);
            if (simpleRandomWalkData.startRandomEachIteration) {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        return floorPositions;
    }
}
