using System;

namespace plazius_test_task
{
	public struct TravelCard : IEquatable<TravelCard>
	{
		public TravelCard( string departureFrom, string arriveTo )
		{
			ValidateCity( departureFrom, nameof( departureFrom ) );
			ValidateCity( arriveTo, nameof( arriveTo ) );
			if ( departureFrom == arriveTo )
				throw new ArgumentException( "Arrival and departure cities should not match" );
			DepartureFrom = departureFrom;
			ArriveTo = arriveTo;

			void ValidateCity( string value, string argName )
			{
				if ( value is null )
					throw new ArgumentNullException( argName );
				if ( value == String.Empty )
					throw new ArgumentException( "Empty city is not allowed: " + argName );
				if ( Char.IsLower( value[ 0 ] ) )
					throw new ArgumentException( "Lowercase-starting city is not allowed: " + argName );
			}
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
