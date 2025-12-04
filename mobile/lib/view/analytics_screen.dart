import 'package:flutter/material.dart';
import 'package:fl_chart/fl_chart.dart';
import 'package:provider/provider.dart';
import 'package:spend_scope/model/expense.dart';
import 'package:spend_scope/model/category.dart';
import 'package:intl/intl.dart';
import 'package:spend_scope/service/data_provider_service.dart';

class AnalyticsScreen extends StatelessWidget {
  const AnalyticsScreen({super.key});

  @override
  Widget build(BuildContext context) {
    final dataProvider = Provider.of<DataProviderService>(context);

    return dataProvider.expenses.isEmpty
        ? const Center(
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Icon(Icons.bar_chart, size: 64, color: Colors.grey),
                SizedBox(height: 16),
                Text(
                  "Нет данных для анализа",
                  style: TextStyle(fontSize: 18, color: Colors.grey),
                ),
              ],
            ),
          )
        : Padding(
            padding: EdgeInsetsGeometry.all(12),
            child: Column(
              children: [
                Text(
                  "Траты по дням недели",
                  style: TextStyle(
                    color: Theme.of(context).colorScheme.primary,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                const SizedBox(height: 12),
                SizedBox(
                  height: 200,
                  child: _WeeklyBarChart(expenses: dataProvider.expenses),
                ),
                const SizedBox(height: 32),
                Text(
                  "Расходы по категориям",
                  style: TextStyle(
                    color: Theme.of(context).colorScheme.primary,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                const SizedBox(height: 12),
                Container(
                  height: 240,
                  alignment: Alignment.bottomCenter,
                  child: _CategoryPieChart(
                    expenses: dataProvider.expenses,
                    categories: dataProvider.categories,
                    defaultIcons: dataProvider.defaultIcons,
                  ),
                ),
              ],
            ),
          );
  }
}

class _WeeklyBarChart extends StatelessWidget {
  final List<Expense> expenses;

  const _WeeklyBarChart({required this.expenses});

  List<double> _getWeeklySpent() {
    final now = DateTime.now();
    final endDate = DateTime(now.year, now.month, now.day, 23, 59);
    final startDate = DateTime(
      now.year,
      now.month,
      now.day,
    ).subtract(const Duration(days: 6));
    final dailyTotals = List<double>.filled(7, 0.0);

    for (final expense in expenses) {
      final expenseDate = expense.dateTime;
      if (expenseDate.isAfter(startDate.subtract(const Duration(days: 1))) &&
          // expenseDate.isBefore(startDate.add(const Duration(days: 7)))) {
          expenseDate.isBefore(endDate)) {
        final dayOffset = expenseDate.difference(startDate).inDays;
        if (dayOffset >= 0 && dayOffset < 7) {
          dailyTotals[dayOffset] += expense.amount;
        }
      }
    }
    return dailyTotals;
  }

  @override
  Widget build(BuildContext context) {
    final spent = _getWeeklySpent();
    final maxSpent = spent.reduce((a, b) => a > b ? a : b);
    // final weekDays = ['Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб', 'Вс'];
    final now = DateTime.now();
    final weekDays = List.generate(7, (i) {
      final date = DateTime(
        now.year,
        now.month,
        now.day,
      ).subtract(Duration(days: 6 - i));
      final dayNumber = date.day < 10 ? "0${date.day}" : date.day;
      final shortDay = DateFormat('E', 'ru').format(date).substring(0, 2);
      return '$dayNumber $shortDay';
    });

    return BarChart(
      BarChartData(
        alignment: BarChartAlignment.spaceAround,
        maxY: maxSpent > 0 ? maxSpent * 1.2 : 10,
        barGroups: List.generate(7, (i) {
          return BarChartGroupData(
            x: i,
            barRods: [
              BarChartRodData(
                toY: spent[i],
                width: 16,
                borderRadius: BorderRadius.circular(6),
                color: Theme.of(context).colorScheme.primary,
              ),
            ],
          );
        }),
        titlesData: FlTitlesData(
          show: true,
          bottomTitles: AxisTitles(
            sideTitles: SideTitles(
              showTitles: true,
              getTitlesWidget: (value, meta) {
                final index = value.toInt();
                if (index < 0 || index >= weekDays.length) {
                  return const SizedBox();
                }
                return Text(
                  weekDays[index],
                  style: const TextStyle(fontSize: 12, color: Colors.grey),
                );
              },
            ),
          ),
          leftTitles: const AxisTitles(
            sideTitles: SideTitles(showTitles: false),
          ),
          topTitles: const AxisTitles(
            sideTitles: SideTitles(showTitles: false),
          ),
          rightTitles: const AxisTitles(
            sideTitles: SideTitles(showTitles: false),
          ),
        ),
        gridData: const FlGridData(show: false),
        borderData: FlBorderData(show: false),
      ),
    );
  }
}

class _CategoryPieChart extends StatelessWidget {
  final List<Expense> expenses;
  final List<Category> categories;
  final Map<String, IconData> defaultIcons;

  const _CategoryPieChart({
    required this.expenses,
    required this.categories,
    required this.defaultIcons,
  });

  Map<int, double> _getCategoryTotals() {
    final totals = <int, double>{};
    for (final exp in expenses) {
      totals.update(
        exp.categoryId,
        (v) => v + exp.amount,
        ifAbsent: () => exp.amount,
      );
    }
    return totals;
  }

  @override
  Widget build(BuildContext context) {
    final totals = _getCategoryTotals();
    final totalSum = totals.values.fold(0.0, (a, b) => a + b);

    if (totals.isEmpty) {
      return const Center(child: Text('Нет данных'));
    }

    final List<PieChartSectionData> sections = [];
    final colors = [
      Colors.blue,
      Colors.green,
      Colors.orange,
      Colors.red,
      Colors.purple,
      Colors.teal,
      Colors.brown,
      Colors.pink,
      Colors.amber,
    ];

    int i = 0;
    for (final category in categories) {
      if (totals.containsKey(category.id)) {
        final amount = totals[category.id]!;
        final percent = (amount / totalSum * 100).round();
        final color = colors[i % colors.length];

        sections.add(
          PieChartSectionData(
            value: amount,
            color: color,
            title: '$percent%',
            radius: 40,
            badgeWidget: Container(
              padding: const EdgeInsets.all(4),
              decoration: BoxDecoration(
                color: color.withValues(alpha: 0.2),
                shape: BoxShape.circle,
              ),
              child: Icon(
                defaultIcons[category.icon] ?? Icons.category,
                size: 14,
                color: color,
              ),
            ),
            titlePositionPercentageOffset: 1.4,
            badgePositionPercentageOffset: 2,
          ),
        );
        i++;
      }
    }

    return PieChart(
      PieChartData(
        sections: sections,
        centerSpaceRadius: 60,
        sectionsSpace: 2,
        startDegreeOffset: 90,
      ),
    );
  }
}
