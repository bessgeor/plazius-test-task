using plazius_test_task;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace plazius_test_task_tests
{
	[TestFixture]
	public class TravelCardTests
	{
		[Test]
		[TestCase( null, "Oslo", TestName = "Departure city is null" )]
		[TestCase( "Oslo", null, TestName = "Arrival city is null" )]
		public void NullCityIsThrowing( string from, string to )
			=> AssertException<ArgumentNullException>( from, to );

		[Test]
		[TestCase( "", "Oslo", "Empty city is not allowed: departureFrom", TestName = "Departure city is empty" )]
		[TestCase( "Oslo", "", "Empty city is not allowed: arriveTo", TestName = "Arrival city is empty" )]
		[TestCase( "paris", "Oslo", "Lowercase-starting city is not allowed: departureFrom", TestName = "Departure city is starting with lowercase letter" )]
		[TestCase( "Oslo", "paris", "Lowercase-starting city is not allowed: arriveTo", TestName = "Arrival city is starting with lowercase letter" )]
		[TestCase( "Oslo", "Oslo", "Arrival and departure cities should not match", TestName = "Arrival city is the same as departure one" )]
		public void InvalidCityIsThrowing( string from, string to, string message )
			=> AssertException<ArgumentException>( from, to )
			.WithMessage( message );

		[Test]
		public void TravelCardIsCreatedAndFilledOnValidData()
		{
			const string departureFrom = "Oslo";
			const string arriveTo = "Paris";
			TravelCard card = new TravelCard( departureFrom, arriveTo );
			card.DepartureFrom.Should().Be( departureFrom );
			card.ArriveTo.Should().Be( arriveTo );
		}

		private FluentAssertions.Specialized.ExceptionAssertions<TException> AssertException<TException>( string from, string to )
			where TException : Exception
			=> ( (Action) ( () => new TravelCard( from, to ) ) )
			.ShouldThrow<TException>();
	}
}
