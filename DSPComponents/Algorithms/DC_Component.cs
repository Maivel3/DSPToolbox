using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            // throw new NotImplementedException();
            List<float> output = new List<float>();
            float avg = 0, outputs=0;
            for(int i = 0; i < InputSignal.Samples.Count; i++)
            {
                avg = avg + InputSignal.Samples[i];
            }
            avg = avg / InputSignal.Samples.Count;
            for(int j = 0; j < InputSignal.Samples.Count; j++)
            {
                outputs = InputSignal.Samples[j] - avg;
                output.Add(outputs);
            }
            OutputSignal = new Signal(output, false);
        }
    }
}
