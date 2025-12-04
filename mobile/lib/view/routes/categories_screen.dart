import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
// import 'package:spend_scope/model/category.dart';
import 'package:spend_scope/service/data_provider_service.dart';

class CategoriesScreen extends StatefulWidget {
  const CategoriesScreen({super.key});

  @override
  State<CategoriesScreen> createState() => _CategoriesScreenState();
}

class _CategoriesScreenState extends State<CategoriesScreen> {
  int _selectedCategory = -1;

  @override
  Widget build(BuildContext context) {
    final dataProvider = context.watch<DataProviderService>();

    return Scaffold(
      appBar: AppBar(
        title: Text('Категории'),
        actions: [
          IconButton(
            onPressed: () {
              Navigator.pushNamed(context, "/add_category");
              setState(() {
                _selectedCategory = -1;
              });
            },
            icon: Icon(Icons.add),
          ),
        ],
      ),
      body: ListView(
        children: [
          for (final category in dataProvider.categories)
            Padding(
              padding: EdgeInsetsGeometry.all(5),
              child: OutlinedButton(
                onPressed: () => setState(() {
                  _selectedCategory = _selectedCategory == category.id
                      ? -1
                      : category.id;
                }),
                child: Row(
                  children: [
                    Icon(dataProvider.defaultIcons[category.icon], size: 26),
                    const SizedBox(width: 10),
                    Text(category.name, style: TextStyle(fontSize: 16)),
                    Spacer(),
                    _selectedCategory == category.id
                        ? IconButton(
                            onPressed: () {
                              Navigator.pushNamed(
                                context,
                                "/add_category",
                                arguments: category,
                              );
                              setState(() {
                                _selectedCategory = -1;
                              });
                            },
                            icon: Icon(
                              Icons.edit,
                              color: Theme.of(context).colorScheme.tertiary,
                              size: 20,
                            ),
                          )
                        : SizedBox(),
                    _selectedCategory == category.id
                        ? IconButton(
                            onPressed: _selectedCategory != 1
                                ? () {
                                    dataProvider.deleteCategory(category.id);
                                    setState(() {
                                      _selectedCategory = -1;
                                    });
                                  }
                                : null,
                            icon: Icon(
                              Icons.delete_forever,
                              color: Theme.of(context).colorScheme.error,
                              size: 20,
                            ),
                          )
                        : SizedBox(),
                  ],
                ),
              ),
            ),
        ],
      ),
    );
  }
}
