using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using Hexlibrium;

namespace HEXLIBRIUM
{
    

    public class H12 : State
    {
        private void OnEnable()
        {
            StateMachineManager.instance.OnStateEnteredHandler(this.gameObject);
            
            StateMachineManager.instance.playerCamera.gameObject.SetActive(false);
            StateMachineManager.instance.boardCamera.gameObject.SetActive(false);
            StateMachineManager.instance.theFiveButtons.gameObject.SetActive(false);
            
            StateMachineManager.instance.buttonCoin.gameObject.SetActive(false);
            StateMachineManager.instance.buttonCrystal.gameObject.SetActive(false);
            StateMachineManager.instance.buttonHeath.gameObject.SetActive(false);
            StateMachineManager.instance.buttonSolidity.gameObject.SetActive(false);
            StateMachineManager.instance.buttonCronos.gameObject.SetActive(false);
            StateMachineManager.instance.buttonGold.gameObject.SetActive(false);
            
            StateMachineManager.instance.appCamera.gameObject.SetActive(true);
            
            StateMachineManager.instance.intro.gameObject.SetActive(false);
            
            StateMachineManager.instance.horacle.gameObject.SetActive(false);
            StateMachineManager.instance.heroJourney.gameObject.SetActive(true);
            StateMachineManager.instance.helements.gameObject.SetActive(false);
            
            StateMachineManager.instance.holonTime.gameObject.SetActive(false);
            StateMachineManager.instance.hologenetics.gameObject.SetActive(false);
        }
    }
}