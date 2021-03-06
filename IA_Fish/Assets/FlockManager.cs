using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    public Vector3 goalPos; //definição da direção de referência do cardume

    [Header("Configurações do Cardume")]
    [Range(0.0f, 5.0f)]
    public float minSpeed; //velocidade mínima
    [Range(0.0f, 5.0f)]
    public float maxSpeed;  //velocidade máximoa
    [Range(1.0f, 10.0f)] 
    public float neighbourDistance; //distância dos peixes vizinhos
    [Range(0.0f, 5.0f)] 
    public float rotationSpeed; //velocidade de rotação

    void Start()
    {
        allFish = new GameObject[numFish];
        for (int i = 0; i < numFish; i++)
        {
            //define a posição e rumo que cada peixe criado na matriz deve ter
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                Random.Range(-swinLimits.y, swinLimits.y),
                Random.Range(-swinLimits.z, swinLimits.z));

            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity); //define os parâmetros para cada peixe criado
            allFish[i].GetComponent<Flock>().myManager = this; //linka o scrip Flcok em cada peixe
        }
        goalPos = this.transform.position;
    }
    void Update()
    {
        goalPos = this.transform.position; //define a posição "final" do cardume 
        if (Random.Range(0, 100) < 10) 
            goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x), 
                Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z, swinLimits.z));
    }
}
