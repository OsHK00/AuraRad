using ModCommon.Util;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using System.Collections;
using System.Collections.Generic;
using GlobalEnums;


internal class Abs : MonoBehaviour
{

    private const int HP_BONUS = 2000;


    private const int PHASE_SPIKE_WAVES = 4500;
    private const int PHASE_SWORD_RAIN  = 3600;
    private const int PHASE_PLATFORMS   = 3000;
    private const int PHASE_CLIMB       = 1900;
    private const int FINAL_PHASE_HEAL  = 100;


    private const float HALO_CLOCK_QUARTER_TIME = 1.0f;

    private const bool  HALO1_BEAT_ENABLED = true;   
    private const float HALO1_BEAT_MIN     = 1.00f;  
    private const float HALO1_BEAT_MAX     = 1.12f;  
    private const float HALO1_BEAT_FREQ    = 0.85f;  


    private const bool  HALO1_REV_ENABLED  = true;   
    private const float HALO1_REV_SIZE     = 0.90f;  
    private const float HALO1_REV_ALPHA    = 0.38f;  
    private const float HALO1_REV_SPIN     = 2.6f;   // grados/frame


    private const bool  HALO_EXTRA_B_ENABLED = true;
    private const float HALO_EXTRA_B_SIZE    = 1.45f;
    private const float HALO_EXTRA_B_ALPHA   = 0.42f;
    private const float HALO_EXTRA_B_SPIN    =  3.0f;  // horario

    private const bool  HALO_EXTRA_C_ENABLED = true;
    private const float HALO_EXTRA_C_SIZE    = 0.62f;
    private const float HALO_EXTRA_C_ALPHA   = 0.60f;
    private const float HALO_EXTRA_C_SPIN    = -7.5f;  // rápido antihorario

 
    private const float CLIMB_FINAL_TRANSITION_DUR = 2.2f;


    private const bool  HAZE_OSC_ENABLED     = true; 
    private static readonly Color HAZE_OSC_A = new Color(0.32f, 0.20f, 0.42f, 1f); // violeta grisáceo
    private static readonly Color HAZE_OSC_B = new Color(0.18f, 0.22f, 0.35f, 1f); // azul noche oscuro
    private const float HAZE_OSC_HALF_PERIOD = 3.5f; // segundos por mitad de ciclo


    private const int PALETTE_ACTIVE = 0;

    private static readonly Color[] P0_SKY    = {
        new Color(0.30f, 0.50f, 0.90f, 1f),
        new Color(1.00f, 0.50f, 0.00f, 1f),
        new Color(0.18f, 0.03f, 0.08f, 1f),
        new Color(0.10f, 0.06f, 0.20f, 1f)
    };
    private static readonly Color[] P0_PILLAR = {
        new Color(0.20f, 0.40f, 0.80f, 1f),
        new Color(0.80f, 0.40f, 0.00f, 1f),
        new Color(0.12f, 0.02f, 0.05f, 1f),
        new Color(0.07f, 0.04f, 0.14f, 1f)
    };
    private static readonly Color[] P0_HAZE   = {
        new Color(0.40f, 0.60f, 0.90f, 1f),
        new Color(1.00f, 0.60f, 0.20f, 1f),
        new Color(0.25f, 0.04f, 0.10f, 1f),
        new Color(0.45f, 0.20f, 0.70f, 1f)
    };
    private static readonly Color[] P0_CLOUD  = {
        new Color(0.40f, 0.55f, 0.80f, 1f),
        new Color(0.90f, 0.45f, 0.10f, 1f),
        new Color(0.22f, 0.04f, 0.09f, 1f),
        new Color(0.52f, 0.11f, 0.04f, 1f)
    };
    private static readonly Color[] P0_RAY    = {
        new Color(0.50f, 0.70f, 1.00f, 0.40f),
        new Color(1.00f, 0.70f, 0.30f, 0.40f),
        new Color(0.55f, 0.10f, 0.18f, 0.35f),
        new Color(0.80f, 0.50f, 0.10f, 0.45f)
    };

    private static readonly Color[] P1_SKY    = { new Color(0.25f, 0.15f, 0.55f, 1f), new Color(0.50f, 0.10f, 0.70f, 1f), new Color(0.10f, 0.02f, 0.25f, 1f), new Color(0.45f, 0.05f, 0.50f, 1f) };
    private static readonly Color[] P1_PILLAR = { new Color(0.18f, 0.10f, 0.45f, 1f), new Color(0.38f, 0.07f, 0.55f, 1f), new Color(0.07f, 0.01f, 0.18f, 1f), new Color(0.32f, 0.03f, 0.38f, 1f) };
    private static readonly Color[] P1_HAZE   = { new Color(0.35f, 0.20f, 0.65f, 1f), new Color(0.60f, 0.15f, 0.80f, 1f), new Color(0.15f, 0.04f, 0.32f, 1f), new Color(0.55f, 0.08f, 0.60f, 1f) };
    private static readonly Color[] P1_CLOUD  = { new Color(0.30f, 0.18f, 0.55f, 1f), new Color(0.55f, 0.12f, 0.70f, 1f), new Color(0.12f, 0.03f, 0.28f, 1f), new Color(0.50f, 0.06f, 0.55f, 1f) };
    private static readonly Color[] P1_RAY    = { new Color(0.60f, 0.40f, 1.00f, 0.40f), new Color(0.80f, 0.30f, 1.00f, 0.40f), new Color(0.40f, 0.10f, 0.70f, 0.35f), new Color(0.70f, 0.20f, 0.80f, 0.45f) };

    private static readonly Color[] P2_SKY    = { new Color(0.05f, 0.40f, 0.15f, 1f), new Color(0.20f, 0.60f, 0.05f, 1f), new Color(0.02f, 0.18f, 0.05f, 1f), new Color(0.10f, 0.50f, 0.08f, 1f) };
    private static readonly Color[] P2_PILLAR = { new Color(0.03f, 0.30f, 0.10f, 1f), new Color(0.15f, 0.48f, 0.03f, 1f), new Color(0.01f, 0.12f, 0.03f, 1f), new Color(0.07f, 0.38f, 0.05f, 1f) };
    private static readonly Color[] P2_HAZE   = { new Color(0.10f, 0.55f, 0.20f, 1f), new Color(0.28f, 0.75f, 0.08f, 1f), new Color(0.05f, 0.25f, 0.08f, 1f), new Color(0.15f, 0.65f, 0.12f, 1f) };
    private static readonly Color[] P2_CLOUD  = { new Color(0.08f, 0.48f, 0.18f, 1f), new Color(0.22f, 0.65f, 0.06f, 1f), new Color(0.04f, 0.20f, 0.06f, 1f), new Color(0.12f, 0.55f, 0.10f, 1f) };
    private static readonly Color[] P2_RAY    = { new Color(0.30f, 1.00f, 0.40f, 0.40f), new Color(0.50f, 1.00f, 0.20f, 0.40f), new Color(0.10f, 0.60f, 0.20f, 0.35f), new Color(0.25f, 0.90f, 0.30f, 0.45f) };

    private static readonly Color[] P3_SKY    = { new Color(0.60f, 0.80f, 0.95f, 1f), new Color(0.30f, 0.60f, 0.90f, 1f), new Color(0.05f, 0.12f, 0.35f, 1f), new Color(0.15f, 0.35f, 0.70f, 1f) };
    private static readonly Color[] P3_PILLAR = { new Color(0.50f, 0.70f, 0.88f, 1f), new Color(0.22f, 0.50f, 0.80f, 1f), new Color(0.03f, 0.08f, 0.25f, 1f), new Color(0.10f, 0.25f, 0.55f, 1f) };
    private static readonly Color[] P3_HAZE   = { new Color(0.70f, 0.88f, 1.00f, 1f), new Color(0.40f, 0.70f, 0.95f, 1f), new Color(0.08f, 0.18f, 0.45f, 1f), new Color(0.20f, 0.45f, 0.80f, 1f) };
    private static readonly Color[] P3_CLOUD  = { new Color(0.65f, 0.83f, 0.98f, 1f), new Color(0.35f, 0.65f, 0.92f, 1f), new Color(0.06f, 0.14f, 0.38f, 1f), new Color(0.18f, 0.40f, 0.72f, 1f) };
    private static readonly Color[] P3_RAY    = { new Color(0.70f, 0.90f, 1.00f, 0.50f), new Color(0.50f, 0.80f, 1.00f, 0.45f), new Color(0.20f, 0.40f, 0.80f, 0.35f), new Color(0.40f, 0.65f, 1.00f, 0.45f) };

    private static readonly Color[] P4_SKY    = { new Color(0.55f, 0.55f, 0.58f, 1f), new Color(0.35f, 0.35f, 0.38f, 1f), new Color(0.08f, 0.08f, 0.09f, 1f), new Color(0.22f, 0.20f, 0.20f, 1f) };
    private static readonly Color[] P4_PILLAR = { new Color(0.42f, 0.42f, 0.45f, 1f), new Color(0.25f, 0.25f, 0.28f, 1f), new Color(0.05f, 0.05f, 0.06f, 1f), new Color(0.15f, 0.14f, 0.14f, 1f) };
    private static readonly Color[] P4_HAZE   = { new Color(0.65f, 0.65f, 0.68f, 1f), new Color(0.45f, 0.45f, 0.48f, 1f), new Color(0.12f, 0.12f, 0.13f, 1f), new Color(0.30f, 0.28f, 0.28f, 1f) };
    private static readonly Color[] P4_CLOUD  = { new Color(0.58f, 0.58f, 0.60f, 1f), new Color(0.38f, 0.38f, 0.40f, 1f), new Color(0.09f, 0.09f, 0.10f, 1f), new Color(0.24f, 0.22f, 0.22f, 1f) };
    private static readonly Color[] P4_RAY    = { new Color(0.80f, 0.80f, 0.85f, 0.35f), new Color(0.60f, 0.60f, 0.65f, 0.35f), new Color(0.20f, 0.20f, 0.22f, 0.30f), new Color(0.40f, 0.38f, 0.38f, 0.40f) };

    private static readonly Color[] P5_SKY    = { new Color(0.90f, 0.70f, 0.10f, 1f), new Color(0.95f, 0.50f, 0.05f, 1f), new Color(0.35f, 0.15f, 0.01f, 1f), new Color(0.80f, 0.35f, 0.02f, 1f) };
    private static readonly Color[] P5_PILLAR = { new Color(0.75f, 0.58f, 0.08f, 1f), new Color(0.80f, 0.40f, 0.03f, 1f), new Color(0.25f, 0.10f, 0.00f, 1f), new Color(0.65f, 0.28f, 0.01f, 1f) };
    private static readonly Color[] P5_HAZE   = { new Color(1.00f, 0.80f, 0.20f, 1f), new Color(1.00f, 0.60f, 0.10f, 1f), new Color(0.45f, 0.20f, 0.02f, 1f), new Color(0.95f, 0.45f, 0.05f, 1f) };
    private static readonly Color[] P5_CLOUD  = { new Color(0.95f, 0.75f, 0.15f, 1f), new Color(0.98f, 0.55f, 0.07f, 1f), new Color(0.40f, 0.17f, 0.01f, 1f), new Color(0.88f, 0.40f, 0.03f, 1f) };
    private static readonly Color[] P5_RAY    = { new Color(1.00f, 0.90f, 0.40f, 0.45f), new Color(1.00f, 0.70f, 0.20f, 0.45f), new Color(0.70f, 0.30f, 0.05f, 0.40f), new Color(0.95f, 0.55f, 0.10f, 0.50f) };

    private Color BG_SKY_PHASE(int ph)    => GetPaletteArr(0)[ph];
    private Color BG_PILLAR_PHASE(int ph) => GetPaletteArr(1)[ph];
    private Color BG_HAZE_PHASE(int ph)   => GetPaletteArr(2)[ph];
    private Color BG_CLOUD_PHASE(int ph)  => GetPaletteArr(3)[ph];
    private Color BG_RAY_PHASE(int ph)    => GetPaletteArr(4)[ph];

    private Color[] GetPaletteArr(int type)
    {
        Color[][] palettes;
        switch (type)
        {
            case 0: palettes = new[] { P0_SKY,    P1_SKY,    P2_SKY,    P3_SKY,    P4_SKY,    P5_SKY    }; break;
            case 1: palettes = new[] { P0_PILLAR,  P1_PILLAR,  P2_PILLAR,  P3_PILLAR,  P4_PILLAR,  P5_PILLAR  }; break;
            case 2: palettes = new[] { P0_HAZE,   P1_HAZE,   P2_HAZE,   P3_HAZE,   P4_HAZE,   P5_HAZE   }; break;
            case 3: palettes = new[] { P0_CLOUD,  P1_CLOUD,  P2_CLOUD,  P3_CLOUD,  P4_CLOUD,  P5_CLOUD  }; break;
            default: palettes = new[] { P0_RAY,    P1_RAY,    P2_RAY,    P3_RAY,    P4_RAY,    P5_RAY    }; break;
        }
        int idx = Mathf.Clamp(PALETTE_ACTIVE, 0, palettes.Length - 1);
        return palettes[idx];
    }

    private const float BG_TRANSITION_DURATION = 4f;

    internal const bool ENABLE_CLOUD_COLOR  = true;
    internal const bool ENABLE_RAY_COLOR    = true;
    internal const bool ENABLE_PILLAR_COLOR = true;
    internal const bool ENABLE_HAZE_COLOR   = true;
    internal const bool ENABLE_SKY_COLOR    = true;


    private const string TEXT_SWORD_RAIN_END       = "YOUR GOD CANNOT HEAR YOUR PRAYERS HERE";
    private const string TEXT_CLIMB_START          = "RISE HIGH AND FALL";
    private const string TEXT_CLIMB_FINAL_MESSAGE  = "COME MEET YOUR END VESSEL";
    private const string TEXT_AFTER_FINAL_MESSAGE  = "WHY";
    private const string TEXT_DEAD                 = "UNTHINKABLE...";

    private const float CLIMB_START_TEXT_Y = 135f;

    private const float SWORDRAIN_BEAM_OFF_A  =  4f;
    private const float SWORDRAIN_BEAM_OFF_B  =  7f;
    private const float SWORDRAIN_BEAM_OFF_C  = 10f;
    private const float SWORDRAIN_BEAM_OFF_D  = 13f;
    private const float SWORDRAIN_BEAM_OFF_E  = 16f;
    private const float SWORDRAIN_BEAM_Y      = 20f;


    private const bool  ORB_CROSS_X_GROUND          = true;
    private const bool  ORB_CROSS_X_PLAT            = true;
    private const bool  ORB_CROSS_X_FINAL           = true;

    private const float ORB_CROSS_ANTIC_TIME_PLAT   = 0.35f;
    private const float ORB_CROSS_ANTIC_TIME_OTHER  = 0.30f;
    private const float ORB_CROSS_FIRE_TIME_PLAT    = 0.05f;
    private const float ORB_CROSS_FIRE_TIME_FINAL   = 0.05f;
    private const float ORB_CROSS_FIRE_TIME_OTHER   = 0.1f;

    private const float FATAL_SWEEP_FIRST_DELAY = 2f;
    private const float FATAL_SWEEP_INTERVAL    = 4f;
    private const float FATAL_SWEEP_STEP        = 3f;
    private const float FATAL_SWEEP_SAFE_ZONE   = 4f;
    private const float FATAL_SWEEP_ANTIC_TIME  = 0.4f;
    private const float FATAL_SWEEP_FIRE_TIME   = 0.25f;

    private const float CLIMB_DEACT_Y1         = 77f;
    private const float CLIMB_DEACT_Y3         = 110f;
    private const float CLIMB_FLANKS_FIRE_TIME = 0.5f;
    private const float CLIMB_SWEEP_ANGLE_START= 90f;


    private const int   PLAT_DOWN_SPIKES_HP = 2450;
    private const float PLAT_DOWN_Y_G0 = 37.5f;
    private const float PLAT_DOWN_Y_G1 = 40.4f;
    private const float PLAT_DOWN_Y_G2 = 34.5f;
    private const float PLAT_DOWN_Y_G3 = 46.5f;
    private const float PLAT_DOWN_Y_G5 = 41.9f;
    private const float PLAT_DOWN_Y_G6 = 47.9f;
    private const float PLAT_DOWN_Y_G7 = 48.7f;
    private const float ADJUST_Y = 6.5f;
    private const float PLAT_DOWN_Y_G2B = 34.3f;


    private GameObject    _spikeMaster;
    private GameObject    _spikeTemplate;
    private GameObject    _beamsweeper;
    private GameObject    _beamsweeper2;
    private GameObject    _knight;
    private HealthManager _hm;
    private PlayMakerFSM  _attackChoices;
    private PlayMakerFSM  _attackCommands;
    private PlayMakerFSM  _control;
    private PlayMakerFSM  _phaseControl;
    private PlayMakerFSM  _spikeMasterControl;
    private PlayMakerFSM  _beamsweepercontrol;
    private PlayMakerFSM  _beamsweeper2control;
    private PlayMakerFSM  _teleport;
    private GameObject    _eyeBeamTemplate = null;

    private GameObject    _halo;
    private GameObject    _halo1;              
    private GameObject    _halo2;             
    private GameObject    _halo1Rev;             
    private GameObject    _halo1ExtraB;          
    private GameObject    _halo1ExtraC;          

    private FsmEventTarget _combTopTarget;
    private int            _spiralOrbIndex = 0;

    // Spike groups Arena 2
    private List<GameObject> _spikeGroup0  = new List<GameObject>();
    private List<GameObject> _spikeGroup1  = new List<GameObject>();
    private List<GameObject> _spikeGroup2  = new List<GameObject>();
    private List<GameObject> _spikeGroup3  = new List<GameObject>();
    private List<GameObject> _spikeGroup5  = new List<GameObject>();
    private List<GameObject> _spikeGroup6  = new List<GameObject>();
    private List<GameObject> _spikeGroup7  = new List<GameObject>();

    // Spike groups Climb
    private List<GameObject> _spikeGroup8  = new List<GameObject>();
    private List<GameObject> _spikeGroup9  = new List<GameObject>();
    private List<GameObject> _spikeGroup10 = new List<GameObject>();
    private List<GameObject> _spikeGroup11 = new List<GameObject>();
    private List<GameObject> _spikeGroup12 = new List<GameObject>();
    private List<GameObject> _spikeGroup13 = new List<GameObject>();
    private List<GameObject> _spikeGroup14 = new List<GameObject>();
    private List<GameObject> _spikeGroup15 = new List<GameObject>();
    private List<GameObject> _spikeGroup16 = new List<GameObject>();
    private List<GameObject> _spikeGroup17 = new List<GameObject>();
    private List<GameObject> _spikeGroup18 = new List<GameObject>();
    private List<GameObject> _spikeGroup19 = new List<GameObject>();
    private List<GameObject> _spikeGroup20 = new List<GameObject>();
    private List<GameObject> _spikeGroup21 = new List<GameObject>();
    private List<GameObject> _spikeGroup22 = new List<GameObject>();
    private List<GameObject> _spikeGroup23 = new List<GameObject>();

    // Spike groups plataformas (rotados/laterales)
    private List<GameObject> _spikeGroup24 = new List<GameObject>();
    private List<GameObject> _spikeGroup25 = new List<GameObject>();
    private List<GameObject> _spikeGroup26 = new List<GameObject>();
    private List<GameObject> _spikeGroup27 = new List<GameObject>();
    private List<GameObject> _spikeGroup28 = new List<GameObject>();
    private List<GameObject> _spikeGroup29 = new List<GameObject>();

    // Spike groups HACIA ABAJO
    private List<GameObject> _spikeGroup30 = new List<GameObject>();
    private List<GameObject> _spikeGroup31 = new List<GameObject>();
    private List<GameObject> _spikeGroup32 = new List<GameObject>();
    private List<GameObject> _spikeGroup33 = new List<GameObject>();
    private List<GameObject> _spikeGroup35 = new List<GameObject>();
    private List<GameObject> _spikeGroup36 = new List<GameObject>();
    private List<GameObject> _spikeGroup37 = new List<GameObject>();

    // Ascend beams
    private GameObject               ascendBeam  = null;
    private readonly List<GameObject> _ascendBeams = new List<GameObject>();

    // Beams verticales de sword rain
    private GameObject _srLA, _srRA, _srLB, _srRB, _srLC, _srRC, _srLD, _srRD, _srLE, _srRE;

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    // ATAQUE DE ESPADAS — FASE FINAL
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    private enum FinalTeleSpot { None, Left, Center, Right }
    private FinalTeleSpot _lastTeleSpot = FinalTeleSpot.None;

    private const float FINAL_SWORD_SPOT_L        = 53.88f;
    private const float FINAL_SWORD_SPOT_C        = 62.94f;
    private const float FINAL_SWORD_SPOT_R        = 72.4f;
    private const float FINAL_SWORD_SPAWN_Y       = 148.0f;
    private const float FINAL_SWORD_SEPARATION    =  2.5f;
    private const float FINAL_SWORD_ANTIC_TIME    =  0.5f;
    private const float FINAL_SWORD_FIRE_TIME     =  0.35f;
    private const float FINAL_SWORD_DELAY         =  0.4f;
    private const bool  FINAL_SWORD_ENABLED       =  true;
    private const float FINAL_SWORD_CENTER_SIDE_OFFSET = 2f;
    private const bool  FINAL_STATIC_BEAM_ENABLED =  false;

    private const float FINAL_SWORD_LEFT_X  = FINAL_SWORD_SPOT_L;
    private const float FINAL_SWORD_RIGHT_X = FINAL_SWORD_SPOT_R;
    private const float FINAL_SWORD_SPREAD  = FINAL_SWORD_SEPARATION;

    private const int FINAL_ORB_ACCEL_HP_1 = 2600;
    private const int FINAL_ORB_ACCEL_HP_2 = 1900;
    private const int FINAL_ORB_ACCEL_HP_3 = 1150;

    // Flags de estado
    private int  CWRepeats                       = 0;
    private bool disableBeamSet                  = false;
    private bool swordRainSpikesSet              = false;
    private bool swordRainEndSet                 = false;
    private bool arena2Set                       = false;
    private bool _climbHPReached                 = false;
    private bool ascendPhaseSet                  = false;
    private bool onePlatSet                      = false;
    private bool finalDanceOrbsConfigured        = false;
    private bool _finalOrbsAccel1                = false;
    private bool _finalOrbsAccel2                = false;
    private bool _finalOrbsAccel3                = false;
    private bool finalDanceExtraAttackConfigured = false;
    private bool groundSpikesLocked              = false;
    private bool _deadConvoShown                 = false;
    private bool _platVerticalSpikesShown        = false;
    private bool _platDownSpikesShown            = false;
    private bool _platRotatedSpikesShown         = false;
    private bool _climbStartTextShown            = false;
    private int  _platHpAtEntry                  = 0;

    private bool _groundSpikeStage1 = false;
    private bool _groundSpikeStage2 = false;
    private bool _groundSpikeStage3 = false;
    private bool _groundSpikeStage4 = false;

    private Coroutine        _finalSwordCoroutine   = null;
    private List<GameObject> _activeFinalSwordBeams = new List<GameObject>();
    private bool             _finalSwordReady       = false;
    private const float      FINAL_SWORD_START_DELAY = 8f;

    private List<GameObject> _allGroundSpikes     = new List<GameObject>();
    private List<GameObject> _extremeGroundSpikes = new List<GameObject>();

    // Coroutine de oscilación haze
    private Coroutine _finalHazeOscillateCoroutine;

    private void Awake()
    {
        Log("Added AbsRad MonoBehaviour");
        _hm                  = base.gameObject.GetComponent<HealthManager>();
        _attackChoices       = base.gameObject.LocateMyFSM("Attack Choices");
        _attackCommands      = base.gameObject.LocateMyFSM("Attack Commands");
        _control             = base.gameObject.LocateMyFSM("Control");
        _phaseControl        = base.gameObject.LocateMyFSM("Phase Control");
        _spikeMaster         = GameObject.Find("Spike Control");
        _spikeMasterControl  = _spikeMaster.LocateMyFSM("Control");
        _spikeTemplate       = GameObject.Find("Radiant Spike");
        _beamsweeper         = GameObject.Find("Beam Sweeper");
        _beamsweeper2        = Object.Instantiate(_beamsweeper);
        _beamsweeper2.AddComponent<BeamSweeperClone>();
        _beamsweepercontrol  = _beamsweeper.LocateMyFSM("Control");
        _beamsweeper2control = _beamsweeper2.LocateMyFSM("Control");
        _knight              = GameObject.Find("Knight");
        _teleport            = base.gameObject.LocateMyFSM("Teleport");

        var staleInfiniteDash = _knight.GetComponent<AuraInfiniteDash>();
        if (staleInfiniteDash != null)
        {
            Object.Destroy(staleInfiniteDash);
            Log("Awake: AuraInfiniteDash sobrante destruido.");
        }

        var bsT = _beamsweeper.transform.Find("Eye Beam");
        if (bsT != null) _eyeBeamTemplate = bsT.gameObject;
    }


    private void Start()
    {
        GameObject arena_prefab = GameObject.Find("GG_Arena_Prefab");
        if (arena_prefab != null)
        {
            GameObject crowd = GameObject.Find("Crowd");
            if (crowd != null) crowd.SetActive(false);
            GameObject gs_crowd = GameObject.Find("Godseeker Crowd");
            if (gs_crowd != null) gs_crowd.SetActive(false);
        }

        try
        {
            Material[] materials = base.gameObject.GetComponent<tk2dSprite>().Collection.materials;
            int num = 0;
            foreach (Material material in materials)
            {
                if (num >= AuraRadiance.Sprites.Count) break;
                material.mainTexture = AuraRadiance.Sprites[num].texture;
                num++;
            }
            Log("Skin aplicada.");
        }
        catch (System.Exception e) { Log("Error aplicando skin: " + e.Message); }

        SetupHalos();

        Log("Iniciando Aura Radiance...");

        _control.GetAction<SendEventByName>("First Tele", 3).sendEvent = "HugeShake";
        _hm.hp += HP_BONUS;

        _phaseControl.FsmVariables.GetFsmInt("P2 Spike Waves").Value = PHASE_SPIKE_WAVES;
        _phaseControl.FsmVariables.GetFsmInt("P3 A1 Rage").Value     = PHASE_SWORD_RAIN;
        _phaseControl.FsmVariables.GetFsmInt("P4 Stun1").Value       = PHASE_PLATFORMS;
        _phaseControl.FsmVariables.GetFsmInt("P5 Acend").Value       = PHASE_CLIMB;
        _control.GetAction<SetHP>("Scream", 7).hp = 2000;

        _attackCommands.GetAction<Wait>("Orb Antic", 0).time            = 0.15f;
        _attackCommands.GetAction<SetIntValue>("Orb Antic", 1).intValue = 10;
        _attackCommands.GetAction<RandomInt>("Orb Antic", 2).min        = 8;
        _attackCommands.GetAction<RandomInt>("Orb Antic", 2).max        = 10;
        _attackCommands.GetAction<Wait>("Orb Summon", 2).time           = 0.09f;
        _attackCommands.GetAction<Wait>("Orb Pause", 0).time            = 0.35f;
        _attackChoices.GetAction<Wait>("Orb Recover", 0).time           = 1.1f;

        FsmutilExt.InsertAction(_attackCommands, "Spawn Fireball",
            new AuraAction(() =>
            {
                var orb = _attackCommands.FsmVariables.FindFsmGameObject("Projectile").Value;
                if (orb == null) return;
                var beamTpl = _attackCommands.FsmVariables.FindFsmGameObject("Ascend Beam").Value ?? ascendBeam;
                var orbFSM = orb.LocateMyFSM("Orb Control");
                if (orbFSM == null) return;
                FsmutilExt.InsertAction(orbFSM, "Impact",
                    new AuraAction(() => { if (beamTpl != null) StartCoroutine(OrbCrossBeam(orb.transform.position, beamTpl)); }), 0);
                if (!(swordRainSpikesSet && !swordRainEndSet))
                    StartCoroutine(ApplySpiralVelocity(orb));
            }), 2);

        _attackCommands.GetAction<SetIntValue>("Nail Fan", 4).intValue.Value    = 24;
        _attackCommands.GetAction<Wait>("Nail Fan", 2).time.Value               = 0.006f;
        _attackCommands.GetAction<SetIntValue>("CW Restart",  0).intValue.Value = 24;
        _attackCommands.GetAction<SetIntValue>("CCW Restart", 0).intValue.Value = 5;
        _attackCommands.GetAction<Wait>("CW Repeat",  0).time = 0.001f;
        _attackCommands.GetAction<Wait>("CCW Repeat", 0).time = 0.001f;
        _attackCommands.GetAction<FloatAdd>("CW Restart",  2).add.Value = -15f;
        _attackCommands.GetAction<FloatAdd>("CCW Restart", 2).add.Value =  72f;
        _attackCommands.RemoveAction("CW Restart",  1);
        _attackCommands.RemoveAction("CCW Restart", 1);
        _attackCommands.RemoveAction("CW Repeat",   0);
        _attackCommands.RemoveAction("CCW Repeat",  0);
        FsmutilExt.InsertAction(_attackCommands, "Nail Fan",
            new AuraAction(() =>
            {
                float[] cwAngles = { 124.7f, 74.9f, -87.8f, 50.6f, 87.4f };
                float a   = cwAngles[Random.Range(0, cwAngles.Length)];
                float ccw = a + 90f;
                _attackCommands.GetAction<FloatAdd>("CW Spawn",  2).add.Value = a;
                _attackCommands.GetAction<FloatAdd>("CCW Spawn", 2).add.Value = ccw;
            }), 0);

        _attackChoices.GetAction<Wait>("Beam Sweep L", 0).time = 0.5f;
        _attackChoices.GetAction<Wait>("Beam Sweep R", 0).time = 0.5f;
        _attackChoices.ChangeTransition("A1 Choice", "BEAM SWEEP R", "Beam Sweep L");
        _attackChoices.ChangeTransition("A2 Choice", "BEAM SWEEP R", "Beam Sweep L 2");
        _attackChoices.GetAction<SendEventByName>("Beam Sweep L 2", 1).sendEvent = "BEAM SWEEP L";
        _attackChoices.GetAction<SendEventByName>("Beam Sweep R 2", 1).sendEvent = "BEAM SWEEP R";
        _attackChoices.GetAction<Wait>("Beam Sweep L 2", 0).time = 3.5f;
        _attackChoices.GetAction<Wait>("Beam Sweep R 2", 0).time = 3.5f;

        _attackCommands.GetAction<SendEventByName>("EB 1", 9).delay = 0.2f;
        _attackCommands.GetAction<Wait>("EB 1", 10).time            = 0.4f;
        _attackCommands.GetAction<SendEventByName>("EB 2", 9).delay = 0.2f;
        _attackCommands.GetAction<Wait>("EB 2", 10).time            = 0.4f;
        _attackCommands.GetAction<SendEventByName>("EB 3", 9).delay = 0.2f;
        _attackCommands.GetAction<Wait>("EB 3", 10).time            = 0.4f;
        _attackCommands.GetAction<SendEventByName>("EB 4", 4).delay = 0.3f;
        _attackCommands.GetAction<Wait>("EB 4", 5).time             = 0.5f;
        _attackCommands.GetAction<SendEventByName>("EB 5", 5).delay = 0.3f;
        _attackCommands.GetAction<Wait>("EB 5", 6).time             = 0.5f;
        _attackCommands.GetAction<SendEventByName>("EB 6", 5).delay = 0.3f;
        _attackCommands.GetAction<Wait>("EB 6", 6).time             = 0.5f;
        _attackCommands.GetAction<SendEventByName>("EB 7", 8).delay = 0.4f;
        _attackCommands.GetAction<Wait>("EB 7", 9).time             = 0.6f;
        _attackCommands.GetAction<SendEventByName>("EB 8", 8).delay = 0.4f;
        _attackCommands.GetAction<Wait>("EB 8", 9).time             = 0.6f;
        _attackCommands.GetAction<SendEventByName>("EB 9", 8).delay = 0.4f;
        _attackCommands.GetAction<Wait>("EB 9", 9).time             = 0.6f;
        _attackCommands.GetAction<Wait>("Eb Extra Wait", 0).time    = 0.05f;
        _attackCommands.GetAction<SendEventByName>("Aim", 10).delay = 2f;
        _attackCommands.GetAction<Wait>("Aim", 11).time             = 0.7f;
        _attackCommands.GetAction<SendEventByName>("Aim", 8).delay  = 0.8f;
        _attackCommands.GetAction<SendEventByName>("Aim", 9).delay  = 0.4f;

        _combTopTarget = _attackChoices.GetAction<SendEventByName>("Nail Top Sweep", 2).eventTarget;
        _attackChoices.GetAction<SendEventByName>("Nail Top Sweep", 1).delay = 0.35f;
        _attackChoices.GetAction<SendEventByName>("Nail Top Sweep", 2).delay = 0.7f;
        _attackChoices.GetAction<SendEventByName>("Nail Top Sweep", 3).delay = 1.05f;

        _attackChoices.GetAction<SendEventByName>("Nail L Sweep", 1).delay = 1.85f;
        _attackChoices.GetAction<SendEventByName>("Nail L Sweep", 2).delay = 3.45f;
        _attackChoices.GetAction<Wait>("Nail L Sweep", 3).time             = 4.5f;
        _attackChoices.GetAction<SendEventByName>("Nail R Sweep", 1).delay = 1.85f;
        _attackChoices.GetAction<SendEventByName>("Nail R Sweep", 2).delay = 3.45f;
        _attackChoices.GetAction<Wait>("Nail R Sweep", 3).time             = 4.5f;
        AddNailWall("Nail L Sweep",   "COMB R",  1.3f, 1);
        AddNailWall("Nail R Sweep",   "COMB L",  1.3f, 1);
        AddNailWall("Nail L Sweep",   "COMB R",  2.9f, 1);
        AddNailWall("Nail R Sweep",   "COMB L",  2.9f, 1);
        AddNailWall("Nail L Sweep 2", "COMB R2", 1.0f, 1);
        AddNailWall("Nail R Sweep 2", "COMB L2", 1.0f, 1);

        FsmutilExt.InsertAction(_attackCommands, "Comb L",
            new AuraAction(() =>
            {
                if (_attackChoices.FsmVariables.GetFsmInt("Arena").Value == 2) return;
                var wall  = _attackCommands.FsmVariables.FindFsmGameObject("Attack Obj").Value;
                var tween = wall?.LocateMyFSM("Control")?.GetAction<iTweenMoveBy>("Tween", 0);
                if (tween != null) { tween.speed = 12f; tween.easeType = iTween.EaseType.easeInOutSine; }
            }), 3);
        FsmutilExt.InsertAction(_attackCommands, "Comb R",
            new AuraAction(() =>
            {
                if (_attackChoices.FsmVariables.GetFsmInt("Arena").Value == 2) return;
                var wall  = _attackCommands.FsmVariables.FindFsmGameObject("Attack Obj").Value;
                var tween = wall?.LocateMyFSM("Control")?.GetAction<iTweenMoveBy>("Tween", 0);
                if (tween != null) { tween.speed = 12f; tween.easeType = iTween.EaseType.easeInOutSine; }
            }), 3);

        FsmutilExt.InsertAction(_attackCommands, "Comb L 2",
            new AuraAction(() => { if (_attackChoices.FsmVariables.GetFsmInt("Arena").Value != 2) RedirectWallVertical(); }), 3);
        FsmutilExt.InsertAction(_attackCommands, "Comb R 2",
            new AuraAction(() => { if (_attackChoices.FsmVariables.GetFsmInt("Arena").Value != 2) RedirectWallVertical(); }), 3);

        _teleport.GetAction<SendEventByName>("Arrive", 5).eventTarget =
            _control.GetAction<SendEventByName>("Stun1 Out", 9).eventTarget;
        _teleport.GetAction<SendEventByName>("Arrive", 5).sendEvent = "SmallShake";

        FsmutilExt.InsertAction(_control, "Tele 11", new AuraAction(() => { _lastTeleSpot = FinalTeleSpot.Center; }), 0);
        FsmutilExt.InsertAction(_control, "Tele 12", new AuraAction(() => { _lastTeleSpot = FinalTeleSpot.Left;   }), 0);
        FsmutilExt.InsertAction(_control, "Tele 13", new AuraAction(() => { _lastTeleSpot = FinalTeleSpot.Right;  }), 0);

        FsmutilExt.InsertAction(_teleport, "Arrive",
            new AuraAction(() =>
            {
                if (!FINAL_SWORD_ENABLED || !_finalSwordReady) return;
                if (_lastTeleSpot == FinalTeleSpot.None) return;

                if (_finalSwordCoroutine != null)
                {
                    StopCoroutine(_finalSwordCoroutine);
                    _finalSwordCoroutine = null;
                    foreach (var b in _activeFinalSwordBeams) if (b != null) Object.Destroy(b);
                    _activeFinalSwordBeams.Clear();
                }

                switch (_lastTeleSpot)
                {
                    case FinalTeleSpot.Left:
                        _finalSwordCoroutine = StartCoroutine(FinalSwordVolley(false, true));
                        break;
                    case FinalTeleSpot.Right:
                        _finalSwordCoroutine = StartCoroutine(FinalSwordVolley(true, false));
                        break;
                    case FinalTeleSpot.Center:
                        _finalSwordCoroutine = StartCoroutine(FinalSwordVolley(true, true));
                        break;
                }
            }), 0);

        _spikeMasterControl.GetAction<SendEventByName>("Spikes Left",  0).sendEvent = "UP";
        _spikeMasterControl.GetAction<SendEventByName>("Spikes Left",  1).sendEvent = "UP";
        _spikeMasterControl.GetAction<SendEventByName>("Spikes Left",  2).sendEvent = "UP";
        _spikeMasterControl.GetAction<SendEventByName>("Spikes Left",  3).sendEvent = "UP";
        _spikeMasterControl.GetAction<SendEventByName>("Spikes Left",  4).sendEvent = "UP";
        _spikeMasterControl.GetAction<SendEventByName>("Spikes Right", 0).sendEvent = "UP";
        _spikeMasterControl.GetAction<SendEventByName>("Spikes Right", 1).sendEvent = "UP";
        _spikeMasterControl.GetAction<SendEventByName>("Spikes Right", 2).sendEvent = "UP";
        _spikeMasterControl.GetAction<SendEventByName>("Spikes Right", 3).sendEvent = "UP";
        _spikeMasterControl.GetAction<SendEventByName>("Spikes Right", 4).sendEvent = "UP";
        _spikeMasterControl.GetAction<SendEventByName>("Wave L", 2).sendEvent = "UP";
        _spikeMasterControl.GetAction<SendEventByName>("Wave L", 3).sendEvent = "UP";
        _spikeMasterControl.GetAction<SendEventByName>("Wave L", 4).sendEvent = "UP";
        _spikeMasterControl.GetAction<SendEventByName>("Wave L", 5).sendEvent = "UP";
        _spikeMasterControl.GetAction<SendEventByName>("Wave L", 6).sendEvent = "UP";
        _spikeMasterControl.GetAction<WaitRandom>("Wave L", 7).timeMin = 0.6f;
        _spikeMasterControl.GetAction<WaitRandom>("Wave L", 7).timeMax = 0.9f;
        _spikeMasterControl.GetAction<SendEventByName>("Wave R", 2).sendEvent = "UP";
        _spikeMasterControl.GetAction<SendEventByName>("Wave R", 3).sendEvent = "UP";
        _spikeMasterControl.GetAction<SendEventByName>("Wave R", 4).sendEvent = "UP";
        _spikeMasterControl.GetAction<SendEventByName>("Wave R", 5).sendEvent = "UP";
        _spikeMasterControl.GetAction<SendEventByName>("Wave R", 6).sendEvent = "UP";
        _spikeMasterControl.GetAction<WaitRandom>("Wave R", 7).timeMin = 0.6f;
        _spikeMasterControl.GetAction<WaitRandom>("Wave R", 7).timeMax = 0.9f;
        _spikeMasterControl.SetState("Spike Waves");

        foreach (Transform sg in _spikeMaster.transform)
            foreach (Transform sp in sg)
            {
                ConfigureAuraSpike(sp.gameObject);
                _allGroundSpikes.Add(sp.gameObject);
                sp.gameObject.LocateMyFSM("Control")?.SendEvent("DOWN");
            }
        _spikeMasterControl.enabled = false;

        _allGroundSpikes.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        if (_allGroundSpikes.Count >= 2)
        {
            _extremeGroundSpikes.Add(_allGroundSpikes[0]);
            _extremeGroundSpikes.Add(_allGroundSpikes[_allGroundSpikes.Count - 1]);
        }

        _phaseControl.RemoveAction("Set Phase 3", 0);
        _control.GetAction<Wait>("Stun1 Start", 9).time = 4f;
        _control.GetAction<Wait>("Stun1 Roar",  3).time = 2f;
        _control.GetAction<Wait>("Plat Setup",  6).time = 2f;

        // SPIKE GROUPS ARENA 2
        for (float x = 58.5f; x <= 62.5f; x += 1f) { var g = SpawnSpike(new Vector2(x, 34.7f));  g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup0.Add(g); }
        for (float x = 49.7f; x <= 53.7f; x += 1f) { var g = SpawnSpike(new Vector2(x, 37.6f));  g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup1.Add(g); }
        for (float x = 41.4f; x <= 43.4f; x += 1f) { var g = SpawnSpike(new Vector2(x, 36.7f));  g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup2.Add(g); }
        for (float x = 46.2f; x <= 48.2f; x += 1f) { var g = SpawnSpike(new Vector2(x, 43.7f));  g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup3.Add(g); }
        for (float x = 66.8f; x <= 68.8f; x += 1f) { var g = SpawnSpike(new Vector2(x, 39.1f));  g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup5.Add(g); }
        for (float x = 71.7f; x <= 73.7f; x += 1f) { var g = SpawnSpike(new Vector2(x, 45.1f));  g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup6.Add(g); }
        for (float x = 57.5f; x <= 61.5f; x += 1f) { var g = SpawnSpike(new Vector2(x, 45.9f));  g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup7.Add(g); }
        for (float x = 41.4f; x <= 43.4f; x += 1f) { var g = SpawnSpikeRotated(new Vector2(x, 31.5f), 180f); g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup2.Add(g); }

        { var sR = SpawnSpikeRotated(new Vector2(63f,  44f), -90f); sR.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup24.Add(sR); }
        { var sL = SpawnSpikeRotated(new Vector2(56f,  44f),  90f); sL.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup24.Add(sL); }
        { var sL = SpawnSpikeRotated(new Vector2(45f, 41.7f),  90f); sL.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup25.Add(sL); }
        { var sR = SpawnSpikeRotated(new Vector2(50f, 41.7f), -90f); sR.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup25.Add(sR); }
        { var sL = SpawnSpikeRotated(new Vector2(47.9f, 35.7f),  90f); sL.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup26.Add(sL); }
        { var sR = SpawnSpikeRotated(new Vector2(55.1f, 35.7f), -90f); sR.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup26.Add(sR); }
        { var sL = SpawnSpikeRotated(new Vector2(57f, 32.8f),  90f); sL.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup27.Add(sL); }
        { var sR = SpawnSpikeRotated(new Vector2(64f, 32.8f), -90f); sR.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup27.Add(sR); }
        { var sL = SpawnSpikeRotated(new Vector2(65.25f, 37f),  90f); sL.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup28.Add(sL); }
        { var sR = SpawnSpikeRotated(new Vector2(70.5f,  37f), -90f); sR.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup28.Add(sR); }
        { var sL = SpawnSpikeRotated(new Vector2(70f, 43f),  90f); sL.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup29.Add(sL); }
        { var sR = SpawnSpikeRotated(new Vector2(75.5f, 43f), -90f); sR.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup29.Add(sR); }

        // SPIKE GROUPS HACIA ABAJO
        for (float x = 58.5f; x <= 62.5f; x += 1f) { var g = SpawnSpikeRotated(new Vector2(x, PLAT_DOWN_Y_G0 - ADJUST_Y), 180f); g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup30.Add(g); }
        for (float x = 49.7f; x <= 53.7f; x += 1f) { var g = SpawnSpikeRotated(new Vector2(x, PLAT_DOWN_Y_G1 - ADJUST_Y), 180f); g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup31.Add(g); }
        for (float x = 41.4f; x <= 43.4f; x += 1f) { var g = SpawnSpikeRotated(new Vector2(x, PLAT_DOWN_Y_G2 - ADJUST_Y),  180f); g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup32.Add(g); }
        for (float x = 41.4f; x <= 43.4f; x += 1f) { var g = SpawnSpikeRotated(new Vector2(x, PLAT_DOWN_Y_G2B), 0f);   g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup32.Add(g); }
        for (float x = 46.2f; x <= 48.2f; x += 1f) { var g = SpawnSpikeRotated(new Vector2(x, PLAT_DOWN_Y_G3 - ADJUST_Y),  180f); g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup33.Add(g); }
        for (float x = 66.8f; x <= 68.8f; x += 1f) { var g = SpawnSpikeRotated(new Vector2(x, PLAT_DOWN_Y_G5 - ADJUST_Y),  180f); g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup35.Add(g); }
        for (float x = 71.7f; x <= 73.7f; x += 1f) { var g = SpawnSpikeRotated(new Vector2(x, PLAT_DOWN_Y_G6 - ADJUST_Y),  180f); g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup36.Add(g); }
        for (float x = 57.5f; x <= 61.5f; x += 1f) { var g = SpawnSpikeRotated(new Vector2(x, PLAT_DOWN_Y_G7 - ADJUST_Y),  180f); g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup37.Add(g); }

        // SPIKE GROUPS CLIMB
        for (float x = 61.6f; x <= 65.6f;  x += 1f) { var g = SpawnSpike(new Vector2(x, 51.8f));   g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup8.Add(g); }
        for (int   x = 57;    x <= 59;      x++)      { var g = SpawnSpike(new Vector2(x, 58.2f));   g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup9.Add(g); }
        for (float x = 63.2f; x <= 65.2f;  x += 1f) { var g = SpawnSpike(new Vector2(x, 64.1f));   g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup10.Add(g); }
        for (float x = 64.7f; x <= 66.7f;  x += 1f) { var g = SpawnSpike(new Vector2(x, 70.8f));   g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup11.Add(g); }
        for (float x = 57.2f; x <= 61.2f;  x += 1f) { var g = SpawnSpike(new Vector2(x, 77.2f));   g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup12.Add(g); }
        for (float x = 55.4f; x <= 57.4f;  x += 1f) { var g = SpawnSpike(new Vector2(x, 84.7f));   g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup13.Add(g); }
        for (float x = 58.9f; x <= 60.9f;  x += 1f) { var g = SpawnSpike(new Vector2(x, 89.2f));   g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup14.Add(g); }
        for (float x = 61.1f; x <= 65.1f;  x += 1f) { var g = SpawnSpike(new Vector2(x, 94.4f));   g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup15.Add(g); }
        for (float x = 57.2f; x <= 59.2f;  x += 1f) { var g = SpawnSpike(new Vector2(x, 101.1f));  g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup16.Add(g); }
        for (float x = 63.3f; x <= 65.3f;  x += 1f) { var g = SpawnSpike(new Vector2(x, 107.4f));  g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup17.Add(g); }
        for (float x = 64.6f; x <= 66.6f;  x += 1f) { var g = SpawnSpike(new Vector2(x, 113.7f));  g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup18.Add(g); }
        for (float x = 57.2f; x <= 61.2f;  x += 1f) { var g = SpawnSpike(new Vector2(x, 120.5f));  g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup19.Add(g); }
        for (float x = 55.3f; x <= 57.3f;  x += 1f) { var g = SpawnSpike(new Vector2(x, 128.4f));  g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup20.Add(g); }
        for (int   x = 59;    x <= 61;      x++)      { var g = SpawnSpike(new Vector2(x, 133.4f));  g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup21.Add(g); }
        for (float x = 61.3f; x <= 65.3f;  x += 1f) { var g = SpawnSpike(new Vector2(x, 139.2f));  g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup22.Add(g); }
        for (float x = 61.8f; x <= 63.8f;  x += 1f) { var g = SpawnSpike(new Vector2(x, 146.5f));  g.LocateMyFSM("Control").SendEvent("DOWN"); _spikeGroup23.Add(g); }

        _attackCommands.RemoveAction("Set Final Orbs", 0);
        GameObject.Find("Radiant Plat Small (11)").LocateMyFSM("radiant_plat")
            .GetAction<Wait>("Vanish Antic", 1).time = 3.5f;
        _control.GetAction<SetVector3Value>("Tele 11", 1).vector3Value = new Vector3(62.94f, 157.65f, 0.006f);
        _control.GetAction<SetVector3Value>("Tele 12", 1).vector3Value = new Vector3(53.88f, 157.65f, 0.006f);
        _control.GetAction<SetVector3Value>("Tele 13", 1).vector3Value = new Vector3(72.4f,  157.65f, 0.006f);

        StartCoroutine(GroundPhaseNailWalls());
        InitBossAfterimageSystem();
        Log("Configuración completada.");
    }

    private void Update()
    {
        if (_attackCommands.FsmVariables.GetFsmBool("Repeated").Value)
        {
            switch (CWRepeats)
            {
                case 0: CWRepeats = 1; _attackCommands.FsmVariables.GetFsmBool("Repeated").Value = false; break;
                case 1: CWRepeats = 2; _attackCommands.FsmVariables.GetFsmBool("Repeated").Value = false; break;
                case 2: CWRepeats = 3; break;
            }
        }
        else if (CWRepeats == 3) CWRepeats = 0;

        if (_beamsweepercontrol.ActiveStateName == _beamsweeper2control.ActiveStateName)
        {
            var st = _beamsweepercontrol.ActiveStateName;
            if      (st == "Beam Sweep L") _beamsweeper2control.SendEvent("BEAM SWEEP R");
            else if (st == "Beam Sweep R") _beamsweeper2control.SendEvent("BEAM SWEEP L");
        }

        // PINCHOS SUELO
        if (!swordRainSpikesSet)
        {
            if (!_groundSpikeStage1 && _hm.hp <= 5000) { _groundSpikeStage1 = true; ActivateGroundSpikesExcludingN(5); }
            if (!_groundSpikeStage2 && _hm.hp <= 4500) { _groundSpikeStage2 = true; ActivateGroundSpikesExcludingN(2); }
            if (!_groundSpikeStage3 && _hm.hp <= 4400) { _groundSpikeStage3 = true; ActivateGroundSpikesExcludingN(1); }
            if (!_groundSpikeStage4 && _hm.hp <= 4000) { _groundSpikeStage4 = true; ActivateGroundSpikesExcludingN(0); }
        }

        if (_hm.hp <= PHASE_SWORD_RAIN + 30 && !disableBeamSet)
        {
            disableBeamSet = true;
            _attackChoices.ChangeTransition("A1 Choice", "BEAM SWEEP L", "Orb Wait");
            _attackChoices.ChangeTransition("A1 Choice", "BEAM SWEEP R", "Eye Beam Wait");
        }

        // SWORD RAIN INICIA
        if (_hm.hp <= PHASE_SWORD_RAIN && !swordRainSpikesSet)
        {
            swordRainSpikesSet = true;
            groundSpikesLocked = true;
            _spikeMasterControl.enabled = false;
            foreach (var spike in _allGroundSpikes)
            {
                spike?.LocateMyFSM("Control")?.SendEvent("UP");
                var d = spike?.GetComponent<DamageHero>();
                if (d != null) d.damageDealt = 1;
            }
            StartCoroutine(SwordRainVerticalBeams());
            StartCoroutine(FatalBeamSweepLoop());
            StartCoroutine(SwordRainGroundLoop());
            SetBossAfterimage(BOSS_AFTERIMAGE_PHASE_SWORD_RAIN);
            SetHalo1Phase(1);
            SetHalo2Phase(1);
        }


        if (_hm.hp <= PHASE_PLATFORMS && !swordRainEndSet)
        {
            swordRainEndSet = true;

            foreach (var spike in _allGroundSpikes)
            {
                var d = spike?.GetComponent<DamageHero>();
                if (d != null) d.damageDealt = 1;
            }
            foreach (var sp in _extremeGroundSpikes)
                if (sp != null) { sp.LocateMyFSM("Control")?.SendEvent("DOWN"); Object.Destroy(sp, 1.0f); }
            _extremeGroundSpikes.Clear();

            StartCoroutine(DelayedConvo(TEXT_SWORD_RAIN_END, 7f));


            StartCoroutine(HaloClockThenBeat(_halo1, () =>
            {
                StartCoroutine(TransitionBgColors(
                    BG_SKY_PHASE(1), BG_PILLAR_PHASE(1), BG_HAZE_PHASE(1),
                    BG_CLOUD_PHASE(1), BG_RAY_PHASE(1), BG_TRANSITION_DURATION));
            }));


            StartCoroutine(FadeOutHalo(_halo2));

            foreach (var b in new[] { _srLA, _srRA, _srLB, _srRB, _srLC, _srRC, _srLD, _srRD, _srLE, _srRE })
                if (b != null) StartCoroutine(FadeAndDestroyBeam(b));
            _srLA = _srRA = _srLB = _srRB = _srLC = _srRC = _srLD = _srRD = _srLE = _srRE = null;

            _platHpAtEntry = _hm.hp;
        }


        if (_attackChoices.FsmVariables.GetFsmInt("Arena").Value == 2 && !arena2Set)
        {
            arena2Set = true;
            if (_platHpAtEntry == 0) _platHpAtEntry = _hm.hp;
            _beamsweepercontrol.GetAction<SetPosition>("Beam Sweep L", 3).x            = 89f;
            _beamsweepercontrol.GetAction<iTweenMoveBy>("Beam Sweep L", 5).vector       = new Vector3(-75f, 0f, 0f);
            _beamsweepercontrol.GetAction<iTweenMoveBy>("Beam Sweep L", 5).time         = 5f;
            _beamsweepercontrol.GetAction<SetPosition>("Beam Sweep R", 4).x            = 32.6f;
            _beamsweepercontrol.GetAction<iTweenMoveBy>("Beam Sweep R", 6).vector       = new Vector3(75f, 0f, 0f);
            _beamsweepercontrol.GetAction<iTweenMoveBy>("Beam Sweep R", 6).time         = 5f;
            _beamsweeper2control.GetAction<SetPosition>("Beam Sweep L", 2).x           = 89f;
            _beamsweeper2control.GetAction<iTweenMoveBy>("Beam Sweep L", 4).vector      = new Vector3(-75f, 0f, 0f);
            _beamsweeper2control.GetAction<iTweenMoveBy>("Beam Sweep L", 4).time        = 5f;
            _beamsweeper2control.GetAction<SetPosition>("Beam Sweep R", 3).x           = 32.6f;
            _beamsweeper2control.GetAction<iTweenMoveBy>("Beam Sweep R", 5).vector      = new Vector3(75f, 0f, 0f);
            _beamsweeper2control.GetAction<iTweenMoveBy>("Beam Sweep R", 5).time        = 5f;


            if (_halo1 != null && !_halo1.activeSelf) _halo1.SetActive(true);
            if (_halo2 != null && !_halo2.activeSelf) _halo2.SetActive(true);

            if (_halo2 != null)
            {
                var sr2 = _halo2.GetComponent<SpriteRenderer>();
                if (sr2 != null) { var c = sr2.color; c.a = 0.5f; sr2.color = c; }
            }

            if (HALO1_REV_ENABLED && _halo1Rev != null)
            {
                _halo1Rev.SetActive(true);
                StartCoroutine(FadeInHalo(_halo1Rev, HALO1_REV_ALPHA, 1.5f));
            }

            StartCoroutine(LaserColumnsLoop());
            StartCoroutine(Arena2BeamBurstLoop());
            StartCoroutine(SwordRainPlatLoop());
            StartCoroutine(DiagonalBeamSweepLoop());
            SetBossAfterimage(BOSS_AFTERIMAGE_PHASE_PLATFORMS);
            SetHalo1Phase(2);
            SetHalo2Phase(2);

            _attackCommands.GetAction<Wait>("Orb Pause", 0).time.Value  = 0.6f;
            _attackChoices.GetAction<Wait>("Orb Recover", 0).time.Value = 1.5f;
        }

        // PINCHOS ARENA 2 — VERTICALES
        if (arena2Set && !_platVerticalSpikesShown && _hm.hp <= 2800)
        {
            _platVerticalSpikesShown = true;
            SendUpToGroup(_spikeGroup0); SendUpToGroup(_spikeGroup1);
            SendUpToGroup(_spikeGroup2); SendUpToGroup(_spikeGroup3);
            SendUpToGroup(_spikeGroup5); SendUpToGroup(_spikeGroup6);
            SendUpToGroup(_spikeGroup7);
        }

        // PINCHOS ARENA 2 — HACIA ABAJO
        if (arena2Set && !_platDownSpikesShown && _hm.hp <= PLAT_DOWN_SPIKES_HP)
        {
            _platDownSpikesShown = true;
            SendUpToGroup(_spikeGroup30); SendUpToGroup(_spikeGroup31);
            SendUpToGroup(_spikeGroup32); SendUpToGroup(_spikeGroup33);
            SendUpToGroup(_spikeGroup35); SendUpToGroup(_spikeGroup36);
            SendUpToGroup(_spikeGroup37);
        }

        // PINCHOS ARENA 2 — ROTADOS
        if (arena2Set && !_platRotatedSpikesShown && _hm.hp <= 2100)
        {
            _platRotatedSpikesShown = true;
            SendUpToGroup(_spikeGroup24); SendUpToGroup(_spikeGroup25);
            SendUpToGroup(_spikeGroup26); SendUpToGroup(_spikeGroup27);
            SendUpToGroup(_spikeGroup28); SendUpToGroup(_spikeGroup29);
        }

        if (_hm.hp <= PHASE_CLIMB && !_climbHPReached)
            _climbHPReached = true;

        if (ascendBeam == null)
            ascendBeam = GameObject.Find("Ascend Beam");

        if (_climbHPReached && base.gameObject.transform.position.y >= 150f)
        {
            SendUpToGroup(_spikeGroup8);  SendUpToGroup(_spikeGroup9);
            SendUpToGroup(_spikeGroup8);  SendUpToGroup(_spikeGroup9);
            SendUpToGroup(_spikeGroup10); SendUpToGroup(_spikeGroup11);
            SendUpToGroup(_spikeGroup12); SendUpToGroup(_spikeGroup13);
            SendUpToGroup(_spikeGroup14); SendUpToGroup(_spikeGroup15);
            SendUpToGroup(_spikeGroup16); SendUpToGroup(_spikeGroup17);
            SendUpToGroup(_spikeGroup18); SendUpToGroup(_spikeGroup19);
            SendUpToGroup(_spikeGroup20); SendUpToGroup(_spikeGroup21);
            SendUpToGroup(_spikeGroup22); SendUpToGroup(_spikeGroup23);
        }

        // CLIMB INICIA
        if (_climbHPReached && base.gameObject.transform.position.y >= 150f && !ascendPhaseSet)
        {
            ascendPhaseSet = true;
            StartCoroutine(TemporarilyLowerPlatformSpikes(5f));
            StartCoroutine(TransitionBgColors(
                BG_SKY_PHASE(2), BG_PILLAR_PHASE(2), BG_HAZE_PHASE(2),
                BG_CLOUD_PHASE(2), BG_RAY_PHASE(2), BG_TRANSITION_DURATION));
            StartCoroutine(SpawnAscendBeams());
            StartCoroutine(AscendCameraZoom());
            SetBossAfterimage(BOSS_AFTERIMAGE_PHASE_CLIMB);
            SetHalo1Phase(2);
            SetHalo2Phase(2);

            if (_knight != null && _knight.GetComponent<AuraInfiniteDash>() == null)
                _knight.AddComponent<AuraInfiniteDash>();
        }

        if (ascendPhaseSet && !finalDanceExtraAttackConfigured
            && !_climbStartTextShown
            && _knight.transform.GetPositionY() >= CLIMB_START_TEXT_Y)
        {
            _climbStartTextShown = true;
            ShowConvo(TEXT_CLIMB_START);
        }


        if (_knight.transform.GetPositionY() > 152f && !finalDanceExtraAttackConfigured)
        {
            finalDanceExtraAttackConfigured = true;

            // Lanzar transición suave de halos y fondo
            StartCoroutine(ClimbToFinalTransition());

            _hm.hp += FINAL_PHASE_HEAL;

            ascendBeam?.LocateMyFSM("Control").SendEvent("END");

            _attackCommands.GetAction<SendEventByName>("Aim", 10).delay = 0.65f;
            _attackCommands.GetAction<Wait>("Aim", 11).time             = 0.95f;
            _attackCommands.GetAction<SendEventByName>("Aim", 8).delay  = 0.5f;
            _attackCommands.GetAction<SendEventByName>("Aim", 9).delay  = 0.5f;

            _control.AddAction("Final Idle", _attackCommands.GetAction<ActivateGameObject>("AB Start", 0));
            for (int i = 0; i <= 10; i++)
                _control.AddAction("Final Idle", _attackCommands.GetAction("Aim", i));
            _control.InsertAction("A2 Tele Choice 2", new ActivateGameObject {
                gameObject = _attackCommands.GetAction<ActivateGameObject>("AB Start", 0).gameObject,
                activate = false, recursive = false, resetOnExit = false, everyFrame = false }, 0);

            foreach (var b in _ascendBeams) if (b != null) b.SetActive(false);
            _ascendBeams.Clear();

            var p10s = GameObject.Find("Radiant Plat Small (10)");
            if (p10s != null) p10s.LocateMyFSM("radiant_plat").SendEvent("SLOW VANISH");
            StartCoroutine(FinalStaticBeam());
            StartCoroutine(EnableFinalSwordAfterDelay());
            SetBossAfterimage(BOSS_AFTERIMAGE_PHASE_FINAL);
        }


        if (_hm.hp <= PHASE_CLIMB && !finalDanceOrbsConfigured)
        {
            ShowConvo(TEXT_CLIMB_FINAL_MESSAGE);
            finalDanceOrbsConfigured = true;
            _attackCommands.GetAction<Wait>("Orb Antic", 0).time.Value      = 1.8f;
            _attackCommands.GetAction<Wait>("FinalOrb Pause", 0).time.Value = 1.4f;
            _attackChoices.GetAction<Wait>("Orb Recover", 0).time.Value     = 1.8f;
        }
        if (finalDanceOrbsConfigured && !_finalOrbsAccel1 && _hm.hp <= FINAL_ORB_ACCEL_HP_1)
        {
            _finalOrbsAccel1 = true;
            _attackCommands.GetAction<Wait>("Orb Antic", 0).time.Value      = 1f;
            _attackCommands.GetAction<Wait>("FinalOrb Pause", 0).time.Value = 0.4f;
            _attackChoices.GetAction<Wait>("Orb Recover", 0).time.Value     = 0.5f;
        }
        if (_finalOrbsAccel1 && !_finalOrbsAccel2 && _hm.hp <= FINAL_ORB_ACCEL_HP_2)
        {
            _finalOrbsAccel2 = true;
            _attackCommands.GetAction<Wait>("Orb Antic", 0).time.Value      = 0.05f;
            _attackCommands.GetAction<Wait>("FinalOrb Pause", 0).time.Value = 0.1f;
            _attackChoices.GetAction<Wait>("Orb Recover", 0).time.Value     = 0.1f;
        }
        if (_finalOrbsAccel2 && !_finalOrbsAccel3 && _hm.hp <= FINAL_ORB_ACCEL_HP_3)
        {
            _finalOrbsAccel3 = true;
            _attackCommands.GetAction<Wait>("Orb Antic", 0).time.Value      = 0.01f;
            _attackCommands.GetAction<Wait>("FinalOrb Pause", 0).time.Value = 0.1f;
            _attackChoices.GetAction<Wait>("Orb Recover", 0).time.Value     = 0.11f;
        }

        if (_hm.hp < PHASE_CLIMB && !onePlatSet)
        {
            onePlatSet = true;
            var p10 = GameObject.Find("Radiant Plat Small (10)");
            if (p10 != null) p10.LocateMyFSM("radiant_plat").SendEvent("SLOW VANISH");
            var p11 = GameObject.Find("Radiant Plat Small (11)");
            if (p11 != null) p11.LocateMyFSM("radiant_plat").SendEvent("SLOW VANISH");
        }
        if (onePlatSet)
        {
            var p11r = GameObject.Find("Radiant Plat Small (11)");
            if (p11r != null && p11r.LocateMyFSM("radiant_plat").ActiveStateName == "Appear 2")
                p11r.LocateMyFSM("radiant_plat").SendEvent("SLOW VANISH");
        }
        if (_hm.hp < 720 && !_deadConvoShown)
        {
            _deadConvoShown = true;
            ShowConvo(TEXT_DEAD);
        }
    }

    private void SetupHalos()
    {
        try
        {
            Transform haloTransform = transform.Find("Halo");
            if (haloTransform == null) { Log("SetupHalos: No se encontró 'Halo'."); return; }

            _halo = haloTransform.gameObject;
            var srHalo = _halo.GetComponent<SpriteRenderer>();
            if (srHalo != null && AuraRadiance.Halo0Texture != null)
            {
                Sprite newSprite = Sprite.Create(
                    AuraRadiance.Halo0Texture,
                    new Rect(0f, 0f, AuraRadiance.Halo0Texture.width, AuraRadiance.Halo0Texture.height),
                    new Vector2(0.5f, 0.5f), srHalo.sprite.pixelsPerUnit);
                srHalo.sprite = newSprite;
                Log("SetupHalos: Halo0 aplicado.");
            }

            //HALO 1
            if (AuraRadiance.Halo1Texture != null)
            {
                _halo1 = Object.Instantiate(_halo, _halo.transform.position, Quaternion.Euler(0, 0, 90), _halo.transform.parent);
                _halo1.transform.localScale = new Vector3(1f, 1f, 1f);
                _halo1.name = "Halo1";

                var srHalo1 = _halo1.GetComponent<SpriteRenderer>();
                if (srHalo1 != null && srHalo != null)
                {
                    Sprite newSprite1 = Sprite.Create(
                        AuraRadiance.Halo1Texture,
                        new Rect(0f, 0f, AuraRadiance.Halo1Texture.width, AuraRadiance.Halo1Texture.height),
                        new Vector2(0.5f, 0.5f), srHalo.sprite.pixelsPerUnit);
                    srHalo1.sprite = newSprite1;
                }

                var anim1 = _halo1.AddComponent<AuraHaloAnimator>();
                anim1.beatEnabled  = HALO1_BEAT_ENABLED;
                anim1.beatMinScale = HALO1_BEAT_MIN;
                anim1.beatMaxScale = HALO1_BEAT_MAX;
                anim1.beatFrequency = HALO1_BEAT_FREQ;
                anim1.BuildDefaultPhases();
                anim1.SetPhase(0);
                Log("SetupHalos: Halo1 con AuraHaloAnimator.");
            }


            if (AuraRadiance.Halo2Texture != null)
            {
                _halo2 = Object.Instantiate(_halo, _halo.transform.position, Quaternion.Euler(0, 0, 0), _halo.transform.parent);
                _halo2.transform.localScale = new Vector3(1.15f, 1.15f, 1f);
                _halo2.name = "Halo2";

                var srHalo2 = _halo2.GetComponent<SpriteRenderer>();
                if (srHalo2 != null && srHalo != null)
                {
                    Sprite newSprite2 = Sprite.Create(
                        AuraRadiance.Halo2Texture,
                        new Rect(0f, 0f, AuraRadiance.Halo2Texture.width, AuraRadiance.Halo2Texture.height),
                        new Vector2(0.5f, 0.5f), srHalo.sprite.pixelsPerUnit);
                    srHalo2.sprite = newSprite2;
                }

                var rot2 = _halo2.AddComponent<AuraHaloSimpleRotator>();
                rot2.BuildDefaultPhases();
                rot2.SetPhase(0);
                Log("SetupHalos: Halo2 con AuraHaloSimpleRotator.");
            }
            else { Log("SetupHalos: Halo2Texture no disponible."); }



            if (HALO1_REV_ENABLED && AuraRadiance.Halo1Texture != null && _halo1 != null)
            {
                _halo1Rev = Object.Instantiate(_halo, _halo.transform.position, Quaternion.Euler(0, 0, -90), _halo.transform.parent);
                _halo1Rev.transform.localScale = new Vector3(HALO1_REV_SIZE, HALO1_REV_SIZE, 1f);
                _halo1Rev.name = "Halo1Rev";

                var srRev = _halo1Rev.GetComponent<SpriteRenderer>();
                if (srRev != null && srHalo != null)
                {
                    Sprite sprRev = Sprite.Create(
                        AuraRadiance.Halo1Texture,
                        new Rect(0f, 0f, AuraRadiance.Halo1Texture.width, AuraRadiance.Halo1Texture.height),
                        new Vector2(0.5f, 0.5f), srHalo.sprite.pixelsPerUnit);
                    srRev.sprite = sprRev;
                    var c = srRev.color; c.a = 0f; srRev.color = c;
                }

                var rotRev = _halo1Rev.AddComponent<AuraHaloSimpleRotator>();
                rotRev.phases.Clear();

                rotRev.phases.Add(new AuraHaloSimpleRotatorPhase
                {
                    spinSpeed      = HALO1_REV_SPIN,
                    targetAlpha    = HALO1_REV_ALPHA,
                    alphaSmoothing = 5f
                });
                rotRev.SetPhase(0);
                _halo1Rev.SetActive(false); 
                Log("SetupHalos: Halo1Rev creado (desactivado hasta plataformas).");
            }


            if (HALO_EXTRA_B_ENABLED && AuraRadiance.Halo1Texture != null)
            {
                _halo1ExtraB = Object.Instantiate(_halo, _halo.transform.position, Quaternion.Euler(0, 0, 45), _halo.transform.parent);
                _halo1ExtraB.transform.localScale = new Vector3(HALO_EXTRA_B_SIZE, HALO_EXTRA_B_SIZE, 1f);
                _halo1ExtraB.name = "Halo1ExtraB";

                var srB = _halo1ExtraB.GetComponent<SpriteRenderer>();
                if (srB != null && srHalo != null)
                {
                    Sprite sprB = Sprite.Create(
                        AuraRadiance.Halo1Texture,
                        new Rect(0f, 0f, AuraRadiance.Halo1Texture.width, AuraRadiance.Halo1Texture.height),
                        new Vector2(0.5f, 0.5f), srHalo.sprite.pixelsPerUnit);
                    srB.sprite = sprB;
                    var c = srB.color; c.a = 0f; srB.color = c;
                }

                var rotB = _halo1ExtraB.AddComponent<AuraHaloSimpleRotator>();
                rotB.phases.Clear();
                rotB.phases.Add(new AuraHaloSimpleRotatorPhase
                {
                    spinSpeed      = HALO_EXTRA_B_SPIN,
                    targetAlpha    = HALO_EXTRA_B_ALPHA,
                    alphaSmoothing = 6f
                });
                rotB.SetPhase(0);
                _halo1ExtraB.SetActive(false);
                Log("SetupHalos: Halo1ExtraB creado (desactivado hasta fase final).");
            }

            if (HALO_EXTRA_C_ENABLED && AuraRadiance.Halo1Texture != null)
            {
                _halo1ExtraC = Object.Instantiate(_halo, _halo.transform.position, Quaternion.Euler(0, 0, -30), _halo.transform.parent);
                _halo1ExtraC.transform.localScale = new Vector3(HALO_EXTRA_C_SIZE, HALO_EXTRA_C_SIZE, 1f);
                _halo1ExtraC.name = "Halo1ExtraC";

                var srC = _halo1ExtraC.GetComponent<SpriteRenderer>();
                if (srC != null && srHalo != null)
                {
                    Sprite sprC = Sprite.Create(
                        AuraRadiance.Halo1Texture,
                        new Rect(0f, 0f, AuraRadiance.Halo1Texture.width, AuraRadiance.Halo1Texture.height),
                        new Vector2(0.5f, 0.5f), srHalo.sprite.pixelsPerUnit);
                    srC.sprite = sprC;
                    var c = srC.color; c.a = 0f; srC.color = c;
                }

                var rotC = _halo1ExtraC.AddComponent<AuraHaloSimpleRotator>();
                rotC.phases.Clear();
                rotC.phases.Add(new AuraHaloSimpleRotatorPhase
                {
                    spinSpeed      = HALO_EXTRA_C_SPIN,
                    targetAlpha    = HALO_EXTRA_C_ALPHA,
                    alphaSmoothing = 7f
                });
                rotC.SetPhase(0);
                _halo1ExtraC.SetActive(false);
                Log("SetupHalos: Halo1ExtraC creado (desactivado hasta fase final).");
            }
        }
        catch (System.Exception e) { Log("Error en SetupHalos: " + e.Message); }
    }


    private void SetHalo1Phase(int phase)
    {
        if (_halo1 == null) return;
        _halo1.GetComponent<AuraHaloAnimator>()?.SetPhase(phase);
    }

    private void SetHalo2Phase(int phase)
    {
        if (_halo2 == null) return;
        _halo2.GetComponent<AuraHaloSimpleRotator>()?.SetPhase(phase);
    }

    private void Halo1Bounce(float scale = 1.5f, float duration = 0.15f)
    {
        _halo1?.GetComponent<AuraHaloAnimator>()?.TriggerBounce(scale, duration);
    }

    private void Halo1SpinImpulse(float amount = -6f)
    {
        _halo1?.GetComponent<AuraHaloAnimator>()?.SpinImpulse(amount);
    }


    private IEnumerator HaloClockThenBeat(GameObject haloGO, System.Action onColorDone = null)
    {
        if (haloGO == null) yield break;
        var anim = haloGO.GetComponent<AuraHaloAnimator>();
        if (anim == null) yield break;

        anim.StartClockAnimation(HALO_CLOCK_QUARTER_TIME);

        yield return new WaitUntil(() => anim.IsClockDone);
        onColorDone?.Invoke();


        yield return new WaitUntil(() => anim.IsVanishDone);

        if (haloGO != null && !haloGO.activeSelf) haloGO.SetActive(true);
    }


    private IEnumerator ClimbToFinalTransition()
    {
        float dur = CLIMB_FINAL_TRANSITION_DUR;


        if (HALO1_REV_ENABLED && _halo1Rev != null && _halo1Rev.activeSelf)
            StartCoroutine(FadeOutAndDisableHalo(_halo1Rev, dur * 0.6f));

        StartCoroutine(TransitionBgColors(
            BG_SKY_PHASE(3), BG_PILLAR_PHASE(3), BG_HAZE_PHASE(3),
            BG_CLOUD_PHASE(3), BG_RAY_PHASE(3), BG_TRANSITION_DURATION));

        // Breve pausa antes de cambiar halos para suavizar el corte
        yield return new WaitForSeconds(dur * 0.35f);


        SetHalo1Phase(3);
        SetHalo2Phase(3);

        if (HALO_EXTRA_B_ENABLED && _halo1ExtraB != null)
        {
            _halo1ExtraB.SetActive(true);
            StartCoroutine(FadeInHalo(_halo1ExtraB, HALO_EXTRA_B_ALPHA, dur * 0.8f));
        }
        yield return new WaitForSeconds(dur * 0.25f);
        if (HALO_EXTRA_C_ENABLED && _halo1ExtraC != null)
        {
            _halo1ExtraC.SetActive(true);
            StartCoroutine(FadeInHalo(_halo1ExtraC, HALO_EXTRA_C_ALPHA, dur * 1.0f));
        }

        // Esperar a que termine la transición antes de arrancar el haze oscilante
        yield return new WaitForSeconds(dur * 0.5f);

        if (HAZE_OSC_ENABLED)
        {
            if (_finalHazeOscillateCoroutine != null) StopCoroutine(_finalHazeOscillateCoroutine);
            _finalHazeOscillateCoroutine = StartCoroutine(FinalHazeOscillate());
        }
    }


    private IEnumerator FadeInHalo(GameObject haloGO, float targetAlpha, float duration)
    {
        if (haloGO == null) yield break;
        var sr = haloGO.GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        float elapsed = 0f;
        float startA  = sr.color.a;
        while (elapsed < duration && haloGO != null)
        {
            elapsed += Time.deltaTime;
            if (sr != null)
            {
                var c = sr.color;
                c.a = Mathf.Lerp(startA, targetAlpha, Mathf.Clamp01(elapsed / duration));
                sr.color = c;
            }
            yield return null;
        }
        if (haloGO != null && sr != null)
        {
            var c = sr.color; c.a = targetAlpha; sr.color = c;
        }
    }

    private IEnumerator FadeOutHalo(GameObject haloGO)
    {
        if (haloGO == null) yield break;
        var sr = haloGO.GetComponent<SpriteRenderer>();
        if (sr == null) { haloGO.SetActive(false); yield break; }

        float elapsed = 0f;
        const float duration = 4f * HALO_CLOCK_QUARTER_TIME;
        Color startCol = sr.color;

        while (elapsed < duration && haloGO != null)
        {
            elapsed += Time.deltaTime;
            if (sr != null)
                sr.color = new Color(startCol.r, startCol.g, startCol.b,
                                     Mathf.Lerp(startCol.a, 0f, elapsed / duration));
            yield return null;
        }
        if (haloGO != null) haloGO.SetActive(false);
    }

    private IEnumerator FadeOutAndDisableHalo(GameObject haloGO, float duration)
    {
        if (haloGO == null) yield break;
        var sr = haloGO.GetComponent<SpriteRenderer>();
        if (sr == null) { haloGO.SetActive(false); yield break; }

        float elapsed = 0f;
        float startA  = sr.color.a;
        while (elapsed < duration && haloGO != null)
        {
            elapsed += Time.deltaTime;
            if (sr != null) { var c = sr.color; c.a = Mathf.Lerp(startA, 0f, elapsed / duration); sr.color = c; }
            yield return null;
        }
        if (haloGO != null) haloGO.SetActive(false);
    }


    private IEnumerator SwordRainVerticalBeams()
    {
        yield return new WaitForSeconds(3f);

        GameObject tpl = _attackCommands.FsmVariables.FindFsmGameObject("Ascend Beam")?.Value;
        if (tpl == null) tpl = GameObject.Find("Ascend Beam");
        if (tpl == null) { var bsT = _beamsweeper?.transform.Find("Eye Beam"); if (bsT != null) tpl = bsT.gameObject; }
        if (tpl == null) tpl = GameObject.Find("Eye Beam");
        if (tpl == null) { Log("SwordRainVerticalBeams: ningún template encontrado."); yield break; }

        _srLA = Object.Instantiate(tpl, Vector3.zero, Quaternion.Euler(0,0,90f)); _srLA.name = "SR LA"; _srLA.SetActive(true);
        _srRA = Object.Instantiate(tpl, Vector3.zero, Quaternion.Euler(0,0,90f)); _srRA.name = "SR RA"; _srRA.SetActive(true);
        _srLB = Object.Instantiate(tpl, Vector3.zero, Quaternion.Euler(0,0,90f)); _srLB.name = "SR LB"; _srLB.SetActive(true);
        _srRB = Object.Instantiate(tpl, Vector3.zero, Quaternion.Euler(0,0,90f)); _srRB.name = "SR RB"; _srRB.SetActive(true);
        _srLC = Object.Instantiate(tpl, Vector3.zero, Quaternion.Euler(0,0,90f)); _srLC.name = "SR LC"; _srLC.SetActive(true);
        _srRC = Object.Instantiate(tpl, Vector3.zero, Quaternion.Euler(0,0,90f)); _srRC.name = "SR RC"; _srRC.SetActive(true);
        _srLD = Object.Instantiate(tpl, Vector3.zero, Quaternion.Euler(0,0,90f)); _srLD.name = "SR LD"; _srLD.SetActive(true);
        _srRD = Object.Instantiate(tpl, Vector3.zero, Quaternion.Euler(0,0,90f)); _srRD.name = "SR RD"; _srRD.SetActive(true);
        _srLE = Object.Instantiate(tpl, Vector3.zero, Quaternion.Euler(0,0,90f)); _srLE.name = "SR LE"; _srLE.SetActive(true);
        _srRE = Object.Instantiate(tpl, Vector3.zero, Quaternion.Euler(0,0,90f)); _srRE.name = "SR RE"; _srRE.SetActive(true);

        yield return null;

        var allBeams = new GameObject[] { _srLA, _srRA, _srLB, _srRB, _srLC, _srRC, _srLD, _srRD, _srLE, _srRE };
        var allFsms  = System.Array.ConvertAll(allBeams, b => b?.LocateMyFSM("Control"));

        while (!swordRainEndSet && _hm.hp > 0)
        {
            float bx = base.gameObject.transform.position.x;

            void Pos(GameObject b, float offset) {
                if (b != null) { b.transform.position = new Vector3(offset, SWORDRAIN_BEAM_Y, 0f); b.transform.rotation = Quaternion.Euler(0,0,90f); }
            }
            Pos(_srLA, bx - SWORDRAIN_BEAM_OFF_A); Pos(_srRA, bx + SWORDRAIN_BEAM_OFF_A);
            Pos(_srLB, bx - SWORDRAIN_BEAM_OFF_B); Pos(_srRB, bx + SWORDRAIN_BEAM_OFF_B);
            Pos(_srLC, bx - SWORDRAIN_BEAM_OFF_C); Pos(_srRC, bx + SWORDRAIN_BEAM_OFF_C);
            Pos(_srLD, bx - SWORDRAIN_BEAM_OFF_D); Pos(_srRD, bx + SWORDRAIN_BEAM_OFF_D);
            Pos(_srLE, bx - SWORDRAIN_BEAM_OFF_E); Pos(_srRE, bx + SWORDRAIN_BEAM_OFF_E);

            allFsms[8]?.SendEvent("ANTIC"); allFsms[9]?.SendEvent("ANTIC");
            yield return new WaitForSeconds(0.08f);
            allFsms[6]?.SendEvent("ANTIC"); allFsms[7]?.SendEvent("ANTIC");
            yield return new WaitForSeconds(0.08f);
            allFsms[4]?.SendEvent("ANTIC"); allFsms[5]?.SendEvent("ANTIC");
            yield return new WaitForSeconds(0.08f);
            allFsms[2]?.SendEvent("ANTIC"); allFsms[3]?.SendEvent("ANTIC");
            yield return new WaitForSeconds(0.08f);
            allFsms[0]?.SendEvent("ANTIC"); allFsms[1]?.SendEvent("ANTIC");
            yield return new WaitForSeconds(0.22f);

            bx = base.gameObject.transform.position.x;
            Pos(_srLA, bx - SWORDRAIN_BEAM_OFF_A); Pos(_srRA, bx + SWORDRAIN_BEAM_OFF_A);
            Pos(_srLB, bx - SWORDRAIN_BEAM_OFF_B); Pos(_srRB, bx + SWORDRAIN_BEAM_OFF_B);
            Pos(_srLC, bx - SWORDRAIN_BEAM_OFF_C); Pos(_srRC, bx + SWORDRAIN_BEAM_OFF_C);
            Pos(_srLD, bx - SWORDRAIN_BEAM_OFF_D); Pos(_srRD, bx + SWORDRAIN_BEAM_OFF_D);
            Pos(_srLE, bx - SWORDRAIN_BEAM_OFF_E); Pos(_srRE, bx + SWORDRAIN_BEAM_OFF_E);

            foreach (var f in allFsms) f?.SendEvent("FIRE");
            yield return new WaitForSeconds(0.5f);
            foreach (var f in allFsms) f?.SendEvent("END");
            yield return new WaitForSeconds(0.6f);
        }
    }

    private IEnumerator FatalBeamSweepLoop()
    {
        yield return new WaitForSeconds(FATAL_SWEEP_FIRST_DELAY);
        while (swordRainSpikesSet && !swordRainEndSet && _hm.hp > 0)
        {
            yield return StartCoroutine(DoFatalBeamSweep());
            yield return new WaitForSeconds(FATAL_SWEEP_INTERVAL);
        }
    }

    private IEnumerator SwordRainGroundLoop()
    {
        while (swordRainSpikesSet && !swordRainEndSet && _hm.hp > 0)
        {
            try
            {
                foreach (var rb in Object.FindObjectsOfType<Rigidbody2D>())
                {
                    if (rb == null) continue;
                    float py = rb.transform.position.y;
                    if (py < 10f || py > 30f) continue;
                    var n = rb.gameObject.name;
                    if (!n.Contains("Nail") && !n.Contains("nail") &&
                        !n.Contains("Sword") && !n.Contains("sword")) continue;
                    if (rb.gravityScale != 0f) rb.gravityScale = 0f;
                }
            }
            catch { }
            yield return new WaitForSeconds(0.04f);
        }
    }

    private IEnumerator DoFatalBeamSweep()
    {
        if (_eyeBeamTemplate == null) yield break;
        float safeX = _knight.transform.position.x;
        const float arenaL = 36f, arenaR = 84f;
        var beams = new List<GameObject>();
        var fsms  = new List<PlayMakerFSM>();
        for (float x = arenaL; x <= arenaR; x += FATAL_SWEEP_STEP)
        {
            if (Mathf.Abs(x - safeX) < FATAL_SWEEP_SAFE_ZONE) continue;
            var b = Object.Instantiate(_eyeBeamTemplate,
                        new Vector3(x, SWORDRAIN_BEAM_Y + 1f, 0f),
                        Quaternion.Euler(0f, 0f, 90f));
            b.name = "Fatal Sweep Beam"; b.SetActive(true);
            var dmg = b.GetComponent<DamageHero>(); if (dmg != null) dmg.shadowDashHazard = true;
            var f = b.LocateMyFSM("Control");
            beams.Add(b); fsms.Add(f);
            f?.SendEvent("ANTIC");
        }
        yield return new WaitForSeconds(FATAL_SWEEP_ANTIC_TIME);
        foreach (var f in fsms) f?.SendEvent("FIRE");
        yield return new WaitForSeconds(FATAL_SWEEP_FIRE_TIME);
        foreach (var f in fsms) f?.SendEvent("END");
        yield return new WaitForSeconds(0.35f);
        foreach (var b in beams) { if (b != null) Object.Destroy(b); }
    }

    private IEnumerator FadeAndDestroyBeam(GameObject beam)
    {
        if (beam == null) yield break;
        var fsm = beam.LocateMyFSM("Control");
        fsm?.SendEvent("END");
        var sr = beam.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            float t = 0f; Color orig = sr.color;
            while (t < 0.5f && beam != null)
            { t += Time.deltaTime; if (sr != null) sr.color = new Color(orig.r, orig.g, orig.b, Mathf.Lerp(orig.a, 0f, t / 0.5f)); yield return null; }
        }
        else yield return new WaitForSeconds(0.5f);
        if (beam != null) Object.Destroy(beam);
    }


    private void ShowConvo(string msg)
    {
        try
        {
            var displayGO = HutongGames.PlayMaker.FsmVariables.GlobalVariables
                .GetFsmGameObject("Enemy Dream Msg")?.Value;
            if (displayGO == null) return;
            var fsm = PlayMakerFSM.FindFsmOnGameObject(displayGO, "Display");
            if (fsm == null) return;
            fsm.FsmVariables.GetFsmString("Convo Title").Value = "Radiance";
            fsm.FsmVariables.GetFsmInt("Convo Amount").Value   = 1;
            fsm.SendEvent("DISPLAY ENEMY DREAM");
            StartCoroutine(OverrideDreamText(displayGO, msg));
        }
        catch { }
    }

    private IEnumerator OverrideDreamText(GameObject displayGO, string msg)
    {
        for (int i = 0; i < 5; i++) yield return null;
        try
        {
            foreach (var t in displayGO.GetComponentsInChildren<TMPro.TextMeshPro>(true))     t.text = msg;
            foreach (var t in displayGO.GetComponentsInChildren<TMPro.TextMeshProUGUI>(true)) t.text = msg;
        }
        catch { }
    }


    private void RedirectWallVertical()
    {
        var wall = _attackCommands.FsmVariables.FindFsmGameObject("Attack Obj").Value;
        if (wall == null) return;
        var wallFSM = wall.LocateMyFSM("Control");
        if (wallFSM == null) return;
        wall.transform.SetPosition2D(Random.Range(48f, 74f), 26f);
        var tween = wallFSM.GetAction<iTweenMoveBy>("Tween", 0);
        if (tween != null) { tween.vector = new Vector3(0f, 65f, 0f); tween.speed = 20f; tween.easeType = iTween.EaseType.easeInQuad; }
    }

    private IEnumerator OrbCrossBeam(Vector3 pos, GameObject template)
    {
        if (template == null) yield break;
        bool isPlat  = _attackChoices.FsmVariables.GetFsmInt("Arena").Value == 2 && !finalDanceExtraAttackConfigured;
        bool isFinal = finalDanceExtraAttackConfigured;
        float anticTime = isPlat ? ORB_CROSS_ANTIC_TIME_PLAT : ORB_CROSS_ANTIC_TIME_OTHER;
        float fireTime  = isPlat  ? ORB_CROSS_FIRE_TIME_PLAT
                        : isFinal ? ORB_CROSS_FIRE_TIME_FINAL
                                  : ORB_CROSS_FIRE_TIME_OTHER;
        bool useX = isFinal ? ORB_CROSS_X_FINAL : isPlat ? ORB_CROSS_X_PLAT : ORB_CROSS_X_GROUND;
        float[] angles = useX ? new float[] { 45f, 135f, 225f, 315f } : new float[] { 0f, 90f, 180f, 270f };
        var beams = new List<GameObject>(); var fsms = new List<PlayMakerFSM>();
        foreach (var a in angles) { var b = Object.Instantiate(template, pos, Quaternion.Euler(0f, 0f, a)); b.SetActive(true); beams.Add(b); fsms.Add(b.LocateMyFSM("Control")); }
        foreach (var f in fsms) f?.SendEvent("ANTIC");
        yield return new WaitForSeconds(anticTime);
        foreach (var f in fsms) f?.SendEvent("FIRE");
        yield return new WaitForSeconds(fireTime);
        foreach (var f in fsms) f?.SendEvent("END");
        yield return new WaitForSeconds(0.6f);
        foreach (var b in beams) { if (b != null) Object.Destroy(b); }
    }

    private IEnumerator ApplySpiralVelocity(GameObject orb)
    {
        yield return null;
        if (orb == null) yield break;
        var rb = orb.GetComponent<Rigidbody2D>();
        if (rb == null) yield break;
        float angleDeg = (_spiralOrbIndex % 12) * 30f; _spiralOrbIndex++;
        float rad = angleDeg * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad), sin = Mathf.Sin(rad);
        var vel = rb.velocity;
        rb.velocity = new Vector2(vel.x * cos - vel.y * sin, vel.x * sin + vel.y * cos);
    }

    private IEnumerator FinalStaticBeam()
    {
        if (!FINAL_STATIC_BEAM_ENABLED) yield break;
        if (ascendBeam == null) yield break;
        const float maxY = 157.5f, minY = 149.0f, fixedX = 63f, stepY = 0.5f;

        var bR = Object.Instantiate(ascendBeam, new Vector3(fixedX, maxY, 0f), Quaternion.Euler(0f, 0f, 0f));
        bR.name = "Aura Mirror Beam R"; bR.SetActive(true); var fsmR = bR.LocateMyFSM("Control");
        var bL = Object.Instantiate(ascendBeam, new Vector3(fixedX, maxY, 0f), Quaternion.Euler(0f, 0f, 180f));
        bL.name = "Aura Mirror Beam L"; bL.SetActive(true); var fsmL = bL.LocateMyFSM("Control");

        yield return new WaitForSeconds(2f);
        float currentY = maxY; bool goingDown = true;

        while (bR != null && bL != null && _hm != null && _hm.hp > 0)
        {
            float anticTime, fireTime, pauseTime;
            if (_hm.hp > FINAL_ORB_ACCEL_HP_1)
            { anticTime = 0.10f; fireTime = 0.10f; pauseTime = 0.25f; }
            else if (_hm.hp > FINAL_ORB_ACCEL_HP_2)
            { anticTime = 0.08f; fireTime = 0.08f; pauseTime = 0.12f; }
            else if (_hm.hp > FINAL_ORB_ACCEL_HP_3)
            { anticTime = 0.06f; fireTime = 0.06f; pauseTime = 0.06f; }
            else
            { anticTime = 0.03f; fireTime = 0.03f; pauseTime = 0.02f; }

            bR.transform.position = new Vector3(fixedX, currentY, 0f); bR.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            bL.transform.position = new Vector3(fixedX, currentY, 0f); bL.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            fsmR?.SendEvent("ANTIC"); fsmL?.SendEvent("ANTIC");
            yield return new WaitForSeconds(anticTime);
            bR.transform.position = new Vector3(fixedX, currentY, 0f); bL.transform.position = new Vector3(fixedX, currentY, 0f);
            fsmR?.SendEvent("FIRE"); fsmL?.SendEvent("FIRE");
            yield return new WaitForSeconds(fireTime);
            fsmR?.SendEvent("END"); fsmL?.SendEvent("END");
            yield return new WaitForSeconds(pauseTime);

            if (goingDown) { currentY -= stepY; if (currentY <= minY) { currentY = minY; goingDown = false; } }
            else           { currentY += stepY; if (currentY >= maxY) { currentY = maxY; goingDown = true;  } }
        }
        if (bR != null) Object.Destroy(bR);
        if (bL != null) Object.Destroy(bL);
    }


    private IEnumerator IndependentBeamLoop(GameObject beam, float xOffset, float startDelay)
    {
        if (startDelay > 0f) yield return new WaitForSeconds(startDelay);
        var fsm = beam?.LocateMyFSM("Control");
        if (fsm == null) yield break;

        while (beam != null && beam.activeSelf && _knight.transform.GetPositionY() <= 152f)
        {
            if (ascendBeam != null)
                beam.transform.position = new Vector3(
                    ascendBeam.transform.position.x + xOffset,
                    ascendBeam.transform.position.y,
                    ascendBeam.transform.position.z);
            beam.transform.rotation = ascendBeam != null ? ascendBeam.transform.rotation : beam.transform.rotation;
            fsm.SendEvent("ANTIC");
            yield return new WaitForSeconds(0.3f);
            if (ascendBeam != null)
                beam.transform.position = new Vector3(
                    ascendBeam.transform.position.x + xOffset,
                    ascendBeam.transform.position.y,
                    ascendBeam.transform.position.z);
            fsm.SendEvent("FIRE");
            yield return new WaitForSeconds(0.3f);
            fsm.SendEvent("END");
            yield return new WaitForSeconds(Random.Range(0.2f, 1f));
        }
    }

    private IEnumerator SpawnAscendBeams()
    {
        float waited = 0f;
        while (ascendBeam == null && waited < 15f)
        { ascendBeam = GameObject.Find("Ascend Beam"); waited += Time.deltaTime; yield return null; }
        if (ascendBeam == null) { Log("SpawnAscendBeams: sin ascendBeam."); yield break; }

        yield return new WaitForSeconds(2f);

        float[] xOff = { 0f, -6f, 6f, -12f, 12f };
        float[] del  = { 0f, 0.1f, 0.2f, 0.05f, 0.15f };
        var beams = new GameObject[xOff.Length];

        for (int i = 0; i < xOff.Length; i++)
        {
            beams[i] = Object.Instantiate(ascendBeam);
            beams[i].name = "Ascend Beam Clone " + i;
            beams[i].SetActive(true);
            _ascendBeams.Add(beams[i]);
            StartCoroutine(IndependentBeamLoop(beams[i], xOff[i], del[i]));
        }
        ascendBeam.SetActive(false);

        yield return new WaitForSeconds(0f);
        //ShowConvo(TEXT_AFTER_FINAL_MESSAGE);

        bool d1 = false, d3 = false;
        while (_hm != null && _hm.hp > 0 && !finalDanceExtraAttackConfigured
               && _knight.transform.GetPositionY() < 152f)
        {
            float ky = _knight.transform.GetPositionY();
            if (!d1 && ky >= CLIMB_DEACT_Y1)
            {
                d1 = true;
                if (beams[3] != null) beams[3].SetActive(false);
                if (beams[4] != null) beams[4].SetActive(false);
            }
            if (!d3 && ky >= CLIMB_DEACT_Y3)
            {
                d3 = true;
                if (beams[1] != null) beams[1].SetActive(false);
                if (beams[2] != null) beams[2].SetActive(false);
                if (beams[0] != null) beams[0].SetActive(false);
                yield return null;
                if (beams[0] != null)
                {
                    var dmgC = beams[0].GetComponent<DamageHero>();
                    if (dmgC != null) dmgC.enabled = false;
                    beams[0].SetActive(true);
                }
                StartCoroutine(ClimbFinalPattern(beams[0], null, null));
                break;
            }
            yield return null;
        }
    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    // CLIMB FINAL PATTERNa
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    private const float CLIMB_GHOST_SPEED_MULT   = 1f;
    private const float CLIMB_SWEEP_ACTIVE_SPEED = 70f;
    private const float CLIMB_SWEEP_MIN_FACTOR   = 1f;
    private const float CLIMB_SWEEP_MAX_FACTOR   = 1.5f;

    private IEnumerator ClimbFinalPattern(GameObject center, GameObject flankL, GameObject flankR)
    {
        if (center == null) yield break;

        var dmgCenter = center.GetComponent<DamageHero>();
        if (dmgCenter != null) dmgCenter.enabled = false;
        var srCenter = center.GetComponent<SpriteRenderer>();
        if (srCenter != null) srCenter.enabled = false;
        foreach (var sr in center.GetComponentsInChildren<SpriteRenderer>())
            if (sr != null) sr.enabled = false;
        var fsmC = center.LocateMyFSM("Control");
        fsmC?.SendEvent("END");
        if (fsmC != null) fsmC.enabled = false;

        Vector3 centerFixedPos = center.transform.position;

        if (flankL != null) { flankL.SetActive(true); flankL.transform.rotation = Quaternion.Euler(0f, 0f, -90f); }
        if (flankR != null) { flankR.SetActive(true); flankR.transform.rotation = Quaternion.Euler(0f, 0f, -90f); }
        var fsmFL = flankL?.LocateMyFSM("Control");
        var fsmFR = flankR?.LocateMyFSM("Control");

        float blinkTime = 0f, blinkElapsed = 0f;
        while (blinkElapsed < blinkTime && _hm != null && _hm.hp > 0)
        {
            if (flankL != null) flankL.transform.position = new Vector3(centerFixedPos.x - 18f, centerFixedPos.y, 0f);
            if (flankR != null) flankR.transform.position = new Vector3(centerFixedPos.x + 18f, centerFixedPos.y, 0f);
            fsmFL?.SendEvent("FIRE"); fsmFR?.SendEvent("FIRE");
            yield return new WaitForSeconds(CLIMB_FLANKS_FIRE_TIME);
            fsmFL?.SendEvent("END");  fsmFR?.SendEvent("END");
            yield return new WaitForSeconds(CLIMB_FLANKS_FIRE_TIME);
            blinkElapsed += CLIMB_FLANKS_FIRE_TIME * 1f;
        }
        if (flankL != null) flankL.SetActive(false);
        if (flankR != null) flankR.SetActive(false);

        center.transform.position = centerFixedPos;
        Vector3 toKnight  = (_knight.transform.position - centerFixedPos).normalized;
        float targetAngle = Mathf.Atan2(toKnight.y, toKnight.x) * Mathf.Rad2Deg;
        center.transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
        yield return new WaitForSeconds(0.0f);

        while (_hm != null && _hm.hp > 0 && !finalDanceExtraAttackConfigured
               && _knight.transform.GetPositionY() <= 152f)
        {
            center.transform.position = centerFixedPos;
            var tpl = ascendBeam != null ? ascendBeam : center;
            float ghostSweepSpeed = CLIMB_SWEEP_ACTIVE_SPEED * CLIMB_GHOST_SPEED_MULT;

            float gAngL = targetAngle - CLIMB_SWEEP_ANGLE_START;
            float gAngR = targetAngle + CLIMB_SWEEP_ANGLE_START;
            var ghostL = Object.Instantiate(tpl, centerFixedPos, Quaternion.Euler(0f, 0f, gAngL));
            ghostL.name = "Climb Ghost L"; ghostL.SetActive(true);
            var ghostR = Object.Instantiate(tpl, centerFixedPos, Quaternion.Euler(0f, 0f, gAngR));
            ghostR.name = "Climb Ghost R"; ghostR.SetActive(true);
            var dmgGL = ghostL.GetComponent<DamageHero>(); if (dmgGL != null) dmgGL.enabled = false;
            var dmgGR = ghostR.GetComponent<DamageHero>(); if (dmgGR != null) dmgGR.enabled = false;
            var fsmGL = ghostL.LocateMyFSM("Control");
            var fsmGR = ghostR.LocateMyFSM("Control");
            yield return null;
            fsmGL?.SendEvent("ANTIC"); fsmGR?.SendEvent("ANTIC");

            while (_hm != null && _hm.hp > 0 && _knight.transform.GetPositionY() <= 151f)
            {
                bool doneL = Mathf.Abs(Mathf.DeltaAngle(gAngL, targetAngle)) < 1.5f;
                bool doneR = Mathf.Abs(Mathf.DeltaAngle(gAngR, targetAngle)) < 1.5f;
                if (doneL && doneR) break;

                float remL = Mathf.Abs(Mathf.DeltaAngle(gAngL, targetAngle));
                float remR = Mathf.Abs(Mathf.DeltaAngle(gAngR, targetAngle));
                float sinL = Mathf.Clamp(Mathf.Sin(remL * Mathf.Deg2Rad), 0.01f, 1f);
                float sinR = Mathf.Clamp(Mathf.Sin(remR * Mathf.Deg2Rad), 0.01f, 1f);
                float factorL = Mathf.Clamp(1f / sinL, CLIMB_SWEEP_MIN_FACTOR, CLIMB_SWEEP_MAX_FACTOR);
                float factorR = Mathf.Clamp(1f / sinR, CLIMB_SWEEP_MIN_FACTOR, CLIMB_SWEEP_MAX_FACTOR);
                float stepL = ghostSweepSpeed * factorL * Time.deltaTime;
                float stepR = ghostSweepSpeed * factorR * Time.deltaTime;

                if (!doneL) gAngL = Mathf.MoveTowardsAngle(gAngL, targetAngle, stepL);
                if (!doneR) gAngR = Mathf.MoveTowardsAngle(gAngR, targetAngle, stepR);
                if (ghostL != null) { ghostL.transform.position = centerFixedPos; ghostL.transform.rotation = Quaternion.Euler(0f, 0f, gAngL); }
                if (ghostR != null) { ghostR.transform.position = centerFixedPos; ghostR.transform.rotation = Quaternion.Euler(0f, 0f, gAngR); }
                center.transform.position = centerFixedPos;
                yield return null;
            }
            fsmGL?.SendEvent("END"); fsmGR?.SendEvent("END");
            if (ghostL != null) Object.Destroy(ghostL);
            if (ghostR != null) Object.Destroy(ghostR);

            if (_knight.transform.GetPositionY() > 151f) yield break;

            yield return new WaitForSeconds(0.001f);

            float angL = targetAngle - CLIMB_SWEEP_ANGLE_START;
            float angR = targetAngle + CLIMB_SWEEP_ANGLE_START;
            var sweepL = Object.Instantiate(tpl, centerFixedPos, Quaternion.Euler(0f, 0f, angL));
            sweepL.name = "Climb Sweep L"; sweepL.SetActive(true);
            var sweepR = Object.Instantiate(tpl, centerFixedPos, Quaternion.Euler(0f, 0f, angR));
            sweepR.name = "Climb Sweep R"; sweepR.SetActive(true);
            var fsmSL = sweepL.LocateMyFSM("Control");
            var fsmSR = sweepR.LocateMyFSM("Control");
            yield return null;
            fsmSL?.SendEvent("ANTIC"); fsmSR?.SendEvent("ANTIC");
            yield return new WaitForSeconds(0.1f);
            fsmSL?.SendEvent("FIRE"); fsmSR?.SendEvent("FIRE");

            while (_hm != null && _hm.hp > 0 && _knight.transform.GetPositionY() <= 151f)
            {
                bool doneL = Mathf.Abs(Mathf.DeltaAngle(angL, targetAngle)) < 1.5f;
                bool doneR = Mathf.Abs(Mathf.DeltaAngle(angR, targetAngle)) < 1.5f;
                if (doneL && doneR) break;

                float remL = Mathf.Abs(Mathf.DeltaAngle(angL, targetAngle));
                float remR = Mathf.Abs(Mathf.DeltaAngle(angR, targetAngle));
                float sinL = Mathf.Clamp(Mathf.Sin(remL * Mathf.Deg2Rad), 0.01f, 1f);
                float sinR = Mathf.Clamp(Mathf.Sin(remR * Mathf.Deg2Rad), 0.01f, 1f);
                float factorL = Mathf.Clamp(1f / sinL, CLIMB_SWEEP_MIN_FACTOR, CLIMB_SWEEP_MAX_FACTOR);
                float factorR = Mathf.Clamp(1f / sinR, CLIMB_SWEEP_MIN_FACTOR, CLIMB_SWEEP_MAX_FACTOR);
                float stepL = CLIMB_SWEEP_ACTIVE_SPEED * factorL * Time.deltaTime;
                float stepR = CLIMB_SWEEP_ACTIVE_SPEED * factorR * Time.deltaTime;

                if (!doneL) angL = Mathf.MoveTowardsAngle(angL, targetAngle, stepR);
                if (!doneR) angR = Mathf.MoveTowardsAngle(angR, targetAngle, stepL);
                if (sweepL != null) { sweepL.transform.position = centerFixedPos; sweepL.transform.rotation = Quaternion.Euler(0f, 0f, angL); }
                if (sweepR != null) { sweepR.transform.position = centerFixedPos; sweepR.transform.rotation = Quaternion.Euler(0f, 0f, angR); }
                center.transform.position = centerFixedPos;
                yield return null;
            }
            fsmSL?.SendEvent("END"); fsmSR?.SendEvent("END");
            if (sweepL != null) Object.Destroy(sweepL);
            if (sweepR != null) Object.Destroy(sweepR);

            if (_knight.transform.GetPositionY() > 151f) yield break;

            center.transform.position = centerFixedPos;
            toKnight    = (_knight.transform.position - centerFixedPos).normalized;
            targetAngle = Mathf.Atan2(toKnight.y, toKnight.x) * Mathf.Rad2Deg;
            center.transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator AscendCameraZoom()
    {
        const float baseZoom = 0.85f, peakZoom = 0.83f;
        while (!finalDanceExtraAttackConfigured && _hm != null && _hm.hp > 0)
        {
            yield return StartCoroutine(LerpZoom(baseZoom, peakZoom, 0.08f));
            yield return new WaitForSeconds(0.15f);
            yield return StartCoroutine(LerpZoom(peakZoom, baseZoom, 0.22f));
            yield return new WaitForSeconds(0.35f);
        }
        GameCameras.instance.tk2dCam.ZoomFactor = baseZoom;
    }

    private IEnumerator LerpZoom(float from, float to, float duration)
    {
        float e = 0f;
        while (e < duration)
        { e += Time.deltaTime; GameCameras.instance.tk2dCam.ZoomFactor = Mathf.Lerp(from, to, e / duration); yield return null; }
        GameCameras.instance.tk2dCam.ZoomFactor = to;
    }

    private IEnumerator GroundPhaseNailWalls()
    {
        yield return new WaitForSeconds(20f);
        while (_attackChoices.FsmVariables.GetFsmInt("Arena").Value < 2 && _hm.hp > 0 && base.gameObject.transform.position.y < 40f)
        {
            yield return new WaitUntil(() =>
                _attackChoices.ActiveStateName == "A1 Choice" ||
                _attackChoices.ActiveStateName == "Orb Wait"  ||
                _attackChoices.ActiveStateName == "Orb Recover");
            var ct = _attackChoices.GetAction<SendEventByName>("Nail L Sweep", 0).eventTarget;
            FsmutilExt.InsertAction(_attackCommands, "A1 Choice", (FsmStateAction)new SendEventByName { eventTarget = ct, sendEvent = "COMB L", delay = 0f, everyFrame = false }, 0);
            yield return new WaitForSeconds(0.5f);
            FsmutilExt.InsertAction(_attackCommands, "A1 Choice", (FsmStateAction)new SendEventByName { eventTarget = ct, sendEvent = "COMB R", delay = 0f, everyFrame = false }, 0);
            yield return new WaitForSeconds(18f);
        }
    }

    private IEnumerator LaserColumnsLoop()
    {
        yield return new WaitForSeconds(8f);
        while (_attackChoices.FsmVariables.GetFsmInt("Arena").Value == 2 && _hm.hp > 0 && base.gameObject.transform.position.y < 150f)
        { yield return StartCoroutine(DoLaserColumns()); yield return new WaitForSeconds(12f); }
    }

    private IEnumerator DoLaserColumns()
    {
        if (_eyeBeamTemplate == null) yield break;
        const float al = 40f, ar = 80f;
        float safe = Random.Range(al + 3f, ar - 3f);
        var beams = new List<GameObject>();
        for (float x = al; x <= ar; x += 2f)
        {
            if (Mathf.Abs(x - safe) < 2.5f) continue;
            var b = Object.Instantiate(_eyeBeamTemplate, new Vector3(x, 55f, 0f), Quaternion.Euler(0f, 0f, -90f));
            b.SetActive(true);
            var dmg = b.GetComponent<DamageHero>(); if (dmg != null) dmg.shadowDashHazard = true;
            b.LocateMyFSM("Control")?.SendEvent("ANTIC"); beams.Add(b);
        }
        float dist = Mathf.Abs(HeroController.instance.transform.position.x - safe);
        float anticTime = Mathf.Clamp(dist / HeroController.instance.RUN_SPEED + 0.35f, 0.8f, 2.5f);
        yield return new WaitForSeconds(anticTime);
        foreach (var b in beams) b?.LocateMyFSM("Control")?.SendEvent("FIRE");
        yield return new WaitForSeconds(0.6f);
        foreach (var b in beams) { if (b != null) Object.Destroy(b); }
    }

    private IEnumerator DiagonalBeamSweepLoop()
    {
        yield return new WaitForSeconds(14f);
        while (_attackChoices.FsmVariables.GetFsmInt("Arena").Value == 2 && _hm.hp > 0 && base.gameObject.transform.position.y < 150f)
        { yield return StartCoroutine(DoDiagonalBeamSweep()); yield return new WaitForSeconds(18f); }
    }

    private IEnumerator DoDiagonalBeamSweep()
    {
        if (_eyeBeamTemplate == null) yield break;
        bool sr = Random.value > 0.5f;
        float sx = sr ? 38f : 82f, ex = sr ? 82f : 38f;
        const float sy = 28f, ey = 55f;
        var b = Object.Instantiate(_eyeBeamTemplate, new Vector3(sx, sy, 0f), Quaternion.Euler(0f, 0f, sr ? 45f : -45f));
        b.SetActive(true);
        var dmg = b.GetComponent<DamageHero>(); if (dmg != null) dmg.shadowDashHazard = true;
        var fsm = b.LocateMyFSM("Control");
        fsm?.SendEvent("ANTIC"); yield return new WaitForSeconds(0.9f); fsm?.SendEvent("FIRE");
        float t = 0f, sd = 2.2f;
        while (t < sd && b != null)
        { t += Time.deltaTime; float p = t / sd; b.transform.position = new Vector3(Mathf.Lerp(sx, ex, p), Mathf.Lerp(sy, ey, p), 0f); yield return null; }
        if (b != null) { fsm?.SendEvent("END"); yield return new WaitForSeconds(0.4f); Object.Destroy(b); }
    }

    private IEnumerator Arena2BeamBurstLoop()
    {
        yield return new WaitForSeconds(4f);
        FsmEventTarget ebT = null;
        try { ebT = _attackCommands.GetAction<SendEventByName>("EB 1", 9).eventTarget; } catch { yield break; }
        if (ebT == null) yield break;
        string[] sts = { "EB 1","EB 2","EB 3","EB 4","EB 5","EB 6","EB 7","EB 8","EB 9" };
        foreach (var st in sts)
        {
            try
            {
                FsmutilExt.InsertAction(_attackCommands, st, new SendEventByName { eventTarget = ebT, sendEvent = "FIRE", delay = 0.35f, everyFrame = false }, 0);
                FsmutilExt.InsertAction(_attackCommands, st, new SendEventByName { eventTarget = ebT, sendEvent = "FIRE", delay = 0.70f, everyFrame = false }, 0);
                FsmutilExt.InsertAction(_attackCommands, st, new SendEventByName { eventTarget = ebT, sendEvent = "FIRE", delay = 1.05f, everyFrame = false }, 0);
            }
            catch { }
        }
        yield return null;
    }

    private const float SWORD_SPEED_MULT = 5f;

    private IEnumerator SwordRainPlatLoop()
    {
        yield return new WaitForSeconds(2f);
        while (_attackChoices.FsmVariables.GetFsmInt("Arena").Value == 2 && _hm.hp > 0 && base.gameObject.transform.position.y < 150f)
        {
            foreach (var rb in Object.FindObjectsOfType<Rigidbody2D>())
            {
                if (rb == null) continue;
                float py = rb.transform.position.y;
                if (py < 28f || py > 62f) continue;
                var n = rb.gameObject.name;
                if (!n.Contains("Nail") && !n.Contains("nail") && !n.Contains("Sword") && !n.Contains("sword")) continue;
                if (rb.gravityScale != 0f) rb.gravityScale = 0f;
                if (rb.velocity.y < -0.5f) rb.velocity = new Vector2(rb.velocity.x, Mathf.Abs(rb.velocity.y) * SWORD_SPEED_MULT);
            }
            yield return new WaitForSeconds(0.018f);
        }
    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    // COLOR DE ESCENA
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    private struct BgRenderers
    {
        public SpriteRenderer sky;
        public List<SpriteRenderer> pillars;
        public List<SpriteRenderer> hazes;
        public List<SpriteRenderer> clouds;
        public List<SpriteRenderer> rays;
    }
    private BgRenderers _bgR;
    private bool _bgRCollected = false;

    private void CollectBgRenderers()
    {
        if (_bgRCollected) return;
        _bgRCollected = true;
        _bgR.pillars = new List<SpriteRenderer>();
        _bgR.hazes   = new List<SpriteRenderer>();
        _bgR.clouds  = new List<SpriteRenderer>();
        _bgR.rays    = new List<SpriteRenderer>();
        try
        {
            var arena = GameObject.Find("GG_Arena_Prefab");
            if (arena == null) return;
            var bg = arena.transform.Find("BG")?.gameObject;
            if (bg == null) return;
            _bgR.sky = bg.transform.Find("sky_colour")?.GetComponent<SpriteRenderer>();
            foreach (Transform child in bg.transform)
            {
                var nm = child.name.ToLower();
                if (nm.Contains("haze") || nm.Contains("fog"))
                {
                    var s = child.GetComponent<SpriteRenderer>();
                    if (s != null) _bgR.hazes.Add(s);
                }
                else if (child.name.Contains("GG_scenery_0004_17"))
                {
                    var s = child.GetComponent<SpriteRenderer>();
                    if (s != null) _bgR.clouds.Add(s);
                }
                else if (nm.Contains("pillar") || nm.Contains("column"))
                {
                    var s = child.GetComponent<SpriteRenderer>();
                    if (s != null) _bgR.pillars.Add(s);
                    foreach (Transform pChild in child)
                    { var cs = pChild.GetComponent<SpriteRenderer>(); if (cs != null) _bgR.pillars.Add(cs); }
                }
                else if (nm.Contains("ray"))
                {
                    foreach (Transform rChild in child)
                    { var cs = rChild.GetComponent<SpriteRenderer>(); if (cs != null) _bgR.rays.Add(cs); }
                    var s = child.GetComponent<SpriteRenderer>();
                    if (s != null) _bgR.rays.Add(s);
                }
            }
            var hazeGO = GameObject.Find("Haze");
            if (hazeGO != null) { var s = hazeGO.GetComponent<SpriteRenderer>(); if (s != null) _bgR.hazes.Add(s); }
        }
        catch { }
    }

    private IEnumerator TransitionBgColors(Color targetSky, Color targetPillar, Color targetHaze,
                                           Color targetCloud, Color targetRay, float duration)
    {
        CollectBgRenderers();
        float elapsed = 0f;
        Color fromSky    = _bgR.sky != null ? _bgR.sky.color : targetSky;
        var fromPillars  = new List<Color>();
        var fromHazes    = new List<Color>();
        var fromClouds   = new List<Color>();
        var fromRays     = new List<Color>();
        foreach (var s in _bgR.pillars) fromPillars.Add(s != null ? s.color : targetPillar);
        foreach (var s in _bgR.hazes)   fromHazes.Add(s   != null ? s.color : targetHaze);
        foreach (var s in _bgR.clouds)  fromClouds.Add(s  != null ? s.color : targetCloud);
        foreach (var s in _bgR.rays)    fromRays.Add(s    != null ? s.color : targetRay);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float smooth = Mathf.Clamp01(elapsed / duration); smooth = smooth * smooth * (3f - 2f * smooth);
            try
            {
                if (ENABLE_SKY_COLOR && _bgR.sky != null) _bgR.sky.color = Color.Lerp(fromSky, targetSky, smooth);
                if (ENABLE_PILLAR_COLOR) for (int i = 0; i < _bgR.pillars.Count; i++) if (_bgR.pillars[i] != null) _bgR.pillars[i].color = Color.Lerp(fromPillars[i], targetPillar, smooth);
                if (ENABLE_HAZE_COLOR)   for (int i = 0; i < _bgR.hazes.Count;   i++) if (_bgR.hazes[i]   != null) { float a = _bgR.hazes[i].color.a;  _bgR.hazes[i].color  = Color.Lerp(new Color(fromHazes[i].r,  fromHazes[i].g,  fromHazes[i].b,  a), new Color(targetHaze.r,  targetHaze.g,  targetHaze.b,  a), smooth); }
                if (ENABLE_CLOUD_COLOR)  for (int i = 0; i < _bgR.clouds.Count;  i++) if (_bgR.clouds[i]  != null) { float a = _bgR.clouds[i].color.a; _bgR.clouds[i].color = Color.Lerp(new Color(fromClouds[i].r, fromClouds[i].g, fromClouds[i].b, a), new Color(targetCloud.r, targetCloud.g, targetCloud.b, a), smooth); }
                if (ENABLE_RAY_COLOR)    for (int i = 0; i < _bgR.rays.Count;    i++) if (_bgR.rays[i]    != null) _bgR.rays[i].color = Color.Lerp(fromRays[i], targetRay, smooth);
            }
            catch { }
            yield return null;
        }
        try
        {
            if (ENABLE_SKY_COLOR    && _bgR.sky != null) _bgR.sky.color = targetSky;
            if (ENABLE_PILLAR_COLOR) foreach (var s in _bgR.pillars) if (s != null) s.color = targetPillar;
            if (ENABLE_HAZE_COLOR)   foreach (var s in _bgR.hazes)   if (s != null) s.color = new Color(targetHaze.r,  targetHaze.g,  targetHaze.b,  s.color.a);
            if (ENABLE_CLOUD_COLOR)  foreach (var s in _bgR.clouds)  if (s != null) s.color = new Color(targetCloud.r, targetCloud.g, targetCloud.b, s.color.a);
            if (ENABLE_RAY_COLOR)    foreach (var s in _bgR.rays)    if (s != null) s.color = targetRay;
        }
        catch { }
    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    // OSCILACIÓN DEL HAZE EN FASE FINAL 
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    private IEnumerator FinalHazeOscillate()
    {
        if (!HAZE_OSC_ENABLED) yield break;
        CollectBgRenderers();
        bool towardB = true;
        while (true)
        {
            Color from = towardB ? HAZE_OSC_A : HAZE_OSC_B;
            Color to   = towardB ? HAZE_OSC_B : HAZE_OSC_A;
            float elapsed = 0f;
            while (elapsed < HAZE_OSC_HALF_PERIOD)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / HAZE_OSC_HALF_PERIOD);
                float smooth = t * t * (3f - 2f * t);
                Color cur = Color.Lerp(from, to, smooth);
                try
                {
                    if (ENABLE_HAZE_COLOR)
                        foreach (var s in _bgR.hazes)
                            if (s != null) s.color = new Color(cur.r, cur.g, cur.b, s.color.a);
                }
                catch { }
                yield return null;
            }
            towardB = !towardB;
        }
    }


    private IEnumerator FinalSwordVolley(bool fireLeft, bool fireRight)
    {
        if (!FINAL_SWORD_ENABLED) yield break;
        var tpl = ascendBeam ?? _eyeBeamTemplate;
        if (tpl == null) yield break;

        yield return new WaitForSeconds(FINAL_SWORD_DELAY);

        const float angleFromLeft  = 80f;
        const float angleFromRight = 100f;

        var fsms = new List<PlayMakerFSM>();

        void SpawnBeamSword(float spawnX, float angle)
        {
            var b = Object.Instantiate(tpl, new Vector3(spawnX, FINAL_SWORD_SPAWN_Y, 0f), Quaternion.Euler(0f, 0f, angle));
            b.name = "Final Sword Beam"; b.SetActive(true);
            _activeFinalSwordBeams.Add(b);
            fsms.Add(b.LocateMyFSM("Control"));
        }

        if (fireLeft  && !fireRight) for (int i = -2; i <= 2; i++) SpawnBeamSword(FINAL_SWORD_LEFT_X  + i * FINAL_SWORD_SPREAD, angleFromLeft);
        if (fireRight && !fireLeft)  for (int i = -2; i <= 2; i++) SpawnBeamSword(FINAL_SWORD_RIGHT_X + i * FINAL_SWORD_SPREAD, angleFromRight);
        if (fireLeft  && fireRight)
        {
            float leftBase  = FINAL_SWORD_LEFT_X  - FINAL_SWORD_CENTER_SIDE_OFFSET;
            float rightBase = FINAL_SWORD_RIGHT_X + FINAL_SWORD_CENTER_SIDE_OFFSET;
            for (int i = -1; i <= 1; i++) SpawnBeamSword(leftBase  + i * FINAL_SWORD_SPREAD, angleFromLeft);
            for (int i = -1; i <= 1; i++) SpawnBeamSword(rightBase + i * FINAL_SWORD_SPREAD, angleFromRight);
        }

        yield return null;
        foreach (var f in fsms) f?.SendEvent("ANTIC");
        yield return new WaitForSeconds(FINAL_SWORD_ANTIC_TIME);
        foreach (var f in fsms) f?.SendEvent("FIRE");
        yield return new WaitForSeconds(FINAL_SWORD_FIRE_TIME);
        foreach (var f in fsms) f?.SendEvent("END");
        yield return new WaitForSeconds(0.5f);
        foreach (var b in _activeFinalSwordBeams) { if (b != null) Object.Destroy(b); }
        _activeFinalSwordBeams.Clear();
        _finalSwordCoroutine = null;
        Log("FinalSwordVolley | L=" + fireLeft + " R=" + fireRight + " spot=" + _lastTeleSpot);
    }

    private IEnumerator EnableFinalSwordAfterDelay()
    {
        yield return new WaitForSeconds(FINAL_SWORD_START_DELAY);
        _finalSwordReady = true;
        Log("FinalSwordVolley: ready.");
    }


    private void ActivateGroundSpikesExcludingN(int n)
    {
        int total = _allGroundSpikes.Count;
        for (int i = 0; i < total; i++)
        {
            var sp = _allGroundSpikes[i];
            if (sp == null) continue;
            if (i >= n && i < total - n)
                sp.LocateMyFSM("Control")?.SendEvent("UP");
        }
    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    // HELPERS
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    private IEnumerator DelayedConvo(string msg, float delay)
    { yield return new WaitForSeconds(delay); ShowConvo(msg); }

    private GameObject SpawnSpike(Vector2 pos)
    { var s = Object.Instantiate(_spikeTemplate, pos, Quaternion.identity); s.SetActive(true); ConfigureAuraSpike(s); return s; }

    private GameObject SpawnSpikeRotated(Vector2 pos, float angle)
    { var s = Object.Instantiate(_spikeTemplate, pos, Quaternion.Euler(0f, 0f, angle)); s.SetActive(true); ConfigureAuraSpike(s); return s; }

    private static void SendUpToGroup(List<GameObject> group)
    { foreach (var go in group) if (go != null && go.name == "Radiant Spike(Clone)") go.LocateMyFSM("Control").SendEvent("UP"); }

    private void AddNailWall(string state, string ev, float delay, int idx)
    {
        FsmutilExt.InsertAction(_attackChoices, state,
            (FsmStateAction)new SendEventByName {
                eventTarget = _attackChoices.GetAction<SendEventByName>("Nail L Sweep", 0).eventTarget,
                sendEvent = ev, delay = delay, everyFrame = false }, idx);
    }

    private static void ConfigureAuraSpike(GameObject spike)
    {
        var d = spike.GetComponent<DamageHero>(); if (d != null) { d.damageDealt = 1; d.hazardType = 0; }
        var hs = spike.LocateMyFSM("Hero Saver"); if (hs != null) Object.Destroy(hs);
    }

    private static void Log(object o) => Modding.Logger.Log("[Aura Radiance] " + o);


    private const bool  BOSS_AFTERIMAGE_PHASE_SWORD_RAIN = false;
    private const bool  BOSS_AFTERIMAGE_PHASE_PLATFORMS  = true;
    private const bool  BOSS_AFTERIMAGE_PHASE_CLIMB      = true;
    private const bool  BOSS_AFTERIMAGE_PHASE_FINAL      = true;

    private const float BOSS_AI_RED        = 1.0f;
    private const float BOSS_AI_GREEN      = 0.3f;
    private const float BOSS_AI_BLUE       = 0.1f;
    private const float BOSS_AI_STRENGTH   = 0.55f;
    private const float BOSS_AI_DECAY      = 1.35f;
    private const float BOSS_AI_INTERVAL   = 0.05f;

    private AuraBossImagePool              _bossImagePool;
    private AuraBossAfterimageGenerator    _bossAfterimage;

    private void InitBossAfterimageSystem()
    {
        var template = new GameObject("AuraBossAfterimageTemplate");
        template.AddComponent<tk2dSprite>();
        template.AddComponent<tk2dSpriteAnimator>();
        Object.DontDestroyOnLoad(template);
        template.SetActive(false);

        _bossImagePool = new AuraBossImagePool();
        _bossImagePool.SetTemplate(template);

        _bossAfterimage             = base.gameObject.AddComponent<AuraBossAfterimageGenerator>();
        _bossAfterimage.pool        = _bossImagePool;
        _bossAfterimage.red         = BOSS_AI_RED;
        _bossAfterimage.green       = BOSS_AI_GREEN;
        _bossAfterimage.blue        = BOSS_AI_BLUE;
        _bossAfterimage.fullStrength = BOSS_AI_STRENGTH;
        _bossAfterimage.decayTime   = BOSS_AI_DECAY;
        _bossAfterimage.interval    = BOSS_AI_INTERVAL;
        _bossAfterimage.enabled     = false;
    }

    private void SetBossAfterimage(bool on)
    {
        if (_bossAfterimage != null) _bossAfterimage.enabled = on;
    }

    private IEnumerator TemporarilyLowerPlatformSpikes(float duration)
    {
    var platGroups = new List<List<GameObject>>
    {
        _spikeGroup0, _spikeGroup1, _spikeGroup2, _spikeGroup3,
        _spikeGroup5, _spikeGroup6, _spikeGroup7
    };

    foreach (var group in platGroups)
        foreach (var go in group)
            go?.LocateMyFSM("Control")?.SendEvent("DOWN");

    float elapsed = 0f;
    while (elapsed < duration)
    {
        foreach (var group in platGroups)
            foreach (var go in group)
                go?.LocateMyFSM("Control")?.SendEvent("DOWN");

        yield return new WaitForSeconds(0.5f);
        elapsed += 0.5f;
    }

    foreach (var group in platGroups)
        foreach (var go in group)
            go?.LocateMyFSM("Control")?.SendEvent("UP");
    }

} 


internal class AuraAction : FsmStateAction
{
    private readonly System.Action _fn;
    public AuraAction(System.Action fn) { _fn = fn; }
    public override void OnEnter() { _fn?.Invoke(); Finish(); }
}


internal class AuraWaitAction : FsmStateAction
{
    private bool _done = false;
    public void Release() { _done = true; }
    public override void OnEnter() { _done = false; }
    public override void OnUpdate() { if (_done) Finish(); }
}


internal class AuraInfiniteDash : MonoBehaviour
{
    private void Awake()
    {
        On.HeroController.CanDash += AlwaysEnableDash;
        Modding.Logger.Log("[Aura Radiance] AuraInfiniteDash: activado.");
    }

    private static bool AlwaysEnableDash(On.HeroController.orig_CanDash orig, HeroController self)
    {
        if (self.hero_state != GlobalEnums.ActorStates.no_input &&
            self.hero_state != GlobalEnums.ActorStates.hard_landing &&
            self.hero_state != GlobalEnums.ActorStates.dash_landing &&
            Modding.ReflectionHelper.GetField<HeroController, float>(self, "dashCooldownTimer") <= 0 &&
            !self.cState.dashing &&
            !self.cState.backDashing &&
            (!self.cState.attacking ||
            !(Modding.ReflectionHelper.GetField<HeroController, float>(self, "attack_time") <
            Modding.ReflectionHelper.GetField<HeroController, float>(self, "ATTACK_RECOVERY_TIME"))) &&
            !self.cState.preventDash &&
            !self.cState.hazardDeath &&
            PlayerData.instance.GetBool("canDash"))
        {
            return true;
        }
        return false;
    }

    private void OnDestroy()
    {
        On.HeroController.CanDash -= AlwaysEnableDash;
        Modding.Logger.Log("[Aura Radiance] AuraInfiniteDash: desactivado.");
    }
}


internal class AuraBossImagePool
{
    private GameObject _template;
    private readonly List<GameObject> _inactive = new List<GameObject>();

    public void SetTemplate(GameObject template) => _template = template;

    public GameObject Spawn(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        GameObject go;
        if (_inactive.Count > 0)
        {
            go = _inactive[0];
            _inactive.RemoveAt(0);
            go.SetActive(true);
        }
        else
        {
            go = UnityEngine.Object.Instantiate(_template);
            go.SetActive(true);
        }
        go.transform.position   = position;
        go.transform.rotation   = rotation;
        go.transform.localScale = scale;
        go.name = "BossAfterimage";
        go.tag  = "Untagged";
        UnityEngine.Object.DontDestroyOnLoad(go);
        return go;
    }

    public void Return(GameObject go) => _inactive.Add(go);
}


internal class AuraBossAfterimageFrame : MonoBehaviour
{
    public tk2dSpriteAnimationClip clip;
    public Color                   startColor;
    public float                   decayTime;
    public AuraBossImagePool       pool;

    private float _elapsed;

    private void OnEnable() { _elapsed = 0f; }

    private void Update()
    {
        var anim = gameObject.GetComponent<tk2dSpriteAnimator>();
        if (anim != null && clip != null) anim.Play(clip);
        _elapsed += Time.deltaTime;
        var sprite = gameObject.GetComponent<tk2dSprite>();
        if (sprite != null)
        {
            float alpha = Mathf.Lerp(startColor.a, 0f, _elapsed / decayTime);
            sprite.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
        }
        if (_elapsed >= decayTime)
        {
            _elapsed = 0f;
            gameObject.SetActive(false);
            pool?.Return(gameObject);
        }
    }
}


internal class AuraBossAfterimageGenerator : MonoBehaviour
{
    public AuraBossImagePool pool;
    public float red         = 1f;
    public float green       = 0.3f;
    public float blue        = 0.1f;
    public float fullStrength = 0.55f;
    public float decayTime   = 0.35f;
    public float interval    = 0.05f;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer < interval) return;
        _timer = 0f;

        var pos = transform.position;
        pos.z += 1e-3f;
        var frame = pool.Spawn(pos, transform.rotation, transform.localScale);

        try
        {
            var srcAnim = gameObject.GetComponent<tk2dSpriteAnimator>();
            if (srcAnim == null) return;
            var dstAnim = frame.GetComponent<tk2dSpriteAnimator>();
            if (dstAnim == null) dstAnim = frame.AddComponent<tk2dSpriteAnimator>();
            dstAnim.SetSprite(srcAnim.Sprite.Collection, srcAnim.Sprite.spriteId);
            dstAnim.Library = srcAnim.Library;
            var srcClip = srcAnim.CurrentClip;
            var newClip = new tk2dSpriteAnimationClip();
            newClip.CopyFrom(srcClip);
            newClip.frames    = new tk2dSpriteAnimationFrame[1];
            newClip.frames[0] = srcClip.frames[srcAnim.CurrentFrame];
            newClip.wrapMode  = tk2dSpriteAnimationClip.WrapMode.Once;
            dstAnim.enabled = false;
            foreach (var col in frame.GetComponents<Collider2D>()) Destroy(col);
            var fadeComp = frame.GetComponent<AuraBossAfterimageFrame>();
            if (fadeComp == null) fadeComp = frame.AddComponent<AuraBossAfterimageFrame>();
            fadeComp.clip       = newClip;
            fadeComp.startColor = new Color(red, green, blue, fullStrength);
            fadeComp.decayTime  = decayTime;
            fadeComp.pool       = pool;
        }
        catch (System.Exception)
        {
            pool.Return(frame);
            frame.SetActive(false);
        }
    }
}


internal class AuraHaloPhaseConfig
{
    public bool  spinEnabled      = true;
    public float spinSpeed        = -1f;
    public float spinAcceleration = 3f;
    public bool  spinJumpOnEnter  = false;

    public bool  pulseEnabled     = false;
    public float pulseBaseScale   = 1f;
    public float pulseAmplitude   = 0.15f;
    public float pulseFrequency   = 1.5f;
    public float pulseSmoothing   = 6f;

    public bool  swingEnabled     = false;
    public float swingAmpX        = 0f;
    public float swingAmpY        = 0.3f;
    public float swingFreqX       = 0f;
    public float swingFreqY       = 1.2f;
    public float swingPhaseOffset = 0f;

    public bool  bounceOnEnter    = true;
    public float bouncePeakScale  = 1.3f;
    public float bounceDuration   = 0.18f;

    public float targetAlpha      = 0.5f;
    public float alphaSmoothing   = 4f;
}


internal class AuraHaloAnimator : MonoBehaviour
{
    public System.Collections.Generic.List<AuraHaloPhaseConfig> phases
        = new System.Collections.Generic.List<AuraHaloPhaseConfig>();


    public bool  beatEnabled   = true;
    public float beatMinScale  = 1.00f;
    public float beatMaxScale  = 1.12f;
    public float beatFrequency = 0.85f;

    private int   _phase          = 0;
    private float _spinCurrent    = 0f;
    private float _scaleCurrent   = 1f;
    private float _swingTimer     = 0f;
    private bool  _inBounce       = false;
    private float _bounceTimer    = 0f;
    private float _bounceDuration = 0.18f;
    private float _bouncePeak     = 1.3f;
    private Vector3 _baseLocalPos = Vector3.zero;
    private SpriteRenderer _sr;


    private bool _clockMode = false;

    /// <summary>true al terminar el 3er giro → dispara callback de color naranja</summary>
    public bool IsClockDone  { get; private set; } = true;
    /// <summary>true al terminar el 4º giro → halo entra en modo beat (NO se oculta)</summary>
    public bool IsVanishDone { get; private set; } = true;

    // ── Modo beat ────────────────────────────────────────────────────
    private bool  _beatMode  = false;
    private float _beatTimer = 0f;

    private void Awake()
    {
        _sr           = GetComponent<SpriteRenderer>();
        _baseLocalPos = transform.localPosition;
    }

    public void BuildDefaultPhases()
    {
        phases.Clear();

        // Fase 0 — mínimo
        phases.Add(new AuraHaloPhaseConfig
        {
            spinEnabled    = true,  spinSpeed        = -0.4f, spinAcceleration = 2f,
            pulseEnabled   = false, pulseBaseScale   = 1f,
            swingEnabled   = false,
            bounceOnEnter  = false,
            targetAlpha    = 0.1f,  alphaSmoothing   = 4f
        });

        // Fase 1 — Sword Rain
        phases.Add(new AuraHaloPhaseConfig
        {
            spinEnabled    = true,  spinSpeed        = -0.35f, spinAcceleration = 2f,
            pulseEnabled   = true,  pulseBaseScale   = 1f,    pulseAmplitude   = 0.07f, pulseFrequency = 0.6f,
            swingEnabled   = false,
            bounceOnEnter  = true,  bouncePeakScale  = 1.15f, bounceDuration   = 0.18f,
            targetAlpha    = 0.35f, alphaSmoothing   = 4f
        });

        // Fase 2 — Plataformas / Climb
        phases.Add(new AuraHaloPhaseConfig
        {
            spinEnabled    = true,  spinSpeed        = -2.0f, spinAcceleration = 5f,
            spinJumpOnEnter = true,
            pulseEnabled   = true,  pulseBaseScale   = 1f,    pulseAmplitude   = 0.18f, pulseFrequency = 1.8f,
            swingEnabled   = true,  swingAmpY        = 0.18f, swingFreqY       = 1.4f,
            bounceOnEnter  = true,  bouncePeakScale  = 1.25f, bounceDuration   = 0.18f,
            targetAlpha    = 0.55f, alphaSmoothing   = 6f
        });

        // Fase 3 — Final
        phases.Add(new AuraHaloPhaseConfig
        {
            spinEnabled    = true,  spinSpeed        = -5.5f, spinAcceleration = 9f,
            spinJumpOnEnter = true,
            pulseEnabled   = true,  pulseBaseScale   = 1f,    pulseAmplitude   = 0.38f, pulseFrequency  = 3.0f,
            swingEnabled   = true,  swingAmpX        = 0.10f, swingAmpY        = 0.28f,
                                    swingFreqX       = 2.5f,  swingFreqY       = 1.9f,
            bounceOnEnter  = true,  bouncePeakScale  = 1.45f, bounceDuration   = 0.22f,
            targetAlpha    = 0.80f, alphaSmoothing   = 8f
        });
    }

    public void SetPhase(int phase)
    {
        if (phase < 0 || phase >= phases.Count) return;
        _phase    = phase;
        _beatMode = false; 
        var cfg = phases[phase];
        if (cfg.spinJumpOnEnter) _spinCurrent += cfg.spinSpeed * 2.5f;
        if (cfg.bounceOnEnter)   TriggerBounce(cfg.bouncePeakScale, cfg.bounceDuration);
    }

    public void TriggerBounce(float scale = 1.4f, float duration = 0.18f)
    {
        _bouncePeak     = scale;
        _bounceDuration = duration;
        _bounceTimer    = duration;
        _inBounce       = true;
    }

    public void SpinImpulse(float amount = -5f) => _spinCurrent += amount;

    public void SetSpinSpeed(float speed)
    {
        if (_phase < phases.Count) phases[_phase].spinSpeed = speed;
    }

    public void StartClockAnimation(float quarterTime)
    {
        if (!IsClockDone || !IsVanishDone) return;
        StartCoroutine(ClockRoutine(quarterTime));
    }

    private IEnumerator ClockRoutine(float quarterTime)
    {
        IsClockDone  = false;
        IsVanishDone = false;
        _clockMode   = true;
        _beatMode    = false;

        float baseScale = _phase < phases.Count ? phases[_phase].pulseBaseScale : 1f;
        _scaleCurrent = baseScale;
        transform.localScale   = new Vector3(baseScale, baseScale, 1f);
        transform.localPosition = _baseLocalPos;

        float baseAngle = transform.eulerAngles.z;

        for (int q = 0; q < 4; q++)
        {
            float fromAngle = transform.eulerAngles.z;
            float toAngle   = baseAngle + (q + 1) * 90f;
            float elapsed   = 0f;

            while (elapsed < quarterTime)
            {
                elapsed += Time.deltaTime;
                float t     = Mathf.Clamp01(elapsed / quarterTime);
                float eased = ClockEase(t);
                float angle = Mathf.LerpAngle(fromAngle, toAngle, eased);
                transform.eulerAngles = new Vector3(0f, 0f, angle);
                yield return null;
            }
            transform.eulerAngles = new Vector3(0f, 0f, toAngle);
            yield return new WaitForSeconds(0.04f);


            if (q == 3)
                IsClockDone = true;
        }


        _clockMode = false;

        if (beatEnabled)
        {

            if (_sr != null && _phase < phases.Count)
            {
                var c = _sr.color;
                c.a = phases[_phase].targetAlpha;
                _sr.color = c;
            }
            _beatMode  = true;
            _beatTimer = 0f;
        }
        else
        {

        }

        IsVanishDone = true;
    }

    private static float ClockEase(float t)
    {
        if (t < 0.18f)
        {
            float norm = t / 0.18f;
            return norm * norm * 0.12f;
        }
        if (t < 0.78f)
        {
            return 0.12f + (t - 0.18f) / 0.60f * 0.78f;
        }
        float bt  = (t - 0.78f) / 0.22f;
        float ov  = Mathf.Sin(bt * Mathf.PI) * 0.10f;
        return 0.90f + bt * 0.10f + ov * (1f - bt);
    }

    private void FixedUpdate()
    {
        if (_clockMode) return;
        if (_phase < 0 || _phase >= phases.Count) return;
        var cfg = phases[_phase];
        float dt = Time.deltaTime;
        _swingTimer += dt;


        float targetSpin = cfg.spinEnabled ? cfg.spinSpeed : 0f;
        _spinCurrent += (targetSpin - _spinCurrent) * dt * cfg.spinAcceleration;
        if (Mathf.Abs(_spinCurrent) > 0.001f)
            transform.eulerAngles += new Vector3(0f, 0f, _spinCurrent);

        if (_beatMode)
        {
            _beatTimer += dt;
            float sine = Mathf.Sin(_beatTimer * beatFrequency * Mathf.PI * 2f) * 0.5f + 0.5f; // 0→1
            float beatScale = Mathf.Lerp(beatMinScale, beatMaxScale, sine);
            transform.localScale    = new Vector3(beatScale, beatScale, 1f);
            transform.localPosition = _baseLocalPos; 

            if (_sr != null)
            {
                Color c = _sr.color;
                c.a = Mathf.Lerp(c.a, cfg.targetAlpha, dt * cfg.alphaSmoothing);
                _sr.color = c;
            }
            return; 
        }


        float targetScale = cfg.pulseBaseScale;
        if (cfg.pulseEnabled)
        {
            float sine2 = Mathf.Sin(_swingTimer * cfg.pulseFrequency * Mathf.PI * 2f);
            targetScale = cfg.pulseBaseScale + sine2 * cfg.pulseAmplitude;
        }

        if (_inBounce)
        {
            _bounceTimer -= dt;
            float bt = Mathf.Clamp01(1f - _bounceTimer / Mathf.Max(_bounceDuration, 0.001f));
            float bounceOffset = Mathf.Sin(bt * Mathf.PI) * (_bouncePeak - cfg.pulseBaseScale);
            targetScale = cfg.pulseBaseScale + bounceOffset;
            if (_bounceTimer <= 0f) _inBounce = false;
        }

        _scaleCurrent += (targetScale - _scaleCurrent) * dt * cfg.pulseSmoothing;
        transform.localScale = new Vector3(_scaleCurrent, _scaleCurrent, 1f);

        // ── Swing ──────────────────────────────────────────────────────
        Vector3 swingOffset = Vector3.zero;
        if (cfg.swingEnabled)
        {
            swingOffset.x = Mathf.Sin(_swingTimer * cfg.swingFreqX * Mathf.PI * 2f + cfg.swingPhaseOffset) * cfg.swingAmpX;
            swingOffset.y = Mathf.Sin(_swingTimer * cfg.swingFreqY * Mathf.PI * 2f + cfg.swingPhaseOffset) * cfg.swingAmpY;
        }
        transform.localPosition = _baseLocalPos + swingOffset;

        // ── Alpha ──────────────────────────────────────────────────────
        if (_sr != null)
        {
            Color c = _sr.color;
            c.a = Mathf.Lerp(c.a, cfg.targetAlpha, dt * cfg.alphaSmoothing);
            _sr.color = c;
        }
    }
}


internal class AuraHaloSimpleRotatorPhase
{
    public float spinSpeed      = 1.5f;
    public float targetAlpha    = 0.4f;
    public float alphaSmoothing = 4f;
}


internal class AuraHaloSimpleRotator : MonoBehaviour
{
    public System.Collections.Generic.List<AuraHaloSimpleRotatorPhase> phases
        = new System.Collections.Generic.List<AuraHaloSimpleRotatorPhase>();

    private int   _phase = 0;
    private SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    public void BuildDefaultPhases()
    {
        phases.Clear();

        phases.Add(new AuraHaloSimpleRotatorPhase { spinSpeed =  0.3f, targetAlpha = 0.08f, alphaSmoothing = 3f }); // 0
        phases.Add(new AuraHaloSimpleRotatorPhase { spinSpeed =  0.8f, targetAlpha = 0.30f, alphaSmoothing = 4f }); // 1 Sword Rain
        phases.Add(new AuraHaloSimpleRotatorPhase { spinSpeed =  2.2f, targetAlpha = 0.50f, alphaSmoothing = 5f }); // 2 Plat/Climb
        phases.Add(new AuraHaloSimpleRotatorPhase { spinSpeed =  5.0f, targetAlpha = 0.75f, alphaSmoothing = 7f }); // 3 Final
    }

    public void SetPhase(int phase)
    {
        if (phase < 0 || phase >= phases.Count) return;
        _phase = phase;
    }

    private void FixedUpdate()
    {
        if (_phase < 0 || _phase >= phases.Count) return;
        var cfg = phases[_phase];

        transform.eulerAngles += new Vector3(0f, 0f, cfg.spinSpeed);

        if (_sr != null)
        {
            Color c = _sr.color;
            c.a = Mathf.Lerp(c.a, cfg.targetAlpha, Time.deltaTime * cfg.alphaSmoothing);
            _sr.color = c;
        }
    }
}