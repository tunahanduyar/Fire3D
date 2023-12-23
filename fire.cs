using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class fire : MonoBehaviour
{


    AudioSource mermi_dosyasi;
    public AudioClip sarjorsesi;
    public AudioClip mermisesi;


    public ParticleSystem mermi_anim;
    public GameObject mermi;
    Animator anim;


    Coroutine mermi_fonksiyonu;


    Transform kamera;

    bool ates_etme_durumu = false;

    int kalan_sarjor = 1;
    int sarjordeki_mermi = 1000000;

    TextMeshProUGUI kursun_txt;

    List<GameObject> kursunlar;

    void Start()
    {

        mermi_dosyasi = gameObject.GetComponent<AudioSource>();

        anim = GetComponent<Animator>();
        kamera = Camera.main.transform;
        kursun_txt = GameObject.Find("kursun_txt").GetComponent<TextMeshProUGUI>();

        kursunlar = new List<GameObject>();

        mermi_anim.Stop();


        kursun_txt.text = sarjordeki_mermi + "/" + kalan_sarjor;

        for (int i = 0; i < 20; i++)
        {
            GameObject y_mermi = Instantiate(mermi);
            kursunlar.Add(y_mermi);
            y_mermi.SetActive(false);

        }



    }


    public void ates_et()
    {
        if (sarjordeki_mermi > 0)
        {

            ates_etme_durumu = !ates_etme_durumu;

            if (ates_etme_durumu == true)
            {

                mermi_fonksiyonu = StartCoroutine(kursun_gonder());
                mermi_anim.Play();
                anim.SetBool("ates", true);
                if (!mermi_dosyasi.isPlaying)
                {
                    mermi_dosyasi.PlayOneShot(mermisesi);
                }

            

            }

            else

            {

                StopCoroutine(mermi_fonksiyonu);
                mermi_anim.Stop();
                anim.SetBool("ates", false);

            }


        }

    }

    public void degis()
    {
        if (kalan_sarjor > 0)
        {
            anim.SetTrigger("sarjor");
            mermi_dosyasi.PlayOneShot(sarjorsesi);
            kalan_sarjor--;
            sarjordeki_mermi = 1000000;
            kursun_txt.text = sarjordeki_mermi + "/" + kalan_sarjor;
        }
    }

   

    IEnumerator kursun_gonder()
    {

        if (sarjordeki_mermi > 0)
        {
            for (int i = 0; i < kursunlar.Count; i++)
            {
                if (kursunlar[i].activeSelf == false)
                {
                    kursunlar[i].SetActive(true);
                    kursunlar[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                    kursunlar[i].transform.position = kamera.position;
                    kursunlar[i].GetComponent<Rigidbody>().AddForce(kamera.transform.forward * 1f, ForceMode.Impulse);


                    StartCoroutine(kursunu_yoket(kursunlar[i]));

                    sarjordeki_mermi--;
                    kursun_txt.text = sarjordeki_mermi + "/" + kalan_sarjor;

                    break;

                }
                else
                {
                    GameObject y_mermi = Instantiate(mermi, kamera.position, Quaternion.identity);
                    kursunlar.Add(y_mermi);

                    y_mermi.GetComponent<Rigidbody>().AddForce(kamera.transform.forward * 1.0f, ForceMode.Impulse);

                    StartCoroutine(kursunu_yoket(y_mermi));

                    sarjordeki_mermi--;
                    kursun_txt.text = sarjordeki_mermi + "/" + kalan_sarjor;
                    anim.SetBool("ates", false);
                    mermi_anim.Stop();
                    break;

                }

            }

            yield return new WaitForSeconds(4.0f);
            yield return kursun_gonder();


        }
        else
        {
            ates_etme_durumu = false;
            StopCoroutine(mermi_fonksiyonu);
            anim.SetBool("ates", false);
            mermi_anim.Stop();
        }

    }

    IEnumerator kursunu_yoket(GameObject kursun)
    {

        yield return new WaitForSeconds(1.0f);

        if (kursun.activeSelf == true)
        {
            kursun.SetActive(false);

        }


    }

}
