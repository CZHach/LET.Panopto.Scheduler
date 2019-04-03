using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SessionManagement;
using RemoteRecorderManagement;
//using EFGetStarted.AspNetCore.ExistingDb.Views.Configuration;
using EFGetStarted.AspNetCore.ExistingDb.Models;
using Microsoft.Extensions.Options;
using System.Globalization;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EFGetStarted.AspNetCore.ExistingDb.Controllers
{
    public class SessionController : BaseController
    {
        public SessionController(
                    IOptions<SessionManagementAuthConfig> sessionManagementAuthConfig,
                    IOptions<SessionManagementPagingConfig> sessionManagementPagingConfig,
                    IOptions<RecorderManagementAuthConfig> recorderManagementAuthConfig,
                    IOptions<RecorderManagementPagingConfig> recorderManagementPagingConfig
                    ) : base(sessionManagementAuthConfig, sessionManagementPagingConfig,
                             recorderManagementAuthConfig, recorderManagementPagingConfig)
        {
            _sessionManagementAuthConfig = sessionManagementAuthConfig.Value;
            _sessionManagementPagingConfig = sessionManagementPagingConfig.Value;
            _recorderManagementAuthConfig = recorderManagementAuthConfig.Value;
            _recorderManagementPagingConfig = recorderManagementPagingConfig.Value;
        }

        public async Task<IActionResult> RecorderSessionIndex(Guid Id)
        {

            ISessionManagement sessionMgr = new SessionManagementClient();

            ListSessionsResponse response;

            response = await sessionMgr.GetSessionsListAsync(this._sessionManagementAuth, new ListSessionsRequest
            {
                Pagination = this._sessionPagination,
                RemoteRecorderId = Id,
                SortIncreasing = true
            }, null);

            var r = response.Results;

            return View(response.Results);

        }

        public async Task<IActionResult> GetSessionByFolderId(Guid Id)
        {
            ISessionManagement sessionMgr = new SessionManagementClient();
            ListSessionsResponse response;

            response = await sessionMgr.GetSessionsListAsync(this._sessionManagementAuth, new ListSessionsRequest
            {
                Pagination = this._sessionPagination,
                FolderId = Id,
                SortIncreasing = true
            }, null);
            var json = JsonConvert.SerializeObject(response.Results);
            var r = response.Results;

            return View(r);
        }

        public async Task<IActionResult> SessionByDateRange(DateTime startdate)
        {
            bool isAjaxCall = Request.Headers["x-requested-with"] == "XMLHttpRequest";
            ISessionManagement sessionMgr = new SessionManagementClient();
            ListSessionsResponse response;

            response = await sessionMgr.GetSessionsListAsync(this._sessionManagementAuth, new ListSessionsRequest
            {
                Pagination = this._sessionPagination,
                StartDate = startdate,
                EndDate = startdate.AddDays(1),
                SortIncreasing = true
            }, null);

            var r = response.Results;

            if (isAjaxCall)
            {
                return PartialView(r);
            }
            return View(r);
        }

        public async Task<IActionResult> AddSessions(Guid recorderId)
        {
            ISessionManagement sessionMgr = new SessionManagementClient();

            RemoteRecorderManagementClient recorderClient = new RemoteRecorderManagementClient();
            ScheduledRecordingResult scheduleResult;

            Guid remoteRecorderId = Guid.Parse("b996f247-377d-46ed-8410-aa170135aea1");
            Guid panoptoFolderId = Guid.Parse("1e957dca-3fe1-4214-b251-a96e0106997a");

            List<RecorderSettings> recorderSettings = new List<RecorderSettings>
            {
                new RecorderSettings { RecorderId = remoteRecorderId }
            };

            scheduleResult = await recorderClient.ScheduleRecordingAsync(
                _recorderManagementAuth,
                "Test Recording with recorder Client 2",
                panoptoFolderId,
                false,
                new DateTime(2019, 11, 1, 8, 25, 00),
                new DateTime(2019, 11, 1, 9, 45, 00),
                recorderSettings.ToArray()
                );

            var sessionID = scheduleResult.SessionIDs.FirstOrDefault();

            var response = await sessionMgr.GetSessionsByIdAsync(this._sessionManagementAuth, scheduleResult.SessionIDs);

            var responseObj = response.FirstOrDefault();

            return View(responseObj);
        }

        [HttpGet]
        public IActionResult CreateSchedulingEvent()
        {
            return View();
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSchedulingEvent(
            [Bind("SessionName, SessionFolderId, SessionStart, SessionEnd, SessionRecorderId")]
        SchedulingEvent schedulingEvent)
        {
            ISessionManagement sessionMgr = new SessionManagementClient();

            RemoteRecorderManagementClient recorderClient = new RemoteRecorderManagementClient();
            ScheduledRecordingResult scheduleResult;

            //test recorder
            Guid remoteRecorderId = Guid.Parse("b996f247-377d-46ed-8410-aa170135aea1");

            // test folder
            Guid panoptoFolderId = Guid.Parse("1e957dca-3fe1-4214-b251-a96e0106997a");

            List<RecorderSettings> recorderSettings = new List<RecorderSettings>
            {
                new RecorderSettings { RecorderId = remoteRecorderId }
            };

            // auth, sesson.Name, session.FolderId, session.IsBroadcast, session.start, session.end, recorderSettings // 
            scheduleResult = await recorderClient.ScheduleRecordingAsync(
                    _recorderManagementAuth,
                    schedulingEvent.SessionName,
                    panoptoFolderId,
                    //schedulingEvent.SessionFolderId,
                    false,
                    schedulingEvent.SessionStart,
                    schedulingEvent.SessionEnd,
                    recorderSettings.ToArray()
                    );

            SchedulingEvent s = new SchedulingEvent
            {
                SessionName = schedulingEvent.SessionName,
                SessionRecorderId = schedulingEvent.SessionRecorderId,
                SessionFolderId = schedulingEvent.SessionFolderId,
                SessionStart = schedulingEvent.SessionStart,
                SessionEnd = schedulingEvent.SessionEnd
            };


            return View(s);
        }
    }
}
