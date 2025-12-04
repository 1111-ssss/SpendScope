import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:spend_scope/service/data_provider_service.dart';
import 'package:spend_scope/service/theme_provider.dart';
import 'package:spend_scope/view/main_screen.dart';
import 'package:spend_scope/view/analytics_screen.dart';
import 'package:spend_scope/view/history_screen.dart';
import 'package:spend_scope/view/routes/add_category_screen.dart';
import 'package:spend_scope/view/routes/add_expense_screen.dart';
import 'package:spend_scope/view/routes/themes_screen.dart';
import 'package:spend_scope/view/routes/categories_screen.dart';
import 'package:spend_scope/view/routes/settings_screen.dart';
import 'package:spend_scope/view/routes/qr_code_scanner_screen.dart';
import 'package:spend_scope/view/routes/receipt_info_screen.dart';
import 'package:intl/date_symbol_data_local.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  final dataProvider = DataProviderService();
  await dataProvider.initIsar();

  final themeProvider = ThemeProviderService();
  await themeProvider.initTheme();

  initializeDateFormatting('ru_RU', null);

  runApp(
    MultiProvider(
      providers: [
        ChangeNotifierProvider<DataProviderService>(
          create: (_) => dataProvider,
        ),
        ChangeNotifierProvider<ThemeProviderService>(
          create: (_) => themeProvider,
        ),
      ],
      child: const SpendScopeApp(),
    ),
  );
}

class SpendScopeApp extends StatelessWidget {
  const SpendScopeApp({super.key});

  @override
  Widget build(BuildContext context) {
    final theme = Provider.of<ThemeProviderService>(context).currentTheme;

    return MaterialApp(
      title: "SpendScope",
      debugShowCheckedModeBanner: false,
      theme: theme,
      initialRoute: "/",
      routes: {
        "/": (context) => MainApp(),
        "/add_expense": (context) => AddExpenseScreen(),
        "/themes": (context) => ThemesScreen(),
        "/categories": (context) => CategoriesScreen(),
        "/settings": (context) => SettingsScreen(),
        "/add_category": (context) => AddCategoryScreen(),
        "/qr_code_scanner": (context) => QrScannerScreen(),
        "/receipt_info": (context) => ReceiptInfoScreen(),
      },
    );
  }
}

class MainApp extends StatefulWidget {
  const MainApp({super.key});

  @override
  State<MainApp> createState() => _MainAppState();
}

class _MainAppState extends State<MainApp> {
  int _currentIndex = 0;
  final List<Widget> _screens = [
    const MainScreen(),
    const AnalyticsScreen(),
    const HistoryScreen(),
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Transform.scale(
          scaleX: 1.1,
          child: Text(
            "SpendScope",
            style: TextStyle(
              color: Theme.of(context).colorScheme.primary,
              fontWeight: FontWeight.bold,
              fontStyle: FontStyle.italic,
              letterSpacing: 1.1,
            ),
          ),
        ),
        centerTitle: true,
        animateColor: true,
        actions: [
          IconButton(
            onPressed: () => Navigator.pushNamed(context, "/qr_code_scanner"),
            // icon: Icon(Icons.qr_code),
            icon: Icon(Icons.camera_alt),
          ),
        ],
        // backgroundColor: Theme.of(context).colorScheme.primaryContainer,
      ),
      body: _screens[_currentIndex],
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: _currentIndex,
        onTap: (index) => setState(() => _currentIndex = index),
        items: const [
          BottomNavigationBarItem(icon: Icon(Icons.home), label: 'Главная'),
          BottomNavigationBarItem(icon: Icon(Icons.bar_chart), label: 'Анализ'),
          BottomNavigationBarItem(icon: Icon(Icons.history), label: 'История'),
        ],
      ),
      floatingActionButtonLocation: FloatingActionButtonLocation.centerFloat,
      floatingActionButton: _currentIndex == 0
          ? FloatingActionButton(
              onPressed: () {
                Navigator.pushNamed(context, "/add_expense");
              },
              backgroundColor: Theme.of(context).colorScheme.primary,
              child: Icon(
                Icons.add,
                size: 35,
                color: Theme.of(context).colorScheme.onPrimary,
              ),
            )
          : null,
      drawer: Drawer(
        child: ListView(
          padding: EdgeInsets.zero,
          children: [
            DrawerHeader(
              decoration: BoxDecoration(
                color: Theme.of(context).colorScheme.primaryContainer,
              ),
              child: Text(
                'Меню',
                style: TextStyle(
                  color: Theme.of(context).colorScheme.primary,
                  fontSize: 24,
                ),
              ),
            ),
            ListTile(
              leading: const Icon(Icons.color_lens),
              title: const Text('Темы'),
              onTap: () {
                Navigator.pop(context);
                Navigator.pushNamed(context, "/themes");
              },
            ),
            ListTile(
              leading: const Icon(Icons.category),
              title: const Text('Категории'),
              onTap: () {
                Navigator.pop(context);
                Navigator.pushNamed(context, "/categories");
              },
            ),
            ListTile(
              leading: const Icon(Icons.settings),
              title: const Text('Настройки'),
              onTap: () {
                Navigator.pop(context);
                Navigator.pushNamed(context, "/settings");
              },
            ),
          ],
        ),
      ),
    );
  }
}
