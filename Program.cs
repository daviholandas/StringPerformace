using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
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
        string param;
        
        [Benchmark]
        public void RunBenchConcat()
        {
            for (int i = 0; i < 100; i++)
            {
                CreateQueryStringConcat();
                param = Guid.NewGuid().ToString();
            }
        }

        [Benchmark]
        public void RunBenchBuilder()
        {
            for (int i = 0; i < 100; i++)
            {
                CreateQueryStringBuilder();
                param = Guid.NewGuid().ToString();
            }
        }

       [Benchmark]
        public string CreateQueryStringConcat()
            => $@"SELECT 
                IncidentId, QueueId, QueueName, Name, CreatedDate, UpdateDate
                from vw_Incidents_Report 
                WHERE
                ((CreatedDate >= @CreatedDateFrom or @CreatedDateFrom IS NULL) AND (CreatedDate <= @CreatedDateTo or @CreatedDateTo IS NULL))
                AND ((UpdateDate >= @UpdatedDateFrom or @UpdatedDateFrom IS NULL) AND (UpdateDate <= @UpdatedDateTo or @UpdatedDateTo IS NULL))
                AND (@IncidentId = 0 OR IncidentId = @IncidentId)
                AND (@QueueId = 0 OR QueueId = @QueueId)
                ORDER BY {Guid.NewGuid()} and {param}
                OFFSET @Skip ROWS
                FETCH NEXT @Take ROWS ONLY
            ";
       [Benchmark]
        public  string CreateQueryStringBuilder()
        {
            var query = new StringBuilder();
            query.Append("SELECT");
            query.Append("IncidentId, QueueId, QueueName, Name, CreatedDate, UpdateDate");
            query.Append("from vw_Incidents_Report");
            query.Append("WHERE");
            query.Append("((CreatedDate >= @CreatedDateFrom or @CreatedDateFrom IS NULL) AND(CreatedDate <= @CreatedDateTo or @CreatedDateTo IS NULL))");
            query.Append("AND((UpdateDate >= @UpdatedDateFrom or @UpdatedDateFrom IS NULL) AND(UpdateDate <= @UpdatedDateTo or @UpdatedDateTo IS NULL))");
            query.Append("AND(@IncidentId = 0 OR IncidentId = @IncidentId)");
            query.Append("AND(@QueueId = 0 OR QueueId = @QueueId)");
            query.AppendFormat("ORDER BY {0} and {1}", Guid.NewGuid(), param);
            query.Append("OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY");
            return query.ToString();
        }
    }
}