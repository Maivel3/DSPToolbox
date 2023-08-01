using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }



        public override void Run()
        {
            DirectConvolution conv = new DirectConvolution();
            List<float> listSamples = new List<float>();
            List<int> index = new List<int>();
            double HD = 0, W = 0;

            double N = new double();
            double trans = InputTransitionBand / InputFS;
            double Fc;
            int x = new int();
            double f1, f2;
            int[] stopband = new int[4];
            stopband[0] = 21;
            stopband[1] = 44;
            stopband[2] = 53;
            stopband[3] = 74;

            if (InputStopBandAttenuation > stopband[2] && InputStopBandAttenuation < stopband[3])
            {

                N = 5.5 / trans;
                x = 4;
            }
            else if (InputStopBandAttenuation < stopband[2] && InputStopBandAttenuation > stopband[1])
            {
                N = 3.3 / trans;
                x = 3;

            }
            else if (InputStopBandAttenuation > stopband[0] && InputStopBandAttenuation <= stopband[1])
            {
                N = 3.1 / trans;
                x = 2;
            }
            else if (InputStopBandAttenuation <= stopband[0])
            {
                N = 0.9 / trans;
                x = 1;
            }
            //N = (Math.Round(N, 1));
            N = (int)N;
            if (N % 2 == 0)
                N++;
            for (int n = (int)-N / 2; n < N / 2; n++)
            {
                index.Add((int)n);
                int j = Math.Abs(n);

                if (x == 4)
                    W = (0.42 + 0.5 * (Math.Cos(2 * (Math.PI) * j / (N - 1))) + (0.08 * (Math.Cos(4 * Math.PI * j / (N - 1)))));

                else if (x == 3)
                    W = (0.54 + 0.46 * Math.Cos(2 * Math.PI * j / N));

                else if (x == 2)
                    W = (0.5 + 0.5 * (Math.Cos(2 * Math.PI * j / N)));

                else if (x == 1)
                    W = 1;


                if (InputFilterType == FILTER_TYPES.HIGH)
                {
                    Fc = (double)(InputCutOffFrequency-InputTransitionBand / 2 ) / InputFS;
                    if (n == 0)
                    {
                        HD = (1 - (2 * Fc));
                    }
                    else
                    {
                        HD = (-2 * Fc * (Math.Sin(j * 2 * Math.PI * Fc)) / (j * 2 * (Math.PI) * Fc));

                    }
                    listSamples.Add((float)(W) * (float)(HD));
                }
                else if (InputFilterType == FILTER_TYPES.LOW)
                {
                    Fc = (float)(InputTransitionBand / 2 + InputCutOffFrequency) / InputFS;
                    if (n == 0)
                    {
                        HD = (2 * Fc);
                    }
                    else
                    {
                        HD = ((2 * Fc / (j * 2 * Math.PI * Fc)) * Math.Sin(j * 2 * Math.PI * Fc));
                    }
                    double y = HD * W;
                    listSamples.Add((float)y);
                }
                else if (InputFilterType == FILTER_TYPES.BAND_PASS)
                {
                    f1 = (double)(InputF1 - InputTransitionBand / 2) / InputFS;
                    f2 = (double)(InputF2 + InputTransitionBand / 2) / InputFS;

                    if (n == 0)
                    {
                        HD = (2 * (f2 - f1));
                    }
                    else
                    {
                        HD = ((2 * f2 / (j * 2 * Math.PI * f2)) * (Math.Sin(j * 2 * Math.PI * (f2))) -
                              (((2 * f1) / (j * 2 * Math.PI * f1)) * Math.Sin(j * 2 * Math.PI * f1)));
                    }
                    listSamples.Add((float)(W * HD));
                }
                else if (InputFilterType == FILTER_TYPES.BAND_STOP)
                {
                    f1 = (double)(InputF1 + InputTransitionBand / 2) / InputFS;
                    f2 = (double)(InputF2 - InputTransitionBand / 2) / InputFS;
                    if (n == 0)
                    {
                        HD = (1 - 2 * (f2 - f1));
                    }
                    else
                    {
                        HD = ((2 * f1 * (Math.Sin(j * 2 * (Math.PI) * f1)) / (j * 2 * (Math.PI) * f1)) - ((2 * f2 * (Math.Sin(j * 2 * (Math.PI) * f2)) / (j * 2 * (Math.PI) * f2))));

                    }
                    listSamples.Add((float)(HD * W));
                }



            }

            OutputHn = new Signal(listSamples, index, false);
            conv.InputSignal1 = OutputHn;
            conv.InputSignal2 = InputTimeDomainSignal;
            conv.Run();
            OutputYn = conv.OutputConvolvedSignal;
            StreamWriter WriteInFile = new StreamWriter("FIRCoefficients.txt");
            for (int i = 0; i < OutputHn.Samples.Count(); i++)
            {
                WriteInFile.WriteLine("Coefficient #" + i + ":" + OutputHn.Samples[i]);
            }
            WriteInFile.WriteLine("");
            WriteInFile.Close();

        }
    }
}