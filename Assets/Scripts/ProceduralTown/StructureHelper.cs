using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SVS
{
    public class StructureHelper : MonoBehaviour
    {
        public BuildingType[] buildingTypes;
        public GameObject[] naturePrefabs;
        public bool  randomNaturePlacement = false;
        [Range(0,1)] public float randomNaturePlacementThreshold= 0.3f;

        public Dictionary<Vector3Int, GameObject> structuresDictionary = new Dictionary<Vector3Int, GameObject>();
        public Dictionary<Vector3Int, GameObject> natureDictionary = new Dictionary<Vector3Int, GameObject>();
       
        public float animationTime = 0.01f;

        public IEnumerator PlaceStructuresAroundRoad(HashSet<Vector3Int> roadPositions)
        {
            List<Vector3Int> currentRoadPos = roadPositions.ToList();
            Dictionary<Vector3Int, Direction> freeStateSpots = FindFreeSpacesAroundRoad(currentRoadPos);

            List<Vector3Int> blockedPosition = new List<Vector3Int>();

            foreach(var freeSpot in freeStateSpots)
            {
                if(blockedPosition.Contains(freeSpot.Key))
                {
                    continue;
                }
                var rotation = UnityEngine.Quaternion.identity;
                switch(freeSpot.Value)
                {
                    case Direction.Up:
                        rotation = Quaternion.Euler(0, 90, 0);
                        break;
                    case Direction.Down:
                        rotation = Quaternion.Euler(0, -90, 0);
                        break;
                    case Direction.Right:
                        rotation = Quaternion.Euler(0, 180, 0);
                        break;
                    default:
                    break;
                }

                for(int i = 0; i  < buildingTypes.Length; i++)
                {
                    if(buildingTypes[i].quantity == -1)
                    {   
                            if(randomNaturePlacement)
                            {
                                var rnd = UnityEngine.Random.value;
                                if(rnd < randomNaturePlacementThreshold)
                                {
                                    int rd = UnityEngine.Random.Range(0, naturePrefabs.Length);
                                    var nature = SpawnPrefab(naturePrefabs[rd],freeSpot.Key,rotation);
                                    natureDictionary.TryAdd(freeSpot.Key, nature);
                                    break;
                                }
                            }
                           var building = SpawnPrefab(buildingTypes[i].GetPrefab(), freeSpot.Key, rotation);
                           structuresDictionary.TryAdd(freeSpot.Key, building);
                           break;
                    }
                    if(buildingTypes[i].IsBuildingAvailable())
                    {
                        if(buildingTypes[i].sizeRequired > 1)
                        {
                            var halfSize = Mathf.CeilToInt(buildingTypes[i].sizeRequired / 2.0f);
                            List<Vector3Int> tempPositionsBlocked = new List<Vector3Int>();
                            if(VerifyIfBuildingFits(halfSize, freeStateSpots, freeSpot, ref tempPositionsBlocked))
                            {
                                blockedPosition.AddRange(tempPositionsBlocked);
                                var building = SpawnPrefab(buildingTypes[i].GetPrefab(), freeSpot.Key, rotation);
                                structuresDictionary.TryAdd(freeSpot.Key, building);
                               
                                foreach(var pos in tempPositionsBlocked)
                                {
                                    structuresDictionary.TryAdd(pos, building);
                                }
                                break;
                            }
                        }
                        else
                        {
                            var building = SpawnPrefab(buildingTypes[i].GetPrefab(), freeSpot.Key, rotation);
                            structuresDictionary.TryAdd(freeSpot.Key, building);
                        }
                        break;
                    }
                }
                
                yield return new WaitForSeconds(animationTime);
            }
            
        }

        private bool VerifyIfBuildingFits(int halfSize, Dictionary<Vector3Int, Direction> freeStateSpots, KeyValuePair<Vector3Int, Direction> freeSpot, ref List<Vector3Int> tempPositionsBlocked)
        {
            Vector3Int direction = Vector3Int.zero;
            
            if(freeSpot.Value == Direction.Down || freeSpot.Value == Direction.Up)
            {
                direction = Vector3Int.right;
            }
            else
            {
                direction = new Vector3Int(0, 0, 1);
            }

            for (int i = 1; i < halfSize; i++)
            {
                var pos1 = freeSpot.Key + direction * i;
                var pos2 = freeSpot.Key - direction * i;

                if(!freeStateSpots.ContainsKey(pos1) || !freeStateSpots.ContainsKey(pos2))
                {
                    return false;
                }
                tempPositionsBlocked.Add(pos1);
                tempPositionsBlocked.Add(pos2);
            }
            return true;
        }
        

        private GameObject SpawnPrefab(GameObject prefab, Vector3Int position, Quaternion rotation)
        {
            var newStructure = Instantiate(prefab, position, rotation, transform);
            newStructure.AddComponent<FallTween>();
            return newStructure;
        }
            


        private Dictionary<Vector3Int, Direction> FindFreeSpacesAroundRoad(List<Vector3Int> roadPositions)
        {
            Dictionary<Vector3Int, Direction> freeSpaces = new Dictionary<Vector3Int, Direction>();

            foreach(var position in roadPositions)
            {
                var neighbourDirections = PlacementHelper.FindNeighbour(position, roadPositions);
                
                foreach(Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    if(neighbourDirections.Contains(direction) == false)
                    { 
                        var newPosition = position + PlacementHelper.GetOffsetFromDirection(direction);
                        if(freeSpaces.ContainsKey(newPosition))
                        {
                            continue;
                        }
                        freeSpaces.Add(newPosition, PlacementHelper.GetReverseDirection(direction));
                    }
                }
            }
            return freeSpaces;
        }

        public void Reset()
        {
            foreach(var item in structuresDictionary.Values)
            {
                Destroy(item);
            }
            structuresDictionary.Clear();

            foreach(var item in natureDictionary.Values)
            {
                Destroy(item);
            }
            natureDictionary.Clear();

            foreach(var buildType in buildingTypes)
            {
                buildType.Reset();
            }
        }
    }
}