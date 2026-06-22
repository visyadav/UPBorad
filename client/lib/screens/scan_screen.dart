import 'package:flutter/material.dart';
import 'package:mobile_scanner/mobile_scanner.dart';
import 'package:http/http.dart' as http;
import '../models/student_result.dart';
import '../utils/encryption.dart';

class ScanScreen extends StatefulWidget {
  const ScanScreen({super.key});

  @override
  State<ScanScreen> createState() => _ScanScreenState();
}

class _ScanScreenState extends State<ScanScreen> {
  final MobileScannerController _controller = MobileScannerController();
  bool _isProcessing = false;

  void _onDetect(BarcodeCapture capture) {
    if (_isProcessing) return;

    final List<Barcode> barcodes = capture.barcodes;
    for (final barcode in barcodes) {
      final rawValue = barcode.rawValue;
      if (rawValue != null) {
        setState(() {
          _isProcessing = true;
        });

        _processQRCode(rawValue);
        break; // Only process the first barcode found in this frame
      }
    }
  }

  Future<void> _processQRCode(String rawValue) async {
    try {
      // 1. Validate the scanned QR code string
      Uri? uri;
      try {
        uri = Uri.parse(rawValue);
      } catch (e) {
        _showError('Scanned QR code is not a valid URL.');
        return;
      }

      // 2. Detect if the QR contains a Scalar UI reference (e.g., https://localhost:7193/scalar/#/student/11098)
      // If so, extract the roll number from the fragment and build a clean API endpoint.
      if (uri.pathSegments.contains('scalar')) {
        // The fragment part contains something like "tag/student/GET/api/Student/11098"
        final fragment = uri
            .fragment; // may be empty if # used differently; also support after '#/'
        final rollNoMatch = RegExp(
          r"api/Student/([^/]+)",
          caseSensitive: false,
        ).firstMatch(fragment);
        String? rollNo;
        if (rollNoMatch != null) {
          rollNo = rollNoMatch.group(1);
        } else {
          // Fallback: try to get the last path segment after the last '/'
          final segments = fragment.split('/');
          if (segments.isNotEmpty) rollNo = segments.last;
        }
        if (rollNo == null || rollNo.isEmpty) {
          _showError('Could not extract roll number from QR code.');
          return;
        }
        // Build the API URI using the development machine IP (adjust if needed).
        uri = Uri.parse('http://192.168.1.5:5120/api/student/$rollNo');
      } else if (uri.host == 'upboard.com') {
        // Existing case for custom host mapping
        uri = uri.replace(
          host: '192.168.1.5',
          port: 5120,
          scheme: 'http',
          path: '/api/student/${uri.pathSegments.last}',
        );
      }

      // 2. Fetch the data from the API
      final response = await http.get(uri);

      if (response.statusCode != 200) {
        _showError(
          'Failed to fetch data from API. (Status: ${response.statusCode})',
        );
        return;
      }

      final encryptedJson = response.body;

      if (encryptedJson.isEmpty) {
        _showError('Received empty data from the API.');
        return;
      }

      // 3. Decrypt the API response
      final decryptedJson = EncryptionUtil.decryptData(encryptedJson);

      if (decryptedJson.isEmpty) {
        _showError(
          'Failed to decrypt data. Invalid encryption key or corrupted data.',
        );
        return;
      }

      // 4. Parse the JSON to StudentResult
      final result = StudentResult.fromJson(decryptedJson);
      _controller.stop();
      if (!mounted) return;
      Navigator.pushReplacementNamed(context, '/result', arguments: result);
    } catch (e) {
      _showError('Error processing data: $e');
    }
  }

  void _showError(String message) {
    if (!mounted) return;
    ScaffoldMessenger.of(
      context,
    ).showSnackBar(SnackBar(content: Text(message)));
    Future.delayed(const Duration(seconds: 2), () {
      if (mounted) {
        setState(() {
          _isProcessing = false;
        });
      }
    });
  }

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Scan QR Code')),
      body: Stack(
        children: [
          MobileScanner(controller: _controller, onDetect: _onDetect),
          if (_isProcessing)
            Container(
              color: Colors.black45,
              child: const Center(child: CircularProgressIndicator()),
            ),
          Positioned(
            bottom: 40,
            left: 0,
            right: 0,
            child: Center(
              child: Container(
                padding: const EdgeInsets.all(12),
                decoration: BoxDecoration(
                  color: Colors.black54,
                  borderRadius: BorderRadius.circular(8),
                ),
                child: const Text(
                  'Align QR code within the frame',
                  style: TextStyle(color: Colors.white),
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}
