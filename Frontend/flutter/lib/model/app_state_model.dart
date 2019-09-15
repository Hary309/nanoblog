import 'package:nanoblog/api/repository/account_repository.dart';
import 'package:nanoblog/api/repository/comment_repository.dart';
import 'package:nanoblog/api/repository/entry_repository.dart';
import 'package:nanoblog/api/repository/karma_repository.dart';
import 'package:nanoblog/model/user.dart';
import 'package:nanoblog/services/jwt_service.dart';
import 'package:scoped_model/scoped_model.dart';

class AppStateModel extends Model
{
  AccountRepository accountRepository;
  EntryRepository entryRepository;
  CommentRepository commentRepository;
  KarmaRepository karmaRepository;
  JwtService jwtService;
  User currentUser;

  AppStateModel()
  {
    accountRepository = AccountRepository();
    jwtService = JwtService(accountRepository);
    entryRepository = EntryRepository();
    commentRepository = CommentRepository(accountRepository);
    karmaRepository = KarmaRepository();
  }
}