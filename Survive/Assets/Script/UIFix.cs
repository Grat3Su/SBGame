using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFix : MonoBehaviour
{
	public GameObject floatUI;//움직이는건 이건데 왜 텐트가 움직이냐????

	private Transform target;
	public float height = 2.0f;

	void Start()
	{
		target = GetComponent<Transform>();
		
		gameObject.SetActive(false);

		//moveUI();
	}

	void Update()
	{

	}

	void moveUI()
	{
		Vector3 tpos = target.position;
		Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(tpos.x, tpos.y + height, tpos.z));//타겟의 포지션을 뷰포트 좌표로 변환

		floatUI.transform.position = screenPos;

	}
}
