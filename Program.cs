using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Text;

namespace StringPerformace
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<QueryString>();
        }
    }


    [MemoryDiagnoser]
    public class QueryString
    {
       
        [Benchmark]
        public void RunBenchConcat()
        {
            for (int i = 0; i < 100; i++)
            {
                CreateQueryStringConcat(Guid.NewGuid().ToString());
            }
        }

        [Benchmark]
        public void RunBenchBuilder()
        {
            for (int i = 0; i < 100; i++)
            {
                CreateQueryStringBuilder(Guid.NewGuid().ToString());
            }
        }

        public static string CreateQueryStringConcat(string trimmedSchema)
            => $@"SELECT TOP 1 C.* FROM [{trimmedSchema}].[Customers] C 
                INNER JOIN [{trimmedSchema}].[PaymentForms] PF ON PF.CustomerId = C.Id 
                INNER JOIN [{trimmedSchema}].[PaymentConditions] PC ON PC.CustomerId = C.Id
                 AND PC.CodePayment = PF.CodePayment 
                 AND PC.CodePaymentTerm = @codePaymentTerm 
                 AND PC.Status = 1 WHERE C.Status = 1 
                 AND PF.CodePayment = @codePayment 
                 AND PF.Status = 1";


        public static string CreateQueryStringBuilder(string trimmedSchema)
        {
            var query = new StringBuilder();
            query.Append(" SELECT ");
            query.Append("     TOP 1 C.* ");
            query.Append(" FROM ");
            query.Append($"     [{trimmedSchema}].[Customers] C ");
            query.Append($" INNER JOIN [{trimmedSchema}].[PaymentForms] PF ON ");
            query.Append("     PF.CustomerId = C.Id ");
            query.Append($" INNER JOIN [{trimmedSchema}].[PaymentConditions] PC ON ");
            query.Append("     PC.CustomerId = C.Id ");
            query.Append("     AND PC.CodePayment = PF.CodePayment ");
            query.Append("     AND PC.CodePaymentTerm = @codePaymentTerm ");
            query.Append("     AND PC.Status = 1 "); 
            query.Append(" WHERE ");
            query.Append("     C.Status = 1 ");
            query.Append("     AND PF.CodePayment = @codePayment ");
            query.Append("     AND PF.Status = 1 ");
            return query.ToString();
        }
    }
}