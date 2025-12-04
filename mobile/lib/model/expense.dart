import 'package:isar/isar.dart';

part 'expense.g.dart';

@collection
class Expense {
  Id id = Isar.autoIncrement;
  late double amount;
  @Index()
  late DateTime dateTime;
  late String note;

  // final category = IsarLink<Category>();
  late int categoryId;

  Expense({
    required this.amount,
    required this.dateTime,
    required this.categoryId,
    this.note = "",
  });
}
