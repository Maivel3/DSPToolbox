using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            List<float> temp = new List<float>();
            // int x= (InputSignal1.Samples.Count() + InputSignal2.Samples.Count());
            float[] sums = new float[InputSignal1.Samples.Count() + InputSignal2.Samples.Count()-1];
           // float[] ar;
            for (int i = 0; i < InputSignal2.Samples.Count(); i++)
            {
                for (int j = 0; j < InputSignal1.Samples.Count(); j++)
                {
                    float s1 = InputSignal1.Samples[j];
                    float s2 = InputSignal2.Samples[i];
                    sums[i + j]+= s1 * s2;
                    
                }
            }
            for (int i = 0; i < InputSignal1.Samples.Count() + InputSignal2.Samples.Count()-1; i++)
            {
                temp.Add(sums[i]);
            }
            List<int> ind = new List<int>();
            if(InputSignal1.SamplesIndices[0]<0 || InputSignal2.SamplesIndices[0]<0)
                for (int i = 0; i < InputSignal1.Samples.Count() + InputSignal2.Samples.Count() - 1; i++)
                {
                    if(InputSignal1.SamplesIndices[0]> InputSignal2.SamplesIndices[0])
                    ind.Add(i + InputSignal2.SamplesIndices[0]);
                    else
                        ind.Add(i + InputSignal1.SamplesIndices[0]);
                }
            OutputConvolvedSignal = new Signal(temp,ind, false);
        }
    }
}
