using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

          public override void Run()
        {


            List<float> phase = new List<float>();
            List<float> amp = new List<float>();
            float phi = 0;
            List<float> Xlist = new List<float>();
            List<float> Ylist = new List<float>();
            //Given sequence of Numbers
            for (int i=0;i<InputFreqDomainSignal.FrequenciesAmplitudes.Count;i++)
            {
                float res1, res2;
                float X = InputFreqDomainSignal.FrequenciesAmplitudes[i];
                float Y = InputFreqDomainSignal.FrequenciesPhaseShifts[i];
                res1 = (X * (float)Math.Cos(Y));
                res2 = (float)Math.Ceiling(X * (float)Math.Sin(Y));
                Xlist.Add(res1);
                Ylist.Add(res2);
            }
            

            //  List<float> Y = InputTimeDomainSignal.FrequenciesAmplitudes;
            float N;
                N= InputFreqDomainSignal.FrequenciesAmplitudes.Count;


            for (int k = 0; k < N; k++)
            {
                float result= 0;
               // float im = 0;
                //The discrete Fourier transform, lists of Re & Im instead of a+ib
                for (int n = 0; n < N; n++)
                {
                    phi = (2 * (float)Math.PI * k * n) / N;
                    if(Math.Cos(phi)!=0)
                    result += Xlist[n] * (float)Math.Cos(phi);
                    if(Math.Sin(phi)!=0)
                    result -= Ylist[n] * (float)Math.Sin(phi);
                    if (Math.Cos(phi) == 0 && Math.Sin(phi) == 0)
                        result += Xlist[n];
                }
                //  var ampp = Math.Sqrt(re * re + im * im);
                // var phasse = Math.Atan2(im, re);

                //  phase.Add((float));
                result = result / N;
                result=(float)Math.Round(result);
                amp.Add(result);
            }
            for (int i = 0; i < amp.Count; i++)
            {
                Console.Write(amp[i]);
                Console.Write(" ");
            }
            OutputTimeDomainSignal = new Signal(amp, false);
           // InputFreqDomainSignal.Samples = amp;
           // OutputFreqDomainSignal.FrequenciesPhaseShifts = phase;
        }
    }
}
