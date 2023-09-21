using iCloud.Dav.Core.Extensions;
using iCloud.Dav.Core.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace iCloud.Dav.Core
{
    /// <summary>
    /// Utility class for building a URI using <see cref="BuildUri" /> or a HTTP request using
    /// <see cref="CreateRequest" /> from the query and path parameters of a REST call.
    /// </summary>
    public class RequestBuilder
    {
        private static readonly ILogger Logger = ApplicationContext.Logger.ForType<RequestBuilder>();

        /// <summary>
        /// Pattern to get the groups that are part of the path.
        /// </summary>
        private static readonly Regex PathParametersPattern = new Regex("{[^{}]*}*");

        /// <summary>
        /// Supported HTTP methods.
        /// </summary>
        private static readonly IEnumerable<string> SupportedMethods = new List<string>()
        {
            "GET",
            "PUT",
            "DELETE",
            "PROPFIND",
            "PROPPATCH",
            "REPORT",
            "MKCALENDAR"
        };

        /// <summary>
        /// The HTTP method used for this request.
        /// </summary>
        private string _method = "GET";

        /// <summary>
        /// Operator list that can appear in the path argument.
        /// </summary>
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

        /// <summary>
        /// The base URI for this request (usually applies to the service itself).
        /// </summary>
        public Uri BaseUri { get; }

        /// <summary>
        /// The path portion of this request. It's appended to the <see cref="BaseUri" /> and the parameters are
        /// substituted from the <see cref="PathParameters" /> dictionary.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// The HTTP method used for this request (such as GET, PUT, POST, etc...).
        /// </summary>
        /// <remarks>The default Value is <see cref="Constants.Get" />.</remarks>
        public string Method
        {
            get => _method;
            private set
            {
                if (!SupportedMethods.Contains(value))
                    throw new ArgumentOutOfRangeException(nameof(Method));
                _method = value;
            }
        }

        /// <summary>Construct a new request builder.</summary>
        public RequestBuilder(Uri baseUri, string path, string method)
        {
            BaseUri = baseUri;
            Path = path;
            Method = method;
            PathParameters = new Dictionary<string, IList<string>>();
            QueryParameters = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Constructs a Uri as defined by the parts of this request builder.
        /// </summary>
        public Uri BuildUri()
        {
            var stringBuilder = BuildRestPath();
            if (QueryParameters.Count > 0)
            {
                stringBuilder.Append(stringBuilder.ToString().Contains('?') ? "&" : "?");
                stringBuilder.Append(string.Join("&", QueryParameters.Select(x =>
                {
                    if (string.IsNullOrEmpty(x.Value))
                        return Uri.EscapeDataString(x.Key);
                    return string.Format("{0}={1}", Uri.EscapeDataString(x.Key), Uri.EscapeDataString(x.Value));
                }).ToArray()));
            }
            return new Uri(BaseUri, stringBuilder.ToString());
        }

        /// <summary>
        /// Builds the REST path string builder based on <see cref="PathParameters" />.
        /// </summary>
        /// <returns>The REST path.</returns>
        private StringBuilder BuildRestPath()
        {
            if (string.IsNullOrEmpty(Path))
            {
                return new StringBuilder(string.Empty);
            }

            StringBuilder stringBuilder = new StringBuilder(Path);
            foreach (object item in PathParametersPattern.Matches(stringBuilder.ToString()))
            {
                string text = item.ToString();
                string text2 = text.Substring(1, text.Length - 2);
                string text3 = string.Empty;
                if (OPERATORS.Contains(text2[0].ToString()))
                {
                    text3 = text2[0].ToString();
                    text2 = text2.Substring(1);
                }

                StringBuilder stringBuilder2 = new StringBuilder();
                string[] array = text2.Split(new char[1] { ',' });
                for (int i = 0; i < array.Length; i++)
                {
                    string text4 = array[i];
                    bool flag = false;
                    int result = 0;
                    if (text4[text4.Length - 1] == '*')
                    {
                        flag = true;
                        text4 = text4.Substring(0, text4.Length - 1);
                    }

                    if (text4.Contains(":"))
                    {
                        if (!int.TryParse(text4.Substring(text4.IndexOf(":") + 1), out result))
                        {
                            throw new ArgumentException($"Can't parse number after ':' in Path \"{Path}\". Parameter is \"{text4}\"", Path);
                        }

                        text4 = text4.Substring(0, text4.IndexOf(":"));
                    }

                    string separator = text3;
                    string text5 = text3;
                    switch (text3)
                    {
                        case "+":
                            text5 = ((i == 0) ? "" : ",");
                            separator = ",";
                            break;
                        case ".":
                            if (!flag)
                            {
                                separator = ",";
                            }

                            break;
                        case "/":
                            if (!flag)
                            {
                                separator = ",";
                            }

                            break;
                        case "#":
                            text5 = ((i == 0) ? "#" : ",");
                            separator = ",";
                            break;
                        case "?":
                            text5 = ((i == 0) ? "?" : "&") + text4 + "=";
                            separator = ",";
                            if (flag)
                            {
                                separator = "&" + text4 + "=";
                            }

                            break;
                        case "&":
                        case ";":
                            text5 = text3 + text4 + "=";
                            separator = ",";
                            if (flag)
                            {
                                separator = text3 + text4 + "=";
                            }

                            break;
                        default:
                            if (i > 0)
                            {
                                text5 = ",";
                            }

                            separator = ",";
                            break;
                    }

                    if (PathParameters.ContainsKey(text4))
                    {
                        string text6 = string.Join(separator, PathParameters[text4]);
                        if (result != 0 && result < text6.Length)
                        {
                            text6 = text6.Substring(0, result);
                        }

                        if (text3 != "+" && text3 != "#" && PathParameters[text4].Count == 1)
                        {
                            text6 = Uri.EscapeDataString(text6);
                        }

                        text6 = text5 + text6;
                        stringBuilder2.Append(text6);
                        continue;
                    }

                    throw new ArgumentException($"Path \"{Path}\" misses a \"{text4}\" parameter", Path);
                }

                if (text3 == ";")
                {
                    if (stringBuilder2[stringBuilder2.Length - 1] == '=')
                    {
                        stringBuilder2 = stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
                    }

                    stringBuilder2 = stringBuilder2.Replace("=;", ";");
                }

                stringBuilder = stringBuilder.Replace(text, stringBuilder2.ToString());
            }

            return stringBuilder;
        }

        /// <summary>
        /// Adds a parameter value.
        /// </summary>
        /// <param name="type">Type of the parameter (must be 'Path' or 'Query').</param>
        /// <param name="name">Parameter name.</param>
        /// <param name="value">Parameter value.</param>
        public void AddParameter(RequestParameterType type, string name, string value)
        {
            name.ThrowIfNull(nameof(name));
            if (value == null)
                Logger.Warning("Add parameter should not get null values. type={0}, name={1}", type, name);
            else if (type != RequestParameterType.Path)
            {
                if (type != RequestParameterType.Query)
                    throw new ArgumentOutOfRangeException(nameof(type));
                QueryParameters.Add(new KeyValuePair<string, string>(name, value));
            }
            else if (!PathParameters.ContainsKey(name))
                PathParameters[name] = new List<string>() { value };
            else
                PathParameters[name].Add(value);
        }

        /// <summary>
        /// Creates a new HTTP request message.
        /// </summary>
        public HttpRequestMessage CreateRequest() => new HttpRequestMessage(new HttpMethod(Method), BuildUri().ToString());
    }
}