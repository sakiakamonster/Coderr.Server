﻿using System;
using System.Linq;
using System.Threading.Tasks;
using DotNetCqs;
using Griffin.Container;
using OneTrueError.Api.Core.Applications;
using OneTrueError.Api.Core.Applications.Queries;

namespace OneTrueError.App.Core.Applications.QueryHandlers
{
    /// <summary>
    ///     Handler for <see cref="GetApplicationInfo" />.
    /// </summary>
    [Component]
    public class GetApplicationListHandler : IQueryHandler<GetApplicationList, ApplicationListItem[]>
    {
        private readonly IApplicationRepository _applicationRepository;

        /// <summary>
        ///     Creates a new instance of <see cref="GetApplicationInfoHandler" />.
        /// </summary>
        /// <param name="applicationRepository">repos</param>
        public GetApplicationListHandler(IApplicationRepository applicationRepository)
        {
            if (applicationRepository == null) throw new ArgumentNullException("applicationRepository");
            _applicationRepository = applicationRepository;
        }

        /// <summary>
        ///     Method used to execute the query
        /// </summary>
        /// <param name="query">Query to execute.</param>
        /// <returns>
        ///     Task which will contain the result once completed.
        /// </returns>
        public async Task<ApplicationListItem[]> ExecuteAsync(GetApplicationList query)
        {
            if (query == null) throw new ArgumentNullException("query");
            ApplicationListItem[] result;

            if (query.AccountId != 0)
                result = (await _applicationRepository.GetForUserAsync(query.AccountId))
                    .Select(x => new ApplicationListItem(x.ApplicationId, x.ApplicationName) {IsAdmin = x.IsAdmin})
                    .ToArray();
            else
                result = (await _applicationRepository.GetAllAsync())
                    .Select(x => new ApplicationListItem(x.Id, x.Name))
                    .ToArray();

            return result;
        }
    }
}