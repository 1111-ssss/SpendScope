import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:spend_scope/service/theme_provider.dart';

class ThemesScreen extends StatefulWidget {
  const ThemesScreen({super.key});

  @override
  State<ThemesScreen> createState() => _ThemesScreenState();
}

class _ThemesScreenState extends State<ThemesScreen> {
  @override
  Widget build(BuildContext context) {
    final themeProvider = Provider.of<ThemeProviderService>(context);

    return Scaffold(
      appBar: AppBar(title: Text('Темы')),
      body: ListView.builder(
        padding: const EdgeInsets.all(20.0),
        itemCount: themeProvider.themes.length,
        itemBuilder: (BuildContext context, int index) {
          return Padding(
            padding: EdgeInsetsGeometry.symmetric(vertical: 5),
            child: OutlinedButton(
              onPressed: () {
                themeProvider.changeTheme(index);
              },
              style: OutlinedButton.styleFrom(
                shape: const RoundedRectangleBorder(
                  borderRadius: BorderRadiusGeometry.all(Radius.circular(6)),
                ),
              ),
              child: Column(
                spacing: 5,
                children: [
                  const SizedBox(height: 5),
                  Text(
                    themeProvider.getThemeById(index).name,
                    style: TextStyle(fontWeight: FontWeight.bold),
                  ),
                  Row(
                    spacing: 5,
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Expanded(
                        child: Container(
                          padding: EdgeInsets.all(5),
                          color: themeProvider
                              .getColorSchemeById(index)
                              .primary,
                        ),
                      ),
                      Expanded(
                        child: Container(
                          padding: EdgeInsets.all(5),
                          color: themeProvider
                              .getColorSchemeById(index)
                              .secondary,
                        ),
                      ),
                      Expanded(
                        child: Container(
                          padding: EdgeInsets.all(5),
                          color: themeProvider
                              .getColorSchemeById(index)
                              .tertiary,
                        ),
                      ),
                      Expanded(
                        child: Container(
                          padding: EdgeInsets.all(5),
                          color: themeProvider
                              .getColorSchemeById(index)
                              .onPrimary,
                        ),
                      ),
                      Expanded(
                        child: Container(
                          padding: EdgeInsets.all(5),
                          color: themeProvider
                              .getColorSchemeById(index)
                              .onSecondary,
                        ),
                      ),
                      Expanded(
                        child: Container(
                          padding: EdgeInsets.all(5),
                          color: themeProvider
                              .getColorSchemeById(index)
                              .onTertiary,
                        ),
                      ),
                      Expanded(
                        child: Container(
                          padding: EdgeInsets.all(5),
                          color: themeProvider
                              .getColorSchemeById(index)
                              .surface,
                        ),
                      ),
                      Expanded(
                        child: Container(
                          padding: EdgeInsets.all(5),
                          color: themeProvider
                              .getColorSchemeById(index)
                              .onSurface,
                        ),
                      ),
                    ],
                  ),
                  const SizedBox(height: 5),
                ],
              ),
            ),
          );
        },
      ),
    );
  }
}
