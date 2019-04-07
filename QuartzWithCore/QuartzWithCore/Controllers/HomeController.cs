﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using QuartzWithCore.Models;
using QuartzWithCore.Tasks;

namespace QuartzWithCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IScheduler _scheduler;

        public HomeController(IScheduler factory)
        {
            _scheduler = factory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CheckAvailability()
        {
            ITrigger trigger = TriggerBuilder.Create()
             .WithIdentity($"Check Availability-{DateTime.Now}")
             .StartAt(new DateTimeOffset(DateTime.Now.AddSeconds(15)))
             .WithPriority(1)
             .Build();

            IJobDetail job = JobBuilder.Create<CheckAvailabilityTask>()
                        .WithIdentity("Check Availability")
                        .Build();

            await _scheduler.ScheduleJob(job, trigger);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}