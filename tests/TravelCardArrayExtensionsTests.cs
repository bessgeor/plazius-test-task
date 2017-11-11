using plazius_test_task;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace plazius_test_task_tests
{
	[TestFixture]
	public class TravelCardArrayExtensionsTests
	{
		[Test]
		public void NullArrayIsThrowing()
			=> ( (Action) ( () => ( (TravelCard[]) null ).Sort() ) )
			.ShouldThrow<ArgumentNullException>();

		private class UnsortedAndSortedTravelCardArraysSource : IEnumerable<TestCaseData>
		{
			public IEnumerator<TestCaseData> GetEnumerator()
			{
				return EnumerateRealCases()
					.Select( c => new TestCaseData( c.unsorted, c.sorted ).SetName( c.name ) )
					.GetEnumerator();

				IEnumerable<(TravelCard[] unsorted, TravelCard[] sorted, string name)> EnumerateRealCases()
				{
					yield return (Array.Empty<TravelCard>(), Array.Empty<TravelCard>(), "Empty arrays are handled properly");
					yield return (new[] { MakeCard( "Stockholm", "Oslo" ) }, new[] { MakeCard( "Stockholm", "Oslo" ) }, "Array of one element is sorted");
					yield return
					(
						new[] { MakeCard( "Stockholm", "Oslo" ), MakeCard( "Västerås", "Stockholm" ) },
						new[] { MakeCard( "Västerås", "Stockholm" ), MakeCard( "Stockholm", "Oslo" ) },
						"Array of two elements is sorted"
					);
					yield return
					(
						new[] { MakeCard( "Stockholm", "Oslo" ), MakeCard( "Västerås", "Stockholm" ), MakeCard( "Oslo", "Trondheim" ) },
						new[] { MakeCard( "Västerås", "Stockholm" ), MakeCard( "Stockholm", "Oslo" ), MakeCard( "Oslo", "Trondheim" ) },
						"Array of three elements is sorted"
					);
					yield return
					(
						new[] { MakeCard( "Stockholm", "Oslo" ), MakeCard( "Trondheim", "Reykjavík" ), MakeCard( "Västerås", "Stockholm" ), MakeCard( "Oslo", "Trondheim" ) },
						new[] { MakeCard( "Västerås", "Stockholm" ), MakeCard( "Stockholm", "Oslo" ), MakeCard( "Oslo", "Trondheim" ), MakeCard( "Trondheim", "Reykjavík" ) },
						"Array of four elements is sorted"
					);

					TravelCard MakeCard( string from, string to ) // just a shortcut
						=> new TravelCard( from, to );
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
				=> GetEnumerator();
		}

		[Test]
		[TestCaseSource( typeof( UnsortedAndSortedTravelCardArraysSource ) )]
		public void SortIsSorting( TravelCard[] unsorted, TravelCard[] sorted )
			=> unsorted.Sort()
			.Should().ContainInOrder( sorted ) // asserts only order disregarding of consecutiveness
			.And.Subject
			.Should().HaveCount( sorted.Length ); // forces consecutiveness
	}
}
