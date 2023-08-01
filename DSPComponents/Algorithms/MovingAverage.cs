using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }
 
        public override void Run()
        {
            int ratio = InputWindowSize / 2;
            List<float> temp = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count(); i++)
            {
                float sum = 0;
                for (int j = 0; j < InputWindowSize; j++)
                {
                    if (i >= ratio && i< InputSignal.Samples.Count()-ratio)
                        sum += InputSignal.Samples[i + j - ratio];
                }
                if (i <ratio || i >= InputSignal.Samples.Count() - ratio)
                    continue;
                    sum /= InputWindowSize;
                    temp.Add(sum);
                
            }
            OutputAverageSignal = new Signal(temp, false);
        }
    }
}
