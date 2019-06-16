using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace MAS_InterestRates
{
    public class InterestRatesMonthly
    {
        private static string _resourceId = ConfigurationManager.AppSettings["ResourceId_Monthly"];
        private static string _address = ConfigurationManager.AppSettings["MAS_URL"];

        public void CompareFinancialRates(string fromDate, string toDate)
        {
            try
            {
                int offset = 0;
                int totalRecords = 0;
                int limit = 100;
                int pendingRecords = 0;
                DateTime dateTime = DateTime.Now;
                string outputFile = string.Concat("RateComparision_", fromDate, "_", toDate, ".txt");
                if (File.Exists(outputFile))
                    File.Delete(outputFile);
                Helper.WriteToFile(outputFile, string.Concat("YYYY-MM|Bank_Savings_Rate|FC_Savings_Rate"));
                var result = Helper.GetData(_address, _resourceId, fromDate, toDate).GetAwaiter().GetResult();
                if (result != null && result.Success.Equals("true"))
                {
                    if (result.Result != null && result.Result.Total > 0)
                    {
                        totalRecords = result.Result.Total;
                        if (totalRecords > 100)
                        {
                            foreach (var records in result.Result.Records)
                            {
                                Console.WriteLine(string.Concat(records.End_of_month, "|", records.Banks_savings_deposits, "|", records.Fc_savings_deposits));
                                Helper.WriteToFile(outputFile, string.Concat(records.End_of_month, "|", records.Banks_savings_deposits, "|", records.Fc_savings_deposits));
                            }
                            pendingRecords = totalRecords - 100;
                            while (pendingRecords > 0)
                            {
                                limit = pendingRecords > 100 ? 100 : pendingRecords;
                                offset += 100;
                                var pagingResult = Helper.GetData(_address, _resourceId, fromDate, toDate, limit.ToString(), offset.ToString()).GetAwaiter().GetResult();

                                if (pagingResult != null && pagingResult.Success.Equals("true"))
                                {
                                    if (pagingResult.Result != null)
                                    {
                                        limit = result.Result.Limit;
                                        pendingRecords -= limit;

                                        foreach (var records in pagingResult.Result.Records)
                                        {
                                            Console.WriteLine(string.Concat(records.End_of_month, "|", records.Banks_savings_deposits, "|", records.Fc_savings_deposits));
                                            Helper.WriteToFile(outputFile, string.Concat(records.End_of_month, "|", records.Banks_savings_deposits, "|", records.Fc_savings_deposits));
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No data avaialble for the date range");
                                    break;
                                }

                            }
                        }
                        else
                        {
                            foreach (var records in result.Result.Records)
                            {
                                Console.WriteLine(string.Concat(records.End_of_month, "|", records.Banks_savings_deposits, "|", records.Fc_savings_deposits));
                                Helper.WriteToFile(outputFile, string.Concat(records.End_of_month, "|", records.Banks_savings_deposits, "|", records.Fc_savings_deposits));
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data avaialble for the date range");
                    }
                }
                else
                {
                    Console.WriteLine("Error getting data. Try again.");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error geting data. Try again.");
            }
        }

        public void CompareOverallAverage(string fromDate, string toDate)
        {
            try
            {
                int offset = 0;
                int totalRecords = 0;
                int limit = 100;
                int pendingRecords = 0;
                DateTime dateTime = DateTime.Now;
                string outputFile = string.Concat("Average_", fromDate, "_", toDate, ".txt");
                if (File.Exists(outputFile))
                    File.Delete(outputFile);
                var result = Helper.GetData(_address, _resourceId, fromDate, toDate).GetAwaiter().GetResult();

                double bankSavingsRate = 0.0;
                double fcSavingsRate = 0.0;

                if (result != null && result.Success.Equals("true"))
                {
                    if (result.Result != null && result.Result.Total > 0)
                    {
                        totalRecords = result.Result.Total;
                        if (totalRecords > 100)
                        {
                            foreach (var records in result.Result.Records)
                            {
                                bankSavingsRate += records.Banks_savings_deposits;
                                fcSavingsRate += records.Fc_savings_deposits;
                            }
                            pendingRecords = totalRecords - 100;
                            while (pendingRecords > 0)
                            {
                                limit = pendingRecords > 100 ? 100 : pendingRecords;
                                offset += 100;
                                var pagingResult = Helper.GetData(_address, _resourceId, fromDate, toDate, limit.ToString(), offset.ToString()).GetAwaiter().GetResult();

                                if (pagingResult != null && pagingResult.Success.Equals("true"))
                                {
                                    if (pagingResult.Result != null)
                                    {
                                        limit = result.Result.Limit;
                                        pendingRecords -= limit;

                                        foreach (var records in pagingResult.Result.Records)
                                        {
                                            bankSavingsRate += records.Banks_savings_deposits;
                                            fcSavingsRate += records.Fc_savings_deposits;
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No data avaialble for the date range");
                                    break;
                                }

                            }
                        }
                        else
                        {
                            totalRecords = result.Result.Total;
                            foreach (var records in result.Result.Records)
                            {
                                bankSavingsRate += records.Banks_savings_deposits;
                                fcSavingsRate += records.Fc_savings_deposits;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data avaialble for the date range");
                    }
                }
                else
                {
                    Console.WriteLine("Error getting data. Try again.");
                }

                double avgBankSavingsRate = 0.0;
                double avgFCSavingsRate = 0.0;

                if (totalRecords != 0)
                {
                    avgBankSavingsRate = bankSavingsRate / totalRecords;
                    avgFCSavingsRate = fcSavingsRate / totalRecords;
                }

                Console.WriteLine("AverageBankSavingsRage", "|", "AverageFCSavingsRate");
                Console.WriteLine(Math.Round(avgBankSavingsRate, 2) + "|" + Math.Round(avgFCSavingsRate, 2));

                Helper.WriteToFile(outputFile, string.Concat("AverageBankSavingsRage", "|", "AverageFCSavingsRate"));
                Helper.WriteToFile(outputFile, string.Concat(Math.Round(avgBankSavingsRate, 2), "|", Math.Round(avgFCSavingsRate, 2)));
            }
            catch (Exception)
            {
                Console.WriteLine("Error geting data. Try again.");
            }
        }

        public void CheckInterestTrends(string fromDate, string toDate)
        {
            try
            {
                int offset = 0;
                int totalRecords = 0;
                int limit = 100;
                int pendingRecords = 0;
                DateTime dateTime = DateTime.Now;
                string outputFile = string.Concat("SlopeTrend", fromDate, "_", toDate, ".txt");
                if (File.Exists(outputFile))
                    File.Delete(outputFile);
                var result = Helper.GetData(_address, _resourceId, fromDate, toDate).GetAwaiter().GetResult();

                List<double> bankSavingRate = new List<double>();
                if (result != null && result.Success.Equals("true"))
                {
                    if (result.Result != null && result.Result.Total > 0)
                    {
                        totalRecords = result.Result.Total;
                        if (totalRecords > 100)
                        {
                            foreach (var records in result.Result.Records)
                            {
                                bankSavingRate.Add(records.Banks_savings_deposits);
                            }
                            pendingRecords = totalRecords - 100;
                            while (pendingRecords > 0)
                            {
                                limit = pendingRecords > 100 ? 100 : pendingRecords;
                                offset += 100;
                                var pagingResult = Helper.GetData(_address, _resourceId, fromDate, toDate, limit.ToString(), offset.ToString()).GetAwaiter().GetResult();

                                if (pagingResult != null && pagingResult.Success.Equals("true"))
                                {
                                    if (pagingResult.Result != null)
                                    {
                                        limit = result.Result.Limit;
                                        pendingRecords -= limit;

                                        foreach (var records in pagingResult.Result.Records)
                                        {
                                            bankSavingRate.Add(records.Banks_savings_deposits);
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No data avaialble for the date range");
                                    break;
                                }

                            }
                        }
                        else
                        {
                            totalRecords = result.Result.Total;
                            foreach (var records in result.Result.Records)
                            {
                                bankSavingRate.Add(records.Banks_savings_deposits);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data avaialble for the date range");
                    }
                }
                else
                {
                    Console.WriteLine("Error getting data. Try again.");
                }

                Trendline trendline = null;
                if (bankSavingRate .Count > 0)
                {
                    trendline = new Statistics().CalculateLinearRegression(bankSavingRate.ToArray());
                }

                Console.WriteLine("Slope" + "|" + "Intercept" + "|" + "Start" + "|" + "End");
                Console.WriteLine(Math.Round(trendline.Slope, 4) + "|" + Math.Round(trendline.Intercept, 4) + "|" + Math.Round(trendline.Start, 4) + "|" + Math.Round(trendline.End, 4));

                Helper.WriteToFile(outputFile, string.Concat("Slope", "|", "Intercept", "|", "Start", "|", "End"));
                Helper.WriteToFile(outputFile, string.Concat(Math.Round(trendline.Slope, 4), "|", Math.Round(trendline.Intercept, 4), "|", Math.Round(trendline.Start, 4), "|", Math.Round(trendline.End, 4)));
            }
            catch (Exception)
            {
                Console.WriteLine("Error geting data. Try again.");
            }
        }

    }
}
