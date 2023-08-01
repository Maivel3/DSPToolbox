using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
            Signal output = new Signal(false, new List<float>(), new List<float>(), new List<float>());
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();


            if (InputSignal2 == null) { InputSignal2 = new Signal(InputSignal1.Samples, false); }

            float sumIN1 = 0;
            float sumIN2 = 0;
            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                sumIN1 += (float)Math.Pow(InputSignal1.Samples[i], 2);
                sumIN2 += (float)Math.Pow(InputSignal2.Samples[i], 2);
            }
            float norm = (float)(Math.Sqrt(sumIN1 * sumIN2) / InputSignal1.Samples.Count);

            dft.InputTimeDomainSignal = InputSignal1;
            dft.Run();
            Signal signal1 = new Signal(false, null,
                new List<float>(dft.OutputFreqDomainSignal.FrequenciesAmplitudes),
                new List<float>(dft.OutputFreqDomainSignal.FrequenciesPhaseShifts));
            dft.InputTimeDomainSignal = InputSignal2;
            dft.Run();
            Signal signal2 = new Signal(false, new List<float>(),
                new List<float>(dft.OutputFreqDomainSignal.FrequenciesAmplitudes),
                new List<float>(dft.OutputFreqDomainSignal.FrequenciesPhaseShifts));


            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                Complex c1 = Complex.FromPolarCoordinates(signal1.FrequenciesAmplitudes[i], -1 * signal1.FrequenciesPhaseShifts[i]),
                c2 = Complex.FromPolarCoordinates(signal2.FrequenciesAmplitudes[i], signal2.FrequenciesPhaseShifts[i]),
                result = Complex.Multiply(c1, c2);
                output.FrequenciesAmplitudes.Add((float)result.Magnitude);
                output.FrequenciesPhaseShifts.Add((float)result.Phase);
                output.Frequencies.Add(0);
            }


            idft.InputFreqDomainSignal = output;
            idft.Run();
            output = idft.OutputTimeDomainSignal;


            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                OutputNonNormalizedCorrelation.Add(output.Samples[i] / InputSignal1.Samples.Count);
                OutputNormalizedCorrelation.Add((output.Samples[i] / InputSignal1.Samples.Count) / norm);
            }
        }
    }
}