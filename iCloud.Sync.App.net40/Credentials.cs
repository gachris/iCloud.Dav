using System.Net;

namespace iCloud.Sync.App
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
                    _networkCredential = new NetworkCredential("christosgatzos@gmail.com", "qvuo-sfko-rklg-mgbi");
                }
                return _networkCredential;
            }
        }
    }
}
