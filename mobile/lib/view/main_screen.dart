// import 'package:flutter/gestures.dart';
import 'package:carousel_slider/carousel_slider.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:spend_scope/service/data_provider_service.dart';

class MainScreen extends StatefulWidget {
  const MainScreen({super.key});

  @override
  State<MainScreen> createState() => _MainScreenState();
}

class _MainScreenState extends State<MainScreen> {
  // final _carouselController = CarouselController();

  @override
  Widget build(BuildContext context) {
    final dataProvider = context.watch<DataProviderService>();
    final double screenHeight = MediaQuery.sizeOf(context).height;

    return Column(
      children: [
        CarouselSlider(
          options: CarouselOptions(
            height: screenHeight / 5,
            enlargeFactor: 0.5,
            enlargeCenterPage: true,
          ),
          items: [
            _card(
              title: "Сегодня",
              titleText:
                  "${dataProvider.getSpentTotal("day").toStringAsFixed(2)} ${dataProvider.currency}",
              subtitle:
                  "из ${dataProvider.limitPerDay.toStringAsFixed(2)} ${dataProvider.currency}",
              titleTextColor: dataProvider.getSpentColor("day"),
            ),
            _card(
              title: "Вчера",
              titleText:
                  "${dataProvider.getSpentTotal("yesterday").toStringAsFixed(2)} ${dataProvider.currency}",
              subtitle:
                  "из ${dataProvider.limitPerDay.toStringAsFixed(2)} ${dataProvider.currency}",
              titleTextColor: dataProvider.getSpentColor("yesterday"),
            ),
          ],
        ),
        CarouselSlider(
          options: CarouselOptions(
            height: screenHeight / 5,
            enlargeFactor: 0.5,
            enlargeCenterPage: true,
          ),
          items: [
            _card(
              title: "За прошедшую неделю",
              titleText:
                  "${(dataProvider.getSpentTotal("week") / 7).toStringAsFixed(2)} ${dataProvider.currency}",
              subtitle:
                  "из ${(dataProvider.limitPerDay * 7).toStringAsFixed(2)} (~ ${(dataProvider.getSpentTotal("week") / 7).toStringAsFixed(2)} в день)",
              titleTextColor: dataProvider.getSpentColor("week"),
            ),
            _card(
              title: "За прошедший месяц",
              titleText:
                  "${dataProvider.getSpentTotal("month").toStringAsFixed(2)} ${dataProvider.currency}",
              subtitle:
                  "из ${(dataProvider.limitPerDay * 30).toStringAsFixed(2)} (~ ${(dataProvider.getSpentTotal("month") / 30).toStringAsFixed(2)} в день)",
              titleTextColor: dataProvider.getSpentColor("month"),
            ),
          ],
        ),
        // SizedBox(height: 25,),
        // CircularProgressIndicator(
        //   // semanticsLabel: "Label",
        //   // semanticsValue: "value",
        //   trackGap: 1,
        //   strokeWidth: 7,
        //   strokeAlign: 5,
        //   value: dataProvider.getSpentTotal("day") / dataProvider.limitPerDay
        // )
      ],
    );
  }

  Widget _card({
    required String title,
    required String titleText,
    required String subtitle,
    Color? titleTextColor,
  }) {
    return Container(
      width: double.infinity,
      // margin: EdgeInsets.symmetric(horizontal: 20),
      decoration: const BoxDecoration(color: Color.fromRGBO(255, 255, 255, 0)),
      child: Card(
        clipBehavior: Clip.hardEdge,
        elevation: 2,
        // margin: EdgeInsets.symmetric(horizontal: 20, vertical: 12),
        color: Theme.of(context).colorScheme.onPrimary,
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            FittedBox(
              alignment: Alignment.centerLeft,
              child: Padding(
                padding: EdgeInsets.all(20),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      title,
                      style: Theme.of(context).textTheme.titleMedium,
                      overflow: TextOverflow.fade,
                      maxLines: 1,
                    ),
                    const SizedBox(height: 8),
                    Text(
                      titleText,
                      style: TextStyle(
                        fontSize: 24,
                        fontWeight: FontWeight.bold,
                        color: titleTextColor,
                      ),
                      overflow: TextOverflow.fade,
                      maxLines: 1,
                    ),
                    Text(
                      subtitle,
                      style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                        color: Theme.of(context).colorScheme.secondary,
                      ),
                      overflow: TextOverflow.fade,
                      maxLines: 1,
                    ),
                  ],
                ),
              ),
            ),
            Padding(
              padding: EdgeInsets.all(15),
              child: Icon(
                Icons.arrow_forward,
                color: Theme.of(context).colorScheme.primary.withAlpha(100),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
