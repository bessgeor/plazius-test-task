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

		private sealed class MergeableLinkedListNode<T>
		{
			private MergeableLinkedListNode<T> _prev;
			private MergeableLinkedListNode<T> _next;
			public MergeableLinkedListNode<T> Previous
			{
				get => _prev;
				set
				{
					if ( value != null )
						value._next = this;
					_prev = value;
				}
			}
			public MergeableLinkedListNode<T> Next
			{
				get => _next;
				set
				{
					if ( value != null )
						value._prev = this;
					_next = value;
				}
			}
			public T Value { get; }

			public MergeableLinkedListNode( T value ) => Value = value;
		}

		private sealed class MergeableLinkedList<T>
		{
			public MergeableLinkedListNode<T> First { get; private set; }
			public MergeableLinkedListNode<T> Last { get; private set; }

			public MergeableLinkedList( T value )
				=> First = Last = Wrap( value );

			public MergeableLinkedListNode<T> AddFirst( T value )
				=> AddFirst( Wrap( value ) );

			public MergeableLinkedListNode<T> AddFirst( MergeableLinkedListNode<T> wrapped )
			{
				wrapped.Next = First;
				First = wrapped;
				while ( wrapped.Previous != null )
					First = wrapped = wrapped.Previous;
				return First;
			}

			public MergeableLinkedListNode<T> AddLast( T value )
				=> AddLast( Wrap( value ) );

			public MergeableLinkedListNode<T> AddLast( MergeableLinkedListNode<T> wrapped )
			{
				wrapped.Previous = Last;
				Last = wrapped;
				while ( wrapped.Next != null )
					Last = wrapped = wrapped.Previous; // is not called at all which is a consumer's implementation detail
				return First;
			}

			private MergeableLinkedListNode<T> Wrap( T item )
				=> new MergeableLinkedListNode<T>( item );
		}

		/// <summary>
		/// Modifies input array to be sorted and returns reference to it
		/// </summary>
		public static TravelCard[] SortAllocatey( this TravelCard[] unsorted )
		{
			if ( unsorted is null )
				throw new ArgumentNullException();
			List<MergeableLinkedList<TravelCard>> clusters = new List<MergeableLinkedList<TravelCard>>( unsorted.Length );
			foreach ( TravelCard current in unsorted )
			{
				int? left = null;
				int? right = null;
				for ( int i = 0; i < clusters.Count && (left == null || right == null); i++ )
					if ( clusters[ i ]?.First.Value.DepartureFrom == current.ArriveTo )
						left = i;
					else if ( clusters[ i ]?.Last.Value.ArriveTo == current.DepartureFrom )
						right = i;
				if ( right.HasValue || left.HasValue )
				{
					if ( right.HasValue == left.HasValue )
					{
						clusters[ left.Value ].AddFirst( current );
						clusters[ left.Value ].AddFirst( clusters[ right.Value ].Last );
						clusters[ right.Value ] = null;
					}
					else if ( right.HasValue )
						clusters[ right.Value ].AddLast( current );
					else clusters[ left.Value ].AddFirst( current );
				}
				else
				{
					MergeableLinkedList<TravelCard> cluster = new MergeableLinkedList<TravelCard>( current );
					clusters.Add( cluster );
				}
			}
			foreach ( MergeableLinkedList<TravelCard> cluster in clusters )
				if ( cluster != null )
				{
					MergeableLinkedListNode<TravelCard> current = cluster.First;
					for ( int i = 0; i < unsorted.Length; i++ )
					{
						unsorted[ i ] = current.Value;
						current = current.Next;
					}
					break;
				}
			return unsorted;
		}
	}
}
