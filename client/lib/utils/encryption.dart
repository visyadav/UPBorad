import 'package:encrypt/encrypt.dart';

class EncryptionUtil {
  // Hardcoded key for this specific app's encryption
  // In a real production app, consider obfuscation or secure storage if necessary,
  // but for "only this app can read it" this is the standard approach.
  static final _key = Key.fromUtf8('my32lengthsupersecretnooneknows1');
  static final _iv = IV.fromUtf8('my16lengthsecret');

  static String encryptData(String plainText) {
    final encrypter = Encrypter(AES(_key, mode: AESMode.cbc));
    final encrypted = encrypter.encrypt(plainText, iv: _iv);
    return encrypted.base64;
  }

  static String decryptData(String encryptedText) {
    try {
      final encrypter = Encrypter(AES(_key, mode: AESMode.cbc));
      final decrypted = encrypter.decrypt64(encryptedText, iv: _iv);
      return decrypted;
    } catch (e) {
      return '';
    }
  }
}
