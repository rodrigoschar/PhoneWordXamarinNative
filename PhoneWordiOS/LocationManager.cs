using System;
using CoreLocation;

namespace PhoneWordiOS
{
	// This class sets a number of properties on the CLLocaionManager class.
	public class LocationManager
	{
		protected CLLocationManager locMgr;
        // event for the location changing
        public event EventHandler<LocationUpdatedEventArgs> LocationUpdated = delegate { };

        public LocationManager()
		{
			this.locMgr = new CLLocationManager();
			// This is a Boolean that can be set depending on whether the system is allowed to pause location updates.
			this.locMgr.PausesLocationUpdatesAutomatically = false;
		}

		public void StartLocationUpdates()
		{
			if (CLLocationManager.LocationServicesEnabled)
			{
				//set the desired accuracy, in meters
				locMgr.DesiredAccuracy = 1;
				locMgr.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
				{
                    // fire our custom Location Updated event
                    LocationUpdated (this, new LocationUpdatedEventArgs(e.Locations[e.Locations.Length - 1]));
                };
				locMgr.StartUpdatingLocation();
			}
		}

		public CLLocationManager LLocationManager
		{
			get { return this.LLocationManager;  }
		}
	}
}

