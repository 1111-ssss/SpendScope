import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:spend_scope/service/data_provider_service.dart';

class SettingsScreen extends StatefulWidget {
  const SettingsScreen({super.key});

  @override
  State<SettingsScreen> createState() => _SettingsScreenState();
}

class _SettingsScreenState extends State<SettingsScreen> {
  final _dailyLimitController = TextEditingController();
  final _currencyController = TextEditingController();

  @override
  void initState() {
    super.initState();
    final dataProvider = Provider.of<DataProviderService>(context, listen: false);
    _dailyLimitController.text = dataProvider.limitPerDay.toString();
    _currencyController.text = dataProvider.currency;
  }

  @override
  Widget build(BuildContext context) {
    final dataProvider = context.watch<DataProviderService>();

    return Scaffold(
      appBar: AppBar(title: Text('Настройки')),
      body: ListView(
        children: [
          Card(
            child: ListTile(
              leading: Icon(Icons.credit_card_off),
              title: Text("Дневной лимит"),
              subtitle: TextField(
                decoration: InputDecoration(
                  hint: Text("20.00"),
                  border: OutlineInputBorder(),
                ),
                controller: _dailyLimitController,
                keyboardType: const TextInputType.numberWithOptions(
                  decimal: true,
                ),
              ),
              trailing: IconButton(onPressed: () {
                final amountText = _dailyLimitController.text.trim();
                if (amountText.isEmpty) return;
                final amount = double.tryParse(amountText);
                if (amount == null || amount <= 0) {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(content: Text('Введите корректную сумму')),
                  );
                  return;
                }
                dataProvider.limitPerDay = amount;
              }, icon: Icon(Icons.edit)),
            ),
          ),
          Card(
            child: ListTile(
              leading: Icon(Icons.attach_money),
              title: Text("Валюта"),
              subtitle: TextField(
                decoration: InputDecoration(
                  hint: Text("BYN"),
                  border: OutlineInputBorder(),
                ),
                controller: _currencyController,
              ),
              trailing: IconButton(onPressed: () {
                final currencyText = _dailyLimitController.text.trim();
                if (currencyText.isEmpty) return;
                // final amount = double.tryParse(amountText);
                // if (amount == null || amount <= 0) {
                //   ScaffoldMessenger.of(context).showSnackBar(
                //     const SnackBar(content: Text('Введите корректную сумму')),
                //   );
                //   return;
                // }
                dataProvider.currency = currencyText;
              }, icon: Icon(Icons.edit)),
            ),
          ),
        ],
      ),
    );
  }

  @override
  void dispose() {
    _dailyLimitController.dispose();
    super.dispose();
  }
}
