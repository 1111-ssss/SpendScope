import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:intl/intl.dart';
import 'package:spend_scope/model/expense.dart';
import 'package:spend_scope/service/data_provider_service.dart';
import 'package:flutter_slidable/flutter_slidable.dart';

class HistoryScreen extends StatefulWidget {
  const HistoryScreen({super.key});

  @override
  State<HistoryScreen> createState() => _HistoryScreenState();
}

class _HistoryScreenState extends State<HistoryScreen> {
  final List<int> _selectedCategories = [];

  @override
  Widget build(BuildContext context) {
    final dataProvider = Provider.of<DataProviderService>(context);
    final groupedExpenses = _groupExpensesByDate(dataProvider.expenses);

    return groupedExpenses.isEmpty
        ? const Center(child: Text('Нет записей'))
        : Column(
            children: [
              dataProvider.categories.isEmpty
                  // ? const Center(child: CircularProgressIndicator())
                  ? const Text("Категории отсутствуют.")
                  : SizedBox(
                      width: double.infinity,
                      height: 40,
                      child: ListView(
                        scrollDirection: Axis.horizontal,
                        children: [
                          for (final category in dataProvider.categories)
                            ChoiceChip(
                              label: Row(
                                mainAxisSize: MainAxisSize.min,
                                children: [
                                  Icon(
                                    dataProvider.defaultIcons[category.icon],
                                  ),
                                  const SizedBox(width: 4),
                                  Text(category.name),
                                ],
                              ),
                              selected: _selectedCategories.contains(
                                category.id,
                              ),
                              onSelected: (selected) => {
                                if (selected)
                                  {
                                    setState(() {
                                      _selectedCategories.add(category.id);
                                    }),
                                  }
                                else if (_selectedCategories.contains(
                                  category.id,
                                ))
                                  {
                                    setState(() {
                                      _selectedCategories.remove(category.id);
                                    }),
                                  },
                              },
                            ),
                        ],
                      ),
                    ),
              Expanded(
                child: ListView.builder(
                  itemCount: groupedExpenses.length,
                  itemBuilder: (context, index) {
                    final entry = groupedExpenses[index];

                    final filteredExpenses = _selectedCategories.isEmpty
                        ? entry.value
                        : entry.value
                              .where(
                                (e) =>
                                    _selectedCategories.contains(e.categoryId),
                              )
                              .toList();

                    if (filteredExpenses.isEmpty) {
                      return const SizedBox();
                    }

                    return Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Padding(
                          padding: const EdgeInsets.symmetric(
                            horizontal: 16.0,
                            vertical: 8.0,
                          ),
                          child: Text(
                            entry.key,
                            style: Theme.of(context).textTheme.titleMedium
                                ?.copyWith(fontWeight: FontWeight.bold),
                          ),
                        ),
                        ...filteredExpenses.map((expense) {
                          return _buildExpenseTile(context, expense);
                        }),
                      ],
                    );
                  },
                ),
              ),
            ],
          );
  }

  List<MapEntry<String, List<Expense>>> _groupExpensesByDate(
    List<Expense> expenses,
  ) {
    final Map<String, List<Expense>> groups = {};

    for (final expense in expenses) {
      final date = expense.dateTime;
      final label = _formatDateLabel(date);
      groups.putIfAbsent(label, () => []).add(expense);
    }

    return groups.entries.toList();
  }

  String _formatDateLabel(DateTime date) {
    final now = DateTime.now();
    final today = DateTime(now.year, now.month, now.day);
    final yesterday = today.subtract(const Duration(days: 1));
    final expenseDay = DateTime(date.year, date.month, date.day);

    if (expenseDay == today) {
      return 'Сегодня';
    } else if (expenseDay == yesterday) {
      return 'Вчера';
    } else {
      return DateFormat('d MMMM').format(date);
    }
  }

  Widget _buildExpenseTile(BuildContext context, Expense expense) {
    final dataProvider = Provider.of<DataProviderService>(
      context,
      listen: false,
    );
    final category = dataProvider.categories
        .where((c) => c.id == expense.categoryId)
        .firstOrNull;
    final categoryName = category != null ? category.name : "Нет категории";
    final iconKey = category != null ? category.icon : "more_horiz";
    final icon = dataProvider.defaultIcons[iconKey] ?? Icons.more_horiz;

    return Slidable(
      key: Key('expense-${expense.id}'),
      endActionPane: ActionPane(
        motion: const ScrollMotion(),
        children: [
          SlidableAction(
            onPressed: (context) => Navigator.pushNamed(
              context,
              '/add_expense',
              arguments: {'expense': expense, 'isEditing': true},
            ),
            backgroundColor: Theme.of(context).colorScheme.surface,
            foregroundColor: Theme.of(context).colorScheme.surfaceTint,
            icon: Icons.edit,
            label: 'Редактировать',
          ),
          SlidableAction(
            onPressed: (context) async {
              final confirm = await showDialog<bool>(
                context: context,
                builder: (context) => AlertDialog(
                  title: const Text('Подтверждение'),
                  content: const Text(
                    'Вы уверены, что хотите удалить эту трату?',
                  ),
                  actions: [
                    TextButton(
                      onPressed: () => Navigator.pop(context, false),
                      child: const Text('Отмена'),
                    ),
                    TextButton(
                      onPressed: () => Navigator.pop(context, true),
                      child: const Text('Удалить'),
                    ),
                  ],
                ),
              );

              if (confirm == true) {
                await dataProvider.deleteExpense(expense.id);
              }
            },
            backgroundColor: Theme.of(context).colorScheme.errorContainer,
            foregroundColor: Theme.of(context).colorScheme.error,
            icon: Icons.delete,
            label: 'Удалить',
          ),
        ],
      ),
      child: ListTile(
        leading: CircleAvatar(
          backgroundColor: Theme.of(context).colorScheme.secondaryContainer,
          foregroundColor: Theme.of(context).colorScheme.onSecondaryContainer,
          child: Icon(icon, size: 20),
        ),
        title: Text(
          '${expense.amount.toStringAsFixed(2)} ${dataProvider.currency}',
          style: const TextStyle(fontWeight: FontWeight.bold),
        ),
        subtitle: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(categoryName),
            if (expense.note.isNotEmpty) Text(expense.note),
            Text(
              DateFormat('HH:mm').format(expense.dateTime),
              style: const TextStyle(fontSize: 12, color: Colors.grey),
            ),
          ],
        ),
        trailing: Icon(
          Icons.arrow_forward_ios,
          size: 16,
          color: Colors.grey.shade500,
        ),
      ),
    );

    /* return Dismissible(
      key: Key('expense-${expense.id}'),
      direction: DismissDirection.endToStart,
      background: Container(
        // color: Colors.red,
        color: Theme.of(context).colorScheme.inversePrimary,
        alignment: Alignment.centerRight,
        padding: const EdgeInsets.only(right: 20.0),
        child: Icon(Icons.edit, color: Theme.of(context).colorScheme.onPrimary),
      ),
      onDismissed: (direction) {
        /* ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Трата удалена'),
            behavior: SnackBarBehavior.floating,
            padding: const EdgeInsets.symmetric(horizontal: 8.0),
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(10.0),
            ),
            action: SnackBarAction(
              label: 'Отмена',
              onPressed: () {
                dataProvider.addExpense(expense); // поменять потом
              },
            ),
          ),
        );
        dataProvider.deleteExpense(expense.id); */

        Navigator.pushNamed(context, "/add_expense", arguments: expense);
      },
      child: ListTile(
        leading: CircleAvatar(
          backgroundColor: Theme.of(context).colorScheme.secondaryContainer,
          foregroundColor: Theme.of(context).colorScheme.onSecondaryContainer,
          child: Icon(icon, size: 20),
        ),
        title: Text(
          '${expense.amount.toStringAsFixed(2)} ${dataProvider.currency}',
          style: const TextStyle(fontWeight: FontWeight.bold),
        ),
        subtitle: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(categoryName),
            if (expense.note.isNotEmpty) Text(expense.note),
            Text(
              DateFormat('HH:mm').format(expense.dateTime),
              style: const TextStyle(fontSize: 12, color: Colors.grey),
            ),
          ],
        ),
        trailing: Icon(
          Icons.arrow_forward_ios,
          size: 16,
          color: Colors.grey.shade500,
        ),
      ),
    ); */
  }
}
