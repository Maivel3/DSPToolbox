using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {
            List<int> res = new List<int>();
            List<float> val = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count(); i++)
            {
                res.Add(InputSignal.SamplesIndices[i] + ShiftingValue);
                val.Add(InputSignal.Samples[i]);
            }
            OutputShiftedSignal = new Signal(val,res,true);
        }
    }
}
