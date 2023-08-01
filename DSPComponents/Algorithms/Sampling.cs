using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }
        public Signal FinalOutputSignal { get; set; }
        public int M { get; set; } //downsampling factor
        public int L { get; set; } //upsampling factor
        public List<float> OutputList = new List<float>();
        public List<float> TempList = new List<float>();


        public override void Run()
        {
            OutputSignal = new Signal(new List<float>(), false);

            List<float> sample = new List<float>();
            int count = InputSignal.Samples.Count;
            int N;


            if (L == 0 && M == 0)
            {
                Console.WriteLine("Error...");
                return;
            }

            if (M == 0 && L > 0)
            {

                for (int i = 0; i < count; i++)
                {
                    sample.Add(InputSignal.Samples[i]);
                    for (int j = 0; j < L - 1; j++)
                    {
                        sample.Add(0);
                    }
                }
                N = count * L; // N = 53
                for (int i = 0, n = (int)-N / 2; i < N; i++, n++) // -26 < n < 26
                {
                    OutputSignal.SamplesIndices.Add(n);
                }

                //low bass filter
                FIR c = new FIR();
                c.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                c.InputFS = 8000;
                c.InputStopBandAttenuation = 50;
                c.InputCutOffFrequency = 1500;
                c.InputTransitionBand = 500;
                c.InputTimeDomainSignal = new Signal(sample, false);
                c.Run();
                List<float> s = new List<float>();
                s = c.OutputYn.Samples;
               s.RemoveAt(s.Count - 1);
                s.RemoveAt(s.Count - 1);
                OutputSignal.Samples = c.OutputYn.Samples;
               /* for (int i = 0; i < OutputSignal.Samples.Count; i++)
                {
                    Console.WriteLine(OutputSignal.Samples[i]);

                }*/

            }
            else if (M > 0 && L == 0)
            {
                //low bass filter
               
                FIR c = new FIR();
                c.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                c.InputFS = 8000;
                c.InputStopBandAttenuation = 50;
                c.InputCutOffFrequency = 1500;
                c.InputTransitionBand = 500;
                c.InputTimeDomainSignal = new Signal(InputSignal.Samples, false);
                c.Run();
                List<float> s = new List<float>();
                for (int i = 0; i < c.OutputYn.Samples.Count; i += M)
                {
                    s.Add(c.OutputYn.Samples[i]);
                }
               
                N = count / M;

                for (int i = 0, n = (int)-N / 2; i < N; i++, n++)
                {
                    OutputSignal.SamplesIndices.Add(n);
                }

                OutputSignal.Samples = s;
               /* for (int i = 0; i < OutputSignal.Samples.Count; i++)
                {
                    Console.WriteLine(OutputSignal.Samples[i]);

                }*/
            }
            else if (M > 0 && L > 0)
            {

                for (int i = 0; i < count; i++)
                {
                    sample.Add(InputSignal.Samples[i]);
                    for (int j = 0; j < L - 1; j++)
                    {
                        sample.Add(0);
                    }
                    //  sample[i] =(float) Math.Round(sample[i], 8, MidpointRounding.AwayFromZero);
                }


                N = count * L;
                for (int i = 0, n = (int)-N / 2; i < N; i++, n++)
                {
                    OutputSignal.SamplesIndices.Add(n);
                }
                //low bass filter
                FIR c = new FIR();
                c.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                c.InputFS = 8000;
                c.InputStopBandAttenuation = 50;
                c.InputCutOffFrequency = 1500;
                c.InputTransitionBand = 500;
                c.InputTimeDomainSignal = new Signal(sample, false);
                c.Run();

                List<float> s = new List<float>();
                for (int i = 0; i < c.OutputYn.Samples.Count; i += M)
                {
                    s.Add(c.OutputYn.Samples[i]);
                }
                N = count / M;
                for (int i = 0, n = (int)-N / 2; i < N; i++, n++)
                {
                    OutputSignal.SamplesIndices.Add(n);
                }
                s.RemoveAt(s.Count - 1);
                OutputSignal.Samples = s;
                for (int i = 0; i < OutputSignal.Samples.Count; i++)
                {
                    Console.WriteLine(OutputSignal.Samples[i]);

                }
            }








        }
    }
}


