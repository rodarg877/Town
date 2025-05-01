using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering;

namespace SVS
{
    public class RoadHelper : MonoBehaviour
    {
        public Action finishedCoroutine;
        [SerializeField] GameObject roadStrainght;
        [SerializeField] GameObject roadCorner;
        [SerializeField] GameObject road3Way;
        [SerializeField] GameObject road4Way;
        [SerializeField] GameObject roadEnd;

        Dictionary<Vector3Int, GameObject> roadDictionary = new Dictionary<Vector3Int, GameObject>();
        public float animationTime = 0.01f;

        public void CreateRoad(HashSet<Vector3Int> printRoadDictionary)
        {
            foreach(var position in printRoadDictionary)
            {
                List<Direction> neighbourDirections = PlacementHelper.FindNeighbour(position, printRoadDictionary);
                UnityEngine.Quaternion rotation = UnityEngine.Quaternion.identity;

                if (neighbourDirections.Count == 1)
                {
                    if (neighbourDirections.Contains(Direction.Down))
                    {
                        rotation = UnityEngine.Quaternion.Euler(0, 90, 0);
                    }
                    else if (neighbourDirections.Contains(Direction.Left))
                    {
                        rotation = UnityEngine.Quaternion.Euler(0, 180, 0);
                    }
                    else if (neighbourDirections.Contains(Direction.Up))
                    {
                        rotation = UnityEngine.Quaternion.Euler(0, -90, 0);
                    }

                    roadDictionary[position] = Instantiate(roadEnd, position, rotation, transform);

                }
                else if (neighbourDirections.Count == 2)
                {
                    if (isRoadStrainght(neighbourDirections))
                    {
                        rotation = neighbourDirections.Contains(Direction.Up) && neighbourDirections.Contains(Direction.Down) ? UnityEngine.Quaternion.Euler(0, 90, 0) : rotation;
                        roadDictionary[position] = Instantiate(roadStrainght, position, rotation, transform);
                        continue;
                    }

                    if (neighbourDirections.Contains(Direction.Up)
                        && neighbourDirections.Contains(Direction.Right))
                    {
                        rotation = UnityEngine.Quaternion.Euler(0, 90, 0);
                    }
                    else if (neighbourDirections.Contains(Direction.Right)
                        && neighbourDirections.Contains(Direction.Down))
                    {
                        rotation = UnityEngine.Quaternion.Euler(0, 180, 0);
                    }
                    else if (neighbourDirections.Contains(Direction.Left)
                        && neighbourDirections.Contains(Direction.Down))
                    {
                        rotation = UnityEngine.Quaternion.Euler(0, -90, 0);
                    }

                    roadDictionary[position] = Instantiate(roadCorner, position, rotation, transform);
                }
                else if (neighbourDirections.Count == 3)
                {
                    if (neighbourDirections.Contains(Direction.Right)
                        && neighbourDirections.Contains(Direction.Down)
                        && neighbourDirections.Contains(Direction.Left))
                    {
                        rotation = UnityEngine.Quaternion.Euler(0, 90, 0);
                    }
                    else if (neighbourDirections.Contains(Direction.Down)
                            && neighbourDirections.Contains(Direction.Left)
                            && neighbourDirections.Contains(Direction.Up))
                    {
                        rotation = UnityEngine.Quaternion.Euler(0, 180, 0);

                    }
                    else if (neighbourDirections.Contains(Direction.Left)
                    && neighbourDirections.Contains(Direction.Up)
                    && neighbourDirections.Contains(Direction.Right))
                    {
                        {
                            rotation = UnityEngine.Quaternion.Euler(0, -90, 0);
                        }
                    }

                    roadDictionary[position] = Instantiate(road3Way, position, rotation, transform);

                }
                else if (neighbourDirections.Count == 4)
                {
                    roadDictionary[position] = Instantiate(road4Way, position, rotation, transform);
                }                    
            }
        }

        private bool isRoadStrainght(List<Direction> neighbourDirections) 
        {
            return neighbourDirections.Contains(Direction.Up)
                        && neighbourDirections.Contains(Direction.Down)
                        || neighbourDirections.Contains(Direction.Right)
                        && neighbourDirections.Contains(Direction.Left);
        }

        public void Reset()
        {
            foreach(var item in roadDictionary.Values)
            {
                Destroy(item);
            }
            roadDictionary.Clear();
        }
    }
}