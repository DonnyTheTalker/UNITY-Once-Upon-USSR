using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{  
    [SerializeField] private GameManager _gameManager;

    private void Awake()
    {   
        if (GameManager.Instance == null)
            Instantiate(_gameManager);
    }

}
