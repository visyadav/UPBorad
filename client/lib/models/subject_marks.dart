class SubjectMarks {
  final String subjectName;
  final String maxMarksTheory;
  final String maxMarksInternal;
  final String maxMarksTotal;
  final String minMarksTheory;
  final String minMarksInternal;
  final String obtainedTheory;
  final String obtainedInternal;
  final String totalMarks;
  final String remarks;

  SubjectMarks({
    required this.subjectName,
    required this.maxMarksTheory,
    required this.maxMarksInternal,
    required this.maxMarksTotal,
    required this.minMarksTheory,
    required this.minMarksInternal,
    required this.obtainedTheory,
    required this.obtainedInternal,
    required this.totalMarks,
    this.remarks = '',
  });

  /// Uses short keys to minimize QR data payload size.
  Map<String, dynamic> toMap() {
    return {
      'n': subjectName,
      'a': maxMarksTheory,
      'b': maxMarksInternal,
      'c': maxMarksTotal,
      'd': minMarksTheory,
      'e': minMarksInternal,
      'f': obtainedTheory,
      'g': obtainedInternal,
      't': totalMarks,
      'r': remarks,
    };
  }

  factory SubjectMarks.fromMap(Map<String, dynamic> map) {
    return SubjectMarks(
      subjectName: map['n'] ?? '',
      maxMarksTheory: map['a'] ?? '',
      maxMarksInternal: map['b'] ?? '',
      maxMarksTotal: map['c'] ?? '',
      minMarksTheory: map['d'] ?? '',
      minMarksInternal: map['e'] ?? '',
      obtainedTheory: map['f'] ?? '',
      obtainedInternal: map['g'] ?? '',
      totalMarks: map['t'] ?? '',
      remarks: map['r'] ?? '',
    );
  }
}
