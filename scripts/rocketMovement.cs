using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class rocketMovement : MonoBehaviour
{
    [SerializeField] float rocketSpeed = 5f;
    [SerializeField] float rocketRotate = 5f;
    [SerializeField] AudioClip ThrustSound;
    [SerializeField] AudioClip DeathSound;
    [SerializeField] AudioClip SuccessSound;
    [SerializeField] float NextSceneLoadTime = 1.5f;
    [SerializeField] ParticleSystem ThrustVFX;
    [SerializeField] GameObject ExplosionVFX;
    

    Rigidbody rocketRG;
    AudioSource rocketSource;
    int currentIndex;
    int temp;
    int SuccessCnt;
    
    void Start()
    {
        ThrustVFX.enableEmission = false;
        rocketRG = GetComponent<Rigidbody>();
        rocketSource = GetComponent<AudioSource>();
        currentIndex = SceneManager.GetActiveScene().buildIndex;
        temp = 0;
        SuccessCnt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (temp == 0)
        {

            rocketRotation();

            if (Input.GetKey(KeyCode.Space))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rocketSource.PlayOneShot(ThrustSound);
                    ThrustVFX.enableEmission = true;
                    ThrustVFX.Play();
                }
                Debug.Log("space");
                rocketThrust();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                rocketSource.Stop();
                ThrustVFX.Stop();
            }

        }
        else
        {
            return;
        }
    }

    public void rocketRotation()
    {
        
        if(Input.GetKey(KeyCode.A))
        {
            var rotateAmount = rocketRotate;
            Vector3 rotateVector = transform.forward * rotateAmount;
            rocketRG.MoveRotation(Quaternion.Euler(rocketRG.rotation.eulerAngles + rotateVector * rocketRotate));
        }

        else if(Input.GetKey(KeyCode.D))
        {
            var rotateAmount = -rocketRotate;
            Vector3 rotateVector = transform.forward * rotateAmount;
            rocketRG.MoveRotation(Quaternion.Euler(rocketRG.rotation.eulerAngles + rotateVector * rocketRotate));
        }
        

    }

    public void rocketThrust()
    {
        rocketRG.AddRelativeForce(rocketSpeed * Time.fixedDeltaTime * Vector3.up* rocketSpeed);

    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if(temp != 0)
        {
            return;
        }
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("OKKK");
                break;
            case "obstacle":
                ProcessDeath();
                break;
            case "Finish":
                ProcessSuccess();
                break;
            default:
                Debug.Log("Untagged");
                break;
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(currentIndex + 1);
        
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene("Level 1");
    }

    private void ProcessDeath()
    {
        GameObject Death;
        rocketRG.freezeRotation = false;
        temp++;
        rocketSource.Stop();
        rocketSource.PlayOneShot(DeathSound);
        Invoke("LoadFirstScene", NextSceneLoadTime);
        Death = Instantiate(ExplosionVFX, transform.position, Quaternion.identity);
        
    }

    private void ProcessSuccess()
    {
        if(SuccessCnt == 0)
        {
            rocketSource.Stop();
            rocketSource.PlayOneShot(SuccessSound);
            Invoke("LoadNextScene", NextSceneLoadTime);
        }
        else
        {
            return;
        }
        SuccessCnt++;

    }
}
