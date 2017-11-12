using System;
using System.Collections.Generic;

namespace plazius_test_task
{
	public static class TravelCardArrayExtensions
	{
		public static TravelCard[] Sort( this TravelCard[] unsorted ) // O( n^3 ), Ω( nlog(n) ) by time, O(n) by memory, O(1) by heap allocations
		{
			if ( unsorted is null )
				throw new ArgumentNullException();
			if ( unsorted.Length == 0 )
				return unsorted;
			List<TravelCard> sortedCards = new List<TravelCard>( unsorted.Length ); // is not LinkedList because of LinkedListNode is a reference type. May be implemented on LinkedList of LinkedLists in some unmanaged language if a really hot path
			for ( int i = 0; i < unsorted.Length; i++ ) // n
			{
				TravelCard current = unsorted[ i ];
				(int insertTo, bool? toLeft) = FindIndexToInsertTo( current ); // O( n ), Ω( log(n) )
				sortedCards.Insert( insertTo, current );  // O( n ), Ω( 1 )
				if ( toLeft.HasValue )
				{
					bool alreadyFound = false;
					if ( toLeft == true )
						for ( int j = sortedCards.Count - 1; j > -1; j-- ) // n
						{
							string currentCity = sortedCards[ insertTo ].DepartureFrom;
							string possibleMatchCity = sortedCards[ j ].ArriveTo;
							if ( currentCity == possibleMatchCity )
							{
								TravelCard original = sortedCards[ j ];
								/// j is always greater than insertTo because of left to right direction in <see cref="FindIndexToInsertTo(TravelCard)"/>
								sortedCards.RemoveAt( j ); // O( n ), Ω( 1 )
								j++;
								sortedCards.Insert( insertTo, original ); // O( n ), Ω( 1 )
								alreadyFound = true;
							}
							else if ( alreadyFound )
								break;
						}
					else
						for ( int j = 0; j < sortedCards.Count; j++ ) // n - 1 at worst
						{
							string currentCity = sortedCards[ insertTo ].ArriveTo;
							string possibleMatchCity = sortedCards[ j ].DepartureFrom;
							if ( currentCity == possibleMatchCity )
							{
								TravelCard original = sortedCards[ j ];
								/// j is always greater than insertTo because of left to right direction in <see cref="FindIndexToInsertTo(TravelCard)"/>
								sortedCards.RemoveAt( j ); // O( n ), Ω( 1 )
								insertTo++;
								sortedCards.Insert( insertTo, original ); // O( n ), Ω( 1 )
								alreadyFound = true;
							}
							else if ( alreadyFound )
								break;
						}
				}
			}
			return sortedCards.ToArray();

			(int index, bool? left) FindIndexToInsertTo( TravelCard toFindPositionFor ) // O( n ), Ω( log(n) )
			{
				(int, bool?) @default = (sortedCards.Count /* not 0 because of excess of array shifts here and there */, null);
				if ( sortedCards.Count == 0 ) return @default;

				return Search( 0, sortedCards.Count - 1 ) ?? @default;

				(int, bool?)? Search( int from, int to )
				{
					if ( from == to - 1 )
						return Search( from, from ) ?? Search( to, to );
					if ( from == to )
						if ( sortedCards[ from ].ArriveTo == toFindPositionFor.DepartureFrom )
							return (from + 1, false);
						else if ( sortedCards[ from ].DepartureFrom == toFindPositionFor.ArriveTo )
							return (from, true);
						else return null;
					int mid = from + ( to - from ) / 2;
					(int, bool?)? result = Search( mid, mid );
					if ( !result.HasValue )
					{
						if ( mid > 0 )
							result = Search( from, mid - 1 );
						if ( !result.HasValue && mid < sortedCards.Count - 1 )
							result = Search( mid + 1, to );
					}
					return result;
				}
			}
		}
	}
}
