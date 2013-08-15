#pragma strict

function Start()
{

}

function Update()
{
	GameObject.Find("Point light").transform.position = new Vector3(camera.transform.position.x, 3f, camera.transform.position.z);
	
	if(Input.GetKey(KeyCode.W))	transform.Translate(new Vector3(0f,.25f,0f));
	if(Input.GetKey(KeyCode.A))	transform.Translate(new Vector3(-.25f,0f,0f));
	if(Input.GetKey(KeyCode.S))	transform.Translate(new Vector3(0f,-.25f,0f));
	if(Input.GetKey(KeyCode.D))	transform.Translate(new Vector3(.25f,0f,0f));
}