import 'package:isar/isar.dart';
import 'package:spend_scope/model/expense.dart';

part 'category.g.dart';

@collection
class Category {
  Id id = Isar.autoIncrement;
  late String name;
  late String icon;

  final expenses = IsarLinks<Expense>();

  Category({required this.name, this.icon = "more_horiz"});
}
