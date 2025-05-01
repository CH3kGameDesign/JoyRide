using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class PlayerController : Target
{
    private int i_coins = 0;
    private float f_timeSinceStart = 0;
    [Header("Big Stuff")]
    public GameState_Enum GameState = GameState_Enum.active;
    public enum GameState_Enum { inactive, active };
    public static PlayerController Instance;

    [Header ("Object References")]
    public NavMeshAgent NMA_player;
    public Transform T_camHolder;
    public Transform T_camHook;
    public Transform T_model;
    public Transform T_canvasHolder;
    public Slider S_healthBar;
    public TextMeshProUGUI TM_coins;
    public TextMeshProUGUI TM_time;


    [Header("Camera Variables")]
    public float F_camLerpSpeed = 5;
    public float F_camDeadSpace = 3f;
    public Transform T_staticCamPos;
    public Transform T_followCamPos;
    public bool B_rotateCam = false;

    [Header("Inputs")]
    public List<Trick> trickList = new List<Trick>();
    List<Input> _inputs = new List<Input>();
    public float F_inputCutoff = 2f;
    bool b_midTrick = false;
    float f_trickInputTimer = 0;
    float f_trickTimer = 0;
    Vector2 v2_inputMoveDir = Vector2.up;
    Vector3 v3_moveDir = Vector3.forward;
    Vector3 v3_tarPos;

    [Header("Combat Variables")]
    public GunManager gunManager;
    public List<GunManager.Gun> guns = new List<GunManager.Gun>();

    [Header("Anims")]
    public AnimCurve_Scriptable A_smooth;
    public AnimCurve_Scriptable A_rebound;
    Camera c_mainCam;
    float f_baseCamFOV;

    [Header("Debugging Stuf")]
    public int DEBUG_gunNum = 0;

    [System.Serializable]
    public class Trick
    {
        public string trickID = "";
        public List<Vector2> inputs = new List<Vector2>();
        public List<Vector2> cancels = new List<Vector2>();
    }
    public class Input
    {
        public Vector2 v2_input;
        public double d_time;
    }
    private void Awake()
    {
        Debug.Log(NMA_player.agentTypeID);
        GunManager.Instance = gunManager;
        Instance = this;
        c_mainCam = Camera.main;
        f_baseCamFOV = c_mainCam.fieldOfView;
        DEBUG_AddGun();

        if (B_rotateCam)
        {
            c_mainCam.transform.localPosition = T_followCamPos.localPosition;
            c_mainCam.transform.localRotation = T_followCamPos.localRotation;
        }
        else
        {
            c_mainCam.transform.localPosition = T_staticCamPos.localPosition;
            c_mainCam.transform.localRotation = T_staticCamPos.localRotation;
        }
    }

    void DEBUG_AddGun()
    {
        guns.Add(gunManager.GetGunByNum(DEBUG_gunNum));
    }


    // Update is called once per frame
    void Update()
    {
        switch (GameState)
        {
            case GameState_Enum.inactive:
                break;
            case GameState_Enum.active:
                Update_Active();
                break;
            default:
                break;
        }
        
    }
    void Update_Active()
    {
        Trick_Update();
        MoveUpdate();
        CameraUpdate();
        ShootUpdate();
        TimeUpdate();
        T_canvasHolder.position = NMA_player.transform.position;
    }

    void TimeUpdate()
    {
        f_timeSinceStart += Time.deltaTime;
        TM_time.text = f_timeSinceStart.ToDuration();
    }

    void Trick_Update()
    {
        Input _temp = new Input();
        _temp.v2_input = Quaternion.Euler(0, 0, NMA_player.transform.eulerAngles.y) * v2_inputMoveDir;
        _temp.d_time = Time.timeSinceLevelLoadAsDouble;
        _inputs.Add(_temp);
        for (int i = 0; i < _inputs.Count; i++)
        {
            if (_temp.d_time - _inputs[i].d_time > F_inputCutoff)
            {
                _inputs.RemoveAt(i);
                i--;
            }
            else
                break;
        }
        if (f_trickTimer <= 0 && !b_midTrick)
            Trick_Check();
        f_trickTimer -= Time.deltaTime;
    }
    void Trick_Check()
    {
        for (int i = 0; i < trickList.Count; i++)
        {
            int _step = trickList[i].inputs.Count - 1;
            bool _fail = false;
            if (Vector2.Distance(_inputs[_inputs.Count - 1].v2_input, trickList[i].inputs[_step]) < 0.45f)
            {
                _step--;
                if (_step < 0)
                {
                    Trick_Do(trickList[i].trickID);
                    return;
                }
            }
            else
                continue;
            for (int j = _inputs.Count - 2; j >= 0; j--)
            {
                foreach (var item in trickList[i].cancels)
                    if (Vector2.Distance(_inputs[j].v2_input, item) < 0.33f)
                    {
                        _fail = true;
                        break;
                    }
                if (_fail)
                    continue;
                if (Vector2.Distance(_inputs[j].v2_input, trickList[i].inputs[_step]) < 0.5f)
                {
                    _step--;
                    if (_step < 0)
                    {
                        Trick_Do(trickList[i].trickID);
                        return;
                    }
                }
            }
        }
    }

    void Trick_Do(string _id)
    {
        _inputs.Clear();
        float _duration = 1f;
        switch (_id)
        {
            case "kickFlip":
                _duration = 1.2f;
                StartCoroutine(KickFlip(_duration));
                f_trickTimer = _duration;
                break;
            case "spinLeft":
                _duration = 1.2f;
                StartCoroutine(Spin_Left(_duration));
                f_trickTimer = _duration;
                break;
            case "spinRight":
                _duration = 1.2f;
                StartCoroutine(Spin_Right(_duration));
                f_trickTimer = _duration;
                break;
            default:
                break;
        }
    }

    IEnumerator KickFlip(float _duration)
    {
        b_midTrick = true;
        NMA_player.updateRotation = false;
        NMA_player.updatePosition = false;
        float _timer = 0;
        Vector3 tarPos = Vector3.up * 5;
        Vector3 tarRot = new Vector3(0, 0, 360);
        StartCoroutine(Cam_FOV(70, _duration / 4));
        while (_timer < 1)
        {
            T_model.localPosition = Vector3.Lerp(Vector3.zero, tarPos, A_rebound.Evaluate(_timer));
            T_model.localEulerAngles = Vector3.Lerp(Vector3.zero, tarRot, A_smooth.Evaluate(_timer));
            _timer += Time.deltaTime / _duration;
            Movement_MidTrick();
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(Cam_FOV(f_baseCamFOV, _duration / 4));

        T_model.localPosition = Vector3.zero;
        T_model.localEulerAngles = Vector3.zero;
        NavMeshHit navHit;
        while (true)
        {
            if (NavMesh.SamplePosition(NMA_player.transform.position, out navHit, 100, -1))
            {
                Vector3 _pos = navHit.position;
                //_pos.y = NMA_player.transform.position.y;
                NMA_player.Warp(_pos);
                NMA_player.updateRotation = true;
                NMA_player.updatePosition = true;
                Movement_MidTrick();
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        _inputs.Clear();
        b_midTrick = false;
    }
    IEnumerator Spin_Right(float _duration)
    {
        b_midTrick = true;
        float _timer = 0;
        Vector3 tarRot = new Vector3(0, 720, 0);
        StartCoroutine(Cam_FOV(70, _duration / 4));
        while (_timer < 1)
        {
            T_model.localEulerAngles = Vector3.Lerp(Vector3.zero, tarRot, A_smooth.Evaluate(_timer));
            _timer += Time.deltaTime / _duration;
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(Cam_FOV(f_baseCamFOV, _duration / 4));

        T_model.localEulerAngles = Vector3.zero;
        _inputs.Clear();
        b_midTrick = false;
    }
    IEnumerator Spin_Left(float _duration)
    {
        b_midTrick = true;
        float _timer = 0;
        Vector3 tarRot = new Vector3(0, -720, 0);
        StartCoroutine(Cam_FOV(70, _duration/4));
        while (_timer < 1)
        {
            T_model.localEulerAngles = Vector3.Lerp(Vector3.zero, tarRot, A_smooth.Evaluate(_timer));
            _timer += Time.deltaTime / _duration;
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(Cam_FOV(f_baseCamFOV, _duration / 4));
        T_model.localEulerAngles = Vector3.zero;
        _inputs.Clear();
        b_midTrick = false;
    }

    IEnumerator Cam_FOV(float _tarFOV, float _duration)
    {
        float _timer = 0;
        float _startFOV = c_mainCam.fieldOfView;
        while (_timer < 1)
        {
            c_mainCam.fieldOfView = Mathf.Lerp(_startFOV, _tarFOV, A_smooth.Evaluate(_timer));
            _timer += Time.deltaTime / _duration;
            yield return new WaitForEndOfFrame();
        }
        c_mainCam.fieldOfView = _tarFOV;
    }

    void Movement_MidTrick()
    {
        Vector3 _tarDir = v3_moveDir * NMA_player.speed * Time.deltaTime;
        Vector3 _tarPos = NMA_player.transform.position + _tarDir;
        NMA_player.transform.LookAt(_tarPos);
        NMA_player.transform.position = _tarPos;
    }

    void ShootUpdate()
    {
        foreach (var item in guns)
        {
            if (item.timer <= 0)
            {
                for (int i = 0; i < T_targetList.Count; i++)
                {
                    if (T_targetList[i] == null)
                    {
                        T_targetList.RemoveAt(i);
                        i--;
                        continue;
                    }
                    Projectile _bullet = Instantiate(item.bullet.gameObject).GetComponent<Projectile>();
                    _bullet.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
                    Vector3 _target = T_targetList[0].GetPos();
                    _target.y = 0.5f;
                    _bullet.transform.LookAt(_target);
                    _bullet.OnCreate(item.stats().damage, TE_targetType == targetEnum.player);
                    item.timer = 1;
                    break;
                }
            }
            else
                item.timer -= Time.deltaTime * item.stats().dps;
        }
    }

    void MoveUpdate()
    {
        v3_moveDir = Vector3.Normalize(Vector3.Lerp(v3_moveDir, Vector3.Normalize(new Vector3(v2_inputMoveDir.x, 0, v2_inputMoveDir.y)), Time.deltaTime / 0.5f));
        v3_tarPos = NMA_player.transform.position + v3_moveDir;
        NMA_player.SetDestination(v3_tarPos);
    }

    void CameraUpdate()
    {
        Vector3 _tarPos = Vector3.MoveTowards(T_camHook.position, T_camHolder.position, F_camDeadSpace);
        T_camHolder.position = Vector3.Lerp(T_camHolder.position, _tarPos, Time.deltaTime * F_camLerpSpeed);

        if (B_rotateCam)
        {
            Quaternion _tarRot = Quaternion.RotateTowards(T_camHook.rotation, T_camHolder.rotation, 10);
            T_camHolder.rotation = Quaternion.Lerp(T_camHolder.rotation, _tarRot, Time.deltaTime * F_camLerpSpeed);
        }
    }
    public void Input_MoveDir(Vector2 _input)
    {
        if (_input.magnitude > 0)
        {
            if (B_rotateCam)
            {
                Vector3 _temp = Vector3.zero;
                _temp += T_camHolder.forward * _input.y;
                _temp += T_camHolder.right * _input.x;
                _input = new Vector2(_temp.x, _temp.z);
            }
            v2_inputMoveDir = _input;
        }
    }

    public override Vector3 GetPos()
    {
        return NMA_player.transform.position;
    }

    public override void OnHit(float _damage)
    {
        S_healthBar.gameObject.SetActive(true);
        S_healthBar.value = F_health / F_maxHealth;
        base.OnHit(_damage);
    }

    public override void OnDeath()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void Coins_Add(int _amt)
    {
        i_coins += _amt;
        TM_coins.text = "Coins".ToSpriteString() + i_coins;
    }
}
