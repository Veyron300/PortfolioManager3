using System;
using System.Collections.Generic;
using System.Linq;
using PortfolioManager3.InstrumentDataFactory;

namespace PortfolioManager3
{
    /// <summary>
    /// Static class containing analytic methods
    /// </summary>
    public static class PortfolioAnalytics
      {
        public static IList<DateTime> AnalyticDates { get; set; }

        public static IList<decimal> AnalyticValues { get; set; }

        public static IList<Analytic> FinalAnalyticCollection { get; set; }

        private const double tradingDaysAdjustmentFactor = 0.685;

        public static decimal CalculatePositionDuration(DateTime? entryDate)
        {
            
            var Days = DateTime.Now - (DateTime)entryDate;
            decimal positionDuration = Math.Round(Convert.ToDecimal((Days.TotalDays) * tradingDaysAdjustmentFactor), 0);
            if (positionDuration < 60)
            {
                return positionDuration;
            }

            else
            {
                return 60;
            }
        }

        public static decimal CalculatePortFolioProfit(IEnumerable<Position> positions)
        {
                return positions.Sum(pprof => pprof.Profit); 
        }

        public static decimal CalculatePortfolioReturn(IEnumerable<Position> positions)
        {
            var portfolioSize = positions.Sum(ptfosize => ptfosize.CurrentOpenPositionValue);
            var WeightedAverageReturn = new List<decimal>();
            decimal portfolioReturn;

            foreach (Position position in positions)
            {
                WeightedAverageReturn.Add((position.CurrentOpenPositionValue / portfolioSize) * position.ReturnRate);
            }

            if (WeightedAverageReturn.Count() > 0)
            {
                portfolioReturn = Math.Round(WeightedAverageReturn.Average(), 2);
            }

            else
            {
                portfolioReturn = 0;
            }

            return portfolioReturn;


        }

        public static decimal CalculatePositionProfit(Position position)
        {
            return (position.CurrentPrice - position.EntryPrice) * position.Units;
        }

        public static decimal CalculateCurrentOpenPositionValue(Position position)
        {
            return (position.CurrentPrice * position.Units);
        }
      
        public static decimal CalculatePositionProfit(decimal entryPrice, decimal units, decimal currentPrice)
        {
            return (currentPrice - entryPrice) * units;
        }

        public static decimal CalculatePositionReturn(Position position)
        {
            decimal daysToDate = CalculatePositionDuration(position.EntryDate);
            decimal profit = CalculatePositionProfit(position);
            decimal entrySize = position.EntryPrice * position.Units;
            return Math.Round((((profit / entrySize) * 100)), 2); 
           
        }

        public static decimal CalculatePositionReturn(DateTime? entryDate, decimal entryPrice, decimal units, decimal currentPrice)
        {

            decimal daysToDate = CalculatePositionDuration(entryDate);
            decimal profit = (currentPrice - entryPrice) * units;
            decimal entrySize = entryPrice * units;
            return Math.Round((((profit / entrySize) / daysToDate * 100) * 365), 2);
           
        }

        public static decimal CalculateDailyPositionReturn(decimal entryPrice, decimal units, decimal currentPrice)
        {
            decimal profit = (currentPrice - entryPrice) * units;
            decimal entrySize = entryPrice * units;
            decimal dailyReturn = Math.Round(((profit / entrySize)*100),2);
            return dailyReturn;
        }

        /// <summary>
        /// Functional method to calculate a historical analytic
        /// </summary>
        /// <param name="position"></param>
        /// <param name="periodQuotes"></param>
        /// <param name="historicalAnalytic"></param>
        /// <returns></returns>
        public static IList<Analytic> CalculateHistoricalAnalytic(this Position position, IEnumerable<SourceInstrumentQuote> periodQuotes, Func<decimal, decimal, decimal, decimal> historicalAnalytic)
        {
            
            List<Analytic> result = new List<Analytic>();

            foreach (var period in periodQuotes)
            {
                Analytic calculatedAnalytic = new Analytic();
                calculatedAnalytic.Date = period.Date;
                calculatedAnalytic.CalculatedAnalytic = historicalAnalytic(position.EntryPrice,position.Units,Convert.ToDecimal(period.Close));
                result.Add(calculatedAnalytic);
            }

            return result;

        }

        /// <summary>
        /// Combines lists of returns for each instrument into a single list using parallel execution
        /// </summary>
        /// <param name="analyticCollections"></param>
        /// <returns></returns>
        public static IEnumerable<Analytic> CombineAnalyticsInParallel(IList<IList<Analytic>> analyticCollections)
        {
            AnalyticDates = new List<DateTime>();
            AnalyticValues = new List<decimal>();
            FinalAnalyticCollection = new List<Analytic>();
            IEnumerable<Analytic> combinedAnalyticCollection = new List<Analytic>();
            combinedAnalyticCollection = analyticCollections.Aggregate(combinedAnalyticCollection, (current, list) => current.Concat(list)).ToList();
            var combineByDate = combinedAnalyticCollection.GroupBy(d => d.Date);
         
             foreach(var date in combineByDate)
            {
                AnalyticDates.Add(date.Key);
                foreach(var value in date)
                {
                    AnalyticValues.Add(value.CalculatedAnalytic);
                }

                Analytic SummedAnalytic = new Analytic();

                SummedAnalytic.Date = date.Key;
                SummedAnalytic.CalculatedAnalytic = Math.Round(AnalyticValues.Sum(),2);

                FinalAnalyticCollection.Add(SummedAnalytic);
                AnalyticValues.Clear();
            }

            return FinalAnalyticCollection.AsParallel();
        }
        

        
    }
}
