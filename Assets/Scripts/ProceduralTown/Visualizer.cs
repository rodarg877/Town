using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace SVS
{
    public class Visualizer : MonoBehaviour
    {
        public LSystemGenerator lSystem;
        List<Vector3> positions = new List<Vector3>();

        [SerializeField] RoadHelper roadHelper;
        [SerializeField] StructureHelper structureHelper;
        [SerializeField] StartEndHelper startEndHelper;
        [SerializeField] EnemyHelper enemySpawner;
        [SerializeField] NavMeshSurface navMeshSurface;

        private int roadLenght = 8;
        private int lenght = 8;

        [SerializeField][Range(0f,90f)]private float angle = 90f;

        private HashSet<Vector3Int> printDictionary = new HashSet<Vector3Int>();

        public int Lenght
        {
            get
            {
               return lenght > 0? lenght : 1;
            }
             set => lenght = value;
        }
        public enum EncodingLetters
        {
            unknown = '1',
            save = '[',
            load = ']',
            draw = 'F',
            turnRight = '+',
            turnLeft = '-'
        }
        
        private void Start() 
        {
            CreateTown();
        }

        private void OnEnable()
        {
            WinAreaBehaviour.OnPlayerWin += CreateTown; // Suscribirse al evento
        }

        private void OnDisable()
        {
            WinAreaBehaviour.OnPlayerWin -= CreateTown; // Desuscribirse del evento
        }

        public void CreateTown()
        {
            lenght = roadLenght;
            
            roadHelper.Reset();
            structureHelper.Reset();
            startEndHelper.ResetCharacter();

            var sequence = lSystem.GenerateSentence();
            Debug.Log(sequence);
            StartCoroutine(VisualizeSequence(sequence)); 
        }


        private IEnumerator VisualizeSequence(string sequence)
        {
            Stack<AgentParameters> savePoints = new Stack<AgentParameters>();
            Vector3 currentPosition = Vector3.zero;

            Vector3 direction = Vector3.forward;
            Vector3 tempPosition = Vector3.zero;

         
            foreach(var letter in sequence)
            {
                EncodingLetters encoding = (EncodingLetters)letter;

                switch(encoding)
                {
                    case EncodingLetters.unknown:
                        break;
                    case EncodingLetters.save:
                        savePoints.Push( new AgentParameters
                        {
                            position = currentPosition,
                            direction = direction,
                            lenght = Lenght
                        });
                        break;
                    case EncodingLetters.load:
                        if(savePoints.Count > 0)
                        {
                            var agentParameter = savePoints.Pop();
                            currentPosition = agentParameter.position;
                            direction = agentParameter.direction;
                            Lenght = agentParameter.lenght;
                        }
                        else
                        {
                            throw new System.Exception("Dont have saved point in Stack");
                        }
                        break;
                    case EncodingLetters.draw:
                        tempPosition = currentPosition;
                        currentPosition += direction * lenght;
                        AddPrintPositions(tempPosition, Vector3Int.RoundToInt(direction),lenght);
                        
                        yield return new WaitForEndOfFrame();

                        Lenght -= 2;
                        break;
                    case EncodingLetters.turnRight:
                        direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                        break;
                    case EncodingLetters.turnLeft:
                        direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                        break;
                    default:
                        break;

                }
           
            }

            yield return new WaitForSeconds(0.1f);
            startEndHelper.PlacePositions(printDictionary);
            roadHelper.CreateRoad(printDictionary);
           

            yield return new WaitForSeconds(0.8f);
         
            StartCoroutine(structureHelper.PlaceStructuresAroundRoad(printDictionary));
            yield return new WaitForSeconds(0.8f);

            navMeshSurface.BuildNavMesh();

            yield return new WaitForSeconds(0.8f);

            enemySpawner.InitializeSpawner(printDictionary, startEndHelper.StartPos);
        }
        
        public void AddPrintPositions(Vector3 startPosition, Vector3Int direction, int lenght)
        {
            var rotation = Quaternion.identity;
            if (direction.x == 0)
            {
                rotation = Quaternion.Euler(0, 90, 0);
            }

            for (int i = 0; i < lenght; i++)
            {
                var position = Vector3Int.RoundToInt(startPosition + direction * i);
                if (printDictionary.Contains(position))
                {
                    continue;
                }

                printDictionary.Add(position);
            }
        }
    }
}