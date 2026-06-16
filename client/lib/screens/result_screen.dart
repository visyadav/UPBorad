import 'dart:io';
import 'package:flutter/material.dart';
import '../models/student_result.dart';
import '../models/subject_marks.dart';

class ResultScreen extends StatefulWidget {
  final StudentResult result;

  const ResultScreen({super.key, required this.result});

  @override
  State<ResultScreen> createState() => _ResultScreenState();
}

class _ResultScreenState extends State<ResultScreen> {
  final TransformationController _transformationController = TransformationController();

  void _zoomIn() {
    final matrix = _transformationController.value.clone();
    matrix.scale(1.2);
    _transformationController.value = matrix;
  }

  void _zoomOut() {
    final matrix = _transformationController.value.clone();
    matrix.scale(0.8);
    _transformationController.value = matrix;
  }

  @override
  void dispose() {
    _transformationController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF9F7E8), // Pale yellow background
      appBar: AppBar(
        title: const Text('Result Reader', style: TextStyle(color: Colors.white, fontSize: 16, fontWeight: FontWeight.bold)),
        backgroundColor: Colors.grey.shade600,
        centerTitle: true,
        iconTheme: const IconThemeData(color: Colors.white),
      ),
      body: InteractiveViewer(
        transformationController: _transformationController,
        boundaryMargin: const EdgeInsets.all(32.0),
        minScale: 0.5,
        maxScale: 5.0,
        constrained: false, // Allows panning horizontally and vertically
        child: SizedBox(
          width: MediaQuery.of(context).size.width, // Initial width matches screen
          child: Padding(
            padding: const EdgeInsets.symmetric(horizontal: 16.0, vertical: 12.0),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: [
                _buildHeader(),
                const SizedBox(height: 12),
                _buildPhoto(),
                const SizedBox(height: 12),
                _buildRegistrationGrid(),
                const SizedBox(height: 16),
                _buildStudentInfo(),
                const SizedBox(height: 12),
                _buildParagraph(),
                const SizedBox(height: 12),
                _buildSchoolName(),
                const SizedBox(height: 16),
                _buildSubjectsTable(),
                const SizedBox(height: 12),
                _buildBottomInfo(),
                const SizedBox(height: 16),
                _buildExtraRemarks(),
                const SizedBox(height: 20),
              ],
            ),
          ),
        ),
      ),
      // Move button to bottomNavigationBar to completely avoid hiding content
      bottomNavigationBar: SafeArea(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(16.0, 8.0, 16.0, 16.0),
          child: SizedBox(
            height: 48,
            width: double.infinity,
            child: ElevatedButton(
              onPressed: () => Navigator.of(context).pop(),
              style: ElevatedButton.styleFrom(
                backgroundColor: const Color(0xFFC7B8FF), // Light purple button
                shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(24)),
              ),
              child: const Text('Scan another Code', style: TextStyle(color: Colors.black87, fontSize: 16)),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildHeader() {
    final year = widget.result.passingYear.isNotEmpty ? widget.result.passingYear : '2024';
    return Column(
      children: [
        // Board Logo
        Container(
          width: 60,
          height: 60,
          decoration: const BoxDecoration(shape: BoxShape.circle),
          clipBehavior: Clip.antiAlias,
          child: Center(
            child: Image.asset(
              'assets/images/up_board_logo.png',
              fit: BoxFit.contain,
              errorBuilder: (context, error, stackTrace) =>
                  Icon(Icons.school, size: 40, color: Colors.blue.shade800),
            ),
          ),
        ),
        const SizedBox(height: 8),
        const Text(
          'माध्यमिक शिक्षा परिषद्, उत्तर प्रदेश, प्रयागराज',
          style: TextStyle(fontSize: 10, fontWeight: FontWeight.bold, color: Colors.black),
          textAlign: TextAlign.center,
        ),
        const Text(
          'BOARD OF HIGH SCHOOL AND INTERMEDIATE EDUCATION U.P., PRAYAGRAJ',
          style: TextStyle(fontSize: 9, fontWeight: FontWeight.w500, color: Colors.black),
          textAlign: TextAlign.center,
        ),
        const SizedBox(height: 8),
        Text(
          'हाई स्कूल सर्टिफिकेट परीक्षा $year',
          style: const TextStyle(fontSize: 10, fontWeight: FontWeight.bold, color: Colors.black),
          textAlign: TextAlign.center,
        ),
        Text(
          'HIGH SCHOOL CERTIFICATE EXAMINATION $year',
          style: const TextStyle(fontSize: 9, fontWeight: FontWeight.w500, color: Colors.black),
          textAlign: TextAlign.center,
        ),
        const SizedBox(height: 4),
        const Text(
          'अंकसूची सह-प्रमाणपत्र',
          style: TextStyle(fontSize: 10, fontWeight: FontWeight.bold, color: Colors.black),
          textAlign: TextAlign.center,
        ),
        Text(
          'MARKSHEET CUM-CERTIFICATE - $year',
          style: const TextStyle(fontSize: 9, fontWeight: FontWeight.w500, color: Colors.black),
          textAlign: TextAlign.center,
        ),
      ],
    );
  }

  Widget _buildPhoto() {
    return Center(
      child: Container(
        width: 65,
        height: 80,
        decoration: BoxDecoration(
          border: Border.all(color: Colors.grey.shade400, width: 1),
          color: Colors.grey.shade200,
        ),
        clipBehavior: Clip.antiAlias,
        child: widget.result.photoPath.isNotEmpty && File(widget.result.photoPath).existsSync()
            ? Image.file(
                File(widget.result.photoPath),
                fit: BoxFit.cover,
              )
            : Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Icon(Icons.person, size: 40, color: Colors.grey.shade500),
                  Text(
                    'PHOTO',
                    style: TextStyle(fontSize: 8, color: Colors.grey.shade500),
                  ),
                ],
              ),
      ),
    );
  }

  Widget _buildRegistrationGrid() {
    return Table(
      border: TableBorder.all(color: Colors.grey.shade400, width: 0.5),
      columnWidths: const {
        0: FlexColumnWidth(1),
        1: FlexColumnWidth(1),
        2: FlexColumnWidth(2),
        3: FlexColumnWidth(1.5),
        4: FlexColumnWidth(1.5),
      },
      children: [
        TableRow(
          children: [
            _headerCell('केन्द्र\nक्रमांक\n\nCENTRE\nNO.'),
            _headerCell('संस्था\nक्रमांक\n\nSCHOOL\nNO.'),
            _headerCell('नामांकन क्रमांक\n\nENROLMENT\nNO.'),
            _headerCell('नियमित/\nस्वाध्यायी\n\nREGULAR\n/\nPRIVATE'),
            _headerCell('रोल नंबर\n\nROLL\nNUMBER'),
          ],
        ),
        TableRow(
          children: [
            _dataCell(widget.result.centreNo),
            _dataCell(widget.result.schoolNo),
            _dataCell(widget.result.enrolmentNo),
            _dataCell(widget.result.examType),
            _dataCell(widget.result.rollNumber),
          ],
        ),
      ],
    );
  }

  Widget _buildStudentInfo() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        const Text(
          'प्रमाणित किया जाता है कि CERTIFIED THAT',
          style: TextStyle(fontSize: 10, fontWeight: FontWeight.bold),
        ),
        const SizedBox(height: 4),
        _infoRow('श्री / सुश्री SHRI / SUSHRI', widget.result.studentName.toUpperCase()),
        _infoRow('जिनके पिता / पति का नाम WHOSE\nFATHER\'S / HUSBAND\'S\nNAME IS', widget.result.fatherName.toUpperCase()),
        _infoRow('व माता का नाम AND MOTHER\'S\nNAME IS', widget.result.motherName.toUpperCase()),
        _infoRow('तथा जन्मतिथि AND DATE OF\nBIRTH IS', widget.result.dateOfBirth.toUpperCase()),
      ],
    );
  }

  Widget _infoRow(String label, String value) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 6.0),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Expanded(
            flex: 2,
            child: Text(label, style: const TextStyle(fontSize: 9, height: 1.2)),
          ),
          Expanded(
            flex: 3,
            child: Text(value, style: const TextStyle(fontSize: 10, fontWeight: FontWeight.bold)),
          ),
        ],
      ),
    );
  }

  Widget _buildParagraph() {
    final year = widget.result.passingYear.isNotEmpty ? widget.result.passingYear : '2024';
    return Text(
      'ने इस मण्डल की हाईस्कूल सर्टिफिकेट परीक्षा वर्ष - $year में संस्था/\n'
      'केन्द्र** से सम्मिलित हुए एवं विषयवार प्राप्तांक निम्नानुसार अर्जित किए है\n'
      ':- APPEARED IN THE HIGH SCHOOL CERTIFICATE\n'
      'EXAMINATION OF THIS BOARD IN THE YEAR $year FROM\n'
      '(SCHOOL / CENTRE)** AND SUBJECT WISE MARKS\n'
      'OBTAINED ARE AS UNDER:-',
      style: const TextStyle(fontSize: 9, height: 1.3),
    );
  }

  Widget _buildSchoolName() {
    return Text(
      widget.result.schoolName.toUpperCase(),
      style: const TextStyle(fontSize: 10, fontWeight: FontWeight.bold),
    );
  }

  Widget _buildSubjectsTable() {
    return Table(
      border: TableBorder.all(color: Colors.grey.shade400, width: 0.5),
      columnWidths: const {
        0: FlexColumnWidth(3), // Subject Name
        1: FlexColumnWidth(1.5),
        2: FlexColumnWidth(1.5),
        3: FlexColumnWidth(1.5),
        4: FlexColumnWidth(1.5),
        5: FlexColumnWidth(1.5),
        6: FlexColumnWidth(1.5),
        7: FlexColumnWidth(1.5),
        8: FlexColumnWidth(1.5),
        9: FlexColumnWidth(1.5),
      },
      children: [
        TableRow(
          children: [
            _tableHeaderCell('विषय\n\n\nSUBJECTS'),
            _tableHeaderCell('अधिकतम\nअंक\nसैद्धांतिक\n\nMAXIMUM\nMARKS\nTHEORY'),
            _tableHeaderCell('अधिकतम\nअंक आंतरिक\n/ प्रायोगिक\nMAXIMUM\nMARKS\nINTERNAL /\nPRACTICAL'),
            _tableHeaderCell('अधिकतम\nअंक योग\n\nMAXIMUM\nMARKS\nTOTAL'),
            _tableHeaderCell('न्यूनतम\nअंक\nसैद्धांतिक\n\nMINIMUM\nMARKS\nTHEORY'),
            _tableHeaderCell('न्यूनतम अंक\nआंतरिक /\nप्रायोगिक\nMINIMUM\nMARKS\nINTERNAL /\nPRACTICAL'),
            _tableHeaderCell('प्राप्तांक\nसैद्धांतिक\n\n\nTHEORY'),
            _tableHeaderCell('प्राप्तांक\nआंतरिक /\nप्रायोगिक\n\nINTERNAL /\nPRACTICAL'),
            _tableHeaderCell('प्राप्तांक\nयोग\n\n\nTOTAL'),
            _tableHeaderCell('विशेष\n\n\nREMARKS'),
          ],
        ),
        ...widget.result.subjects.map((s) => TableRow(
              children: [
                _tableDataCell(s.subjectName, alignLeft: true),
                _tableDataCell(s.maxMarksTheory),
                _tableDataCell(s.maxMarksInternal),
                _tableDataCell(s.maxMarksTotal),
                _tableDataCell(s.minMarksTheory),
                _tableDataCell(s.minMarksInternal),
                _tableDataCell(s.obtainedTheory),
                _tableDataCell(s.obtainedInternal),
                _tableDataCell(s.totalMarks),
                _tableDataCell(s.remarks),
              ],
            )),
        // Grand Total Row
        TableRow(
          children: [
            const SizedBox(),
            const SizedBox(),
            const SizedBox(),
            _tableDataCell(widget.result.maxTotal.isNotEmpty ? widget.result.maxTotal : '600', isBold: true),
            const SizedBox(),
            _tableDataCell('महायोग /\nGRAND TOTAL', isBold: true, alignLeft: false),
            const SizedBox(),
            const SizedBox(),
            _tableDataCell(widget.result.grandTotal, isBold: true),
            const SizedBox(),
          ],
        ),
      ],
    );
  }

  Widget _buildBottomInfo() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        const Text('महायोग शब्दो में :', style: TextStyle(fontSize: 10, fontWeight: FontWeight.bold)),
        Text(
          'GRAND TOTAL IN WORDS: ${widget.result.grandTotalWords.toUpperCase()}***',
          style: const TextStyle(fontSize: 10),
        ),
        const SizedBox(height: 4),
        Text(
          'परीक्षाफल / RESULT ${widget.result.passingStatus.toUpperCase()}',
          style: const TextStyle(fontSize: 10, fontWeight: FontWeight.bold),
        ),
      ],
    );
  }

  Widget _buildExtraRemarks() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        if (widget.result.extraRemarks.isNotEmpty) ...[
          const Text('अतिरिक्त विषय / ADDITIONAL SUBJECT', style: TextStyle(fontSize: 9, fontWeight: FontWeight.bold)),
          const SizedBox(height: 4),
          Text(widget.result.extraRemarks, style: const TextStyle(fontSize: 9, height: 1.3)),
          const SizedBox(height: 12),
        ],
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          crossAxisAlignment: CrossAxisAlignment.end,
          children: [
            Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(widget.result.resultDate.isNotEmpty ? widget.result.resultDate : '24-04-2024',
                    style: const TextStyle(fontSize: 10, fontWeight: FontWeight.bold)),
                const Text('सचिव / SECRETARY', style: TextStyle(fontSize: 9)),
              ],
            ),
            // Functional zoom control
            Container(
              height: 32,
              width: 100,
              decoration: BoxDecoration(
                color: Colors.grey.shade300,
                borderRadius: BorderRadius.circular(16),
                border: Border.all(color: Colors.grey.shade400),
              ),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                children: [
                  IconButton(
                    onPressed: _zoomOut,
                    icon: const Icon(Icons.zoom_out, size: 16, color: Colors.black87),
                    padding: EdgeInsets.zero,
                    constraints: const BoxConstraints(),
                  ),
                  Container(width: 1, height: 20, color: Colors.grey.shade400),
                  IconButton(
                    onPressed: _zoomIn,
                    icon: const Icon(Icons.zoom_in, size: 16, color: Colors.black87),
                    padding: EdgeInsets.zero,
                    constraints: const BoxConstraints(),
                  ),
                ],
              ),
            ),
          ],
        ),
      ],
    );
  }

  // --- Helpers for Tables ---

  Widget _headerCell(String text) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 4, horizontal: 2),
      child: Text(text, style: const TextStyle(fontSize: 8, height: 1.1), textAlign: TextAlign.center),
    );
  }

  Widget _dataCell(String text) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 4, horizontal: 2),
      child: Text(text, style: const TextStyle(fontSize: 9), textAlign: TextAlign.center),
    );
  }

  Widget _tableHeaderCell(String text) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 4, horizontal: 1),
      child: Text(text, style: const TextStyle(fontSize: 7, height: 1.1), textAlign: TextAlign.center),
    );
  }

  Widget _tableDataCell(String text, {bool alignLeft = false, bool isBold = false}) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 4, horizontal: 2),
      child: Text(
        text,
        style: TextStyle(fontSize: 8, fontWeight: isBold ? FontWeight.bold : FontWeight.normal),
        textAlign: alignLeft ? TextAlign.left : TextAlign.center,
      ),
    );
  }
}
