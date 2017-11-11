using System;

namespace plazius_test_task
{
	public static class TravelCardArrayExtensions
	{
		public static TravelCard[] Sort( this TravelCard[] unsorted )
		{
			if ( unsorted is null )
				throw new ArgumentNullException();
			return unsorted;
		}
	}
}
