using System;
using TowerDefense.Data;

[Serializable]
public class SpawnGroup
{
    public EnemyData EnemyData; // Який тип ворога
    public int Count = 5;       // Скільки їх буде
    public float SpawnInterval = 0.5f; // З якою затримкою вони з'являться
}
