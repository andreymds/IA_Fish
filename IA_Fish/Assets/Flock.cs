using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager myManager;
    public float speed;
    bool turning = false; //variável que define se está ou não no limite de nado do cardume
    void Start()
    {
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }

    void Update()
    {
        //limite máximo onde é possível nadar
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);

        RaycastHit hit = new RaycastHit(); //cria o Raycast para controlar a reflexão
        Vector3 direction = myManager.transform.position - transform.position;

        if (!b.Contains(transform.position)) //retorna ao cardume se passa do limite definido
        { 
            turning = true;
            direction = myManager.transform.position - transform.position;
        } 
        else if (Physics.Raycast(transform.position, this.transform.forward*50, out hit))
        {
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal); //determina onde precisa ser feita a reflexão
        }
        else  
            turning = false;

        if (turning)
        {
            //Vector3 rotation = myManager.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10) //define rotação no eixo semelhante do resto do cardume
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            if (Random.Range(0, 100) < 20)
                ApplyRules();
        }
        transform.Translate(0, 0, Time.deltaTime * speed);

        void ApplyRules() //controle das regras de comportamento dos grupos
        {
            GameObject[] gos;
            gos = myManager.allFish;

            Vector3 vcentre = Vector3.zero; //definição do centro de foco do cardume
            Vector3 vavoid = Vector3.zero; //evitar colisão entre os peixes
            float gSpeed = 0.01f;
            float nDistance;
            int groupSize = 0; //tamanho de cada grupo

            foreach (GameObject go in gos)
            {
                if (go != this.gameObject)
                {
                    nDistance = Vector3.Distance(go.transform.position, this.transform.position); //calculo da distância entre eles
                    if (nDistance <= myManager.neighbourDistance)
                    {
                        vcentre += go.transform.position;
                        groupSize++;

                        if (nDistance < 1.0f) //desvia se muito próximos
                        { vavoid = vavoid + (this.transform.position - go.transform.position); }

                        Flock anotherFlock = go.GetComponent<Flock>();
                        gSpeed = gSpeed + anotherFlock.speed; //retorna a velocidade quando retorna em um cardume
                    }
                }
            }
            if (groupSize > 0)
            {
                //o eixo central muda de acordo com a rotação do cardume
                vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position); 
                speed = gSpeed / groupSize;
                direction = (vcentre + vavoid) - transform.position;
                if (direction != Vector3.zero) //rotação natural
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                        Quaternion.LookRotation(direction),
                        myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}

    
