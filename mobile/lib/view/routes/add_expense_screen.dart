import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:spend_scope/model/expense.dart';
import 'package:spend_scope/model/category.dart';
import 'package:spend_scope/service/data_provider_service.dart';
import 'package:intl/intl.dart';

class AddExpenseScreen extends StatefulWidget {
  const AddExpenseScreen({super.key});

  @override
  State<AddExpenseScreen> createState() => _AddExpenseScreenState();
}

class _AddExpenseScreenState extends State<AddExpenseScreen> {
  final _amountController = TextEditingController();
  final _noteController = TextEditingController();
  late DateTime _selectedDate;
  late TimeOfDay _selectedTime;
  Category? _selectedCategory;
  bool isEditing = false;
  Expense? argument;

  @override
  void initState() {
    super.initState();
    _selectedDate = DateTime.now();
    _selectedTime = TimeOfDay.now();

    WidgetsBinding.instance.addPostFrameCallback((_) {
      _initializeFromArguments();
    });
  }

  void _initializeFromArguments() {
    final args =
        (ModalRoute.of(context)?.settings.arguments ?? <String, dynamic>{})
            as Map;

    if (args['expense'] != null) {
      argument = args['expense'];
      isEditing = args['isEditing'] ?? false;
    }

    if (argument != null) {
      final dataProvider = Provider.of<DataProviderService>(
        context,
        listen: false,
      );
      setState(() {
        _amountController.text = argument!.amount.toString();
        _noteController.text = argument!.note;
        _selectedDate = DateTime(
          argument!.dateTime.year,
          argument!.dateTime.month,
          argument!.dateTime.day,
        );
        _selectedTime = TimeOfDay(
          hour: argument!.dateTime.hour,
          minute: argument!.dateTime.minute,
        );
        _selectedCategory = dataProvider.categories
            .where((c) => c.id == argument!.categoryId)
            .firstOrNull;
      });
    }
  }

  /* @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    argument = ModalRoute.of(context)?.settings.arguments as Expense?;
    final dataProvider = Provider.of<DataProviderService>(
      context,
      listen: false,
    );

    if (argument != null) {
      _amountController.text = argument!.amount.toString();
      _noteController.text = argument!.note.toString();
      final dateTime = argument!.dateTime;
      _selectedDate = DateTime(dateTime.year, dateTime.month, dateTime.day);
      _selectedTime = TimeOfDay(hour: dateTime.hour, minute: dateTime.minute);
      _selectedCategory = dataProvider.categories
          .where((e) => e.id == argument!.categoryId)
          .firstOrNull;
    }
  } */

  Future<void> _selectDate(BuildContext context) async {
    final picked = await showDatePicker(
      context: context,
      initialDate: _selectedDate,
      firstDate: DateTime(2000),
      lastDate: DateTime(2100),
    );
    if (picked != null && picked != _selectedDate) {
      setState(() {
        _selectedDate = picked;
      });
    }
  }

  Future<void> _selectTime(BuildContext context) async {
    final picked = await showTimePicker(
      context: context,
      initialTime: _selectedTime,
    );
    if (picked != null && picked != _selectedTime) {
      setState(() {
        _selectedTime = picked;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    final dataProvider = context.watch<DataProviderService>();

    return Scaffold(
      appBar: AppBar(title: const Text('Добавление расхода')),
      floatingActionButtonLocation: FloatingActionButtonLocation.centerFloat,
      floatingActionButton: ElevatedButton.icon(
        onPressed: _selectedCategory == null
            ? null
            : () async {
                final amountText = _amountController.text.trim();
                if (amountText.isEmpty) return;
                final amount = double.tryParse(amountText);
                if (amount == null || amount <= 0) {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(content: Text('Введите корректную сумму')),
                  );
                  return;
                }
                if (argument != null && isEditing) {
                  await dataProvider.editExpense(
                    id: argument!.id,
                    amount: amount,
                    dateTime: DateTime(
                      _selectedDate.year,
                      _selectedDate.month,
                      _selectedDate.day,
                      _selectedTime.hour,
                      _selectedTime.minute,
                    ),
                    categoryId: _selectedCategory?.id ?? 0,
                    note: _noteController.text.trim(),
                  );
                  if (context.mounted) {
                    Navigator.of(context).pop();
                  }
                  return;
                }
                await dataProvider.addExpense(
                  Expense(
                    amount: amount,
                    dateTime: DateTime(
                      _selectedDate.year,
                      _selectedDate.month,
                      _selectedDate.day,
                      _selectedTime.hour,
                      _selectedTime.minute,
                    ),
                    categoryId: _selectedCategory?.id ?? 0,
                    note: _noteController.text.trim(),
                  ),
                );
                if (context.mounted) {
                  Navigator.of(context).pop();
                }
              },
        icon: const Icon(Icons.save),
        label: const Text('Сохранить расход'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: ListView(
          children: [
            TextField(
              controller: _amountController,
              decoration: InputDecoration(
                labelText: "Сумма",
                hintText: "0.00",
                prefixText: "${dataProvider.currency} ",
                border: const OutlineInputBorder(),
              ),
              keyboardType: const TextInputType.numberWithOptions(
                decimal: true,
              ),
              onChanged: (value) {
                if (value.contains(',')) {
                  _amountController.text = _amountController.text.replaceAll(
                    RegExp(r','),
                    '.',
                  );
                  _amountController.selection = TextSelection.fromPosition(
                    TextPosition(offset: _amountController.text.length),
                  );
                }
              },
            ),
            const SizedBox(height: 20),
            const Text("Дата", style: TextStyle(fontWeight: FontWeight.bold)),
            ListTile(
              title: Text(DateFormat("dd.MM.yy").format(_selectedDate)),
              trailing: const Icon(Icons.calendar_today),
              onTap: () => _selectDate(context),
            ),
            const SizedBox(height: 20),
            const Text("Время", style: TextStyle(fontWeight: FontWeight.bold)),
            ListTile(
              title: Text(_selectedTime.format(context)),
              trailing: const Icon(Icons.timer),
              onTap: () => _selectTime(context),
            ),
            const SizedBox(height: 20),
            const Text(
              "Категория",
              style: TextStyle(fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 16),
            dataProvider.categories.isEmpty
                ? const Center(child: CircularProgressIndicator())
                : Wrap(
                    spacing: 8,
                    runSpacing: 8,
                    children: [
                      for (final category in dataProvider.categories)
                        ChoiceChip(
                          label: Row(
                            mainAxisSize: MainAxisSize.min,
                            children: [
                              Icon(dataProvider.defaultIcons[category.icon]),
                              const SizedBox(width: 4),
                              Text(category.name),
                            ],
                          ),
                          selected: _selectedCategory?.id == category.id,
                          onSelected: (selected) {
                            if (selected) {
                              setState(() {
                                _selectedCategory = category;
                              });
                            }
                          },
                        ),
                    ],
                  ),
            const SizedBox(height: 20),
            TextField(
              controller: _noteController,
              decoration: InputDecoration(
                labelText: "Комментарий",
                border: const OutlineInputBorder(),
              ),
            ),
            const Spacer(),
          ],
        ),
      ),
    );
  }

  @override
  void dispose() {
    _amountController.dispose();
    _noteController.dispose();
    super.dispose();
  }
}
