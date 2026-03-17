using System;
using System.Collections.Generic;
using System.Linq;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using TOEICReading4.Controllers;
using TOEICReading4.Exams;
using TOEICReading4.Web.Models.Home;

namespace TOEICReading4.Web.Controllers;

[AbpMvcAuthorize]
public class HomeController : TOEICReading4ControllerBase
{
    private readonly IRepository<Exam, int> _examRepository;
    private readonly IRepository<ExamAttempt, long> _examAttemptRepository;

    public HomeController(
        IRepository<Exam, int> examRepository,
        IRepository<ExamAttempt, long> examAttemptRepository)
    {
        _examRepository = examRepository;
        _examAttemptRepository = examAttemptRepository;
    }

    public ActionResult Index()
    {
        return View();
    }

    public ActionResult Dashboard()
    {
        long? currentUserId = AbpSession.UserId;
        var exams = _examRepository
            .GetAllIncluding(e => e.Questions)
            .ToList();

        var attempts = currentUserId.HasValue
            ? _examAttemptRepository
                .GetAll()
                .Where(a => a.UserId == currentUserId.Value)
                .OrderByDescending(a => a.CompletedAt)
                .ToList()
            : new List<ExamAttempt>();

        var latestAttempts = attempts
            .GroupBy(a => a.ExamId)
            .ToDictionary(g => g.Key, g => g.First());

        var recentExams = exams
            .Select(exam =>
            {
                latestAttempts.TryGetValue(exam.Id, out var latestAttempt);

                return new DashboardExamViewModel
                {
                    ExamId = exam.Id,
                    Title = exam.Title,
                    DurationMinutes = exam.DurationMinutes,
                    QuestionCount = exam.Questions?.Count ?? 0,
                    HasAttempt = latestAttempt != null,
                    LatestAttemptId = latestAttempt?.Id,
                    LatestScore = latestAttempt?.Score,
                    ActivityDate = latestAttempt?.CompletedAt ?? exam.CreatedAt
                };
            })
            .OrderByDescending(item => item.ActivityDate)
            .Take(6)
            .ToList();

        var model = new HomeDashboardViewModel
        {
            TotalExams = exams.Count,
            CompletedExams = latestAttempts.Count,
            AvailableExams = Math.Max(exams.Count - latestAttempts.Count, 0),
            BestScore = attempts.Count == 0 ? 0 : attempts.Max(a => a.Score),
            RecentExams = recentExams
        };

        return View(model);
    }
}
