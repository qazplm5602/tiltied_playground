using UnityEngine;

public class BuildWallSkill : SkillBase
{
    [SerializeField] private SkillWallObj _wallPrefab;
    [SerializeField] private ParticleSystem _buildEffect;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _wallBuildDistance;
    
    public float wallBuildTime;
    public float wallDurationTime;
    public SoundSO buildSound;
    public SoundSO destroySound;

    private SkillWallObj _wall;
    private Ground _ground;
    
    public override bool SkillUseAbleCheck()
    {
        if (_wall != null)
            return false;
        
        return base.SkillUseAbleCheck();
    }

    private void OnDestroy()
    {
        if(_ground!=null)
            _ground.OnClearEvent -= DestroyWall;
    }

    public override void UseSkill()
    {
        Vector3 rayPos = transform.position;
        Vector3 buildForward = transform.forward * _wallBuildDistance;
        
        rayPos += buildForward;
        rayPos.y += 8;

        bool isHit = Physics.Raycast(rayPos, Vector3.down, out RaycastHit hit, 10, _groundMask);

        if (isHit)
        {
            if (_ground == null)
            {
                _ground = hit.collider.GetComponentInParent<Ground>();
                _ground.OnClearEvent += DestroyWall;
            }
            
            _wall = Instantiate(_wallPrefab, transform.position, Quaternion.LookRotation(transform.forward));
            _wall.transform.SetParent(_ground.transform);
            _wall.transform.localRotation = Quaternion.Euler(0, _wall.transform.eulerAngles.y, 0);

            ParticleSystem buildEffect = Instantiate(_buildEffect, _ground.transform);
            _wall.WallInit(this, hit.point, buildEffect);
        }
    }

    private void DestroyWall()
    {
        if (_wall != null)
            _wall.DestroySkill();
    }
}
