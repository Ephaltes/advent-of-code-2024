using System.Collections.Generic;
using System.Linq;

using AOC.Common;

namespace AOC.Day09;

public class DiskCalculator
{
    private readonly Disk _fragmentedDisk;

    public DiskCalculator(List<int> diskMap)
    {
        _fragmentedDisk = CreateFragmentedDisk(diskMap);
    }

    public IReadOnlyCollection<int?> GetDefragmentedDisk()
    {
        IReadOnlyCollection<int?> defragmentedDisk = GetDefragmentedBlocks();

        return defragmentedDisk;
    }

    public IReadOnlyCollection<int?> GetDefragmentedDiskByBlocks()
    {
        IReadOnlyCollection<int?> defragmentedDisk = DefragmentDiskByBlocks();

        return defragmentedDisk.ToList();
    }

    private IReadOnlyCollection<int?> DefragmentDiskByBlocks()
    {
        List<Block> reArrangedBlocks = [];
        List<Block> usedSpace = _fragmentedDisk.UsedSpace.ToList();
        List<Block> freeSpace = _fragmentedDisk.FreeSpace.ToList();

        foreach (Block block in usedSpace.OrderByDescending(block => block.Value))
        {
            (Block? availableSpace, int index) = GetFreeAvailableSpaceOrNull(freeSpace, block);

            if (availableSpace is null)
            {
                reArrangedBlocks.Add(block);

                continue;
            }

            reArrangedBlocks.Add(new Block(block.Value, availableSpace.StartIndex, block.Length));

            if (availableSpace.Length <= block.Length)
            {
                freeSpace.Remove(availableSpace);

                continue;
            }

            CreateLeftOverSpace(availableSpace, block, freeSpace, index);
        }

        int?[] blocks = CreateBlocks(reArrangedBlocks);

        return blocks;
    }

    private int?[] CreateBlocks(List<Block> reArrangedBlocks)
    {
        int?[] blocks = new int?[_fragmentedDisk.Blocks.Count];

        foreach (Block reArrangedBlock in reArrangedBlocks)
        {
            for (int i = 0; i < reArrangedBlock.Length; i++)
                blocks[reArrangedBlock.StartIndex + i] = reArrangedBlock.Value;
        }

        return blocks;
    }

    private IReadOnlyCollection<int?> GetDefragmentedBlocks()
    {
        List<int?> defragmentedDisk = _fragmentedDisk.Blocks.ToList();

        int lastIndex = defragmentedDisk.Count - 1;
        int startIndex = defragmentedDisk.IndexOf(null);

        while (startIndex < lastIndex)
        {
            while (defragmentedDisk[startIndex] is not null)
                startIndex++;

            while (defragmentedDisk[lastIndex] is null)
                lastIndex--;

            defragmentedDisk.Swap(startIndex, lastIndex);
        }

        return defragmentedDisk;
    }

    private static void CreateLeftOverSpace(Block availableSpace, Block block, List<Block> freeSpace, int index)
    {
        int newFreeSpaceStartIndex = availableSpace.StartIndex + block.Length;
        int newFreeSpaceCount = availableSpace.Length - block.Length;
        freeSpace[index] = new Block(null, newFreeSpaceStartIndex, newFreeSpaceCount);
    }

    private static (Block?, int index) GetFreeAvailableSpaceOrNull(List<Block> freeSpace, Block block)
    {
        for (int i = 0; i < freeSpace.Count; i++)
        {
            if (freeSpace[i].StartIndex > block.StartIndex)
                return (null, 0);

            if (freeSpace[i].Length < block.Length)
                continue;

            return (freeSpace[i], i);
        }

        return (null, 0);
    }

    private static Disk CreateFragmentedDisk(IReadOnlyList<int> diskMap)
    {
        List<int?> blocks = [];
        List<Block> freeSpace = [];
        List<Block> usedSpace = [];

        int fileIndex = 0;
        int diskIndex = 0;

        for (int i = 0; i < diskMap.Count; i++)
        {
            if (i % 2 == 0)
            {
                blocks.AddCount(fileIndex, diskMap[i]);
                usedSpace.Add(new(fileIndex, diskIndex, diskMap[i]));
                fileIndex++;
            }
            else
            {
                blocks.AddCount(null, diskMap[i]);
                freeSpace.Add(new(null, diskIndex, diskMap[i]));
            }

            diskIndex += diskMap[i];
        }

        return new Disk(blocks, freeSpace, usedSpace);
    }
}