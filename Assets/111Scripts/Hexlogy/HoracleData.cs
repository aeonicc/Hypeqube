using UnityEngine;
using System;
using UnityEditor.Rendering;
// using Random = System.Random;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ExitGames.Client.Photon.StructWrapping;
using Hexlibrium;
using Nethereum.ABI.Decoders;
using Random = UnityEngine.Random;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Playables;

using TMPro;



namespace HEXLIBRIUM
{
    public class HoracleData : MonoBehaviour
    {
        public int randomButtonCounter;
        public int hexagramEnumLinesState;
        public HexagramData.HexagramEnumLines hexagramLineState;

        public GameObject Mythic;
        
        
        public static HoracleData instance;

        private void Awake()
        {
            instance = this;
        }

        public GameObject Mythic1
        {
            get => Mythic;
            set => Mythic = value;
        }

        public int buttonFive;

        public int buttonEight;

        public int[] five = new int[5]
        {
            1, 2, 3, 4, 5
        };

        public int[] eight = new int[8]
        {
            222, 223, 232, 233, 322, 323, 332, 333
        };

        public int fiveHandRight;
        public int fiveHandLeft;

        public enum StateRound
        {
            initialState,
            stateCoins,
            stateElements,
            printHexagrams,
            finalState
            
        }

        public StateRound stateRound = StateRound.initialState;
        public enum YinYang
        {
            YoungYang,
            OldYang,
            YoungYin,
            OldYin
        }

        public YinYang yinyang;
        public enum Elements
        {
            fire,
            earth,
            metal,
            water,
            wood
        }
        
    
        public Elements elements; //; = new Elements();

        public enum Trigrams
        {
            sky,
            wind,
            lake,
            fire,
            water,
            mountain,
            earth
        }

        public Trigrams trigrams;
        
        private Matrix4x4 _matrix;

        public IEnumerator RandomStar()
       {
           yield return new WaitForSeconds(1.0f);
       }
       
          public void RandomButton()
          {
              stateRound = StateRound.stateCoins;
              int five = Random.Range(5, 10);
           
                StartCoroutine(RandomCube(five));
          }

          public IEnumerator RandomCube(int randomNumber)
       {
            
            randomButtonCounter++;

            // if (randomButtonCounter > 5)
            // {
            //     randomButtonCounter = 0;
            // }
            
            //HexagramData.instance.hexagramEnumLines = HexagramData.HexagramEnumLines.L01;
            
            //var stageLines = HexagramData.instance.hexagramEnumLines;
      
            if(1 <= randomButtonCounter && randomButtonCounter <= 6)
            {
                // int _eight = Random.Range(1, 8);

                if (stateRound == StateRound.stateCoins)
                {
                    switch (randomNumber)
                    {
                        case 5:
                          Debug.Log("5");
                          randomButtonCounter--;
                            break;
                        case 6:
                            HexagramData.hexagramInputArray[0, randomButtonCounter - 1] = 0;
                            HexagramData.hexagramInputArray[1, randomButtonCounter - 1] = 6;
                            HexagramData.hexagramInputArray[2, randomButtonCounter - 1] = 1;
                            break;
                        case 7:
                            HexagramData.hexagramInputArray[0, randomButtonCounter - 1] = 1;
                            HexagramData.hexagramInputArray[1, randomButtonCounter - 1] = 7;
                            HexagramData.hexagramInputArray[2, randomButtonCounter - 1] = 1;
                            break;
                        case 8:
                            HexagramData.hexagramInputArray[0, randomButtonCounter - 1] = 0;
                            HexagramData.hexagramInputArray[1, randomButtonCounter - 1] = 8;
                            HexagramData.hexagramInputArray[2, randomButtonCounter - 1] = 0;
                            break;
                        case 9:
                            HexagramData.hexagramInputArray[0, randomButtonCounter - 1] = 1;
                            HexagramData.hexagramInputArray[1, randomButtonCounter - 1] = 9;
                            HexagramData.hexagramInputArray[2, randomButtonCounter - 1] = 0;
                            break;
                    }
                }
                
                else if (stateRound == StateRound.stateElements)
                {
                    switch (randomNumber)
                    {
                        case 5:
                            Debug.Log("5");
                            randomButtonCounter--;
                            break;
                        case 6:
                            HexagramData.hexagramInputArray[3, randomButtonCounter - 1] = 0;
                            HexagramData.hexagramInputArray[4, randomButtonCounter - 1] = 6;
                            HexagramData.hexagramInputArray[5, randomButtonCounter - 1] = 1;
                            break;
                        case 7:
                            HexagramData.hexagramInputArray[3, randomButtonCounter - 1] = 1;
                            HexagramData.hexagramInputArray[4, randomButtonCounter - 1] = 7;
                            HexagramData.hexagramInputArray[5, randomButtonCounter - 1] = 1;
                            break;
                        case 8:
                            HexagramData.hexagramInputArray[3, randomButtonCounter - 1] = 0;
                            HexagramData.hexagramInputArray[4, randomButtonCounter - 1] = 8;
                            HexagramData.hexagramInputArray[5, randomButtonCounter - 1] = 0;
                            break;
                        case 9:
                            HexagramData.hexagramInputArray[3, randomButtonCounter - 1] = 1;
                            HexagramData.hexagramInputArray[4, randomButtonCounter - 1] = 9;
                            HexagramData.hexagramInputArray[5, randomButtonCounter - 1] = 0;
                            break;
                    }
                }

                //Draw
                AppManager.instance.seedOfLifeCounter.text = randomButtonCounter.ToString();
                //Debug.Log(HexagramData.hexagramInputArray[1, randomButtonCounter- 1]);
               
            }

            else
            {
                  
                StartCoroutine(RandomMatch());
                randomButtonCounter = 0;  
            }
            
            // Debug.Log(randomButtonCounter);

            StateRoundFunction();
            PrintHexagrams();
            
            yield return new WaitForSeconds(1.0f);
        }

       public StateRound StateRoundFunction()
       {
          
                    for (int i = 0; i < 6; i++)
                    {
                        switch (HexagramData.hexagramInputArray[1, i])
                        {
                            case 6:
                                AppManager.instance.seedOfLifeButtonsImages[i].sprite = 
                                    AppManager.instance.seedOfLifeButtonsSprites[1];


                                break;
                            case 7:

                                AppManager.instance.seedOfLifeButtonsImages[i].sprite =
                                    AppManager.instance.seedOfLifeButtonsSprites[2];


                                break;
                            case 8:

                                AppManager.instance.seedOfLifeButtonsImages[i].sprite =
                                    AppManager.instance.seedOfLifeButtonsSprites[3];

                               
                                break;
                            case 9:

                                AppManager.instance.seedOfLifeButtonsImages[i].sprite =
                                    AppManager.instance.seedOfLifeButtonsSprites[4];


                                break;
                        }
                    }

                    for (int i = 0; i < 6; i++)
                    {
                        switch (HexagramData.hexagramInputArray[4, i])
                        {
                            case 6:

                                AppManager.instance.seedOfLifeButtons[i + 7].image.sprite =
                                    AppManager.instance.seedOfLifeButtonsSprites[1];


                                break;
                            case 7:

                                AppManager.instance.seedOfLifeButtons[i + 7].image.sprite =
                                    AppManager.instance.seedOfLifeButtonsSprites[2];


                                break;
                            case 8:

                                AppManager.instance.seedOfLifeButtons[i + 7].image.sprite =
                                    AppManager.instance.seedOfLifeButtonsSprites[3];


                                break;
                            case 9:

                                AppManager.instance.seedOfLifeButtons[i + 7].image.sprite =
                                    AppManager.instance.seedOfLifeButtonsSprites[4];

                                break;
                        }
                    }

           return StateRound.finalState;
       }

       public IEnumerator RandomMatch()
       {
           // Debug.Log("randomMatch");

           HexagramData.hexagramProcessArray[0] = (
               HexagramData.hexagramInputArray[0,0],
               HexagramData.hexagramInputArray[0,1],
               HexagramData.hexagramInputArray[0,2],
               HexagramData.hexagramInputArray[0,3],
               HexagramData.hexagramInputArray[0,4],
               HexagramData.hexagramInputArray[0,5]).ToString();
           
           HexagramData.hexagramProcessArray[1] = (
               HexagramData.hexagramInputArray[1,0],
               HexagramData.hexagramInputArray[1,1],
               HexagramData.hexagramInputArray[1,2],
               HexagramData.hexagramInputArray[1,3],
               HexagramData.hexagramInputArray[1,4],
               HexagramData.hexagramInputArray[1,5]).ToString();
           
           HexagramData.hexagramProcessArray[2] = (
               HexagramData.hexagramInputArray[2,0],
               HexagramData.hexagramInputArray[2,1],
               HexagramData.hexagramInputArray[2,2],
               HexagramData.hexagramInputArray[2,3],
               HexagramData.hexagramInputArray[2,4],
               HexagramData.hexagramInputArray[2,5]).ToString();

           HexagramData.hexagramProcessArray[0] = ((HexagramData.hexagramProcessArray[0].Replace(",", "")).Replace("(", "")).Replace(")","");
           HexagramData.hexagramProcessArray[1] = ((HexagramData.hexagramProcessArray[1].Replace(",", "")).Replace("(", "")).Replace(")","");
           HexagramData.hexagramProcessArray[2] = ((HexagramData.hexagramProcessArray[2].Replace(",", "")).Replace("(", "")).Replace(")","");
        
           for (int i = 0; i < 64; i++)
           {
               if ((HexagramData.hexagramStringData[i, 0, 0]) == HexagramData.hexagramProcessArray[0]) //
               {
                   HexagramData.hexagramOutputArray[0,0] = HexagramData.hexagramStringData[i, 0, 1];
                   
               } ;
           }
           
           //middle
           for (int j = 0; j < 64; j++)
           {
               for (int i = 0; i < 6; i++)
               {
                   if (HexagramData.hexagramInputArray[1, i] == 6 || HexagramData.hexagramInputArray[1, i] == 9) //
                   {

                       HexagramData.hexagramOutputArray[1, i] = HexagramData.hexagramStringData[j, 1, i];

                   }

               }
           }

           HexagramData.hexagramLines[0] = (HexagramData.hexagramOutputArray[1, 0],
               HexagramData.hexagramOutputArray[1, 1],
               HexagramData.hexagramOutputArray[1, 2],
               HexagramData.hexagramOutputArray[1, 3],
               HexagramData.hexagramOutputArray[1, 4],
               HexagramData.hexagramOutputArray[1, 5]).ToString();
           
           
           for (int i = 0; i < 64; i++)
           {
               if ((HexagramData.hexagramStringData[i, 0, 0]) == HexagramData.hexagramProcessArray[2]) //
               {
                   HexagramData.hexagramOutputArray[2,0] = HexagramData.hexagramStringData[i, 0, 1];
                   
               } ;
           }

         
           
           yield return new WaitForSeconds(1.0f);
           
           

       }

       public void PrintHexagrams()
       {
           
           AppManager.instance.hexagramLeftTitle.text = HexagramData.hexagramOutputArray[0, 0];
           AppManager.instance.hexagramLeftDescription.text = HexagramData.hexagramOutputArray[2, 0];
           
           AppManager.instance.hexagramMiddleTitle.text = HexagramData.hexagramLines[0];
           AppManager.instance.hexagramMiddleDescription.text = HexagramData.hexagramOutputArray[2, 0];
           
           AppManager.instance.hexagramRightTitle.text = HexagramData.hexagramOutputArray[2, 0];
           AppManager.instance.hexagramRightDescription.text = HexagramData.hexagramOutputArray[2, 0];
           //
           for(int i = 0; i <6; i++)
           {    switch (
                   HexagramData.hexagramInputArray[0,i])
               {
                   case 0:
                       AppManager.instance.hexagramLeftLines[i].sprite =  AppManager.instance.yinYangLinesSprite[0];
                       break;
                  
                   case 1:
                       AppManager.instance.hexagramLeftLines[i].sprite =  AppManager.instance.yinYangLinesSprite[1];
                       break;
                  
                 
               }
           }
           
           for(int i = 0; i <6; i++)
           {    switch (
                   HexagramData.hexagramInputArray[1,i])
               {
                  case 6:
                     AppManager.instance.hexagramMiddleLines[i].sprite =  AppManager.instance.yinYangLinesSprite[0];
                   
                       break;
                  
                  case 7:
                      AppManager.instance.hexagramMiddleLines[i].sprite =  AppManager.instance.yinYangLinesSprite[1];
                      
                      break;
                  
                  case 8:
                      AppManager.instance.hexagramMiddleLines[i].sprite =  AppManager.instance.yinYangLinesSprite[2];
                      
                      break;
                  
                  case 9:
                      AppManager.instance.hexagramMiddleLines[i].sprite =  AppManager.instance.yinYangLinesSprite[3];
                      
                      break;
               }
           }
           
           for(int i = 0; i <6; i++)
           {    switch (
                   HexagramData.hexagramInputArray[2,i])
               {
                   case 0:
                       AppManager.instance.hexagramRightLines[i].sprite =  AppManager.instance.yinYangLinesSprite[0];
                       break;
                  
                   case 1:
                       AppManager.instance.hexagramRightLines[i].sprite =  AppManager.instance.yinYangLinesSprite[1];
                       break;
                  
                
               }
           }
       

           
           stateRound = StateRound.printHexagrams;
           
           //yield return new WaitForSeconds(1.0f);
       }
       
       
    
        public void Start()
        {
            
            // _matrix.
        }

        public void Update()
        {
            var lineState = (HexagramData.HexagramEnumLines)randomButtonCounter;
            var tetraState = (YinYang)randomButtonCounter;
    
            Debug.Log(randomButtonCounter);
            
            if (randomButtonCounter >= 6)
            {
                StateMachineManager.instance.ChangeState("H14");
            }
            
            
           // Debug.Log(randomButtonCounter);
            
            //Debug.Log(HexagramData.hexagramStringData[0,0]);
            
            switch(yinyang)
            {
                case YinYang.YoungYang:
                    //HexagramData.instance.hexagramEnumLines = (HexagramData.HexagramEnumLines)randomButtonCounter;
                    break;
                case YinYang.OldYang:
                    break;
                case YinYang.YoungYin:
                    break;
                case YinYang.OldYin:
                    break;
               
            }
            
            switch (buttonFive)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
            
             

            switch (elements)
            {
                case Elements.fire:
                    break;
                case Elements.earth:
                    break;
                case Elements.metal:
                    break;
                case Elements.water:
                    break;
                case Elements.wood:
                    break;
            }

            switch (trigrams)
            {
            }
        }

  
    }
}