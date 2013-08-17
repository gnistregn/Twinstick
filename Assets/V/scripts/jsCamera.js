#pragma strict

var t : Transform;

function Start()
{
	t = GameObject.Find("Player").transform;
}

function Update()
{
	GameObject.Find("Main Camera").transform.position = new Vector3(t.position.x, 15f, t.position.z);
	
	// if(Input.GetKey(KeyCode.W))	transform.Translate(new Vector3(0f,.25f,0f));
	// if(Input.GetKey(KeyCode.A))	transform.Translate(new Vector3(-.25f,0f,0f));
	// if(Input.GetKey(KeyCode.S))	transform.Translate(new Vector3(0f,-.25f,0f));
	// if(Input.GetKey(KeyCode.D))	transform.Translate(new Vector3(.25f,0f,0f));
}