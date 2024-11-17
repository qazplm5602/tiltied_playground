using UnityEngine;

public interface ICutsceneCallback {
    public void CutsceneFinish();
}

public class SoccerCutscene : GameModeCompo
{
    private CutsceneEntity cutscene;

    public SoccerCutscene(GameMode mode, string directorName) : base(mode)
    {
        GameObject directorEntity = GameObject.Find(directorName);
        
        if (directorEntity == null) {
            Debug.LogError($"찾을 수 없다. {directorName}"); // 영어 아님.
            return;
        }

        cutscene = directorEntity.GetComponent<CutsceneEntity>();
        if (cutscene == null) {
            Debug.LogError($"{directorName}은 CutsceneEntity가 없습니다.");
        }
    }

    public void Run() {
        cutscene.Play(() => gm.GameStart());
    }
}
