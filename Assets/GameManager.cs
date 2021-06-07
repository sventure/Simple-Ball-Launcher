using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public MyRigidBody player;
    public MyTransform playerMyTransform;

    public float ForceVal, tempF = 0f;
    public float angle, tempA = 0f;

    public Slider ForceSlider;
    public Text AngleText;
    public Text DistText;

    public V3 startPos;
    public V3 curPos;

    bool pressed = false;
    bool Forcepressed = false;
    bool Anglepressed = false;

    // Start is called before the first frame update
    void Start()
    {
        ForceSlider.maxValue = 125f;
        ForceSlider.minValue = 0f;

        AngleText.text = "Angle: 0";
        DistText.text = "Dist: 0";

        startPos = MyMath.toMyVector(playerMyTransform.Position);
    }

    // Update is called once per frame
    void Update()
    {
        ForceVal = Mathf.PingPong(Time.time * 93.75f, 125f);
        angle = Mathf.PingPong(Time.time * 33.75f, 45f);

        curPos = MyMath.toMyVector(playerMyTransform.Position);

        V3 Dist = curPos - startPos;

        float dist = Dist.Length();

        DistText.text = "Dist: " + (int)dist;

        if (Input.GetKeyDown(KeyCode.F))
        {
            tempF = ForceVal;
            Forcepressed = true;
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            tempA = angle * 0.01745f;
            Anglepressed = true;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //player.Force += new Vector3(Mathf.Cos(tempA) * tempF, Mathf.Sin(tempA) * tempF, 0f);
            player.Force +=  (MyMath.AngToDirRoll(tempA) * tempF).toUnityVector();
            pressed = true;
        }

        if(Forcepressed == true)
        {
            ForceSlider.value = tempF;
        }
        else if(Forcepressed == false)
        {
            ForceSlider.value = ForceVal;
        }

        if (Anglepressed == true)
        {
            AngleText.text = "Angle: " + (int)(tempA / 0.01745f);
        }
        else if (Anglepressed == false)
        {
            AngleText.text = "Angle: " + (int)angle;
        }

        if (pressed == true)
        {
            player.Force.x --;
            if(player.Force.x <= 0f)
            {
                player.Force.x = 0f;
            }

            player.Force.y --;
            if (player.Force.y <= -9.8f)
            {
                player.Force.y = -9.8f;
            }
        }
    }
}
