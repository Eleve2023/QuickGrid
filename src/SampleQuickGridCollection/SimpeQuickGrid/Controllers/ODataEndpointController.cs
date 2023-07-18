// Licensed under the MIT License. 
// https://github.com/Eleve2023/QuickGrid/blob/master/LICENSE.txt

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing;
using System.Text;

namespace SimpeQuickGrid.Controllers;

/// <summary>
/// A controller for debugging that shows the OData endpoints.
/// </summary>
public class ODataEndpointController : ControllerBase
{
    private readonly EndpointDataSource _dataSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="ODataEndpointController" /> class.
    /// </summary>
    /// <param name="dataSource">The data source.</param>
    public ODataEndpointController(EndpointDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    /// <summary>
    /// Get all routes.
    /// </summary>
    /// <returns>The content result.</returns>
    [HttpGet("$odata")]
    public IActionResult GetAllRoutes()
    {
        CreateRouteTables(out var stdRouteTable, out var odataRouteTable);

        string output = ODataRouteMappingHtmlTemplate;
        output = output.Replace("ODATA_ROUTE_CONTENT", odataRouteTable, StringComparison.OrdinalIgnoreCase);
        output = output.Replace("STD_ROUTE_CONTENT", stdRouteTable, StringComparison.OrdinalIgnoreCase);

        return base.Content(output, "text/html");
    }

    private void CreateRouteTables(out string stdRouteTable, out string odataRouteTable)
    {
        var stdRoutes = new StringBuilder();
        var odataRoutes = new StringBuilder();
        foreach (var endpoint in _dataSource.Endpoints.OfType<RouteEndpoint>())
        {
            var controllerActionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            if (controllerActionDescriptor == null)
            {
                continue;
            }

            var metadata = endpoint.Metadata.GetMetadata<IODataRoutingMetadata>();
            if (metadata == null)
            {
                AppendRoute(stdRoutes, endpoint);
            }
            else
            {
                AppendRoute(odataRoutes, endpoint);
            }
        }
        stdRouteTable = stdRoutes.ToString();
        odataRouteTable = odataRoutes.ToString();
    }

    private static string GetHttpMethods(Endpoint endpoint)
    {
        var methodMetadata = endpoint.Metadata.GetMetadata<HttpMethodMetadata>();
        if (methodMetadata == null)
        {
            return "";
        }
        return string.Join(", ", methodMetadata.HttpMethods);
    }

    /// <summary xml:lang="fr">
    /// Process the endpoint
    /// </summary>
    /// <param name="sb">The string builder to append HTML to.</param>
    /// <param name="endpoint">The endpoint to render.</param>
    private static void AppendRoute(StringBuilder sb, RouteEndpoint endpoint)
    {
        sb.Append("<tr>");
        sb.Append($"<td>{endpoint.DisplayName}</td>");

        sb.Append($"<td>{string.Join(",", GetHttpMethods(endpoint))}</td>");

        sb.Append("<td>");
#pragma warning disable CS8602 // Déréférencement d'une éventuelle référence null.
        var link = "" + endpoint.RoutePattern.RawText.TrimStart('/');
#pragma warning restore CS8602 // Déréférencement d'une éventuelle référence null.
        sb.Append($"<a href=\"/{link}\">~/{link}</a>");
        sb.Append("</td>");

        sb.Append("</tr>");
    }

    private static readonly string ODataRouteMappingHtmlTemplate = @"<html>
<head>
    <title>OData Endpoint Routing Debugger</title>
    <style>
        table {
            font-family: arial, sans-serif;
            border-collapse: collapse;
            width: 100%;
        }
        td,
        th {
            border: 1px solid #dddddd;
            text-align: left;
            padding: 8px;
        }
        tr:nth-child(even) {
            background-color: #dddddd;
        }
    </style>
</head>
<body>
    <h1 id=""odata"">OData Endpoint Mappings</h1>
    <p>
        <a href=""#standard"">Got to non-OData endpoint mappings</a>
    </p>
    <table>
        <tr>
            <th> Controller & Action </th>
            <th> HttpMethods </th>
            <th> Template </th>
        </tr>
        ODATA_ROUTE_CONTENT
    </table>
    <h1 id=""standard"">Non-OData Endpoint Mappings</h1>
    <p>
        <a href=""#odata"">Go to OData endpoint mappings</a>
    </p>
    <table>
        <tr>
            <th> Controller </th>
            <th> HttpMethods </th>
            <th> Template </th>
        </tr>
        STD_ROUTE_CONTENT
    </table>
</body>
</html>";
}
