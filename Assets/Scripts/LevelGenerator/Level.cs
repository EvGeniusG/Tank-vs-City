
using System.Collections.Generic;
using UnityEngine;

class Level{

    public Vector2Int startPosition = Vector2Int.zero;
    public Vector2Int finishPosition;
    public List<Street> mainRoot = new List<Street>();

    public Level(
        int mainRootLength,
        int minStreetLength,
        int maxStreetLength,
        float crossProbability
    ){
        // Создание улиц
        mainRoot = new List<Street>();

        Vector2Int direction = new Vector2Int(0, 1);

        for(int i = 0; i < mainRootLength; i++){
            mainRoot.Add(new Street(minStreetLength, maxStreetLength, direction, crossProbability));
            direction = changeDirection(direction);
        }
        finishPosition = getFinishPosition();
    }
    Vector2Int getFinishPosition(){
         Vector2Int tilePosition = startPosition;
         for(int i = 0; i < mainRoot.Count; i++){
            var streetRoads = getStreetRoadPositions(tilePosition, mainRoot[i]);
            tilePosition += mainRoot[i].direction *
            (
                mainRoot[i].length * 2
                +
                (mainRoot[i].crossLeft != null || mainRoot[i].crossRight != null ? 1 : 0)
                +
                1
            );
        }
        return tilePosition;

    }
    public HashSet<Vector2Int> getRoadPositions(){
        var roads = new HashSet<Vector2Int>
        {
            startPosition
        };

        Vector2Int tilePosition = startPosition;

        for(int i = 0; i < mainRoot.Count; i++){
            var streetRoads = getStreetRoadPositions(tilePosition, mainRoot[i]);
            foreach(var road in streetRoads){
                roads.Add(road);
            }
            tilePosition += mainRoot[i].direction *
            (
                mainRoot[i].length * 2
                +
                (mainRoot[i].crossLeft != null || mainRoot[i].crossRight != null ? 1 : 0)
                +
                1
            );

            roads.Add(tilePosition); 
        }

        return roads;
        
    }

    public HashSet<Vector2Int> getStreetRoadPositions(Vector2Int zeroPosition, Street street){
        var roads = new HashSet<Vector2Int>();
        Vector2Int tilePosition = new Vector2Int(zeroPosition.x, zeroPosition.y);

        tilePosition += street.direction;
        for(int j = 0; j < street.length; j++){
            roads.Add(tilePosition);
            tilePosition += street.direction;
        }
        if(street.crossLeft != null || street.crossRight != null){
            //Тайл на перекрестке
            roads.Add(tilePosition);
            if(street.crossLeft != null){
                var crossLeft = getStreetRoadPositions(tilePosition, street.crossLeft);
                foreach(var cl in crossLeft){
                    roads.Add(cl);
                }
            }
            if(street.crossRight != null){
                var crossRight = getStreetRoadPositions(tilePosition, street.crossRight);
                foreach(var cl in crossRight){
                    roads.Add(cl);
                }
            }
            tilePosition += street.direction;
        }
        for(int j = 0; j < street.length; j++){
            roads.Add(tilePosition);
            tilePosition += street.direction;
        }
        return roads;
    }

    Vector2Int changeDirection(Vector2Int direction){
        if(direction == new Vector2Int(0, 1)){
            return Random.value > 0.5f
                ? new Vector2Int(1, 0)
                : new Vector2Int(-1, 0);
        }
        return new Vector2Int(0, 1);
    }

}