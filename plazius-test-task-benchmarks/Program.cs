using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using plazius_test_task;
using System;
using System.Linq;

namespace plazius_test_task_benchmarks
{
	[SimpleJob( RunStrategy.ColdStart, launchCount: 50, warmupCount: 0, targetCount: 10, invocationCount: 20, id: "Job" )]
	public class AllocateySortVsNonAllocateySort
	{
		public const int N = 1000;
		private readonly TravelCard[] _sortedData;
		private readonly TravelCard[] _sortedReversedData;
		private readonly TravelCard[] _sortedShuffledData;

		public AllocateySortVsNonAllocateySort()
		{
			_sortedData = new TravelCard[ N ];
			_sortedData[ 0 ] = new TravelCard( "A-1" + Guid.NewGuid().ToString(), "A" );
			for ( int i = 0; i < N - 1; i++ )
			{
				string city = String.Concat( "A", i.ToString(), Guid.NewGuid().ToString() );
				_sortedData[ i ] = new TravelCard( _sortedData[ i ].DepartureFrom, city );
				_sortedData[ i + 1 ] = new TravelCard( city, "A" );
			}
			_sortedReversedData = _sortedData.Reverse().ToArray();
			Random rnd = new Random();
			_sortedShuffledData = _sortedData.OrderBy( d => d.GetHashCode() ).ToArray();
		}

		[Benchmark]
		public void SortSorted()
			=> _sortedData.Sort();

		[Benchmark]
		public void SortSortedAllocatey()
			=> _sortedData.SortAllocatey();

		[Benchmark]
		public void SortReverseSorted()
			=> _sortedReversedData.Sort();

		[Benchmark]
		public void SortReverseSortedAllocatey()
			=> _sortedReversedData.SortAllocatey();

		[Benchmark]
		public void SortShuffled()
			=> _sortedShuffledData.Sort();

		[Benchmark]
		public void SortShuffledAllocatey()
			=> _sortedShuffledData.SortAllocatey();
	}

    public class Program
    {
        public static void Main(string[] args)
        {
			BenchmarkRunner.Run<AllocateySortVsNonAllocateySort>();
        }
    }
}
