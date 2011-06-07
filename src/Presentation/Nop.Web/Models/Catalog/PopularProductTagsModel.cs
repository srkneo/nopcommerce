﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.Catalog
{
    public class PopularProductTagsModel : BaseNopModel
    {
        public PopularProductTagsModel()
        {
            Tags = new List<ProductTagModel>();
        }

        public int GetFontSize(ProductTagModel productTag)
        {
            double mean = 0;
            var itemWeights = new List<double>();
            foreach (var tag in Tags)
                itemWeights.Add(tag.ProductCount);
            double stdDev = StdDev(itemWeights, out mean);

            return GetFontSize(productTag.ProductCount, mean, stdDev);
        }

        protected int GetFontSize(double weight, double mean, double stdDev)
        {
            double factor = (weight - mean);

            if (factor != 0 && stdDev != 0) factor /= stdDev;

            return (factor > 2) ? 150 :
                (factor > 1) ? 120 :
                (factor > 0.5) ? 100 :
                (factor > -0.5) ? 90 :
                (factor > -1) ? 85 :
                (factor > -2) ? 80 :
                75;
        }

        protected double Mean(IEnumerable<double> values)
        {
            double sum = 0;
            int count = 0;

            foreach (double d in values)
            {
                sum += d;
                count++;
            }

            return sum / count;
        }

        protected double StdDev(IEnumerable<double> values, out double mean)
        {
            mean = Mean(values);
            double sumOfDiffSquares = 0;
            int count = 0;

            foreach (double d in values)
            {
                double diff = (d - mean);
                sumOfDiffSquares += diff * diff;
                count++;
            }

            return Math.Sqrt(sumOfDiffSquares / count);
        }


        public IList<ProductTagModel> Tags { get; set; }
    }
}