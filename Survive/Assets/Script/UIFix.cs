using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFix : MonoBehaviour
{
	public GameObject floatUI;//�����̴°� �̰ǵ� �� ��Ʈ�� �����̳�????

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
		Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(tpos.x, tpos.y + height, tpos.z));//Ÿ���� �������� ����Ʈ ��ǥ�� ��ȯ

		floatUI.transform.position = screenPos;

	}
}
