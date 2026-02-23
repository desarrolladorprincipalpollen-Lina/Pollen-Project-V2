using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PollenModule
{

    public class ContainerHandler : MonoBehaviour
    {
        [SerializeField] public string containerName;
        [SerializeField] public string containerColor;
        [SerializeField] public string containerMaterial;
        [SerializeField] float correctTimeOut = 2f;
        [SerializeField] float wrongTimeOut = 4f;
        GameObject correctSFX, wrongSFX;
        private TextMeshPro tmpOnScreen;
        private InstructionBuilder instructions;
        private Scoreboard scoreboard;
        private SceneTimer timer;
        bool enabledTriggerCheck = true;

        List<PlayerRecord> whatPlayerSolved = new List<PlayerRecord>();

        private void OnTriggerEnter(Collider other) {  
            if(other.CompareTag("Pickable") && enabledTriggerCheck){
                if (!timer.TimeIsRunning)
                {
                    timer.StartTimer();
                }
                PickableHandler pickableInfo = checkForPickableCollisioner(other);
                CheckRigthInstructionFollowed(pickableInfo);
                //tmpOnScreen.text = pickableInfo.pickableName +" " + pickableInfo.pickableColor + " Fue puesto dentro de " +containerName + " " + containerColor;
                //Debug.Log("Triggered with pickable");
            }
        }
        private void ShowAndPlayFX(bool rightAnswer){
            if (rightAnswer)
            {
                correctSFX.GetComponent<InteractableSFX>().Play();
            } else
            {
                wrongSFX.GetComponent<InteractableSFX>().Play();
            }
        }

        void CheckRigthInstructionFollowed(PickableHandler pickable){
            int pickableID = pickable.GetInstanceID();
            int containerID = GetInstanceID();

            //Debug.Log($"pickable ID: {pickableID} container ID: {containerID}");
            bool rightAnswer = false;
            if (pickableID == instructions.PickableID)
            {
                if (containerID == instructions.ContainerID)
                {
                    scoreboard.IncreaseRigthScore();
                    instructions.DisplayTextOnScreen("Eso es correcto!");
                    rightAnswer = true;
                    ShowAndPlayFX(rightAnswer);
                    FillPlayerRecord(pickable.pickableName, pickable.pickableColor, true, true);

                } else
                {
                    scoreboard.IncreaseWrongScore();
                    ShowAndPlayFX(rightAnswer);
                    instructions.ShowWrongInstructionFollowed(true, false);
                    FillPlayerRecord(pickable.pickableName, pickable.pickableColor, true, false);
                }
            } else
            {
                scoreboard.IncreaseWrongScore();
                if (containerID == instructions.ContainerID)
                {
                    ShowAndPlayFX(rightAnswer);
                    instructions.ShowWrongInstructionFollowed(false, true);
                    FillPlayerRecord(pickable.pickableName, pickable.pickableColor, false, true);
                } else
                { 
                    ShowAndPlayFX(rightAnswer);
                    instructions.ShowWrongInstructionFollowed(false, false);
                    FillPlayerRecord(pickable.pickableName, pickable.pickableColor, false, false);
                }
            }
            if (rightAnswer)
            {
                StartCoroutine(NextInstruction(correctTimeOut));
            } else
            {
                StartCoroutine(NextInstruction(wrongTimeOut));
            }

        }
        IEnumerator NextInstruction(float waitForSeconds){
            enabledTriggerCheck = false;
            timer.PauseTimer();
            yield return new WaitForSeconds(waitForSeconds);
            instructions.WriteInstruction();
            timer.StartTimer();
            enabledTriggerCheck = true;
        }
        void FillPlayerRecord(string pickableName, string pickableColor, bool rightPickable, bool rightContainer){
            PlayerRecord record = new PlayerRecord(pickableName,pickableColor,containerName,containerColor, rightPickable, rightContainer);
            whatPlayerSolved.Add(record);
            //TODO If needed, the final screen can show the player the mistakes from this list
        }
        private PickableHandler checkForPickableCollisioner(Collider other){
            PickableHandler pickable = null;
            if(other.gameObject.TryGetComponent<PickableHandler>(out PickableHandler pickableInfo)){
                pickableInfo.ReLocate();
                pickable = pickableInfo;
                //Debug.Log("Non parent pickable");
            } else {
                other.gameObject.GetComponentInParent<PickableHandler>().ReLocate();
                pickable = other.gameObject.GetComponentInParent<PickableHandler>();
                //Debug.Log("Parent pickable");
            }
            return pickable;   
        }
        // Start is called before the first frame update
        void Start()
        {
            instructions = FindObjectOfType<InstructionBuilder>();
            scoreboard = FindObjectOfType<Scoreboard>();
            timer = FindObjectOfType<SceneTimer>();
            //LoadAudio();
            LoadParticles();
        }

        void LoadParticles(){
            correctSFX = Instantiate(Resources.Load<GameObject>("Particles/Correct"), transform.parent.position, transform.rotation, transform);
            wrongSFX = Instantiate(Resources.Load<GameObject>("Particles/Wrong"), transform.parent.position, transform.rotation, transform);
        } 

        // Update is called once per frame
        void Update()
        {

        }
    }
}
