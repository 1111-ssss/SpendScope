import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:spend_scope/domain/model/expense.dart';

class ReceiptInfoScreen extends StatefulWidget {
  const ReceiptInfoScreen({super.key});

  @override
  State<ReceiptInfoScreen> createState() => _ReceiptInfoScreenState();
}

class _ReceiptInfoScreenState extends State<ReceiptInfoScreen> {
  Map<String, dynamic>? _receiptData;
  bool _loaded = false;

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addPostFrameCallback((_) {
      _initializeFromArguments();
    });
  }

  void _initializeFromArguments() {
    setState(() {
      _receiptData =
          ModalRoute.of(context)?.settings.arguments as Map<String, dynamic>;
      _loaded = true;
    });
  }

  @override
  Widget build(BuildContext context) {
    if (!_loaded) {
      return Scaffold(
        appBar: AppBar(title: Text("Проверка чека")),
        body: CircularProgressIndicator(),
      );
    }
    final data = _receiptData!;
    final f = NumberFormat("#,##0.00", 'ru_RU');

    List<dynamic> positions = [];
    try {
      final posStr = data['positions'] ?? '[]';
      positions = jsonDecode(posStr is String ? posStr : '[]') as List;
    } catch (e) {
      //
    }

    String paymentTypeText;
    final paymentType = data['payment_type'];
    if (paymentType == 1) {
      paymentTypeText = 'Безналичный';
    } else if (paymentType == 0) {
      paymentTypeText = 'Наличный';
    } else {
      paymentTypeText = 'Неизвестно';
    }

    return Scaffold(
      appBar: AppBar(title: const Text('Информация о чеке')),
      floatingActionButtonLocation: FloatingActionButtonLocation.centerFloat,
      floatingActionButton: ElevatedButton.icon(
        onPressed: () {
          double amount = double.tryParse('${data['payment_amount']}') ?? 0;

          String dateTimeStr = data['issued_at'];
          List<String> parts = dateTimeStr.split(', ');
          List<String> dateParts = parts[0].split('/');
          List<String> timeParts = parts[1].split(':');

          int day = int.parse(dateParts[0]);
          int month = int.parse(dateParts[1]);
          int year = int.parse(dateParts[2]);
          int hour = int.parse(timeParts[0]);
          int minute = int.parse(timeParts[1]);

          DateTime dateTime = DateTime(year, month, day, hour, minute);

          Navigator.popAndPushNamed(
            context,
            "/add_expense",
            arguments: {
              "expense": Expense(
                amount: amount,
                dateTime: dateTime,
                categoryId: 1,
              ),
              "isEditing": false,
            },
          );
        },
        label: const Text("Продолжить"),
        icon: const Icon(Icons.navigate_next),
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Text(
              'Место продажи',
              style: TextStyle(fontWeight: FontWeight.bold, fontSize: 18),
            ),
            const SizedBox(height: 8),
            _infoRow('Наименование', data['name_spd'] ?? '—'),
            _infoRow(
              'Адрес',
              '${data['street_to'] ?? ''}, ${data['name_np'] ?? ''}',
            ),
            _infoRow('Область', data['oblast_soato'] ?? '—'),
            _infoRow('УНП продавца', data['unp'] ?? '—'),

            const SizedBox(height: 16),

            _infoRow('Дата и время выдачи', data['issued_at'] ?? '—'),
            _infoRow('Номер документа', data['doc_num'] ?? '—'),
            _infoRow('Касса №', data['cashbox_number'].toString()),
            _infoRow('СКНО', data['skno_number'] ?? '—'),
            _infoRow('Кассир', data['cashier'] ?? '—'),

            const SizedBox(height: 16),

            _infoRow(
              'Сумма оплаты',
              '${f.format(double.tryParse('${data['payment_amount']}') ?? 0)} ${data['currency'] ?? 'BYN'}',
            ),
            _infoRow(
              'Итого',
              '${f.format(double.tryParse('${data['total_amount']}') ?? 0)} ${data['currency'] ?? 'BYN'}',
            ),

            const SizedBox(height: 16),

            _infoRow('Тип оплаты', paymentTypeText),

            const SizedBox(height: 16),

            const Text(
              'Позиции в чеке',
              style: TextStyle(fontWeight: FontWeight.bold, fontSize: 18),
            ),
            const SizedBox(height: 8),
            ...List.generate(positions.length, (i) {
              final pos = positions[i] as Map<String, dynamic>;
              final name = pos['product_name'] ?? 'Не указано';
              final count = pos['product_count'] ?? '0';
              final amount = double.tryParse('${pos['amount']}') ?? 0.0;
              final discount = double.tryParse('${pos['discount']}') ?? 0.0;

              return Card(
                margin: const EdgeInsets.symmetric(vertical: 4),
                child: ListTile(
                  title: Text(
                    name,
                    style: const TextStyle(fontWeight: FontWeight.bold),
                  ),
                  subtitle: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text('Кол-во: $count'),
                      Text('Сумма: ${f.format(amount)}'),
                      if (discount > 0) Text('Скидка: ${f.format(discount)}'),
                    ],
                  ),
                ),
              );
            }),
          ],
        ),
      ),
    );
  }

  Widget _infoRow(String label, String value) {
    value = value.isEmpty ? "-" : value;
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 4),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          SizedBox(
            width: 160,
            child: Text(
              '$label:',
              style: const TextStyle(fontWeight: FontWeight.bold),
            ),
          ),
          Expanded(child: Text(value)),
        ],
      ),
    );
  }
}
