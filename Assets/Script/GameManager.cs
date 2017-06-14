using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	//GAME ELEMENTS
	public GameObject egg;
	public GameObject areaMarker;
	public GameObject square;
	public Transform squarePosMin;
	public Transform squarePosMax;
	public GameObject squareContainer;
	public GameObject starObj;
	public GameObject starObjNum;
	public GameObject stageContainer;

	//GAME VARIABLES
	private Vector3 saveEggPositionFinishCheck;
	private bool moveMarkerFlag=true;
	private bool finishGameFlag=false;
	private int MaxSquareNum=10;
	private int minSquareNum=2;
	private int squareNum;
	private int level=1;
	private int increseSquareMod=2;
	private int bestLevel=0;
	private string bestLevelKey="BestLevel";
	private int startCount=0;
	private string starCountKey="StarCount";
	private bool gameActiveFlag=false;

	//END FINISH GAME VARIABLES
	private float waitTimeNextLevel=1.5f;
	private float waitTimeCheckMovement=0.5f;

	//EGG SAVE
	private Vector3 saveEggPosition;
	private Quaternion saveEggRotation;

	//UI
	public Text levelTxt;
	public Text bestLevelTxt;
	public Text starCountTxt;

	//Manager
	public UIManager uiManager;

	// Use this for initialization
	void Start () {
		freezeEgg();
		checkLevelData();
		squareNum=minSquareNum;
		areaMarker.SetActive(false);
		starObj.SetActive(false);
		generateSquares();
		changeLevelTxt();
		saveEggPosition=egg.transform.position;
		saveEggRotation=egg.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if(gameActiveFlag){
			if(Input.GetMouseButtonDown(0)){
				areaMarker.SetActive(true);
			}

			if(moveMarkerFlag){
				areaMarker.transform.position=Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,1));
			}

			if(Input.GetMouseButtonUp(0) && areaMarker.active){
				moveMarkerFlag=false;
				releadeEgg();
				StartCoroutine(checkMiniGameFinish());
				gameActiveFlag=false;
			}
		}
			
	}

	private void freezeEgg(){
		egg.GetComponent<Rigidbody2D>().constraints=RigidbodyConstraints2D.FreezeAll;
	}

	private void releadeEgg(){
		egg.GetComponent<Rigidbody2D>().constraints=RigidbodyConstraints2D.None;
	}
	private IEnumerator checkMiniGameFinish(){
		while(!finishGameFlag){
			
			saveEggPositionFinishCheck=egg.transform.position;

			yield return new WaitForSeconds(waitTimeCheckMovement);
			if(Vector3.Distance(saveEggPositionFinishCheck,egg.transform.position)<0.01f){
				Debug.Log("Finish");
				finishGameFlag=true;
				checkScore();
			}
		}
	}

	private void generateSquares(){
		increaseSquares();
		for (int i = 0; i < squareNum; i++) {
			GameObject newSquare= Instantiate(square);
			newSquare.transform.parent=squareContainer.transform;
			newSquare.transform.position=new Vector3(squarePosMin.position.x+(squarePosMax.position.x-squarePosMin.position.x)*Random.Range(0f,1f),squarePosMin.position.y+(squarePosMax.position.y-squarePosMin.position.y)*Random.Range(0f,1f),1);
			newSquare.transform.localScale=new Vector3(Random.Range(1f,3f),Random.Range(0.5f,1f),1);
			newSquare.transform.eulerAngles=new Vector3(0f,0f,Random.Range(0f,360f));
		}
	}

	private void increaseSquares(){
		if(level%increseSquareMod==0 && minSquareNum<MaxSquareNum){
			squareNum++;
		}
	}

	private void checkScore(){
		Debug.Log("Distance"+Vector2.Distance(egg.transform.position,areaMarker.transform.position));
		if(areaMarker.GetComponent<TriggerEnter>().triggerInFlag){
			Debug.Log("YOU WIN");
			increaseLevel();
			addStarCount ();
			StartCoroutine(resetLevel());
		}else{
			Debug.Log("YOU LOOSE");
			level=1;
			squareNum=minSquareNum;
			StartCoroutine(resetLevel());
			uiManager.endGame();
		}
	}

	private void addStarCount (){
		float distance=Vector2.Distance(egg.transform.position,areaMarker.transform.position);
		int winStarNum=0;
		if (distance < 0.2f) {
			winStarNum = 3;
		}
		else
			if (distance < 0.5f) {
				winStarNum = 2;
			}
			else
				if (distance < 0.75f) {
					winStarNum += 1;
				}
		if(winStarNum!=0){
			startCount+=winStarNum;
			starObjNum.GetComponent<TextMesh>().text=""+winStarNum;
			starObj.SetActive(true);
			PlayerPrefs.SetInt(starCountKey,startCount);
		}
	}

	private void increaseLevel(){
		level++;
		if(level>bestLevel){
			bestLevel=level;
			PlayerPrefs.SetInt(bestLevelKey,bestLevel);
		}
	}

	private void changeLevelTxt(){
		levelTxt.text=""+level;
		bestLevelTxt.text="BEST "+bestLevel;
		starCountTxt.text=""+startCount;
	}

	private IEnumerator resetLevel(){
		yield return new WaitForSeconds(waitTimeNextLevel);
		changeLevelTxt();
		destroyAllParentObject(squareContainer.transform);
		generateSquares();
		rePosEgg();
		resetFlags();
		areaMarker.SetActive(false);
		areaMarker.GetComponent<TriggerEnter>().triggerInFlag=false;
		starObj.SetActive(false);
		gameActiveFlag=true;
	}

	private void resetFlags(){
		moveMarkerFlag=true;
		finishGameFlag=false;
	}

	private void rePosEgg(){
		freezeEgg();
		egg.transform.position=saveEggPosition;
		egg.transform.rotation=saveEggRotation;
	}

	private void destroyAllParentObject(Transform parent){
		foreach(Transform child in parent){
			Destroy(child.gameObject);
		}
	}


	private void checkLevelData(){
		if(PlayerPrefs.GetInt(bestLevelKey)!=null){
			bestLevel=PlayerPrefs.GetInt(bestLevelKey);
		}
		if(PlayerPrefs.GetInt(starCountKey)!=null){
			startCount=PlayerPrefs.GetInt(starCountKey);
		}
	}

	public void activateStageContainer(bool state){
		gameActiveFlag=false;
		stageContainer.SetActive(state);
		StartCoroutine(activateGame());

	}

	private IEnumerator activateGame(){
		yield return new WaitForSeconds(1);
		gameActiveFlag=true;

	}

}
