﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coderr.Server.Domain.Core.Incidents
{
    /// <summary>
    ///     Incident repository
    /// </summary>
    public interface IIncidentRepository
    {
        /// <summary>
        /// Delete an incident
        /// </summary>
        /// <param name="incidentId">id</param>
        /// <returns></returns>
        Task Delete(int incidentId);

        /// <summary>
        ///     Get incident
        /// </summary>
        /// <param name="id">incident id</param>
        /// <returns>incident</returns>
        /// <exception cref="ArgumentOutOfRangeException">id</exception>
        /// <exception cref="EntityNotFoundException">No incident was found with the given key.</exception>
        Task<Incident> GetAsync(int id);

        /// <summary>
        /// Get specified incidents
        /// </summary>
        /// <param name="incidentIds">ids to fecth</param>
        /// <returns>All specified (or an exception will be thrown if any of them are missing)</returns>
        Task<IList<Incident>> GetManyAsync(IEnumerable<int> incidentIds);

        /// <summary>
        ///     Count the number of incidents for the given application
        /// </summary>
        /// <param name="applicationId">application</param>
        /// <returns>total count of incidents</returns>
        /// <exception cref="ArgumentOutOfRangeException">applicationId</exception>
        Task<int> GetTotalCountForAppInfoAsync(int applicationId);

        /// <summary>
        ///     Update incident
        /// </summary>
        /// <param name="incident">incdient</param>
        /// <returns>task</returns>
        /// <exception cref="ArgumentNullException">incident</exception>
        Task UpdateAsync(Incident incident);
    }
}