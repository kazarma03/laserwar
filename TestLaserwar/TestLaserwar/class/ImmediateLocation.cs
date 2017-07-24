using System;
using System.Device.Location;
using System.Diagnostics;

namespace TestLaserwar
{
    public class ImmediateLocation : IDisposable
    {
        private GeoCoordinateWatcher _watcher;
        private Action<GeoCoordinate> _action;

        public ImmediateLocation(Action<GeoCoordinate> a)
        {
            Debug.Assert(a != null);

            _action = a;
        }

        public void GetLocation()
        {
            if (_watcher == null)
            {
                _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
                _watcher.MovementThreshold = 1000;

                _watcher.PositionChanged += new
                    EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>
                    (_watcher_PositionChanged);
                _watcher.StatusChanged += new
                    EventHandler<GeoPositionStatusChangedEventArgs>
                    (_watcher_StatusChanged);

                _watcher.Start(false);

                if (_watcher.Status == GeoPositionStatus.Disabled
                    || _watcher.Permission == GeoPositionPermission.Denied)
                    Dispose();
            }
        }

        void _watcher_StatusChanged(object sender,
            GeoPositionStatusChangedEventArgs e)
        {
            if (e.Status == GeoPositionStatus.Disabled
                || _watcher.Permission == GeoPositionPermission.Denied)
                Dispose();
        }

        void _watcher_PositionChanged(object sender,
            GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            _action(e.Position.Location);
            Dispose();
        }

        public void Dispose()
        {
            if (_watcher != null)
            {
                _watcher.Stop();
                _watcher.PositionChanged -= new
                    EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>
                    (_watcher_PositionChanged);
                _watcher.StatusChanged -= new
                    EventHandler<GeoPositionStatusChangedEventArgs>
                    (_watcher_StatusChanged);
                _watcher.Dispose();
            }
            _watcher = null;
            _action = null;
        }

    }
}
