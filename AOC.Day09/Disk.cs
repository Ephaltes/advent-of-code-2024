using System.Collections.Generic;

namespace AOC.Day09;

public class Disk
{
    public Disk(List<int?> blocks, List<Block> freeSpace, List<Block> usedSpace)
    {
        Blocks = blocks;
        FreeSpace = freeSpace;
        UsedSpace = usedSpace;
    }

    public List<int?> Blocks
    {
        get;
    }

    public List<Block> FreeSpace
    {
        get;
    }

    public List<Block> UsedSpace
    {
        get;
    }
}