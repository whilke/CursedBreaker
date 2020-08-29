using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using enBask;
using enBask.curses;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private SpriteRenderer prefab;
    [SerializeField] private SpriteRenderer nonCureprefab;
    [SerializeField] private Vector2 bounds;
    [SerializeField] private float curseRate = 1f;
    [SerializeField] private float gameSpeed = 1f;
    [SerializeField] private int maxCount = 5000;

   
    public QuadTree<IQuadTreeObject> quadTree;
    public Dictionary<CurseData, int> CurseSuppresions = new Dictionary<CurseData, int>();

    private List<GameObject> characters = new List<GameObject>();
    private List<CharacterData> charData = new List<CharacterData>();
    private List<LightCollider> colliders = new List<LightCollider>();
    private float accum;
    private float internalDeltaTime;
    private float lastCurseTime;

    private void Awake()
    {
        var r = new Rect(this.transform.position.x - this.bounds.x / 2f,
                         this.transform.position.y - this.bounds.y / 2f,
                         this.bounds.x,
                         this.bounds.y);
        this.quadTree = new QuadTree<IQuadTreeObject>( 10,r );

        this.StartCoroutine(this.RunCurseSystem());
        this.lastCurseTime = Time.time;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, bounds);

        Gizmos.color = Color.cyan;
        this.quadTree?.DrawDebug();
    }

    public void AddLightCollider(LightCollider collider)
    {
        var b = new Bounds(collider.Area.center, collider.Area.size + new Vector2(0.1f, 0.1f));
        var r = new Rect(b.min, b.size);
        CharacterData[] charactersNear = this.quadTree.RetrieveObjectsInArea(r)
                                             .OfType<CharacterData>().ToArray();

        foreach (CharacterData c in charactersNear)
        {
            CharacterData character = c;
            var dir = (collider.GetPosition() - character.GetPosition()).normalized;

            var newPos = character.position + (-dir * collider.Area.size/ 2 + new Vector2(0.1f, 0.1f));
            character.position = newPos;
            character.renderer.transform.position = newPos;
            this.charData[character.Index] = character;
        }

        this.colliders.Add(collider);
    }

    public void RemoveLightCollider(LightCollider collider)
    {
        this.colliders.Remove(collider);
    }

    public void SpawnCharacters(Wave wave)
    {
        int count = UnityEngine.Random.Range(wave.MinCount, wave.MaxCount+1);
        Enumerable.Range(0, count).Select(i =>
        {
            var reused = false;
            bool infected = UnityEngine.Random.value <= wave.CurseChance;

            SpriteRenderer go;
            CharacterData cd;
            var dataIndex = 0;
            if (this.characters.Count >= this.maxCount)
            {
                reused = true;

                int oldestIdx = this.charData.Where(x=>!x.permanente)
                                    .OrderBy(x => x.spawnTime)
                                    .First().Index;
                go = this.charData[oldestIdx].renderer;
                cd = this.charData[oldestIdx];
                dataIndex = oldestIdx;

                if (infected && !cd.permanente)
                {
                    var go2 = Instantiate(this.nonCureprefab, go.transform, false);
                }
            }
            else
            {
                go = Instantiate(this.prefab, null, false);
                dataIndex = this.characters.Count;

                if (infected)
                {
                    var go2 = Instantiate(this.nonCureprefab, go.transform, false);
                }
            }

            CurseData curse = null;
            if (infected)
            {
                curse = wave.Curses[UnityEngine.Random.Range(0, wave.Curses.Length)];
            }

            var randomAngle = UnityEngine.Random.insideUnitCircle;

            Vector2 waveSize = wave.Size;
            if (waveSize == Vector2.zero)
                waveSize = this.bounds;

            Bounds bBase = new Bounds(Vector2.zero, this.bounds);
            Bounds bSpawn = new Bounds(wave.Position, waveSize);
            Rect rBase = new Rect(bBase.min, bBase.size);
            Rect r = new Rect(bSpawn.min, bSpawn.size);

            if (r.min.x < rBase.min.x)
            {
                r.min = new Vector2(rBase.min.x, r.min.y);
            }
            if (r.min.y < rBase.min.y)
            {
                r.min = new Vector2(r.min.x, rBase.min.y);
            }

            if (r.max.x > rBase.max.x)
            {
                r.max = new Vector2(rBase.max.x, r.max.y);
            }
            if (r.max.y > rBase.max.y)
            {
                r.max = new Vector2(r.max.x, rBase.max.y);
            }

            float rX = UnityEngine.Random.Range(r.xMin, r.xMax);
            float ry = UnityEngine.Random.Range(r.yMin, r.yMax);
            go.transform.position = new Vector3(rX, ry, 0);

            cd = new CharacterData
            {
                renderer = go,
                speed = UnityEngine.Random.Range(0.2f, 1.3f),
                position = go.transform.position,
                direction = randomAngle,
                Index = dataIndex,
                infected = infected,
                permanente = infected,
                incubationTime = curse?.IncubationTime ?? 0,
                rateTime = curse?.InfectionRate ?? 0,
                newCurse = curse,
            };

            if (!reused)
            {
                this.characters.Add(go.gameObject);
                this.charData.Add(cd);
            }
            else
            {
                this.charData[cd.Index] = cd;
            }

            return i;
        }).ToArray();
    }

    private static void DrawRectangle(Vector3 position, Color color, Vector3 size)
    {
        Vector3 halfSize = size / 2f;

        Vector3[] points = new Vector3[]
        {
            position + new Vector3(halfSize.x,halfSize.y,halfSize.z),
            position + new Vector3(-halfSize.x,halfSize.y,halfSize.z),
            position + new Vector3(-halfSize.x,-halfSize.y,halfSize.z),
            position + new Vector3(halfSize.x,-halfSize.y,halfSize.z),
        };

        Debug.DrawLine(points[0], points[1], color);
        Debug.DrawLine(points[1], points[2], color);
        Debug.DrawLine(points[2], points[3], color);
        Debug.DrawLine(points[3], points[0], color);
    }

    List<IQuadTreeObject> checkCollisonList = new List<IQuadTreeObject>();

    public void CheckWallDamage()
    {
        foreach (var wall in this.colliders)
        {
            var b = new Bounds(wall.Area.center, wall.Area.size);
            b.Expand(0.2f);
            var r = new Rect(b.min, b.size);

            this.checkCollisonList.Clear();
            this.quadTree.RetrieveObjectsInAreaNoAlloc(r, ref this.checkCollisonList);

            var characters = this.checkCollisonList
                                 .OfType<CharacterData>()
                                 .Where(x => x.infected && x.incubated);

            foreach (var character in characters)
            {
                int damage = CurseEffectManager.AttackWall(character, wall);
                if (damage != 0)
                {
                    wall.Hit(damage);
                }
            }
        }
    }

    public CharacterData MoveCharacter(CharacterData character)
    {
        Vector2 pos = character.renderer.transform.position;
        pos += (character.direction * (character.speed * Time.deltaTime));

        pos.x = Mathf.Clamp(pos.x, -this.bounds.x / 2, this.bounds.x / 2);
        pos.y = Mathf.Clamp(pos.y, -this.bounds.y / 2, this.bounds.y / 2);


        var b = new Bounds(pos, new Vector2(1f, 1f));
        var r = new Rect(b.min, b.size);
        this.checkCollisonList.Clear();
        this.quadTree.RetrieveObjectsInAreaNoAlloc(r, ref this.checkCollisonList);

        LightCollider[] foundColliders = this.checkCollisonList
                                             .OfType<LightCollider>()
                                             .ToArray();

        float radius = 0.3f;
        foreach (LightCollider collider in foundColliders)
        {
            if (collider.Collides(pos, radius))
            {
                pos = character.position;
                pos += (-character.direction * character.speed * Time.deltaTime);


                pos.x = Mathf.Clamp(pos.x, -this.bounds.x / 2f, this.bounds.x / 2f);
                pos.y = Mathf.Clamp(pos.y, -this.bounds.y / 2f, this.bounds.y / 2f);

                character.renderer.transform.position = pos;
                character.position = pos;
                character.forceNewDirection = true;



                return character;
            }
        }

        character.renderer.transform.position = pos;
        character.position = pos;
        return character;
    }

    public CharacterData RandomCharacterMove(CharacterData character, int changeChance = 20)
    {
        int randomChance = UnityEngine.Random.Range(0, changeChance);
        bool shouldChangeDirection = randomChance == 0;

        if (shouldChangeDirection || character.forceNewDirection)
        {
            character.forceNewDirection = false;
            var randomAngle = UnityEngine.Random.insideUnitCircle;
            character.direction = randomAngle;
        }

        return this.MoveCharacter(character);
    }

    private void FixedUpdate()
    {
        if (this.gameSpeed > 0)
            Time.timeScale = this.gameSpeed;

        this.quadTree.Clear();
        foreach (LightCollider collider in this.colliders)
        {
            this.quadTree.Insert(collider);
        }


        bool allCursed = true;
        for (var i = 0; i < this.characters.Count; ++i)
        {
            GameObject go = this.characters[i];
            CharacterData cd = this.charData[i];

            if (cd.infected && cd.incubated)
            {
                cd = CurseEffectManager.Execute(cd.curse.Effect, cd);
            }
            else
            {
               cd = this.RandomCharacterMove(cd);
            }

            this.charData[i] = cd;
            this.quadTree.Insert(cd);
            if (!cd.curse)
                allCursed = false;
        }

        bool rebuildTree = false;
        for (var i = 0; i < this.characters.Count; ++i)
        {
            GameObject go = this.characters[i];
            CharacterData cd = this.charData[i];

            if (cd.infected && cd.incubated)
            {
                cd = CurseEffectManager.PostExecute(cd.curse.Effect, cd, out bool modified);
                this.charData[i] = cd;

                if (modified)
                {
                    rebuildTree = true;
                }
            }
        }

        if (rebuildTree)
        {
            this.RebuildQuadTree();
        }

        this.CheckWallDamage();

        if (allCursed && this.characters.Count > 0)
        {
            Time.timeScale = 0f;
            this.StartCoroutine(this.ShowEndGameScreen());
        }
    }

    private IEnumerator ShowEndGameScreen()
    {
        var gameData = FindObjectOfType<GameData>();
        gameData.TotalScore = (int) Time.timeSinceLevelLoad;
        if (gameData.TotalScore > gameData.HighScore)
            gameData.HighScore = gameData.TotalScore;

        Time.timeScale = 1f;
        yield return SceneManager.LoadSceneAsync("End Game");
    }

    private IEnumerator RunCurseSystem()
    {
        while (true)
        {
            this.accum += Time.deltaTime;
            if (this.accum >= this.curseRate)
            {
                this.accum -= this.curseRate;

                this.internalDeltaTime = Time.time - this.lastCurseTime;
                this.lastCurseTime = Time.time;
                this.RunCurses();
            }

            yield return null;
        }
    }

    private void RunCurses()
    {
        var nearMe = new List<IQuadTreeObject>();
        var infected = new List<CharacterData>();
        for (var i = 0; i < this.characters.Count; ++i)
        {
            CharacterData cd = this.charData[i];

            if (cd.curseCooldown > 0f)
            {
                cd.curseCooldown -= this.internalDeltaTime;
                if (cd.curseCooldown < 0f)
                    cd.curseCooldown = 0f;
                this.charData[cd.Index] = cd;
            }

            if (!cd.infected) continue;

            cd.incubationTime -= internalDeltaTime;
            cd.rateTime -= internalDeltaTime;

            if (cd.curse != cd.newCurse)
            {
                cd.renderer.color = Color.Lerp(cd.renderer.color, cd.newCurse.Color, 0.1f);
            }

            this.charData[cd.Index] = cd;

            if (cd.incubationTime > 0f) continue;
            if (cd.rateTime > 0f) continue;

            if (cd.curse != null && this.CurseSuppresions.ContainsKey(cd.curse)) continue;

            cd.incubated = true;
            cd.curse = cd.newCurse;

            cd.renderer.color = cd.curse.Color;

            CurseData curse = cd.curse;
            cd.rateTime += curse.InfectionRate;
            this.charData[cd.Index] = cd;

            nearMe.Clear();

            var r = new Rect(new Vector2(cd.position.x - curse.Range.x / 2f,
                                         cd.position.y - curse.Range.y / 2f)
                           , curse.Range);
            this.quadTree.RetrieveObjectsInAreaNoAlloc(r,
                                                       ref nearMe);

            var charactersNearMe = nearMe.Where(x => x is CharacterData).ToList();
            int infectedCount = 0;
            for (var idx = 0; idx < charactersNearMe.Count; ++idx)
            {
                float chance = UnityEngine.Random.Range(0f, 1f);
                if (chance <= curse.InfectionChance)
                {
                    var n = (CharacterData)charactersNearMe[idx];
                    
                    if (n.curse == curse) continue;

                    if (n.infected && n.incubationTime > 0f) continue;
                    if (n.curseCooldown > 0f) continue;
                    if (infectedCount > curse.MaxInfectionCount) break;

                    infectedCount++;
                    n.infected = true;
                    n.newCurse = curse;
                    n.incubated = false;
                    n.incubationTime = curse.IncubationTime;
                    n.rateTime = curse.InfectionRate;
                    infected.Add(n);
                }
            }
        }

        foreach (var cd in infected)
        {
            this.charData[cd.Index] = cd;
        }
    }

    public void HitCharacter(CharacterData hit)
    {
        hit.infected = false;
        hit.renderer.color = Color.white;
        hit.curse = null;
        this.charData[hit.Index] = hit;
    }

    public int CharacteCount()
    {
        return this.characters.Count;
    }

    public void RebuildQuadTree()
    {
        this.quadTree.Clear();
        foreach (CharacterData character in this.charData)
        {
            this.quadTree.Insert(character);
        }

        foreach (LightCollider collider in this.colliders)
        {
            this.quadTree.Insert(collider);
        }
    }

    public CharacterData HealCharacter(CharacterData character)
    {
        character.infected = false;
        character.incubated = false;
        character.incubationTime = 0f;
        character.curse = null;
        character.newCurse = null;
        character.renderer.color = Color.white;
        character.curseCooldown = 1f;
        return character;
    }

    public void UpdateCharacter(CharacterData character)
    {
        this.charData[character.Index] = character;
    }

    public CharacterData FindCharacter(SpriteRenderer renderer)
    {
        return this.charData.FirstOrDefault(x => x.renderer == renderer);
    }
}

public struct CharacterData : IEquatable<CharacterData>, IQuadTreeObject
{
    public SpriteRenderer renderer;
    public Vector2 position;
    public Vector2 direction;
    public bool permanente;
    public float speed;
    public bool infected;
    public int Index;
    public CurseData curse;
    public CurseData newCurse;
    public float incubationTime;
    public float rateTime;
    public float spawnTime;
    public bool incubated;
    public bool forceNewDirection;
    public float curseCooldown;

    public override int GetHashCode()
    {
        return (renderer != null ? renderer.GetHashCode() : 0);
    }

    public Vector2 GetPosition()
    {
        return this.position;
    }

    public bool Equals(CharacterData other)
    {
        return Equals(renderer, other.renderer);
    }

    public override bool Equals(object obj)
    {
        return obj is CharacterData other && Equals(other);
    }
}
