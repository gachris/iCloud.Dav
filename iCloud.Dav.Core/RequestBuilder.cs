using iCloud.Dav.Core.Log;
using iCloud.Dav.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace iCloud.Dav.Core;

/// <summary>Utility class for building a URI using <see cref="BuildUri" /> or a HTTP request using
/// <see cref="CreateRequest" /> from the query and path parameters of a REST call.</summary>
public class RequestBuilder
{
    private static readonly ILogger Logger = ApplicationContext.Logger.ForType<RequestBuilder>();

    /// <summary>Pattern to get the groups that are part of the path.</summary>
    private static readonly Regex PathParametersPattern = new("{[^{}]*}*");

    /// <summary>Supported HTTP methods.</summary>
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

    /// <summary>The HTTP method used for this request.</summary>
    private string _method;

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
    /// The path portion of this request. It's appended to the <see cref="BaseUri" /> and the parameters are
    /// substituted from the <see cref="PathParameters" /> dictionary.
    /// </summary>
    public string Path { get; set; }

    /// <summary>The HTTP method used for this request (such as GET, PUT, POST, etc...).</summary>
    /// <remarks>The default Value is <see cref="ApiMethod.Get" />.</remarks>
    public string Method
    {
        get => _method;
        set
        {
            if (!SupportedMethods.Contains(value))
                throw new ArgumentOutOfRangeException(nameof(Method));
            _method = value;
        }
    }

    /// <summary>Construct a new request builder.</summary>
    ///  
    ///             TODO(peleyal): Consider using the Factory pattern here.
    public RequestBuilder()
    {
        PathParameters = new Dictionary<string, IList<string>>();
        QueryParameters = new List<KeyValuePair<string, string>>();
        Method = "GET";
    }

    /// <summary>Constructs a Uri as defined by the parts of this request builder.</summary>
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
    /// Builds the REST path string builder based on <see cref="PathParameters" /> and the URI template spec
    /// http://tools.ietf.org/html/rfc6570.
    /// </summary>
    /// <returns></returns>
    private StringBuilder BuildRestPath()
    {
        if (string.IsNullOrEmpty(Path))
            return new StringBuilder(string.Empty);
        var stringBuilder1 = new StringBuilder(Path);
        foreach (var match in PathParametersPattern.Matches(stringBuilder1.ToString()))
        {
            var oldValue = match.ToString();
            var str1 = oldValue[1..^1];
            var empty = string.Empty;
            if ("+#./;?&|!@=".Contains(str1[0].ToString()))
            {
                empty = str1[0].ToString();
                str1 = str1[1..];
            }
            var stringBuilder2 = new StringBuilder();
            var strArray = str1.Split(',');
            for (var index = 0; index < strArray.Length; ++index)
            {
                var key = strArray[index];
                var flag = false;
                var result = 0;
                if (key[^1] == '*')
                {
                    flag = true;
                    key = key[..^1];
                }
                if (key.Contains(':'))
                {
                    if (!int.TryParse(key.AsSpan(key.IndexOf(":") + 1), out result))
                        throw new ArgumentException(string.Format("Can't parse number after ':' in Path \"{0}\". Parameter is \"{1}\"", Path, key), Path);
                    key = key[..key.IndexOf(":")];
                }
                var separator = empty;
                var str2 = empty;
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
                if (PathParameters.ContainsKey(key))
                {
                    var stringToEscape = string.Join(separator, PathParameters[key]);
                    if (result != 0 && result < stringToEscape.Length)
                        stringToEscape = stringToEscape[..result];
                    if (empty != "+" && empty != "#" && PathParameters[key].Count == 1)
                        stringToEscape = Uri.EscapeDataString(stringToEscape);
                    var str3 = str2 + stringToEscape;
                    stringBuilder2.Append(str3);
                }
                else
                    throw new ArgumentException(string.Format("Path \"{0}\" misses a \"{1}\" parameter", Path, key), Path);
            }
            if (empty == ";")
            {
                if (stringBuilder2[^1] == '=')
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

    /// <summary>Creates a new HTTP request message.</summary>
    public HttpRequestMessage CreateRequest()
    {
        return new HttpRequestMessage(new HttpMethod(Method), BuildUri().ToString());
    }
}
