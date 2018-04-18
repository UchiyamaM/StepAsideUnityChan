using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour {

    //carPrefabを入れる
    public GameObject carPrefab;
    //coinPrefabを入れる
    public GameObject coinPrefab;
    //cornPrefabを入れる
    public GameObject conePrefab;
    //スタート地点
	private int startPos = -160;
    //ゴール地点
	private int goalPos = 120;
    //アイテムを出すx方向の範囲
    private float posRange = 3.4f;
	//Unityちゃんのオブジェクト
	private GameObject unitychan;
	//Unityちゃんの前方に生成するアイテムとの距離
	private int itemPos = 50;
	//アイテムを生成した場所を保存
	private int posBackUp = 0;

    // Use this for initialization
    void Start()
    {
		//Unityちゃんのオブジェクトを取得
		this.unitychan = GameObject.Find("unitychan");

		//一定の距離ごとにアイテムを生成
//		for (int i = startPos; i < goalPos; i += 15)
//        {
//			SubGenerator (i);
//        }
    }

    // Update is called once per frame
	void Update () {
		//Unityちゃんの前方（itemPosの距離）にアイテムを生成
		if ((startPos - itemPos) <= (int)this.unitychan.transform.position.z && (goalPos - itemPos) >= (int)this.unitychan.transform.position.z) {
			//Unityちゃんが1m進む前に、Updateが数回呼び出されてしまう為、一度生成した場所は再度生成しないようにする
			if ((int)this.unitychan.transform.position.z % 15 == 0 && (int)this.unitychan.transform.position.z != posBackUp) {
				SubGenerator ((int)this.unitychan.transform.position.z + itemPos);
				posBackUp = (int)this.unitychan.transform.position.z;
			}
		}

		//tagごとにオブジェクト削除
		SubDestroy ("CoinTag");
		SubDestroy ("CarTag");
		SubDestroy ("TrafficConeTag");
	}

	//アイテム生成
	private void SubGenerator (int i) {
		//どのアイテムを出すのかをランダムに設定
		int num = Random.Range(0, 10);
		if (num <= 1)
		{
			//コーンをx軸方向に一直線に生成
			for (float j = -1; j <= 1; j += 0.4f)
			{
				GameObject cone = Instantiate(conePrefab) as GameObject;
				cone.transform.position = new Vector3(4 * j, cone.transform.position.y, i);
			}
		}
		else
		{

			//レーンごとにアイテムを生成
			for (int j = -1; j < 2; j++)
			{
				//アイテムの種類を決める
				int item = Random.Range(1, 11);
				//アイテムを置くZ座標のオフセットをランダムに設定
				int offsetZ = Random.Range(-5, 6);
				//60%コイン配置:30%車配置:10%何もなし
				if (1 <= item && item <= 6)
				{
					//コインを生成
					GameObject coin = Instantiate(coinPrefab) as GameObject;
					coin.transform.position = new Vector3(posRange * j, coin.transform.position.y, i + offsetZ);
				}
				else if (7 <= item && item <= 9)
				{
					//車を生成
					GameObject car = Instantiate(carPrefab) as GameObject;
					car.transform.position = new Vector3(posRange * j, car.transform.position.y, i + offsetZ);
				}
			}
		}
	}

	//オブジェクトを探して、削除する
	private void SubDestroy (string tag) {
		GameObject[] coinObjects = GameObject.FindGameObjectsWithTag (tag);
		foreach (GameObject coinObject in coinObjects) {
			if (coinObject.transform.position.z < (this.unitychan.transform.position.z - 15)) {
				Destroy (coinObject);
			}
		}
	}

}
