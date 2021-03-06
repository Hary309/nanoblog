import 'package:flutter/material.dart';
import 'package:nanoblog/api/account_api.dart';
import 'package:nanoblog/model/app_state_model.dart';
import 'package:nanoblog/exceptions/api_exception.dart';
import 'package:nanoblog/screens/loading.dart';
import 'package:scoped_model/scoped_model.dart';

class LoginPage extends StatefulWidget
{
  @override
  _LoginPageState createState() => _LoginPageState();
}

class Base
{

}

class A extends Base
{

}

class B extends Base
{

}

class _LoginPageState extends State<LoginPage>
{
  var email = TextEditingController();
  var password = TextEditingController();

  final _scaffoldKey = GlobalKey<ScaffoldState>();
  final _formKey = GlobalKey<FormState>();

  AppStateModel _model;

  @override
  void dispose()
  {
    email.dispose();
    password.dispose();
    super.dispose();
  }

    @override
  Widget build(BuildContext context)
  {
    _model = ScopedModel.of(context, rebuildOnChange: true);

    return Scaffold(
      key: _scaffoldKey,
      body: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: <Widget>[
          Container(
            alignment: Alignment.center,
            padding: EdgeInsets.all(20),
            child: Text(
              "Nanoblog",
              style: TextStyle(
                fontSize: 42
              ),
            )
          ),
          Form(
            key: _formKey,
            child:Theme(
              data: ThemeData(
                primarySwatch: Colors.teal,
                inputDecorationTheme: InputDecorationTheme(
                  labelStyle: TextStyle(
                    color: Colors.teal,
                  ),
                  border: OutlineInputBorder(
                    borderRadius: BorderRadius.all(Radius.circular(10)),
                    borderSide: BorderSide(
                      color: Colors.teal,
                      width: 2
                    )
                  ),
                )
              ), 
              child: Container(
                padding: EdgeInsets.symmetric(horizontal: 40),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.center,
                  children: <Widget>[
                    TextFormField(
                      validator: _validEmail,
                      controller: email,
                      decoration: InputDecoration(
                        labelText: "Enter Email",
                      ),
                      keyboardType: TextInputType.emailAddress,
                    ),
                    Padding(
                      padding: EdgeInsets.only(top: 20),
                      child: TextFormField(
                        validator: _validPassword,
                        controller: password,
                        decoration: InputDecoration(
                          labelText: "Enter Password",
                        ),
                        keyboardType: TextInputType.text,
                        obscureText: true,
                      ),
                    ),
                    Padding(
                      padding: EdgeInsets.symmetric(vertical: 20),
                      child: Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: <Widget>[
                          FlatButton(
                            child: Text("Register"),
                            onPressed: () => login(context),
                            textColor: Colors.teal,
                          ),
                          MaterialButton(
                            color: Colors.teal,
                            textColor: Colors.white,
                            onPressed: () => login(context),
                            child: Text("Login"),
                          ),
                        ],
                      ),
                    )
                  ],
                ),
              ),
            )
          )
        ],
      )
    );
  }

  Future<bool> _toProcess() async
  {
    try
    {
      // only for debug
      var result = await AccountApi.login("harry@harry.com", "password");

      if (result != null)
      {
        _model.jwtService.setJwt(result);

        var user = await AccountApi.getUser(result.getUserId());

        if (user != null)
        {
          _model.currentUser = user;
          return true;
        }
      }
    }
    on ApiException catch(ex)
    {
      
      if (_scaffoldKey != null && _scaffoldKey.currentState != null)
      {
        _scaffoldKey.currentState.showSnackBar(SnackBar(
          content: Text(ex.toString()),
        ));
      }

      return false;
    }
    on Exception
    {
      if (_scaffoldKey != null && _scaffoldKey.currentState != null)
      {
        _scaffoldKey.currentState.showSnackBar(SnackBar(
          content: Text("Cannot login"),
        ));
      }
      return false;
    }

    return false;
  }

  void _onSuccess(BuildContext context)
  {
    Navigator.pushNamedAndRemoveUntil(context, "/home", (route) => false);
  }

  void _onFail(BuildContext context)
  {
    Navigator.pop(context);
  }

  void login(BuildContext context)
  {
    // if (_formKey.currentState.validate())
    {
      Navigator.push(context, MaterialPageRoute(
        builder: (c2) => LoadingPage(
          message: "Logging in...",
          toProcess: _toProcess,
          onSuccess: () => _onSuccess(context),
          onFail: () => _onFail(context),
        )
      ));
    }
  }

  String _validEmail(String value)
  {
    Pattern pattern =
        r'^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$';
    RegExp regex = new RegExp(pattern);
    if (!regex.hasMatch(value))
      return 'Enter Valid Email';
    else
      return null;
  }

  String _validPassword(String value)
  {
    if (value.isEmpty)
      return "Enter password";
    else
      return null;
  }
}