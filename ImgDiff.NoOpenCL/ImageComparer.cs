using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;

namespace ImgDiff.NoOpenCL
{
    public static class ImageComparer
    {
        public unsafe static void Compare(int length, IntPtr imgA, IntPtr imgB, IntPtr imgC)
        {
            const int coreCount = 4;
            Parallel.For(0, coreCount, j =>
            {
                var blockSize = length / coreCount;
                byte* pA, pB, pC;
                var start = j * blockSize;
                var end = start + blockSize;
                pA = (byte*)imgA.ToPointer() + start;
                pB = (byte*)imgB.ToPointer() + start;
                pC = (byte*)imgC.ToPointer() + start;
                for (int i = start; i < end; i += 4)
                {
                    var pAI = (uint*)pA;
                    var pBI = (uint*)pB;
                    var pCI = (uint*)pC;
                    if (*pAI != *pBI)
                    {
                        *pCI = 0xffffffu ^ *pAI;
                    }
                    else
                    {
                        *pCI = 0;
                    }
                    pA += 4;
                    pB += 4;
                    pC += 4;
                }
            });
        }
    }
}
