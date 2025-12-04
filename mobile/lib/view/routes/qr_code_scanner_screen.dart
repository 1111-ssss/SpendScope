import 'package:flutter/foundation.dart';
import 'package:flutter/gestures.dart';
import 'package:flutter/material.dart';
import 'package:camera/camera.dart';
import 'package:google_mlkit_barcode_scanning/google_mlkit_barcode_scanning.dart';
import 'package:google_mlkit_text_recognition/google_mlkit_text_recognition.dart';
import 'package:provider/provider.dart';
import 'package:spend_scope/model/expense.dart';
import 'package:spend_scope/service/data_provider_service.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

class QrScannerScreen extends StatefulWidget {
  const QrScannerScreen({super.key});

  @override
  State<QrScannerScreen> createState() => _QrScannerScreenState();
}

class _QrScannerScreenState extends State<QrScannerScreen> {
  CameraController? _cameraController;
  late List<CameraDescription> _cameras;
  bool _isInitialized = false;
  bool _qrFound = false;
  bool _isProcessing = false;
  bool _isChecking = false;
  bool _showMore = false;
  String? _error;
  Map<String, dynamic>? _receiptData;
  final TextRecognizer _textRecognizer = TextRecognizer();
  final BarcodeScanner _barcodeScanner = BarcodeScanner(
    formats: [BarcodeFormat.qrCode],
  );
  String _qrCode = '';
  String? _date;
  double? _amount;
  DateTime? _lastTextProcessTime;
  Set<double> _allMatches = {};

  @override
  void initState() {
    super.initState();
    _initCamera();
  }

  Future<void> _initCamera() async {
    _cameras = await availableCameras();
    if (_cameras.isEmpty) {
      return;
    }
    _cameraController = CameraController(
      _cameras.first,
      ResolutionPreset.high,
      enableAudio: false,
      imageFormatGroup: ImageFormatGroup.nv21,
    );
    await _cameraController!.initialize();
    if (!mounted) {
      return;
    }
    setState(() => _isInitialized = true);
    _lastTextProcessTime = DateTime.now();
    _cameraController!.startImageStream(_processFrame);
  }

  Future<void> _processFrame(CameraImage image) async {
    if (_isProcessing || _qrFound) {
      return;
    }
    _isProcessing = true;
    final inputImage = _inputImageFromCameraImage(image);
    if (inputImage == null) {
      _isProcessing = false;
      return;
    }

    final barcodeResult = await _barcodeScanner.processImage(inputImage);

    RecognizedText? textResult;
    if (_lastTextProcessTime == null ||
        DateTime.now().difference(_lastTextProcessTime!) >
            const Duration(seconds: 3)) {
      _lastTextProcessTime = DateTime.now();
      textResult = await _textRecognizer.processImage(inputImage);
      if (textResult.text.isNotEmpty) {
        _parseText(textResult.text);
      }
    }

    if (barcodeResult.isNotEmpty) {
      _qrCode = barcodeResult.first.rawValue ?? '';

      if (textResult == null) {
        textResult = await _textRecognizer.processImage(inputImage);
        _parseText(textResult.text);
      }

      if (_qrCode.isNotEmpty && _qrCode.length == 24 && _date != null) {
        List<String> parts = _date!.split(" ")[0].split('.');
        if (parts.length != 3) {
          _isProcessing = false;
          throw FormatException('Неверный формат даты');
        }
        int day = int.parse(parts[0]);
        int month = int.parse(parts[1]);
        int year = int.parse(parts[2]);

        if (!mounted) {
          return;
        }
        setState(() => _isChecking = true);
        await _fetchReceipt(_qrCode, DateTime(year, month, day));
        if (!mounted) {
          return;
        }
        setState(() => _isChecking = false);

        if (mounted) {
          if (_receiptData != null) {
            if (_receiptData!.containsKey('total_amount')) {
              _amount = double.tryParse(
                _receiptData!['total_amount'].toString(),
              );
            }
            setState(() => _qrFound = true);
          } else if (_error != null) {
            await Future.delayed(const Duration(seconds: 2));
            if (mounted) {
              setState(() {
                _error = null;
                _qrCode = '';
                _date = null;
              });
              _cameraController!.startImageStream(_processFrame);
            }
          }
        }
      }
    }
    _isProcessing = false;
  }

  InputImage? _inputImageFromCameraImage(CameraImage image) {
    final WriteBuffer allBytes = WriteBuffer();
    for (final plane in image.planes) {
      allBytes.putUint8List(plane.bytes);
    }
    final bytes = allBytes.done().buffer.asUint8List();
    final size = Size(image.width.toDouble(), image.height.toDouble());
    final rotation = InputImageRotationValue.fromRawValue(
      _cameras.first.sensorOrientation,
    );
    if (rotation == null) return null;
    final format = InputImageFormatValue.fromRawValue(image.format.raw);
    if (format == null) return null;
    final inputImageData = InputImageMetadata(
      size: size,
      rotation: rotation,
      format: format,
      bytesPerRow: image.planes[0].bytesPerRow,
    );
    return InputImage.fromBytes(bytes: bytes, metadata: inputImageData);
  }

  void _parseText(String text) {
    final dateRegex = RegExp(
      r'(\d{2})\.(\d{2})\.(\d{4})(?: (\d{2}):(\d{2}):(\d{2}))?',
    );
    final dateMatch = dateRegex.firstMatch(text);
    if (dateMatch != null && mounted) {
      setState(() {
        _date = dateMatch.group(0);
      });
    }
    final amountRegex = RegExp(r'(?<!\d[.,])\b\d+[.,]\d{1,2}\b(?![\d.,])');
    final allMatches = amountRegex.allMatches(text);
    double maxAmount = -1;
    _allMatches.clear();
    for (final match in allMatches) {
      String amountStr = match.group(0)!.replaceAll(',', '.');
      double amount = double.tryParse(amountStr) ?? 0.0;
      if (amount > 0 && !_allMatches.contains(amount)) {
        _allMatches.add(amount);
      }
      if (amount > maxAmount) {
        maxAmount = amount;
      }
    }
    if (maxAmount > 0 && mounted) {
      setState(() {
        _amount = maxAmount;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    final dataProvider = Provider.of<DataProviderService>(context);
    return Scaffold(
      appBar: AppBar(title: const Text('Сканер QR')),
      body: Stack(
        children: [
          if (_isInitialized && !_qrFound) CameraPreview(_cameraController!),
          if (_qrFound)
            Center(
              child: Padding(
                padding: const EdgeInsets.all(20),
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    const Text(
                      'QR-код найден',
                      style: TextStyle(
                        fontSize: 18,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    const SizedBox(height: 12),
                    SelectableText(
                      _qrCode,
                      style: const TextStyle(
                        fontFamily: 'monospace',
                        fontSize: 12,
                      ),
                    ),
                    if (_date != null) ...[
                      const SizedBox(height: 8),
                      Text(
                        'Дата: $_date',
                        style: const TextStyle(fontWeight: FontWeight.w500),
                      ),
                    ],
                    if (_amount != null) ...[
                      const SizedBox(height: 8),
                      Text(
                        'Сумма: ${_amount!.toStringAsFixed(2)} ${dataProvider.currency}',
                        style: const TextStyle(fontWeight: FontWeight.w500),
                      ),
                    ],
                  ],
                ),
              ),
            ),
          if (_isChecking)
            Align(
              alignment: Alignment.topCenter,
              child: Row(
                mainAxisAlignment: MainAxisAlignment.center,
                spacing: 20,
                children: [
                  _error != null
                      ? Icon(Icons.warning)
                      : CircularProgressIndicator(),
                  Text(
                    _error != null ? 'Ошибка проверки чека' : 'Проверка QR...',
                    style: const TextStyle(fontSize: 24, color: Colors.white),
                  ),
                ],
              ),
            ),
          if (_amount != null)
            Align(
              alignment: Alignment.bottomCenter,
              child: Row(
                mainAxisAlignment: MainAxisAlignment.center,
                spacing: 0,
                children: [
                  Padding(
                    padding: const EdgeInsets.symmetric(vertical: 10, horizontal: 5),
                    child: ElevatedButton(
                      onPressed: () => Navigator.popAndPushNamed(
                        context,
                        "/add_expense",
                        arguments: {
                          'expense': Expense(
                            amount: _amount!,
                            dateTime: DateTime.now(),
                            categoryId: 1,
                          ),
                          'isEditing': false,
                        },
                      ),
                      style: ElevatedButton.styleFrom(
                        backgroundColor: Colors.green,
                        foregroundColor: Colors.white,
                      ),
                      child: Text(
                        'Сумма ${_amount!.toStringAsFixed(2)} ${dataProvider.currency}${_qrFound ? '' : ' ?'}',
                      ),
                    ),
                  ),
                  IconButton(
                    icon: Icon(
                      _showMore
                          ? Icons.keyboard_arrow_up
                          : Icons.keyboard_arrow_down,
                    ),
                    onPressed: () => setState(() {
                      _showMore = !_showMore;
                    }),
                  ),
                ],
              ),
            ),
          if (_showMore && _allMatches.isNotEmpty)
            Positioned(
              bottom: 75,
              left: 0,
              right: 0,
              child: Center(
                child: Container(
                  constraints: BoxConstraints(maxHeight: 200, maxWidth: 100),
                  decoration: BoxDecoration(
                    color: Colors.white,
                    borderRadius: BorderRadius.circular(10),
                  ),
                  child: ListView.builder(
                    shrinkWrap: true,
                    physics: const ClampingScrollPhysics(),
                    dragStartBehavior: DragStartBehavior.down,
                    itemCount: _allMatches.length,
                    itemBuilder: (context, index) {
                      double currentElement = _allMatches.elementAt(index);
                      return currentElement != _amount
                          ? TextButton(
                              onPressed: () {
                                setState(() {
                                  _amount = currentElement;
                                  _showMore = false;
                                });
                              },
                              child: Text(currentElement.toStringAsFixed(2)),
                            )
                          : SizedBox();
                    },
                  ),
                ),
              ),
            ),
          if (_qrFound)
            Align(
              alignment: Alignment.bottomCenter,
              child: Padding(
                padding: const EdgeInsets.all(16).copyWith(bottom: 80),
                child: ElevatedButton(
                  onPressed: () => Navigator.popAndPushNamed(
                    context,
                    "/receipt_info",
                    arguments: _receiptData,
                  ),
                  child: const Text('Проверить чек'),
                ),
              ),
            ),
        ],
      ),
    );
  }

  Future<void> _fetchReceipt(String origUi, DateTime origDate) async {
    final dateString =
        '${origDate.year}-'
        '${origDate.month.toString().padLeft(2, '0')}-'
        '${origDate.day.toString().padLeft(2, '0')}';
    final request = http.MultipartRequest(
      'POST',
      Uri.parse('https://ch.info-center.by/ajax/check1.php'),
    );
    request.fields['orig_date'] = dateString;
    request.fields['orig_ui'] = origUi;
    try {
      final streamedResponse = await request.send();
      final response = await http.Response.fromStream(streamedResponse);
      if (response.statusCode == 200) {
        final data = jsonDecode(response.body) as Map<String, dynamic>;
        final status = data['status'] as String?;
        if (status == 'success') {
          if (mounted) {
            setState(() {
              _receiptData = data['message'];
            });
          }
        } else {
          final warning =
              data['warning'] ?? data['message'] ?? 'Неизвестная ошибка';
          if (mounted) {
            setState(() {
              _error = '⚠️ Не удалось найти чек, $warning';
            });
          }
        }
      } else {
        if (mounted) {
          setState(() {
            _error = '❌ Ошибка сервера: ${response.statusCode}';
          });
        }
      }
    } catch (e) {
      if (mounted) {
        setState(() {
          _error = '❌ Не удалось загрузить чек: $e';
        });
      }
    }
  }

  @override
  void dispose() {
    _cameraController?.dispose();
    _textRecognizer.close();
    _barcodeScanner.close();
    super.dispose();
  }
}
