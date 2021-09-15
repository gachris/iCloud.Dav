using iCloud.Dav.Core.Enums;
using iCloud.Dav.Core.Logger;
using iCloud.Dav.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace iCloud.Dav.Core.Services
{
    /// <summary>Utility class for building a URI using <see cref="M:ICloud.Api.Requests.RequestBuilder.BuildUri" /> or a HTTP request using
    /// <see cref="M:ICloud.Api.Requests.RequestBuilder.CreateRequest" /> from the query and path parameters of a REST call.</summary>
    public class RequestBuilder
    {
        private static readonly ILogger Logger = ApplicationContext.Logger.ForType<RequestBuilder>();
        /// <summary>Pattern to get the groups that are part of the path.</summary>
        private static Regex PathParametersPattern = new Regex("{[^{}]*}*");
        /// <summary>Supported HTTP methods.</summary>
        private static IEnumerable<string> SupportedMethods = new List<string>()
        {
            "GET",
            "PUT",
            "DELETE",
            "PROPFIND",
            "PROPPATCH",
            "REPORT",
            "MKCALENDAR"
        };
        /// <summary>The HTTP method used for this request.</summary>
        private string method;
        /// <summary>Operator list that can appear in the path argument.</summary>
        private const string OPERATORS = "+#./;?&|!@=";

        /// <summary>
        /// A dictionary containing the parameters which will be inserted into the path of the URI. These parameters
        /// will be substituted into the URI path where the path contains "{key}". See
        /// http://tools.ietf.org/html/rfc6570 for more information.
        /// </summary>
        private IDictionary<string, IList<string>> PathParameters { get; set; }

        /// <summary>
        /// A dictionary containing the parameters which will apply to the query portion of this request.
        /// </summary>
        private List<KeyValuePair<string, string>> QueryParameters { get; set; }

        /// <summary>The base URI for this request (usually applies to the service itself).</summary>
        public Uri BaseUri { get; set; }

        /// <summary>
        /// The path portion of this request. It's appended to the <see cref="P:ICloud.Api.Requests.RequestBuilder.BaseUri" /> and the parameters are
        /// substituted from the <see cref="P:ICloud.Api.Requests.RequestBuilder.PathParameters" /> dictionary.
        /// </summary>
        public string Path { get; set; }

        /// <summary>The HTTP method used for this request (such as GET, PUT, POST, etc...).</summary>
        /// <remarks>The default Value is <see cref="F:ICloud.Api.Http.HttpConsts.Get" />.</remarks>
        public string Method
        {
            get
            {
                return this.method;
            }
            set
            {
                if (!RequestBuilder.SupportedMethods.Contains<string>(value))
                    throw new ArgumentOutOfRangeException(nameof(Method));
                this.method = value;
            }
        }

        /// <summary>Construct a new request builder.</summary>
        ///  
        ///             TODO(peleyal): Consider using the Factory pattern here.
        public RequestBuilder()
        {
            this.PathParameters = new Dictionary<string, IList<string>>();
            this.QueryParameters = new List<KeyValuePair<string, string>>();
            this.Method = "GET";
        }

        /// <summary>Constructs a Uri as defined by the parts of this request builder.</summary>
        public Uri BuildUri()
        {
            StringBuilder stringBuilder = this.BuildRestPath();
            if (this.QueryParameters.Count > 0)
            {
                stringBuilder.Append(stringBuilder.ToString().Contains("?") ? "&" : "?");
                stringBuilder.Append(string.Join("&", this.QueryParameters.Select(x =>
                {
                    if (string.IsNullOrEmpty(x.Value))
                        return Uri.EscapeDataString(x.Key);
                    return string.Format("{0}={1}", Uri.EscapeDataString(x.Key), Uri.EscapeDataString(x.Value));
                }).ToArray<string>()));
            }
            return new Uri(this.BaseUri, stringBuilder.ToString());
        }

        /// <summary>
        /// Builds the REST path string builder based on <see cref="P:ICloud.Api.Requests.RequestBuilder.PathParameters" /> and the URI template spec
        /// http://tools.ietf.org/html/rfc6570.
        /// </summary>
        /// <returns></returns>
        private StringBuilder BuildRestPath()
        {
            if (string.IsNullOrEmpty(this.Path))
                return new StringBuilder(string.Empty);
            StringBuilder stringBuilder1 = new StringBuilder(this.Path);
            foreach (object match in RequestBuilder.PathParametersPattern.Matches(stringBuilder1.ToString()))
            {
                string oldValue = match.ToString();
                string str1 = oldValue.Substring(1, oldValue.Length - 2);
                string empty = string.Empty;
                if ("+#./;?&|!@=".Contains(str1[0].ToString()))
                {
                    empty = str1[0].ToString();
                    str1 = str1.Substring(1);
                }
                StringBuilder stringBuilder2 = new StringBuilder();
                string[] strArray = str1.Split(',');
                for (int index = 0; index < strArray.Length; ++index)
                {
                    string key = strArray[index];
                    bool flag = false;
                    int result = 0;
                    if (key[key.Length - 1] == '*')
                    {
                        flag = true;
                        key = key.Substring(0, key.Length - 1);
                    }
                    if (key.Contains(":"))
                    {
                        if (!int.TryParse(key.Substring(key.IndexOf(":") + 1), out result))
                            throw new ArgumentException(string.Format("Can't parse number after ':' in Path \"{0}\". Parameter is \"{1}\"", Path, key), this.Path);
                        key = key.Substring(0, key.IndexOf(":"));
                    }
                    string separator = empty;
                    string str2 = empty;
                    switch (empty)
                    {
                        case "#":
                            str2 = index == 0 ? "#" : ",";
                            separator = ",";
                            break;
                        case "&":
                        case ";":
                            str2 = empty + key + "=";
                            separator = ",";
                            if (flag)
                            {
                                separator = empty + key + "=";
                                break;
                            }
                            break;
                        case "+":
                            str2 = index == 0 ? "" : ",";
                            separator = ",";
                            break;
                        case ".":
                            if (!flag)
                            {
                                separator = ",";
                                break;
                            }
                            break;
                        case "/":
                            if (!flag)
                            {
                                separator = ",";
                                break;
                            }
                            break;
                        case "?":
                            str2 = (index == 0 ? "?" : "&") + key + "=";
                            separator = ",";
                            if (flag)
                            {
                                separator = "&" + key + "=";
                                break;
                            }
                            break;
                        default:
                            if (index > 0)
                                str2 = ",";
                            separator = ",";
                            break;
                    }
                    if (this.PathParameters.ContainsKey(key))
                    {
                        string stringToEscape = string.Join(separator, this.PathParameters[key]);
                        if (result != 0 && result < stringToEscape.Length)
                            stringToEscape = stringToEscape.Substring(0, result);
                        if (empty != "+" && empty != "#" && this.PathParameters[key].Count == 1)
                            stringToEscape = Uri.EscapeDataString(stringToEscape);
                        string str3 = str2 + stringToEscape;
                        stringBuilder2.Append(str3);
                    }
                    else
                        throw new ArgumentException(string.Format("Path \"{0}\" misses a \"{1}\" parameter", Path, key), this.Path);
                }
                if (empty == ";")
                {
                    if (stringBuilder2[stringBuilder2.Length - 1] == '=')
                        stringBuilder2 = stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
                    stringBuilder2 = stringBuilder2.Replace("=;", ";");
                }
                stringBuilder1 = stringBuilder1.Replace(oldValue, stringBuilder2.ToString());
            }
            return stringBuilder1;
        }

        /// <summary>Adds a parameter value.</summary>
        /// <param name="type">Type of the parameter (must be 'Path' or 'Query').</param>
        /// <param name="name">Parameter name.</param>
        /// <param name="value">Parameter value.</param>
        public void AddParameter(RequestParameterType type, string name, string value)
        {
            name.ThrowIfNull<string>(nameof(name));
            if (value == null)
                RequestBuilder.Logger.Warning("Add parameter should not get null values. type={0}, name={1}", type, name);
            else if (type != RequestParameterType.Path)
            {
                if (type != RequestParameterType.Query)
                    throw new ArgumentOutOfRangeException(nameof(type));
                this.QueryParameters.Add(new KeyValuePair<string, string>(name, value));
            }
            else if (!this.PathParameters.ContainsKey(name))
                this.PathParameters[name] = new List<string>()
        {
          value
        };
            else
                this.PathParameters[name].Add(value);
        }

        /// <summary>Creates a new HTTP request message.</summary>
        public HttpRequestMessage CreateRequest()
        {
            return new HttpRequestMessage(new HttpMethod(this.Method), this.BuildUri().ToString());
        }
    }
}
