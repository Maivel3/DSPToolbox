using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MultiplySignalByConstant : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputConstant { get; set; }
        public Signal OutputMultipliedSignal { get; set; }

        public override void Run()
        {
            float output;
            List<float> outputs = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                output = InputSignal.Samples[i]*InputConstant ;
                outputs.Add(output);
            }
            OutputMultipliedSignal = new Signal(outputs, true);
        }
    }
}
