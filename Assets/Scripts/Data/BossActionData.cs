using BossInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "BossAction Data", menuName = "CastleAdventure/BossActionData")]
public class BossActionData : ScriptableObject
{
    public EBossActionTask EActionTask => eActionTask;
    public float ActionCooldown => actionCooldown;
    public AnimationData ActionAnimData => animationData;
    public SoundData ActionSoundData => soundData;
    public VFXData ActionVFXData => vfxData;


    [SerializeField] private EBossActionTask eActionTask;
    [SerializeField] private float actionCooldown;
    [SerializeField] private AnimationData animationData;
    [SerializeField] private SoundData soundData;
    [SerializeField] private VFXData vfxData;
}
