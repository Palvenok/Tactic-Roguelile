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
        StartCoroutine(Process());
    }

    private IEnumerator Process()
    {
        yield return new WaitForSeconds(Random.Range(.5f, 1.5f));
        if (Random.Range(0, 10) < 2)
        {
            battleSystem.SkipStage();
            yield break;
        }

        battleSystem.Index = Random.Range(0, _groupCount);
        battleSystem.NextState(battleSystem.CurrentState);

        yield return new WaitForSeconds(Random.Range(.5f, 1.5f));

        battleSystem.Index = Random.Range(0, _plauerGroupCount);
        battleSystem.NextState(battleSystem.CurrentState);        
    }
}
