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
        if (!b.Contains(transform.position)) 
        { turning = true; } //retorna ao cardume se passa do limite definido
        else
            turning = false;
        if (turning)
        {
            Vector3 direction = myManager.transform.position - transform.position;
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
                    nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                    if (nDistance <= myManager.neighbourDistance)
                    {
                        vcentre += go.transform.position;
                        groupSize++;

                        if (nDistance < 1.0f)
                        { vavoid = vavoid + (this.transform.position - go.transform.position); }

                        Flock anotherFlock = go.GetComponent<Flock>();
                        gSpeed = gSpeed + anotherFlock.speed;
                    }
                }
            }
            if (groupSize > 0)
            {
                //o eixo central muda de acordo com a rotação do cardume
                vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position); 
                speed = gSpeed / groupSize;
                Vector3 direction = (vcentre + vavoid) - transform.position;
                if (direction != Vector3.zero) //rotação natural
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                        Quaternion.LookRotation(direction),
                        myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}

    
