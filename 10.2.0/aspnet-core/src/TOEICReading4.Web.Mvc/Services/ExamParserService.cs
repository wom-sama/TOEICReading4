using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TOEICReading4.Web.Controllers 
{
    // DTO bọc cả Đoạn văn và Câu hỏi để trả về cho Controller
    public class ParsedExamDto
    {
        public List<PassageTempDto> Passages { get; set; } = new List<PassageTempDto>();
        public List<QuestionTempDto> Questions { get; set; } = new List<QuestionTempDto>();
    }

    public class PassageTempDto
    {
        public int TempId { get; set; } 
        public int PartNumber { get; set; }
        public string Content { get; set; } = "";
    }

    public class QuestionTempDto
    {
        public int QuestionNumber { get; set; }
        public int PartNumber { get; set; }
        public int? PassageTempId { get; set; } 
        public bool IsPQ { get; set; } = false; // Cờ nhận diện câu hỏi PQ (không cần nội dung)
        public string Content { get; set; } = "";
        public string OptionA { get; set; } = "";
        public string OptionB { get; set; } = "";
        public string OptionC { get; set; } = "";
        public string OptionD { get; set; } = "";
        public string CorrectKey { get; set; } = "";
    }

    public class ExamParserService
    {
        public ParsedExamDto ParseAndValidate(string rawText)
        {
            var result = new ParsedExamDto();
            var lines = rawText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            QuestionTempDto currentQuestion = null;
            PassageTempDto currentPassage = null;

            string currentReadingTag = "";
            int currentPartNumber = 5; 
            int passageCounter = 1;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

                // 1. Nhận diện thẻ [PART:X]
                var partMatch = Regex.Match(line, @"^\[PART:(\d+)\]");
                if (partMatch.Success)
                {
                    currentPartNumber = int.Parse(partMatch.Groups[1].Value);
                    currentReadingTag = "PART";
                    continue;
                }

                // 2. Bắt đầu Đoạn văn mới
                if (line.StartsWith("[PASSAGE_START]"))
                {
                    currentPassage = new PassageTempDto { TempId = passageCounter++, PartNumber = currentPartNumber };
                    currentReadingTag = "PASSAGE";
                    continue;
                }

                // 3. Kết thúc Đoạn văn
                if (line.StartsWith("[PASSAGE_END]"))
                {
                    if (currentPassage != null) result.Passages.Add(currentPassage);
                    currentReadingTag = "PASSAGE_END";
                    continue;
                }

                // 4. Khởi tạo Câu hỏi (Hỗ trợ cả [Q:X] và [PQ:X])
                var qMatch = Regex.Match(line, @"^\[(Q|PQ):(\d+)\](.*)");
                if (qMatch.Success)
                {
                    if (currentQuestion != null) ValidateQuestion(currentQuestion);

                    string tagType = qMatch.Groups[1].Value; // "Q" hoặc "PQ"
                    int qNum = int.Parse(qMatch.Groups[2].Value);
                    
                    currentQuestion = new QuestionTempDto
                    {
                        QuestionNumber = qNum,
                        PartNumber = currentPartNumber,
                        IsPQ = (tagType == "PQ"),
                        PassageTempId = (currentPartNumber >= 6 && currentPassage != null) ? currentPassage.TempId : (int?)null
                    };
                    
                    currentQuestion.Content += qMatch.Groups[3].Value.Trim();
                    currentReadingTag = tagType;
                    continue;
                }

                // 5. Các thẻ Đáp án
                if (line.StartsWith("[A]")) { currentReadingTag = "A"; currentQuestion.OptionA += line.Substring(3).Trim(); continue; }
                if (line.StartsWith("[B]")) { currentReadingTag = "B"; currentQuestion.OptionB += line.Substring(3).Trim(); continue; }
                if (line.StartsWith("[C]")) { currentReadingTag = "C"; currentQuestion.OptionC += line.Substring(3).Trim(); continue; }
                if (line.StartsWith("[D]")) { currentReadingTag = "D"; currentQuestion.OptionD += line.Substring(3).Trim(); continue; }

                var keyMatch = Regex.Match(line, @"^\[KEY:([A-D])\]");
                if (keyMatch.Success)
                {
                    currentQuestion.CorrectKey = keyMatch.Groups[1].Value;
                    currentReadingTag = "KEY";
                    result.Questions.Add(currentQuestion);
                    continue;
                }

                // 6. Ghi nối chữ nếu xuống dòng
                if (currentReadingTag == "PASSAGE" && currentPassage != null)
                {
                    currentPassage.Content += (string.IsNullOrEmpty(currentPassage.Content) ? "" : "\n") + line;
                }
                else if (currentQuestion != null)
                {
                    switch (currentReadingTag)
                    {
                        case "Q": 
                        case "PQ": 
                            currentQuestion.Content += "\n" + line; break;
                        case "A": currentQuestion.OptionA += "\n" + line; break;
                        case "B": currentQuestion.OptionB += "\n" + line; break;
                        case "C": currentQuestion.OptionC += "\n" + line; break;
                        case "D": currentQuestion.OptionD += "\n" + line; break;
                    }
                }
            }

            if (currentQuestion != null && string.IsNullOrEmpty(currentQuestion.CorrectKey)) ValidateQuestion(currentQuestion);
            if (result.Questions.Count == 0) throw new Exception("Lỗi Format: Không tìm thấy bất kỳ thẻ [Q:X] hay [PQ:X] nào.");

            return result;
        }

        private void ValidateQuestion(QuestionTempDto q)
        {
            // Bỏ qua check rỗng nếu câu đó là thẻ [PQ]
            if (!q.IsPQ && string.IsNullOrWhiteSpace(q.Content))
                throw new Exception($"Lỗi Format (Câu {q.QuestionNumber}): Nội dung câu hỏi bị trống.");
            
            if (string.IsNullOrWhiteSpace(q.OptionA) || string.IsNullOrWhiteSpace(q.OptionB) ||
                string.IsNullOrWhiteSpace(q.OptionC) || string.IsNullOrWhiteSpace(q.OptionD))
                throw new Exception($"Lỗi Format (Câu {q.QuestionNumber}): Bị thiếu thẻ đáp án [A], [B], [C] hoặc [D].");

            if (string.IsNullOrWhiteSpace(q.CorrectKey))
                throw new Exception($"Lỗi Format (Câu {q.QuestionNumber}): Bị thiếu thẻ đáp án đúng [KEY:X].");
        }
    }
}