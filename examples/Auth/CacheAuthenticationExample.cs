using iCloud.Dav.Auth;
using iCloud.Dav.Auth.Store;
using iCloud.Dav.Core;
using System;
using System.Net;
using System.Threading;

FileDataStore dataStore = new FileDataStore("iCloudApp");
UserCredential userCredential = await AuthorizationBroker.AuthorizeFromCacheAsync(Environment.UserName, dataStore, CancellationToken.None);

BaseClientService.Initializer initializer = new BaseClientService.Initializer()
{
    ApplicationName = "iCloudApp",
    HttpClientInitializer = userCredential
};

// Revoke token
bool result = await userCredential.RevokeTokenAsync(CancellationToken.None);