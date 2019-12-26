using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{


    public KeyCode key;
    bool active = false;
    GameObject beatBox, gm;
    Material mat;
    Color old;

    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Start()
    {
        gm = GameObject.Find("GameManager");
        old = mat.color;

    }

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            StartCoroutine(KeyPressed());
        }

        if (Input.GetKeyDown(key) && active)
        {
            ExplodeBeatBox(beatBox);
            gm.GetComponent<GameManager>().AddStreak();
            
            SetScore(gm.GetComponent<GameManager>().ComputeScore(100));

        } else if(Input.GetKeyDown(key)&&!active){
            gm.GetComponent<GameManager>().ResetStreak();
            SetScore(-50);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        active = true;

        if (col.gameObject.tag == "beatBox")
        {
            beatBox = col.gameObject;

        }
    }


    private void OnTriggerExit(Collider other)
    {
        active = false;
        gm.GetComponent<GameManager>().ResetStreak();
    }


    IEnumerator KeyPressed()
    {
        //rend.material.shader = Shader.Find("ActionLine");
        //rend.material.SetColor("ActionLine", Color.green);
        mat.color = new Color(0, 0, 0);
        yield return new WaitForSeconds(0.1f);
        mat.color = old;
        //rend.material.SetColor("ActionLine", old);
    }


    void SetScore(int score)
    {
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + score);
    }

    void ExplodeBeatBox(GameObject beatBox)
    {
        if (beatBox != null)
        {
        //get current scale
        Vector3 scale = beatBox.transform.localScale;

        //get positions on master cube for child cube placement
        Vector3 pos = beatBox.GetComponent<Renderer>().bounds.center;
        Vector3 lowPoint = beatBox.GetComponent<Renderer>().bounds.min;
        Vector3 highPoint = beatBox.GetComponent<Renderer>().bounds.max;

        //find value to use during placement of new cubes
        float startPos = (highPoint.y - lowPoint.y);

        //randomize position of explosion force
        Vector3 explosionCenter = new Vector3(pos.x + (Random.Range(-2.0f, 2.0f)), lowPoint.y - (Random.Range(5.0f, 10.0f)), pos.z + (Random.Range(-2.0f, 2.0f)));

        //create vector array to hold new positions
        Vector3[] posArray = new Vector3[4];

        //locations for the new cubes
        posArray[0] = new Vector3(startPos, 1f, -startPos);
        posArray[1] = new Vector3(-startPos, 1f, startPos);
        posArray[2] = new Vector3(startPos, 1f, startPos);
        posArray[3] = new Vector3(-startPos, 1f, -startPos);

        //generate random color each time object is clicked
        Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);

        // create cube, scale to 1/4 size, and add rigid body
        for (int i = 0; i < 4; i++)
        {

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //cube.tag = "cube";

            //set new random color
            cube.GetComponent<Renderer>().material.color = newColor;

            //set transform info
            cube.transform.localScale = (scale * .5f);
            cube.transform.localPosition = (pos + posArray[i]);

            //create variables for rigid body dynamics
            float forceScale = (Random.Range(100.0f, 1000.0f));
            float forceRadius = (Random.Range(100.0f, 300.0f));

            //add rigid body
            cube.AddComponent<Rigidbody>();
            cube.GetComponent<Rigidbody>().AddExplosionForce(forceScale, explosionCenter, forceRadius);
            cube.GetComponent<Rigidbody>().SetDensity(cube.transform.localScale.x);

            //attach this script to new cubes
            //UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(cube, "Assets/MoveBeatBox.cs (105,13)", "ExplodingCubes");
            //cube.AddComponent<cube>();
        }

        //destroy original cube
        Destroy(beatBox.gameObject);
        }
    }
}
