using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CompletarJogo2 : MonoBehaviour
{
    public Animator porta;
    public Animator fade;
    public PlayableDirector playableDirector;
    public DialogoFinal dialogoFinal;

    public IEnumerator ComeçarCutscene()
    {
        porta.SetTrigger("abrir");
        yield return new WaitForSeconds(2f);
        fade.SetTrigger("fechar");
        yield return new WaitForSeconds(2f);
        playableDirector.Play();
        yield return new WaitForSeconds(2f);
        dialogoFinal.StartDialogue();
        yield return new WaitForSeconds((float) playableDirector.duration - 2);
        SceneManager.LoadScene("Creditos");

    }
}
