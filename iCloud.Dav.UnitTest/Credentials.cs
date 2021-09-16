using System.Net;

namespace iCloud.Dav.UnitTest
{
    public static class Credentials
    {
        private static NetworkCredential _networkCredential;
        public static NetworkCredential NetworkCredential
        {
            get
            {
                if (_networkCredential == null)
                {
                    _networkCredential = new NetworkCredential("icloud.email", "icloud.app-specific-password");
                }
                return _networkCredential;
            }
        }
    }
}
