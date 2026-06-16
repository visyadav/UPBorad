import 'dart:convert';
import 'subject_marks.dart';

class StudentResult {
  // Registration Info
  final String centreNo;
  final String schoolNo;
  final String enrolmentNo;
  final String rollNumber;
  final String examType; // Regular / Private

  // Student Info
  final String studentName;
  final String fatherName;
  final String motherName;
  final String dateOfBirth;
  final String schoolName;
  final String photoPath;

  // Subject Marks (fixed 6 subjects)
  final List<SubjectMarks> subjects;

  // Result Summary
  final String grandTotal;
  final String maxTotal;
  final String resultText;
  final String grandTotalWords;
  final String grade;
  final String passingStatus;

  // Additional
  final String extraRemarks;
  final String resultDate;
  final String passingYear;

  StudentResult({
    required this.centreNo,
    required this.schoolNo,
    required this.enrolmentNo,
    required this.rollNumber,
    required this.examType,
    required this.studentName,
    required this.fatherName,
    required this.motherName,
    required this.dateOfBirth,
    required this.schoolName,
    this.photoPath = '',
    required this.subjects,
    required this.grandTotal,
    required this.maxTotal,
    required this.resultText,
    required this.grandTotalWords,
    required this.grade,
    required this.passingStatus,
    this.extraRemarks = '',
    this.resultDate = '',
    this.passingYear = '',
  });

  /// Uses short keys to minimize encrypted QR data payload size.
  Map<String, dynamic> toMap() {
    return {
      'cn': centreNo,
      'sn': schoolNo,
      'en': enrolmentNo,
      'rn': rollNumber,
      'et': examType,
      'nm': studentName,
      'fn': fatherName,
      'mn': motherName,
      'db': dateOfBirth,
      'sc': schoolName,
      'ph': photoPath,
      'sb': subjects.map((s) => s.toMap()).toList(),
      'gt': grandTotal,
      'mx': maxTotal,
      'rt': resultText,
      'gw': grandTotalWords,
      'gd': grade,
      'ps': passingStatus,
      'er': extraRemarks,
      'rd': resultDate,
      'py': passingYear,
    };
  }

  factory StudentResult.fromMap(Map<String, dynamic> map) {
    return StudentResult(
      centreNo: map['cn'] ?? '',
      schoolNo: map['sn'] ?? '',
      enrolmentNo: map['en'] ?? '',
      rollNumber: map['rn'] ?? '',
      examType: map['et'] ?? '',
      studentName: map['nm'] ?? '',
      fatherName: map['fn'] ?? '',
      motherName: map['mn'] ?? '',
      dateOfBirth: map['db'] ?? '',
      schoolName: map['sc'] ?? '',
      photoPath: map['ph'] ?? '',
      subjects: (map['sb'] as List<dynamic>?)
              ?.map((s) => SubjectMarks.fromMap(s as Map<String, dynamic>))
              .toList() ??
          [],
      grandTotal: map['gt'] ?? '',
      maxTotal: map['mx'] ?? '',
      resultText: map['rt'] ?? '',
      grandTotalWords: map['gw'] ?? '',
      grade: map['gd'] ?? '',
      passingStatus: map['ps'] ?? '',
      extraRemarks: map['er'] ?? '',
      resultDate: map['rd'] ?? '',
      passingYear: map['py'] ?? '',
    );
  }

  String toJson() => json.encode(toMap());

  factory StudentResult.fromJson(String source) =>
      StudentResult.fromMap(json.decode(source));
}
