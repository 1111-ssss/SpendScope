import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';

class ThemeProviderService with ChangeNotifier {
  late ThemeData _currentTheme;
  static final List<ThemeItem> _themes = [
    ThemeItem("Светлая тема", ThemeData.light()),
    ThemeItem("Темная тема", ThemeData.dark()),
    ThemeItem(
      "Светлая — Синяя",
      ThemeData(
        brightness: Brightness.light,
        colorScheme: ColorScheme.fromSeed(
          seedColor: Colors.blue,
          brightness: Brightness.light,
        ),
        useMaterial3: true,
      ),
    ),
    ThemeItem(
      "Светлая — Зелёная",
      ThemeData(
        brightness: Brightness.light,
        colorScheme: ColorScheme.fromSeed(
          seedColor: Colors.green,
          brightness: Brightness.light,
        ),
        useMaterial3: true,
      ),
    ),
    ThemeItem(
      "Светлая — Фиолетовая",
      ThemeData(
        brightness: Brightness.light,
        colorScheme: ColorScheme.fromSeed(
          seedColor: Colors.purple,
          brightness: Brightness.light,
        ),
        useMaterial3: true,
      ),
    ),
    ThemeItem(
      "Светлая — Оранжевая",
      ThemeData(
        brightness: Brightness.light,
        colorScheme: ColorScheme.fromSeed(
          seedColor: Colors.orange,
          brightness: Brightness.light,
        ),
        useMaterial3: true,
      ),
    ),
    ThemeItem(
      "Светлая — Индиго",
      ThemeData(
        brightness: Brightness.light,
        colorScheme: ColorScheme.fromSeed(
          seedColor: Colors.indigo,
          brightness: Brightness.light,
        ),
        useMaterial3: true,
      ),
    ),
    ThemeItem(
      "Светлая — Красная",
      ThemeData(
        brightness: Brightness.light,
        colorScheme: ColorScheme.fromSeed(
          seedColor: Colors.red,
          brightness: Brightness.light,
        ),
        useMaterial3: true,
      ),
    ),
    ThemeItem(
      "Светлая — Розовая",
      ThemeData(
        brightness: Brightness.light,
        colorScheme: ColorScheme.fromSeed(
          seedColor: Colors.pink,
          brightness: Brightness.light,
        ),
        useMaterial3: true,
      ),
    ),
    ThemeItem(
      "Светлая — Лайм",
      ThemeData(
        brightness: Brightness.light,
        colorScheme: ColorScheme.fromSeed(
          seedColor: Colors.lime,
          brightness: Brightness.light,
        ),
        useMaterial3: true,
      ),
    ),
    ThemeItem(
      "Темная — Синяя",
      ThemeData(
        brightness: Brightness.dark,
        colorScheme: ColorScheme.fromSeed(
          seedColor: Colors.blue,
          brightness: Brightness.dark,
        ),
        useMaterial3: true,
      ),
    ),
    ThemeItem(
      "Темная — Зелёная",
      ThemeData(
        brightness: Brightness.dark,
        colorScheme: ColorScheme.fromSeed(
          seedColor: Colors.green,
          brightness: Brightness.dark,
        ),
        useMaterial3: true,
      ),
    ),
    ThemeItem(
      "Темная — Фиолетовая",
      ThemeData(
        brightness: Brightness.dark,
        colorScheme: ColorScheme.fromSeed(
          seedColor: Colors.purple,
          brightness: Brightness.dark,
        ),
        useMaterial3: true,
      ),
    ),
    ThemeItem(
      "Темная — Аквамарин",
      ThemeData(
        brightness: Brightness.dark,
        colorScheme: ColorScheme.fromSeed(
          seedColor: Colors.teal,
          brightness: Brightness.dark,
        ),
        useMaterial3: true,
      ),
    ),
    ThemeItem(
      "Темная — Индиго",
      ThemeData(
        brightness: Brightness.dark,
        colorScheme: ColorScheme.fromSeed(
          seedColor: Colors.indigo,
          brightness: Brightness.dark,
        ),
        useMaterial3: true,
      ),
    ),
    ThemeItem(
      "Темная — Красная",
      ThemeData(
        brightness: Brightness.dark,
        colorScheme: ColorScheme.fromSeed(
          seedColor: Colors.red,
          brightness: Brightness.dark,
        ),
        useMaterial3: true,
      ),
    ),
    ThemeItem(
      "Темная — Розовая",
      ThemeData(
        brightness: Brightness.dark,
        colorScheme: ColorScheme.fromSeed(
          seedColor: Colors.pink,
          brightness: Brightness.dark,
        ),
        useMaterial3: true,
      ),
    ),
    ThemeItem(
      "Темная — Лайм",
      ThemeData(
        brightness: Brightness.dark,
        colorScheme: ColorScheme.fromSeed(
          seedColor: Colors.lime,
          brightness: Brightness.dark,
        ),
        useMaterial3: true,
      ),
    ),
  ];

  ThemeData get currentTheme => _currentTheme;
  List<ThemeItem> get themes => _themes;
  ColorScheme getColorScheme() => _currentTheme.colorScheme;
  ColorScheme getColorSchemeById(int index) =>
      getThemeById(index).themeData.colorScheme;

  Future<void> initTheme() async {
    await _loadTheme();
  }

  Future<void> _loadTheme() async {
    final prefs = await SharedPreferences.getInstance();
    final savedTheme = prefs.getInt("theme") ?? 0;
    _setTheme(savedTheme);
  }

  void changeTheme(int themeId) async {
    _setTheme(themeId);
    final prefs = await SharedPreferences.getInstance();
    await prefs.setInt("theme", themeId);
  }

  void _setTheme(int themeId) {
    _currentTheme = getThemeById(themeId).themeData;
    notifyListeners();
  }

  ThemeItem getThemeById(int index) {
    if (index >= 0 && index < _themes.length) {
      return _themes[index];
    }
    return _themes[0];
  }
}

class ThemeItem {
  late String _name;
  late ThemeData _themeData;

  String get name => _name;
  ThemeData get themeData => _themeData;

  ThemeItem(String name, ThemeData themeData) {
    _name = name;
    _themeData = themeData;
  }
}
