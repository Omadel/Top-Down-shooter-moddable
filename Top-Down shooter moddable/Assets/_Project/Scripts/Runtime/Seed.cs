using UnityEngine;

public class Seed : Transportable
{
    public SeedData Data => data;
    [SerializeField] SeedData data;

    public static implicit operator SeedData(Seed seed) => seed.Data;
}
