using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {

            // throw new NotImplementedException();
            int n = InputSignal.Samples.Count;
            List<float> first = new List<float>();
            List<float> second = new List<float>();
            List<float> r = new List<float>();
            List<float> l = new List<float>();
            for (int i = 0; i < n; i++)
            {
                if(i!= n - 1)
                {
                    l.Add(InputSignal.Samples[i + 1]);
                }
                else
                {
                    l.Add(0);
                }
            }
            for(int j = 0; j < n; j++)
            {
                if (j != 0)
                {
                    r.Add(InputSignal.Samples[j - 1]);   
                }
                else
                {
                    r.Add(0);
                }
            }
            for(int k = 0; k < n-1; k++)
            {
                first.Add(InputSignal.Samples[k] - r[k]);
            }
            for(int m = 0; m < n-1; m++)
            {
                second.Add(r[m] - (float)2.0 * InputSignal.Samples[m] + l[m]);
            }
            FirstDerivative = new Signal(first, true);
            SecondDerivative = new Signal(second, true);
        }
    }
}
