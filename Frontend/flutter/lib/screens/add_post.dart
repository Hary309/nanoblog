import 'package:flutter/material.dart';
import 'package:nanoblog/api/entry_api.dart';
import 'package:nanoblog/exceptions/api_exception.dart';
import 'package:nanoblog/model/app_state_model.dart';
import 'package:nanoblog/model/entry.dart';
import 'package:scoped_model/scoped_model.dart';

class AddPostPage extends StatefulWidget
{
  @override
  _AddPostPageState createState() => _AddPostPageState();
}

class _AddPostPageState extends State<AddPostPage>
{
  var messageController = TextEditingController();

  final _scaffoldKey = GlobalKey<ScaffoldState>();

  AppStateModel _model;

  @override
  Widget build(BuildContext context)
  {
    _model = ScopedModel.of<AppStateModel>(context);

    return Scaffold(
      key: _scaffoldKey,
      appBar: AppBar(
        title: Text("Add post"),
        actions: <Widget>[
          IconButton(
            icon: Icon(Icons.arrow_forward),
            onPressed: _onSubmit,
          )
        ],
      ),
      body: Container(
        padding: EdgeInsets.all(16),
        child: TextField(
          decoration: InputDecoration(
            hintText: "Your post"
          ),
          textCapitalization: TextCapitalization.sentences,
          keyboardType: TextInputType.multiline,
          scrollPadding: EdgeInsets.all(20),
          maxLines: 999999,
          autofocus: true,
          controller: messageController,
        )
      ),
    );
  }

  @override
  void dispose()
  {
    messageController.dispose();
    super.dispose();
  }

  void _showMessage(String message)
  {
    _scaffoldKey.currentState.showSnackBar(SnackBar(
      content: Text(message),
    ));
  }

  void _onSubmit() async
  {
    if (messageController.text.isEmpty)
    {
      _showMessage("Enter a message!");
    }
    else
    {
      if (_model.jwtService.jwtToken == null)
      {
        _showMessage("You have to log in!");
        return;
      }

      try
      {
        await _model.jwtService.tryRefreshToken();

        Entry result = await EntryApi.addEntry(messageController.text, _model.jwtService.jwtToken);

        Navigator.pop(context, result);
      }
      on ApiException catch (ex)
      {
        _showMessage(ex.toString());
      }
    }
  }
}