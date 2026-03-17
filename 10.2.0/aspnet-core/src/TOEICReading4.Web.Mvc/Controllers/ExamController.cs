using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TOEICReading4.Controllers;
using TOEICReading4.Exams;
using TOEICReading4.Web.Models.Exam;

namespace TOEICReading4.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ExamController : TOEICReading4ControllerBase
    {
        private readonly IRepository<Exam, int> _examRepository;
        private readonly IRepository<Question, int> _questionRepository;
        private readonly IRepository<Passage, int> _passageRepository;
        private readonly IRepository<ExamAttempt, long> _examAttemptRepository;
        private readonly ExamParserService _parserService;

        public ExamController(
            IRepository<Exam, int> examRepository,
            IRepository<Question, int> questionRepository,
            IRepository<Passage, int> passageRepository,
            IRepository<ExamAttempt, long> examAttemptRepository)
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _passageRepository = passageRepository;
            _examAttemptRepository = examAttemptRepository;
            _parserService = new ExamParserService();
        }

        [HttpPost]
        [UnitOfWork]
        public IActionResult UploadAndParse(string examTitle, int duration, string description, IFormFile examFile)
        {
            if (examFile == null || examFile.Length == 0)
            {
                return BadRequest(L("InvalidExamFile"));
            }

            try
            {
                string extractedText = ExtractTextFromDocx(examFile);
                var parsedData = _parserService.ParseAndValidate(extractedText);

                var newExam = new Exam
                {
                    Title = examTitle,
                    DurationMinutes = duration,
                    CreatedAt = DateTime.Now
                };

                int examId = _examRepository.InsertAndGetId(newExam);

                var passageIdMap = new Dictionary<int, int>();
                foreach (var passage in parsedData.Passages)
                {
                    int realId = _passageRepository.InsertAndGetId(new Passage
                    {
                        ExamId = examId,
                        PartNumber = passage.PartNumber,
                        Content = passage.Content
                    });

                    passageIdMap[passage.TempId] = realId;
                }

                foreach (var question in parsedData.Questions)
                {
                    _questionRepository.Insert(new Question
                    {
                        ExamId = examId,
                        PartNumber = question.PartNumber,
                        QuestionNumber = question.QuestionNumber,
                        Content = question.Content,
                        OptionA = question.OptionA,
                        OptionB = question.OptionB,
                        OptionC = question.OptionC,
                        OptionD = question.OptionD,
                        CorrectKey = question.CorrectKey,
                        PassageId = question.PassageTempId.HasValue
                            ? passageIdMap[question.PassageTempId.Value]
                            : (int?)null
                    });
                }

                return Ok(new
                {
                    success = true,
                    examId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Preview(int id)
        {
            var exam = _examRepository
                .GetAllIncluding(e => e.Questions, e => e.Passages)
                .FirstOrDefault(e => e.Id == id);

            return exam == null ? NotFound(L("ExamNotFound")) : View(exam);
        }

        [HttpGet]
        public IActionResult List()
        {
            long? currentUserId = AbpSession.UserId;
            var exams = _examRepository
                .GetAllIncluding(e => e.Questions)
                .OrderByDescending(e => e.CreatedAt)
                .ToList();

            var latestAttempts = currentUserId.HasValue
                ? _examAttemptRepository
                    .GetAll()
                    .Where(a => a.UserId == currentUserId.Value)
                    .OrderByDescending(a => a.CompletedAt)
                    .ToList()
                    .GroupBy(a => a.ExamId)
                    .ToDictionary(g => g.Key, g => g.First())
                : new Dictionary<int, ExamAttempt>();

            var model = exams
                .Select(exam =>
                {
                    latestAttempts.TryGetValue(exam.Id, out var latestAttempt);

                    return new ExamListItemViewModel
                    {
                        Exam = exam,
                        HasAttempt = latestAttempt != null,
                        LatestAttemptId = latestAttempt?.Id,
                        LatestScore = latestAttempt?.Score,
                        LastCompletedAt = latestAttempt?.CompletedAt
                    };
                })
                .ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult Start(int id)
        {
            var exam = _examRepository
                .GetAllIncluding(e => e.Questions, e => e.Passages)
                .FirstOrDefault(e => e.Id == id);

            return exam == null ? NotFound(L("ExamNotFound")) : View(exam);
        }

        [HttpPost]
        [UnitOfWork]
        public IActionResult SubmitExam([FromBody] ExamSubmissionDto input)
        {
            if (input == null || input.ExamId <= 0)
            {
                return BadRequest(L("InvalidExamSubmission"));
            }

            var exam = _examRepository
                .GetAllIncluding(e => e.Questions)
                .FirstOrDefault(e => e.Id == input.ExamId);

            if (exam == null)
            {
                return NotFound(L("ExamNotFound"));
            }

            int correct = 0;
            int incorrect = 0;
            int part5Correct = 0;
            int part5Total = 0;
            int part6Correct = 0;
            int part6Total = 0;
            int part7Correct = 0;
            int part7Total = 0;

            foreach (var question in exam.Questions)
            {
                bool isAnswered = input.Answers != null && input.Answers.ContainsKey(question.Id);
                bool isCorrect = false;

                if (isAnswered)
                {
                    if (string.Equals(input.Answers[question.Id], question.CorrectKey, StringComparison.OrdinalIgnoreCase))
                    {
                        correct++;
                        isCorrect = true;
                    }
                    else
                    {
                        incorrect++;
                    }
                }

                if (question.PartNumber == 5)
                {
                    part5Total++;
                    if (isCorrect)
                    {
                        part5Correct++;
                    }
                }
                else if (question.PartNumber == 6)
                {
                    part6Total++;
                    if (isCorrect)
                    {
                        part6Correct++;
                    }
                }
                else if (question.PartNumber == 7)
                {
                    part7Total++;
                    if (isCorrect)
                    {
                        part7Correct++;
                    }
                }
            }

            int totalQuestions = exam.Questions.Count;
            int skipped = totalQuestions - correct - incorrect;
            int score = totalQuestions == 0
                ? 0
                : (int)(Math.Round((double)correct / totalQuestions * 99) * 5);

            if (score == 0 && correct > 0)
            {
                score = 5;
            }

            long currentUserId = AbpSession.UserId ?? 0;
            long attemptId = _examAttemptRepository.InsertAndGetId(new ExamAttempt
            {
                ExamId = exam.Id,
                UserId = currentUserId,
                CompletedAt = DateTime.Now,
                TimeTakenSeconds = Math.Max(0, input.TimeTakenSeconds),
                Score = score,
                CorrectCount = correct,
                IncorrectCount = incorrect,
                SkippedCount = skipped,
                TotalQuestions = totalQuestions,
                Part5Correct = part5Correct,
                Part5Total = part5Total,
                Part6Correct = part6Correct,
                Part6Total = part6Total,
                Part7Correct = part7Correct,
                Part7Total = part7Total
            });

            CurrentUnitOfWork.SaveChanges();

            return Ok(new
            {
                success = true,
                attemptId,
                targetUrl = Url.Action(nameof(Result), new { id = attemptId })
            });
        }

        [HttpGet]
        public IActionResult Result(long id)
        {
            long currentUserId = AbpSession.UserId ?? 0;
            var attempt = _examAttemptRepository
                .GetAllIncluding(a => a.Exam)
                .FirstOrDefault(a => a.Id == id && a.UserId == currentUserId);

            if (attempt == null)
            {
                return RedirectToAction(nameof(List));
            }

            var model = new ExamResultViewModel
            {
                AttemptId = attempt.Id,
                ExamId = attempt.ExamId,
                ExamTitle = attempt.Exam?.Title,
                CompletedAt = attempt.CompletedAt,
                TimeTakenSeconds = attempt.TimeTakenSeconds,
                Score = attempt.Score,
                CorrectCount = attempt.CorrectCount,
                IncorrectCount = attempt.IncorrectCount,
                SkippedCount = attempt.SkippedCount,
                TotalQuestions = attempt.TotalQuestions,
                Part5 = new PartResultViewModel
                {
                    Correct = attempt.Part5Correct,
                    Total = attempt.Part5Total
                },
                Part6 = new PartResultViewModel
                {
                    Correct = attempt.Part6Correct,
                    Total = attempt.Part6Total
                },
                Part7 = new PartResultViewModel
                {
                    Correct = attempt.Part7Correct,
                    Total = attempt.Part7Total
                }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuestion([FromBody] Question input)
        {
            var question = await _questionRepository.GetAsync(input.Id);
            question.Content = input.Content;
            question.OptionA = input.OptionA;
            question.OptionB = input.OptionB;
            question.OptionC = input.OptionC;
            question.OptionD = input.OptionD;
            question.CorrectKey = input.CorrectKey;
            await _questionRepository.UpdateAsync(question);
            return Ok(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _examAttemptRepository.DeleteAsync(a => a.ExamId == id);
            await _questionRepository.DeleteAsync(q => q.ExamId == id);
            await _passageRepository.DeleteAsync(p => p.ExamId == id);
            await _examRepository.DeleteAsync(id);
            return Ok(new { success = true });
        }

        private string ExtractTextFromDocx(IFormFile file)
        {
            var builder = new StringBuilder();

            using (var stream = file.OpenReadStream())
            using (var wordDoc = WordprocessingDocument.Open(stream, false))
            {
                var body = wordDoc.MainDocumentPart?.Document?.Body;
                if (body == null)
                {
                    return string.Empty;
                }

                foreach (var paragraph in body.Elements<Paragraph>())
                {
                    builder.AppendLine(paragraph.InnerText);
                }
            }

            return builder.ToString();
        }
    }
}
