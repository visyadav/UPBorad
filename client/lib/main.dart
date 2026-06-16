import 'package:flutter/material.dart';
import 'screens/home_screen.dart';
import 'screens/generate_screen.dart';
import 'screens/scan_screen.dart';
import 'screens/result_screen.dart';
import 'models/student_result.dart';

import 'dart:io';

class DevHttpOverrides extends HttpOverrides {
  @override
  HttpClient createHttpClient(SecurityContext? context) {
    return super.createHttpClient(context)
      ..badCertificateCallback = (X509Certificate cert, String host, int port) => true;
  }
}

void main() {
  HttpOverrides.global = DevHttpOverrides();
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'UP Board',
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.red),
        useMaterial3: true,
      ),
      initialRoute: '/',
      routes: {
        '/': (context) => const HomeScreen(),
        '/generate': (context) => const GenerateScreen(),
        '/scan': (context) => const ScanScreen(),
      },
      onGenerateRoute: (settings) {
        if (settings.name == '/result') {
          final args = settings.arguments as StudentResult;
          return MaterialPageRoute(
            builder: (context) {
              return ResultScreen(result: args);
            },
          );
        }
        assert(false, 'Need to implement \${settings.name}');
        return null;
      },
    );
  }
}
