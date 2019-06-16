using System;
using System.Collections.Generic;
using System.Linq;

namespace MAS_InterestRates
{
    public class Statistics
    {
        public Trendline CalculateLinearRegression(double[] values)
        {
            var yAxisValues = new List<double>();
            var xAxisValues = new List<double>();

            for (int i = 0; i < values.Length; i++)
            {
                yAxisValues.Add(values[i]);
                xAxisValues.Add(i + 1);
            }

            return new Trendline(yAxisValues, xAxisValues);
        }
    }

    public class Trendline
    {
        private readonly IList<double> xAxisValues;
        private readonly IList<double> yAxisValues;
        private int count;
        private double xAxisValuesSum;
        private double xxSum;
        private double xySum;
        private double yAxisValuesSum;

        public Trendline(IList<double> yAxisValues, IList<double> xAxisValues)
        {
            this.yAxisValues = yAxisValues;
            this.xAxisValues = xAxisValues;

            Initialize();
        }

        public double Slope { get; private set; }
        public double Intercept { get; private set; }
        public double Start { get; private set; }
        public double End { get; private set; }

        private void Initialize()
        {
            count = yAxisValues.Count;
            yAxisValuesSum = yAxisValues.Sum();
            xAxisValuesSum = xAxisValues.Sum();
            xxSum = 0;
            xySum = 0;

            for (int i = 0; i < this.count; i++)
            {
                xySum += (xAxisValues[i] * yAxisValues[i]);
                xxSum += (xAxisValues[i] * xAxisValues[i]);
            }

            Slope = CalculateSlope();
            Intercept = CalculateIntercept();
            Start = CalculateStart();
            End = CalculateEnd();
        }

        private double CalculateSlope()
        {
            try
            {
                return ((count * xySum) - (xAxisValuesSum * yAxisValuesSum)) / ((count * xxSum) - (xAxisValuesSum * xAxisValuesSum));
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }

        private double CalculateIntercept()
        {
            return (yAxisValuesSum - (Slope * xAxisValuesSum)) / count;
        }

        private double CalculateStart()
        {
            return (Slope * xAxisValues.First()) + Intercept;
        }

        private double CalculateEnd()
        {
            return (Slope * xAxisValues.Last()) + Intercept;
        }
    }
}
