using System.Collections;
using UnityEngine;

public class AiController : MonoBehaviour
{
    [SerializeField] private BattleSystem battleSystem;

    private int _groupCount;
    private int _plauerGroupCount;

    public void Initialize(int plauerGroupCount, int aiGroupCount)
    {
        _plauerGroupCount = plauerGroupCount;
        _groupCount = aiGroupCount;

        Debug.Log(_plauerGroupCount + " / " + _groupCount);
        StartCoroutine(Process());
    }

    private IEnumerator Process()
    {
        var rand = Random.Range(0, 100);
        yield return new WaitForSeconds(Random.Range(.5f, 1.5f));
        if (rand < 2)
        {
            battleSystem.SkipStage();
            yield break;
        }

        rand = Random.Range(0, _groupCount);
        battleSystem.Index = rand;

        yield return new WaitForSeconds(Random.Range(.5f, 1.5f));
        battleSystem.NextState(battleSystem.CurrentState, rand);

        yield return new WaitForSeconds(Random.Range(.5f, 1.5f));
        rand = Random.Range(0, _plauerGroupCount);
        battleSystem.Index = rand;

        yield return new WaitForSeconds(Random.Range(.5f, 1.5f));
        battleSystem.NextState(battleSystem.CurrentState, rand);        
    }
}
