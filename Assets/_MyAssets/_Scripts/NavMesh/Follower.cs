using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
    NavMeshAgent _agent;
    [SerializeField] Transform _destiny;
    [SerializeField] Transform _puntero;
    float _initDistancia;
    Material _punteroMaterial;
    Animator _animator;

    private void Awake()
    {
       _agent = GetComponent<NavMeshAgent>();
       _punteroMaterial = _puntero.GetComponentInChildren<MeshRenderer>().material;
       _animator = GetComponentInChildren<Animator>();
    }
       
    void Update()
    {
        _animator.SetFloat("velocity", _agent.velocity.magnitude);
        //_agent.SetDestination(_destiny.position);
        //print(_agent.remainingDistance); //Debug.log

        //if(_agent.remainingDistance < 0.1f ) 
        //{
        //    _puntero.gameObject.SetActive(false);
        //}
        //else
        //{
        //    _puntero.gameObject.SetActive(true);
        //}

        //Esta linea es igual a las condiciones anteriores
        _puntero.gameObject.SetActive(_agent.remainingDistance >= 0.1f);

        if(_agent.remainingDistance >= 0.1f)
        {
            float alpha = _agent.remainingDistance / _initDistancia;
            _punteroMaterial.SetColor("_Color", new Color(1,1,1,alpha));
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);

            RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, 1000);

            //for(int i = 0; i < hits.Length; i++)
            //{
            //    //hits[i];
            //}


            float altura = -1000.0f;
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Suelo") && hit.point.y>altura)
                {
                    _initDistancia = Vector3.Distance(transform.position, hit.point);
                    _agent.SetDestination(hit.point);
                    _puntero.position = hit.point;
                    _puntero.rotation = Quaternion.LookRotation(hit.normal);
                    altura=hit.point.y;
                }

                ButtonBox buttonBox = hit.collider.GetComponent<ButtonBox>();

                if (buttonBox)
                {
                    //buttonBox.Use();
                }
            }

            //RaycastHit hit;
            //if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000))
            //{
            //    //_agent.SetDestination(hit.point);
            //    Debug.Log(hit.collider.gameObject.name);

            //    ButtonBox buttonBox = hit.collider.GetComponent<ButtonBox>();

            //    if (buttonBox)
            //    {
            //        //buttonBox.Use();
            //    }
            //    else
            //    {
            //        _agent.SetDestination(hit.point);
            //    }
            //}
        }
    }
}
