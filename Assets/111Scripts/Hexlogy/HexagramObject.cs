using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


using TMPro;


namespace HEXLIBRIUM
{
    public class HexagramObject: MonoBehaviour
    {
        
        [SerializeField] private List<GameObject> vertex = new List<GameObject>();
        [SerializeField] private List<GameObject> edges = new List<GameObject>();
        [SerializeField] private List<GameObject> faces = new List<GameObject>();
        
        [SerializeField] private List<GameObject> lines = new List<GameObject>();
        
        private int ballsCount = 0;
        
    
        
    }
}