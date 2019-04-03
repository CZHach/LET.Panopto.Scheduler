using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SessionManagement;
using RemoteRecorderManagement;
using Microsoft.AspNetCore.Mvc;
//using EFGetStarted.AspNetCore.ExistingDb.Views.Configuration;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EFGetStarted.AspNetCore.ExistingDb.Controllers
{
    public class BaseController : Controller
    {
        protected SessionManagementAuthConfig _sessionManagementAuthConfig;
        protected SessionManagementPagingConfig _sessionManagementPagingConfig;

        protected RecorderManagementAuthConfig _recorderManagementAuthConfig;
        protected RecorderManagementPagingConfig _recorderManagementPagingConfig;

        protected SessionManagement.AuthenticationInfo _sessionManagementAuth;
        protected SessionManagement.Pagination _sessionPagination;

        protected RemoteRecorderManagement.AuthenticationInfo _recorderManagementAuth;
        protected RemoteRecorderManagement.Pagination _recorderPagination;

        public BaseController(IOptions<SessionManagementAuthConfig> sessionManagementAuthConfig,
                              IOptions<SessionManagementPagingConfig> sessionManagementPagingConfig,
                              IOptions<RecorderManagementAuthConfig> recorderManagementAuthConfig,
                              IOptions<RecorderManagementPagingConfig> recorderManagementPagingConfig
                              )
        {
            _sessionManagementAuthConfig = sessionManagementAuthConfig.Value;
            _sessionManagementPagingConfig = sessionManagementPagingConfig.Value;

            _recorderManagementAuthConfig = recorderManagementAuthConfig.Value;
            _recorderManagementPagingConfig = recorderManagementPagingConfig.Value;

            this._sessionManagementAuth = new SessionManagement.AuthenticationInfo
            {
                UserKey = _sessionManagementAuthConfig.userKey,
                Password = _sessionManagementAuthConfig.password
            };

            this._sessionPagination = new SessionManagement.Pagination
            {
                MaxNumberResults = _sessionManagementPagingConfig.MaxNumberResults,
                PageNumber = _sessionManagementPagingConfig.PageNumber
            };

            this._recorderManagementAuth = new RemoteRecorderManagement.AuthenticationInfo
            {
                UserKey = _sessionManagementAuthConfig.userKey,
                Password = _sessionManagementAuthConfig.password
            };

            this._recorderPagination = new RemoteRecorderManagement.Pagination
            {
                MaxNumberResults = _sessionManagementPagingConfig.MaxNumberResults,
                PageNumber = _sessionManagementPagingConfig.PageNumber
            };
        }
    }
}
