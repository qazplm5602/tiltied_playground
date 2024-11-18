using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class BallControlBundle : Bundle
{
   static private bool _isInited = false;

   static private Transform _mapTrm;
   static private SoccerBall _soccerBall;
   static private Rigidbody _ballRigid;
   static private Transform _ballVisual;

   static private Player _ballOwner = null;
   static private Tween _ballMoveTween = null;


   private enum BallControlType
   {
      Player,
      Event
   }

   [SerializeField] private BallControlType _ballControlType;
   

   private static void Init()
   {
      // _mapTrm = GameObject.Find("Map").transform;
      // _ballRigid = _mapTrm.Find("Ball").GetComponent<Rigidbody>();
      
      _soccerBall = GameObject.FindAnyObjectByType<SoccerBall>();
      // _ballRigid = _soccerBall.GetComponent<Rigidbody>();
      // _ballVisual = _ballRigid.transform.Find("Visual");
   }

   public bool BallIsFree() => _ballOwner == null;
   public void PushBall(Vector3 force)
   {
      //   DOVirtual.DelayedCall(Time.fixedDeltaTime * 2, () =>
      //   {
      //       _ballRigid.AddForce(force, ForceMode.Impulse);
      //   });
      _soccerBall.Kick(force);
   }

   public override bool Registe(object obj = null)
   {
      if (!base.Registe()) return false;

      if (!_isInited)
      {
         Init();
         _isInited = true;
      }

      switch (_ballControlType)
      {
         case BallControlType.Player:
            _ballOwner = obj as Player;
            // _ballVisual.SetParent(_ballOwner.transform);
            // _ballRigid.isKinematic = true;
            // _ballRigid.Sleep();
            Transform ballVisual = _soccerBall.TakePlayerBall(_ballOwner, this);
            _ballMoveTween = ballVisual.DOLocalMove(endValue: new (0, 0.5f, 1.5f), duration: 0.2f).SetRelative(false);

            break;

         case BallControlType.Event:
            // 공이 어떤 아이템이나 다른 요소에 의해 움직일 때
            break;
      }

      return true;
   }

   

   public override bool Update(object obj = null)
   {
      if (!base.Update()) return false;
      return true;
   }

   public override bool Release(object obj = null)
   {
      if (!base.Release()) return false;
      
      bool isOwner = _ballOwner == obj as Player;
      bool hasBall = obj is SoccerBall;
      
      if (isOwner || hasBall) {
         if (_ballMoveTween is not null && _ballMoveTween.active)
            _ballMoveTween.Kill();

         _ballOwner = null;
      }

      if (isOwner)
         _soccerBall.RemoveOwner(false);

      return true;
   }

}
