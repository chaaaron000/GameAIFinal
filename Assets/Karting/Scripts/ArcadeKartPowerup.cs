using KartGame.KartSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ArcadeKartPowerup : MonoBehaviour {

    public ArcadeKart.StatPowerup boostStats = new ArcadeKart.StatPowerup
    {
        MaxTime = 5
    };

    public bool isCoolingDown { get; private set; }
    public float lastActivatedTimestamp { get; private set; }

    public float cooldown = 5f;
    public Text speedText;
    public bool disableGameObjectWhenActivated;
    public UnityEvent onPowerupActivated;
    public UnityEvent onPowerupFinishCooldown;

    private void Awake()
    {
        lastActivatedTimestamp = -9999f;
        speedText = GameObject.Find("SpeedText").GetComponent<Text>();
    }


    private void Update()
    {
        if (isCoolingDown) { 

            if (Time.time - lastActivatedTimestamp > cooldown) {
                //finished cooldown!
                isCoolingDown = false;
                speedText.text = "";
                onPowerupFinishCooldown.Invoke();
            }

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isCoolingDown) return;

        var rb = other.attachedRigidbody;
        if (rb) {

            var kart = rb.GetComponent<ArcadeKart>();

            if (kart)
            { 
                lastActivatedTimestamp = Time.time;
                kart.AddPowerup(this.boostStats);
                onPowerupActivated.Invoke();
                speedText.text = "Speed Up!!!";
                isCoolingDown = true;

                //if (disableGameObjectWhenActivated) this.gameObject.SetActive(false);
            }
        }
    }

    public void SetIsCollingDownFalse()
    {
        isCoolingDown = false;
    }

}
