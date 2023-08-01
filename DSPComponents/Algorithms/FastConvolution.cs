using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            //1-init new length
            int N1 = InputSignal1.Samples.Count;
            int N2 = InputSignal2.Samples.Count;
            int length = N1 + N2 - 1;



            float realmultOp;
            float imgmultOp;
            List<float> multiOp = new List<float>();
            List<float> multiOp2 = new List<float>();
            Complex complex1;
            Complex complex2;
            List<Complex> complex_list = new List<Complex>();
            //1.1-add ZEROS

            for (int i = N1; i < length; i++)
            {
                InputSignal1.Samples.Add(0);
                InputSignal1.SamplesIndices.Add(i);
            }
            for (int i = N2; i < length; i++)
            {
                InputSignal2.Samples.Add(0);
                InputSignal2.SamplesIndices.Add(i);
            }

            //signal 1 init
            List<float> signal1Phase = new List<float>();
            List<float> signal1Amp = new List<float>();
            float signal1Real;
            float signal1Img;
            //signal2 init
            List<float> signal2Phase = new List<float>();
            List<float> signal2Amp = new List<float>();
            float signa2Real;
            float signa2Img;

            //2-DFT (transform from samples(time-domain) to frequencies(frequency-domain)
            DiscreteFourierTransform DFT1 = new DiscreteFourierTransform();
            DFT1.InputTimeDomainSignal = InputSignal1;
            DFT1.Run();
            signal1Phase = DFT1.OutputFreqDomainSignal.FrequenciesPhaseShifts;
            signal1Amp = DFT1.OutputFreqDomainSignal.FrequenciesAmplitudes;

            DiscreteFourierTransform DFT2 = new DiscreteFourierTransform();
            DFT2.InputTimeDomainSignal = InputSignal2;
            DFT2.Run();
            signal2Phase = DFT2.OutputFreqDomainSignal.FrequenciesPhaseShifts;
            signal2Amp = DFT2.OutputFreqDomainSignal.FrequenciesAmplitudes;

            //real and imaginary 
            for (int i = 0; i < length; i++)
            {
                //signal 1
                signal1Real = signal1Amp[i] * (float)Math.Cos(signal1Phase[i]);
                signal1Img = signal1Amp[i] * (float)Math.Sin(signal1Phase[i]);
                complex1 = new Complex(signal1Real, signal1Img);
                //signal 2
                signa2Real = signal2Amp[i] * (float)Math.Cos(signal2Phase[i]);
                signa2Img = signal2Amp[i] * (float)Math.Sin(signal2Phase[i]);
                complex2 = new Complex(signa2Real, signa2Img);

                complex_list.Add(Complex.Multiply(complex1, complex2));
            }
            //X
            for (int i = 0; i < length; i++)
            {
                float real_p2 = (float)Math.Pow(complex_list[i].Real, 2);
                float img_p2 = (float)Math.Pow(complex_list[i].Imaginary, 2);
                multiOp.Add((float)(Math.Sqrt(real_p2 + img_p2)));
                multiOp2.Add((float)Math.Atan2(complex_list[i].Imaginary, complex_list[i].Real));
            }
            Signal result;
            result = new Signal(false, multiOp, multiOp, multiOp2);
            InverseDiscreteFourierTransform IDFT = new InverseDiscreteFourierTransform();
            IDFT.InputFreqDomainSignal = result;
            IDFT.Run();
            OutputConvolvedSignal = new Signal(IDFT.OutputTimeDomainSignal.Samples, false);

        }
    }
}
