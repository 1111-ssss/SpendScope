import 'package:flutter/material.dart';
import 'package:isar/isar.dart';
import 'package:path_provider/path_provider.dart';
import 'package:spend_scope/model/category.dart';
import 'package:spend_scope/model/expense.dart';

class DataProviderService with ChangeNotifier {
  late Isar isar;

  final Map<String, IconData> defaultIcons = {
    "fastfood": Icons.fastfood,
    "directions_bus": Icons.directions_bus,
    "medical_services": Icons.medical_services,
    "sports_esports": Icons.sports_esports,
    "more_horiz": Icons.more_horiz,
  };

  final _defaultCategories = [
    Category(name: "Прочее", icon: "more_horiz"),
    Category(name: "Еда", icon: "fastfood"),
    Category(name: "Транспорт", icon: "directions_bus"),
    Category(name: "Здоровье", icon: "medical_services"),
    Category(name: "Развлечения", icon: "sports_esports"),
  ];

  List<Expense> expenses = [];
  List<Category> categories = [];

  double spentToday = 0;
  double limitPerDay = 20;
  String currency = "BYN";

  Future<void> initIsar() async {
    final dir = await getApplicationDocumentsDirectory();
    isar = Isar.openSync([ExpenseSchema, CategorySchema], directory: dir.path);

    //await clearAllIsarData(isar);

    await initData();
    await loadAllData();
  }

  Future<void> clearAllIsarData(Isar isar) async {
    await isar.writeTxn(() async {
      await isar.clear();
    });
  }

  Future<void> initData() async {
    if (await isar.categorys.count() == 0) {
      await isar.writeTxn(() async {
        await isar.categorys.putAll(_defaultCategories);
      });
    }
  }

  Future<void> loadAllData() async {
    categories = await isar.categorys.where().findAll();
    expenses = await isar.expenses.where().sortByDateTimeDesc().findAll();

    // _updateTodaySpent();
    notifyListeners();
  }

  double getSpentTotal(String period) {
    final now = DateTime.now();
    DateTime start, end;

    switch (period) {
      case "week":
        end = DateTime(now.year, now.month, now.day).add(Duration(days: 1));
        start = end.subtract(Duration(days: 6));
        break;
      case "month":
        end = DateTime(now.year, now.month, now.day).add(Duration(days: 1));
        start = end.subtract(Duration(days: 29));
        break;
      case "calendarWeek":
        final weekday = now.weekday;
        final startOfWeek = DateTime(
          now.year,
          now.month,
          now.day - weekday + 1,
        );
        start = DateTime(startOfWeek.year, startOfWeek.month, startOfWeek.day);
        end = DateTime(start.year, start.month, start.day + 7);
        break;
      case "yesterday":
        start = DateTime(now.year, now.month, now.day);
        end = start;
        start = start.subtract(Duration(days: 1));
        break;
      case "day":
        start = DateTime(now.year, now.month, now.day);
        end = start.add(Duration(days: 1));
        break;
      default:
        return 0.0;
    }

    return expenses
        .where((e) => e.dateTime.isAfter(start) && e.dateTime.isBefore(end))
        .fold(0.0, (sum, e) => sum + e.amount);
  }

  Color getSpentColor(String val) {
    late double val1;
    late double val2;

    switch (val) {
      case "day":
        val1 = getSpentTotal("day");
        val2 = limitPerDay;
        break;
      case "yesterday":
        val1 = getSpentTotal("yesterday");
        val2 = limitPerDay;
        break;
      case "week":
        val1 = getSpentTotal("week");
        val2 = limitPerDay * 7;
        break;
      case "month":
        val1 = getSpentTotal("month");
        val2 = limitPerDay * 30;
        break;
    }
    if (val1 > val2) {
      return Colors.redAccent.shade700;
    } else if (val1 >= val2 / 2) {
      return Colors.amber.shade700;
    }
    return Colors.green.shade700;
  }

  Future<void> addExpense(Expense expense) async {
    await isar.writeTxn(() async {
      await isar.expenses.put(expense);
    });

    expenses.insert(0, expense);
    // _updateTodaySpent();
    notifyListeners();
  }

  Future<void> editExpense({
    required int id,
    required double amount,
    required int categoryId,
    required DateTime dateTime,
    required String note,
  }) async {
    await isar.writeTxn(() async {
      final expenseObject = await isar.expenses.get(id);
      if (expenseObject != null) {
        expenseObject
          ..amount = amount
          ..categoryId = categoryId
          ..dateTime = dateTime
          ..note = note;
        isar.expenses.put(expenseObject);
      }
    });

    final expenseObject = expenses.where((e) => e.id == id).first;
    expenseObject
      ..amount = amount
      ..categoryId = categoryId
      ..dateTime = dateTime
      ..note = note;
    // _updateTodaySpent();
    notifyListeners();
  }

  Future<void> editCategory({
    required int id,
    required String name,
    required String icon,
  }) async {
    await isar.writeTxn(() async {
      final categoryObject = await isar.categorys.get(id);
      if (categoryObject != null) {
        categoryObject
          ..name = name
          ..icon = icon;
        isar.categorys.put(categoryObject);
      }
    });

    final categoryObject = categories.where((e) => e.id == id).first;
    categoryObject
      ..name = name
      ..icon = icon;
    // _updateTodaySpent();
    notifyListeners();
  }

  Future<void> deleteExpense(int id) async {
    await isar.writeTxn(() async {
      await isar.expenses.delete(id);
    });

    expenses.removeWhere((e) => e.id == id);
    // _updateTodaySpent();
    notifyListeners();
  }

  Future<void> deleteCategory(int id) async {
    await isar.writeTxn(() async {
      final expensesToUpdate = await isar.expenses
          .filter()
          .categoryIdEqualTo(id)
          .findAll();

      for (var expense in expensesToUpdate) {
        expense.categoryId = 1;
      }
      await isar.expenses.putAll(expensesToUpdate);
      await isar.categorys.delete(id);
    });

    categories.removeWhere((e) => e.id == id);
    final expensesToUpdate = expenses.where((e) => e.categoryId == id);
    for (var expense in expensesToUpdate) {
      expense.categoryId = 1;
    }
    // _updateTodaySpent();
    notifyListeners();
  }

  Future<void> addCategory(Category category) async {
    await isar.writeTxn(() async {
      await isar.categorys.put(category);
    });

    categories.add(category);
    // _updateTodaySpent();
    notifyListeners();
  }

  /* void _updateTodaySpent() {
    final today = DateTime.now();
    final start = DateTime(today.year, today.month, today.day);
    final end = start.add(const Duration(days: 1));

    spentToday = expenses
        .where((e) => e.dateTime.isAfter(start) && e.dateTime.isBefore(end))
        .fold(0, (sum, e) => sum + e.amount.toDouble());
    notifyListeners();
  } */
}
