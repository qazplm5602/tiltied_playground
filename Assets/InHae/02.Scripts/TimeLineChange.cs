using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimeLineChange : MonoBehaviour
{
    [SerializeField] private List<TimelineAsset> _timelineList;

    private PlayableDirector _playableDirector;

    private int _timeLineIdx;

    private void Awake()
    {
        _playableDirector = GetComponent<PlayableDirector>();
        _playableDirector.playableAsset = _timelineList[_timeLineIdx];
        _playableDirector.Play();
    }

    public void ChangeTrack()
    {
        _timeLineIdx++;
        if (_timeLineIdx >= _timelineList.Count)
            _timeLineIdx = 0;
        
        _playableDirector.playableAsset = _timelineList[_timeLineIdx];
        _playableDirector.Play();
    }
}
