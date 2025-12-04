import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:spend_scope/model/category.dart';
import 'package:spend_scope/service/data_provider_service.dart';

class AddCategoryScreen extends StatefulWidget {
  const AddCategoryScreen({super.key});

  @override
  State<AddCategoryScreen> createState() => _AddCategoryScreenState();
}

class _AddCategoryScreenState extends State<AddCategoryScreen> {
  final _nameController = TextEditingController();
  bool loaded = false;
  String _selectedIcon = "more_horiz";
  Category? argument;

  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    argument = ModalRoute.of(context)?.settings.arguments as Category?;

    if (argument != null) {
      _selectedIcon = argument!.icon;
      _nameController.text = argument!.name;
    }
  }

  @override
  Widget build(BuildContext context) {
    final dataProvider = context.watch<DataProviderService>();

    return Scaffold(
      appBar: AppBar(title: const Text('Добавление категории')),
      floatingActionButtonLocation: FloatingActionButtonLocation.centerFloat,
      floatingActionButton: ElevatedButton.icon(
        onPressed: _selectedIcon.isEmpty
            ? null
            : () async {
                final categoryName = _nameController.text.trim();
                if (categoryName.isEmpty) return;
                if (argument != null) {
                  await dataProvider.editCategory(
                    id: argument!.id,
                    name: categoryName,
                    icon: _selectedIcon,
                  );
                  if (context.mounted) {
                    Navigator.of(context).pop();
                  }
                  return;
                }
                if (dataProvider.categories.any(
                  (c) => c.name == categoryName,
                )) {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(
                      content: Text(
                        'Категория с таким названием уже существует.',
                      ),
                    ),
                  );
                  return;
                }
                await dataProvider.addCategory(
                  Category(name: categoryName, icon: _selectedIcon),
                );
                if (context.mounted) {
                  Navigator.of(context).pop();
                }
              },
        icon: const Icon(Icons.add),
        label: const Text('Сохранить категорию'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: ListView(
          children: [
            TextField(
              controller: _nameController,
              decoration: InputDecoration(
                labelText: "Название категории",
                hintText: argument?.name ?? "Прочее",
                border: const OutlineInputBorder(),
              ),
            ),
            const SizedBox(height: 20),
            dataProvider.categories.isEmpty
                ? const Center(child: CircularProgressIndicator())
                : Wrap(
                    spacing: 8,
                    runSpacing: 8,
                    children: [
                      for (final entry in dataProvider.defaultIcons.entries)
                        ChoiceChip(
                          key: ValueKey(entry.key),
                          label: Row(
                            mainAxisSize: MainAxisSize.min,
                            children: [Icon(entry.value, size: 26)],
                          ),
                          selected: _selectedIcon == entry.key,
                          onSelected: (selected) {
                            if (selected) {
                              setState(() {
                                _selectedIcon = entry.key;
                              });
                            }
                          },
                        ),
                    ],
                  ),
          ],
        ),
      ),
    );
  }

  @override
  void dispose() {
    _nameController.dispose();
    super.dispose();
  }
}
