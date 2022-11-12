using System;
using System.Collections;
using System.Collections.Generic;
using Hexlibrium;
using HEXLIBRIUM;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using MouseButton = UnityEngine.UIElements.MouseButton;
using Random = UnityEngine.Random;

namespace HEXLIBRIUM
{
    public enum MathResults
    {
        Loss,
        Win,
        GotStunned,
        Stunned,
        Draw
    }
    public class MatchOracle: MonoBehaviour
    {
        private int playerChoice;
        private int npcChoice;
        private int npcChoiceButtons;
        private int TurnCounter;
        private int matchResultCounter;
        
        public int fiveElementsPlayerCounter =0;
        public int fiveElementsNPCCounter=0;

        public HoracleData horacleData = new HoracleData();

       public static MatchOracle instance;

        public void Awake()
        {
            instance = this;
        }

        public void Start()
        {
            
        }

        public void SeedButton(int buttonNumber)
        {
            Debug.Log(buttonNumber);
        }
        public void ElementsButton(int buttonNumber)
        {
            //Debug.Log("Button " + buttonNumber + " pressed");
            playerChoice = buttonNumber;
            
            Debug.Log(buttonNumber);

            //StateMachineManager.instance.the5buttons.gameObject.SetActive(false);
        
            //StateMachineManager.instance.GetRandomNumber(); 

            StartCoroutine(PlayMatch());

            //CallToAction();

        }

        public IEnumerator PlayMatch()
        {
            HoracleData.instance.stateRound = HoracleData.StateRound.stateElements;
            
            
            var whileVar = true;

            npcChoice = Random.Range(5, 10) + 1;
            npcChoiceButtons = npcChoice - 6;
            
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(GetMatchResult());

        }

        private IEnumerator GetMatchResult()
        {
            TurnCounter++;
            
            
            var was_Stunned = false;
            var stunned = false;
            
            yield return new WaitForSeconds(1.0f);

            switch (CompareValues())
            {
                case MathResults.Draw:
                    Debug.Log("Draw");
                    
                    matchResultCounter--;
                    //AppManager.instance.the5ElementsButton[playerChoice].transform.GetChild(0).gameObject.SetActive(true);
                    break;
                case MathResults.Win:
                    Debug.Log("Win");
                  
                    matchResultCounter--;
                    fiveElementsPlayerCounter++;
                    
                    //AppManager.instance.fiveElementsPlayerCounterStatus.text = (fiveElementsPlayerCounter).ToString();
                    
                    AppManager.instance.fiveElementsPlayerButton[playerChoice].transform.GetChild(0).gameObject.SetActive(true);
                    // AppManager.instance.fiveElementsNpcButton[npcChoiceButtons].transform.GetChild(0).gameObject.SetActive(false);
                    //
                    // AppManager.instance.fiveElementsPlayerButton[playerChoice].transform.GetChild(1).gameObject.SetActive(false);
                    AppManager.instance.fiveElementsNpcButton[npcChoiceButtons].transform.GetChild(1).gameObject.SetActive(true);
                    
                    AppManager.instance.fiveElementsNpcButton[playerChoice].interactable = false;
                    StartCoroutine(HoracleData.instance.RandomCube(6));
                    break;
                case MathResults.GotStunned:
                    Debug.Log("GotStunned");
                   
                    matchResultCounter--;
                    //AppManager.instance.fiveElementsPlayerButton[playerChoice].transform.GetChild(0).gameObject.SetActive(false);
                    AppManager.instance.fiveElementsNpcButton[npcChoiceButtons].transform.GetChild(0).gameObject.SetActive(true);
                    
                    AppManager.instance.fiveElementsPlayerButton[playerChoice].transform.GetChild(1).gameObject.SetActive(true);
                    //AppManager.instance.fiveElementsNpcButton[npcChoiceButtons].transform.GetChild(1).gameObject.SetActive(false);
                    
                    StartCoroutine(HoracleData.instance.RandomCube(7));
                    break;
                case MathResults.Stunned:
                    Debug.Log("Stunned");
                    
                    matchResultCounter--;
                    AppManager.instance.fiveElementsPlayerButton[playerChoice].transform.GetChild(0).gameObject.SetActive(true);
                    //AppManager.instance.fiveElementsNpcButton[npcChoiceButtons].transform.GetChild(0).gameObject.SetActive(false);
                    
                    //AppManager.instance.fiveElementsPlayerButton[playerChoice].transform.GetChild(1).gameObject.SetActive(false);
                    AppManager.instance.fiveElementsNpcButton[npcChoiceButtons].transform.GetChild(1).gameObject.SetActive(true);
                    
                    StartCoroutine(HoracleData.instance.RandomCube(8));
                    break;
                case MathResults.Loss:
                    Debug.Log("Loss");
                   
                    matchResultCounter--;
                    fiveElementsNPCCounter++; 
                    
                    //AppManager.instance.fiveElementsNpcCounterStatus.text = (fiveElementsNPCCounter).ToString();
                    
                    //AppManager.instance.fiveElementsPlayerButton[playerChoice].transform.GetChild(0).gameObject.SetActive(false);
                    AppManager.instance.fiveElementsNpcButton[playerChoice].transform.GetChild(0).gameObject.SetActive(true);
                    
                    AppManager.instance.fiveElementsPlayerButton[playerChoice].transform.GetChild(1).gameObject.SetActive(true);
                    //AppManager.instance.fiveElementsNpcButton[playerChoice].transform.GetChild(1).gameObject.SetActive(true);
                    
                    AppManager.instance.fiveElementsPlayerButton[playerChoice].interactable = false;
                    
                    StartCoroutine(HoracleData.instance.RandomCube(9));
                    break;

            }


        }
        
        
        private MathResults CompareValues()
    {
        switch (playerChoice, npcChoice)
        {
            case (1, 6):
                return MathResults.Draw;
                break;
            case (2, 7):
                return MathResults.Draw;
                break;
            case (3, 8):
                return MathResults.Draw;
                break;
            case (4, 9):
                return MathResults.Draw;
                break;
            case (5, 10):
                return MathResults.Draw;
                break;
            
            case (1, 7):
                return MathResults.Stunned;
                break;
            case (2, 8):
                return MathResults.Stunned;
                break;
            case (3, 9):
                return MathResults.Stunned;
                break;
            case (4, 10):
                return MathResults.Stunned;
                break;
            case (5, 6):
                return MathResults.Stunned;
                break;
            
            case (1, 8):
                return MathResults.Win;
                break;
            case (2, 9):
                return MathResults.Win;
                break;
            case (3, 10):
                return MathResults.Win;
                break;
            case (4, 6):
                return MathResults.Win;
                break;
            case (5, 7):
                return MathResults.Win;
                break;
            
            case (1, 9):
                return MathResults.Loss;
                break;
            case (2, 10):
                return MathResults.Loss;
                break;
            case (3, 6):
                return MathResults.Loss;
                break;
            case (4, 7):
                return MathResults.Loss;
                break;
            case (5, 8):
                return MathResults.Loss;
                break;
            
            case (1, 10):
                return MathResults.GotStunned;
                break;
            case (2, 6):
                return MathResults.GotStunned;
                break;
            case (3, 7):
                return MathResults.GotStunned;
                break;
            case (4, 8):
                return MathResults.GotStunned;
                break;
            case (5, 9):
                return MathResults.GotStunned;
                break;
            
            default:
                return MathResults.Draw;
                break;
        }
    }
    }
}