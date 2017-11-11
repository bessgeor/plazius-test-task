using System;

namespace plazius_test_task
{
	public struct TravelCard : IEquatable<TravelCard>
	{
		public TravelCard( string departureFrom, string arriveTo )
		{
			DepartureFrom = departureFrom;
			ArriveTo = arriveTo;
		}

		public string DepartureFrom { get; }
		public string ArriveTo { get; }

		public override int GetHashCode()
		{
			unchecked
			{
				const int prime = -1521134295;
				int hash = 12345701;
				hash = hash * prime + System.Collections.Generic.EqualityComparer<string>.Default.GetHashCode( DepartureFrom );
				hash = hash * prime + System.Collections.Generic.EqualityComparer<string>.Default.GetHashCode( ArriveTo );
				return hash;
			}
		}

		public bool Equals( TravelCard other ) => DepartureFrom == other.DepartureFrom && ArriveTo == other.ArriveTo;
		public override bool Equals( object obj ) => obj is TravelCard && Equals( (TravelCard) obj );

		public static bool operator ==( TravelCard x, TravelCard y ) => x.Equals( y );
		public static bool operator !=( TravelCard x, TravelCard y ) => !x.Equals( y );
	}
}
