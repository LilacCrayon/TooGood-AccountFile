
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace TooGood_Test
{
    class Program
    {
        static void Main(string[] args)
        {

            List<SourceData> sourceData = GetAccountData(args[0]);

            var dataTranformer = GetTransformer(args[0]);

            var accounts = sourceData.Select(a => dataTranformer.Transform(a)).ToList();

        }



        static DataTransformer GetTransformer(string type)
        {
            switch (type)
            {
                case "1":
                    return new Tranformer1();
                case "2":
                    return new Tranformer2();
                default:
                    return null;
            }
        }

        interface DataTransformer
        {
            StandardAccount Transform(SourceData data);
        }

        class Tranformer1 : DataTransformer
        {
            public StandardAccount Transform(SourceData data)
            {
                StandardAccount account = new StandardAccount();
                account.AccountCode = data.Identifier.Split('|')[1];
                account.Name = data.Name;
                account.Type = GetAccountType(data.Type);
                account.OpenDate = DateTime.Parse(data.OpenDate);
                account.Currency = GetCurrency(data.Currency);

                return account;
            }
        }

        class Tranformer2 : DataTransformer
        {
            public StandardAccount Transform(SourceData data)
            {
                StandardAccount account = new StandardAccount();
                account.AccountCode = data.CustodianCode;
                account.Name = data.Name;
                account.Type = GetAccountType(data.Type);
                account.Currency = GetCurrency(data.Currency);

                return account;
            }
        }

        public static AccountType? GetAccountType(int? code)
        {
            switch (code)
            {
                case 1:
                    return AccountType.Trading;
                case 2:
                    return AccountType.RRSP;
                case 3:
                    return AccountType.RESP;
                case 4:
                    return AccountType.Fund;
                default:
                    return null;
            }
        }

        public static Currency? GetCurrency(string currency)
        {
            switch (currency)
            {
                case "CD":
                    return Currency.CAD;
                case "US":
                    return Currency.USD;
                case "C":
                    return Currency.CAD;
                case "U":
                    return Currency.USD;
                default:
                    return null;
            }
        }

        static List<SourceData> GetAccountFormat1()
        {
            List<SourceData> sourceData = new List<SourceData>();
            sourceData.Add(new SourceData { Identifier = "123|AbeCode", Name = "My Account", Type = 2, OpenDate = "01-01-2028", Currency = "CD" });
            sourceData.Add(new SourceData { Identifier = "456|AdeCode", Name = "Their Account", Type = 4, OpenDate = "05-05-2028", Currency = "US" });

            return sourceData;
        }

        static List<SourceData> GetAccountFormat2()
        {
            List<SourceData> sourceData = new List<SourceData>();
            sourceData.Add(new SourceData { Name = "My Account", Type = 2, Currency = "CD", CustodianCode = "XyzCode" });
            sourceData.Add(new SourceData { Name = "Their Account", Type = 4, Currency = "US", CustodianCode = "MnlCode" });

            return sourceData;
        }

        static List<SourceData> GetAccountData(string type)
        {
            switch (type)
            {
                case "1":
                    return GetAccountFormat1();
                case "2":
                    return GetAccountFormat2();
                default:
                    return new List<SourceData>();
            }
        }

    }

    class StandardAccount
    {
        public string AccountCode { get; set; }
        public string Name { get; set; }
        public AccountType? Type { get; set; }
        public DateTime OpenDate { get; set; }
        public Currency? Currency { get; set; }
    }

    class SourceData
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
        public string OpenDate { get; set; }
        public string Currency { get; set; }
        public string CustodianCode { get; set; }
    }

    public enum AccountType
    {
        Trading,
        RRSP,
        RESP,
        Fund
    }

    public enum Currency
    {
        CAD,
        USD
    }

}

