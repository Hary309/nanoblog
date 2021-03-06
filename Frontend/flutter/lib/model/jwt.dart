import 'package:corsac_jwt/corsac_jwt.dart';

class Jwt
{
  String token;
  String refreshToken;
  int expires;

  Jwt({this.token, this.refreshToken, this.expires});

  String getUserId()
  {
    var jwt = JWT.parse(token);

    if (jwt != null)
    {
      return jwt.subject;
    }
    
    return null;
  }

  bool isExpired()
  {
    if (expires == null)
      return true;

    return expires < DateTime.now().millisecondsSinceEpoch;
  }
}
