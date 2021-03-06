import 'dart:convert';
import 'dart:io';

import 'package:http/http.dart' as http;
import 'package:meta/meta.dart';
import 'package:nanoblog/exceptions/api_exception.dart';
import 'package:nanoblog/model/api_error.dart';


class ApiBase
{
  static const String baseUrl = "http://192.168.0.161:53188/api";

  static String _fixApiUrl(String apiUrl)
  {
    if (apiUrl[0] != '/')
    {
      apiUrl = "/" + apiUrl;
    }

    return apiUrl;
  }

  static Future<http.Response> get(String apiUrl, {String jwtToken, Map<String, String> headers}) async
  {
    apiUrl = _fixApiUrl(apiUrl);

    var _headers = {
      HttpHeaders.contentTypeHeader: "application/json",
      HttpHeaders.acceptHeader: "application/json",
    };

    if (jwtToken != null)
    {
      _headers.addAll({
        HttpHeaders.authorizationHeader: "Bearer $jwtToken"
      });
    }

    if (headers != null)
    {
      _headers.addAll(headers);
    }

    var result = await http.get(baseUrl + apiUrl, headers: _headers);

    if (result.statusCode == 400)
    {
      handleApiError(result.body);
    }
    
    return result;
  }

  static Future<http.Response> post(String apiUrl, {@required String jsonBody, String jwtToken, Map<String, String> headers}) async
  {
    apiUrl = _fixApiUrl(apiUrl);

    var _headers = {
      HttpHeaders.contentTypeHeader: "application/json",
      HttpHeaders.acceptHeader: "application/json",
    };

    if (jwtToken != null)
    {
      _headers.addAll({
        HttpHeaders.authorizationHeader: "Bearer $jwtToken"
      });
    }

    if (headers != null)
    {
      _headers.addAll(headers);
    }

    var result = await http.post(
      baseUrl + apiUrl, 
      headers: _headers,
      body: jsonBody
    );

    if (result.statusCode == 400)
    {
      handleApiError(result.body);
    }
    
    return result;
  }

  static Future<http.Response> delete(String apiUrl, {String jwtToken, Map<String, String> headers}) async
  {
    apiUrl = _fixApiUrl(apiUrl);

    var _headers = Map<String, String>();

    if (jwtToken != null)
    {
      _headers.addAll({
        HttpHeaders.authorizationHeader: "Bearer $jwtToken"
      });
    }

    if (headers != null)
    {
      _headers.addAll(headers);
    }

    var result = await http.delete(baseUrl + apiUrl, headers: _headers);

    if (result.statusCode == 400)
    {
      handleApiError(result.body);
    }

    return result;
  }

  static void handleApiError(String jsonMessage)
  {
      var jsonData = json.decode(jsonMessage);
      var apiError = ApiError.fromJson(jsonData);

      throw ApiException(apiError);
  }
}
