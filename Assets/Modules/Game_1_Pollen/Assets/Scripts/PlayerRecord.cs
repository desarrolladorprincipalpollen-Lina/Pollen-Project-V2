using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PollenModule
{

    public class PlayerRecord : MonoBehaviour
    {
        public string pickableName {get; private set;}
        public string pickableColor {get; private set;}
        public string containerName{get; private set;}
        public string containerColor{get; private set;}
        int level;
        public bool rightPickable{get; private set;}
        public bool rightContainer{get; private set;}

        public PlayerRecord(string pickName, string pickColor, string contName, string contColor, bool rightPick, bool rightCont)
        {
            pickableName = pickName;
            pickableColor = pickColor;
            containerName = contName;
            containerColor = contColor;
            rightPickable = rightPick;
            rightContainer = rightCont;

            LevelCreator levelInfo = FindObjectOfType<LevelCreator>();
            level = levelInfo.currentLevel;

            AppendToReport();

        }

        public void AppendToReport(){
            string[] toAppend = new string[7] { pickableName,pickableColor,containerName,containerColor,rightPickable.ToString(),rightContainer.ToString(),level.ToString() };
            CSVManager.AppendToReport(toAppend);
        }


    }
}
