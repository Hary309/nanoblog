import 'package:nanoblog/api/comment_api.dart';
import 'package:nanoblog/api/entry_api.dart';
import 'package:nanoblog/api/repository/account_repository.dart';
import 'package:nanoblog/api/repository/comment_repository.dart';
import 'package:nanoblog/api/response/entry_response.dart';
import 'package:nanoblog/model/comment.dart';
import 'package:nanoblog/model/entry.dart';
import 'package:nanoblog/model/jwt.dart';

class EntryRepository
{
  AccountRepository _accountRepository;
  CommentRepository _commentRepository;

  EntryRepository(this._accountRepository, this._commentRepository);

  Future<List<Entry>> getEntries() async
  {
    var response = await EntryApi.getEntries();

    if (response == null)
      return null;

    List<Entry> result = List<Entry>(response.length);

    int i = 0;

    for (var item in response)
    {
      result[i++] = await _getEntry(item);
    }

    return result;
  }

  Future<Entry> getEntry(String id) async
  {
    var result = await EntryApi.getEntry(id);

    if (result == null)
      return null;

    return _getEntry(result);
  }

  Future<Entry> addEntry(String text, Jwt jwtToken) async
  {
    var result = await EntryApi.addEntry(text, jwtToken);

    if (result == null)
      return null;

    return _getEntry(result);
  }

  Future<bool> deleteEntry(String id, Jwt jwtToken) async
  {
    return await EntryApi.deleteEntry(id, jwtToken);
  }

  Future<Entry> _getEntry(EntryResponse entry) async
  {
    var user = await _accountRepository.getUser(entry.authorId);

    var comments = await _commentRepository.getComments(entry.id);

    return Entry(
      id: entry.id,
      author: user,
      text: entry.text,
      comments: comments,
      createTime: entry.createTime,
    );
  }
}