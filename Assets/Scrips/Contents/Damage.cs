using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    [SerializeField]
    Text text;

    Color alpha;
    
    // Start is called before the first frame update
    void Start()
    {
        //text = GetComponent<Text>();
        alpha = text.color;

      //  Destroy(gameObject, 2.0f);
    }

    public void SetDamage(int damage, bool critical)
    {        
        text.text = damage.ToString();
        critical = true;
        if( critical )
        {
            text.fontSize = text.fontSize * 120 / 100;

            alpha = new Color(255 / 255f, 255 / 255f, 0 / 255f, 255 / 255f);

            text.color = alpha;
        }
    }

    // Update is called once per frame
    void Update()
    {
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * 1f); 
        text.color = alpha;
        
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + (10f * Time.deltaTime), this.transform.position.z);
    }
}
