using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StartEndHelper : MonoBehaviour
{
    [SerializeField] GameObject _playerCharacter;
    [SerializeField] GameObject _endObject;
    [SerializeField] private NewFollowCamera _mainCamera;
    private GameObject _characterObject;
    private GameObject _endGameObject;

    private NewCaracterMove _character;

    private Vector3 _startPos;
    private Vector3 _endPos;
    public Vector3 StartPos => _startPos;
    private void Start()
    {
        if (_mainCamera == null)
        {
            _mainCamera = FindObjectOfType<NewFollowCamera>();
        }
    }
    public void PlacePositions(HashSet<Vector3Int> roadPositions)
    {
        GetStartEndPositions(roadPositions.ToList());
        InstantiateCharacter();
        InstantiateEndObject();
    }

    public void ResetCharacter()
    {
        if (_character)
        {
            _character.gameObject.SetActive(false);
        }

        if (_endGameObject) 
        {
            _endGameObject.SetActive(false);
        }
    }

    private void GetStartEndPositions(List<Vector3Int> roadPositions) 
    {
        float maxDistanceSqr = -1f;

        for (int i = 0; i < roadPositions.Count; i++)
        {
            for (int j = i + 1; j < roadPositions.Count; j++)
            {
                float distanceSqr = (roadPositions[i] - roadPositions[j]).sqrMagnitude;
                if (distanceSqr > maxDistanceSqr)
                {
                    maxDistanceSqr = distanceSqr;
                    _startPos = roadPositions[i];
                    _endPos = roadPositions[j];
                }
            }
        }
    }

    private void InstantiateCharacter() 
    {
        if (_characterObject == null)
        {
            _characterObject = Instantiate(_playerCharacter, _startPos, transform.rotation);
            _character = _characterObject.GetComponentInChildren<NewCaracterMove>();
        }
        else
        {
            _character.transform.position = _startPos;
            _character.transform.rotation = transform.rotation;
            _character.gameObject.SetActive(true);
        }
        NewCaracterMove moveScript = _characterObject.GetComponentInChildren<NewCaracterMove>();
        if (moveScript != null && _mainCamera != null)
        {
            moveScript.camFollow = _mainCamera;
        }
        if (_mainCamera != null)
        {
            _mainCamera.SetTarget(_character.transform);
        }
       
        if (_mainCamera != null)
        {
            _mainCamera.SetTarget(_character.transform);
        }
    }

    private void InstantiateEndObject()
    {
        if (_endGameObject == null)
        {
            _endGameObject = Instantiate(_endObject, _endPos, transform.rotation);
        }
        else
        {
            _endGameObject.transform.position = _endPos;
            _endGameObject.transform.rotation = transform.rotation;
            _endGameObject.gameObject.SetActive(true);
        }
    }
}
