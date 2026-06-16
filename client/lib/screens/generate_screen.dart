import 'dart:typed_data';
import 'package:flutter/material.dart';
import 'package:qr_flutter/qr_flutter.dart';
import 'package:screenshot/screenshot.dart';
import 'package:gal/gal.dart';
import 'package:permission_handler/permission_handler.dart';

class GenerateScreen extends StatefulWidget {
  const GenerateScreen({super.key});

  @override
  State<GenerateScreen> createState() => _GenerateScreenState();
}

class _GenerateScreenState extends State<GenerateScreen> {
  final _formKey = GlobalKey<FormState>();
  final _rollNumberController = TextEditingController();

  String? _qrDataUrl;
  final ScreenshotController _screenshotController = ScreenshotController();

  @override
  void dispose() {
    _rollNumberController.dispose();
    super.dispose();
  }

  void _generateQR() {
    if (_formKey.currentState!.validate()) {
      setState(() {
        // We use a base URL that can point to our API
        _qrDataUrl = 'https://upboard.com/student/${_rollNumberController.text.trim()}';
      });
    }
  }

  Future<void> _saveQRToGallery() async {
    await Permission.storage.request();

    final Uint8List? image = await _screenshotController.capture();
    if (image != null) {
      try {
        await Gal.putImageBytes(image);
        if (!mounted) return;
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('QR Code saved to gallery!')),
        );
      } catch (e) {
        if (!mounted) return;
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Failed to save QR Code: $e')),
        );
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Generate QR'),
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16.0),
        child: _qrDataUrl == null ? _buildForm() : _buildQRView(),
      ),
    );
  }

  Widget _buildForm() {
    return Form(
      key: _formKey,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Container(
            padding: const EdgeInsets.symmetric(vertical: 8, horizontal: 12),
            decoration: BoxDecoration(
              color: Colors.red.shade50,
              borderRadius: BorderRadius.circular(8),
              border: Border.all(color: Colors.red.shade200),
            ),
            child: Row(
              children: [
                Icon(Icons.assignment, color: Colors.red.shade700, size: 20),
                const SizedBox(width: 8),
                Text('Student Details',
                    style: TextStyle(
                        fontSize: 16,
                        fontWeight: FontWeight.bold,
                        color: Colors.red.shade800)),
              ],
            ),
          ),
          const SizedBox(height: 16),
          TextFormField(
            controller: _rollNumberController,
            decoration: const InputDecoration(
              labelText: 'Roll Number',
              border: OutlineInputBorder(),
            ),
            keyboardType: TextInputType.number,
            validator: (value) =>
                value == null || value.isEmpty ? 'Please enter a roll number' : null,
          ),
          const SizedBox(height: 32),
          SizedBox(
            height: 50,
            child: ElevatedButton.icon(
              onPressed: _generateQR,
              icon: const Icon(Icons.qr_code),
              label: const Text('Generate URL QR Code', style: TextStyle(fontSize: 16)),
              style: ElevatedButton.styleFrom(
                backgroundColor: Colors.red.shade700,
                foregroundColor: Colors.white,
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildQRView() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.stretch,
      children: [
        const Text(
          'QR Code Generated!',
          style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
          textAlign: TextAlign.center,
        ),
        const SizedBox(height: 8),
        Text(
          _qrDataUrl!,
          style: TextStyle(fontSize: 14, color: Colors.grey.shade600),
          textAlign: TextAlign.center,
        ),
        const SizedBox(height: 24),
        Center(
          child: Screenshot(
            controller: _screenshotController,
            child: Container(
              padding: const EdgeInsets.all(16.0),
              color: Colors.white,
              child: QrImageView(
                data: _qrDataUrl!,
                version: QrVersions.auto,
                size: 280.0,
                backgroundColor: Colors.white,
              ),
            ),
          ),
        ),
        const SizedBox(height: 24),
        ElevatedButton.icon(
          onPressed: _saveQRToGallery,
          icon: const Icon(Icons.save),
          label: const Text('Save to Gallery'),
          style: ElevatedButton.styleFrom(
            backgroundColor: Colors.red.shade700,
            foregroundColor: Colors.white,
          ),
        ),
        const SizedBox(height: 12),
        TextButton(
          onPressed: () {
            setState(() {
              _qrDataUrl = null;
            });
          },
          child: const Text('Generate Another'),
        ),
      ],
    );
  }
}
