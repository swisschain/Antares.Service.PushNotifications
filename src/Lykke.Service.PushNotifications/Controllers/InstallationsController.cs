using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Common.Log;
using Lykke.Service.PushNotifications.Client;
using Lykke.Service.PushNotifications.Client.Models;
using Lykke.Service.PushNotifications.Contract;
using Lykke.Service.PushNotifications.Contract.Enums;
using Lykke.Service.PushNotifications.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.NotificationHubs;
using Swashbuckle.AspNetCore.Annotations;

namespace Lykke.Service.PushNotifications.Controllers
{
    [Route("api/Installations")]
    public class InstallationsController : Controller, IInstallationsApi
    {
        private readonly NotificationHubClient _notificationHubClient;
        private readonly IInstallationsRepository _installationsRepository;
        private readonly ILog _log;

        public InstallationsController(
            NotificationHubClient notificationHubClient,
            IInstallationsRepository installationsRepository,
            ILogFactory logFactory
        )
        {
            _notificationHubClient = notificationHubClient;
            _installationsRepository = installationsRepository;
            _log = logFactory.CreateLog(this);
        }

        [HttpPost("register")]
        [SwaggerOperation("Register")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task RegisterAsync([FromBody] InstallationModel installationModel)
        {
            string[] tags = new HashSet<string>(installationModel.Tags)
            {
                installationModel.NotificationId,
                installationModel.Platform.ToString()
            }.ToArray();

            Installation installation = new Installation
            {
                InstallationId = Convert.ToBase64String(Encoding.UTF8.GetBytes(installationModel.PushChannel)).TrimEnd('='),
                PushChannel = installationModel.PushChannel,
                Tags = tags,
                Platform = installationModel.Platform == MobileOs.Ios
                    ? NotificationPlatform.Apns
                    : NotificationPlatform.Fcm
            };

            try
            {
                await _notificationHubClient.CreateOrUpdateInstallationAsync(installation);
                await _installationsRepository.AddOrUpdateAsync(new InstallationItem
                {
                    NotificationId = installationModel.NotificationId,
                    InstallationId = installation.InstallationId,
                    PushChannel = installation.PushChannel,
                    Platform = installationModel.Platform,
                    Tags = tags
                });
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw new ValidationApiException(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("remove")]
        [SwaggerOperation("RemoveInstallation")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task RemoveAsync([FromBody] InstallationRemoveModel model)
        {
            await _notificationHubClient.DeleteInstallationAsync(model.InstallationId);
            await _installationsRepository.DeleteAsync(model.NotificationId, model.InstallationId);
        }

        [HttpGet("{notificationId}")]
        [SwaggerOperation("GetInstallations")]
        [ProducesResponseType(typeof(IReadOnlyList<DeviceInstallation>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IReadOnlyList<DeviceInstallation>> GetByNotificationIdAsync(string notificationId)
        {
            var installations = await _installationsRepository.GetByNotificationIdAsync(notificationId);
            return Mapper.Map<IReadOnlyList<DeviceInstallation>>(installations);
        }

        [HttpPost("tags/add")]
        [SwaggerOperation("AddTags")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task AddTagsAsync([FromBody] TagsUpdateModel tagsModel)
        {
            var installation = await _notificationHubClient.GetInstallationAsync(tagsModel.InstallationId);

            if (installation == null)
                throw new ValidationApiException(HttpStatusCode.NotFound, "Installation not found");

            var tags = new HashSet<string>(installation.Tags);

            foreach (var tag in tagsModel.Tags)
            {
                tags.Add(tag);
            }

            installation.Tags = tags.ToArray();

            await _notificationHubClient.CreateOrUpdateInstallationAsync(installation);
            await _installationsRepository.AddOrUpdateAsync(new InstallationItem
            {
                NotificationId = tagsModel.NotificationId,
                InstallationId = tagsModel.InstallationId,
                PushChannel = installation.PushChannel,
                Platform = installation.Platform == NotificationPlatform.Fcm
                    ? MobileOs.Android
                    : MobileOs.Ios,
                Tags = tags.ToArray()
            });
        }

        [HttpPost("tags/remove")]
        [SwaggerOperation("RemoveTags")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task RemoveTagsAsync([FromBody] TagsUpdateModel tagsModel)
        {
            var installation = await _notificationHubClient.GetInstallationAsync(tagsModel.InstallationId);

            if (installation == null)
                throw new ValidationApiException(HttpStatusCode.NotFound, "Installation not found");

            var tags = new HashSet<string>(installation.Tags);

            foreach (var tag in tagsModel.Tags)
            {
                tags.Remove(tag);
            }

            tags.Add(tagsModel.NotificationId);

            installation.Tags = tags.ToArray();

            await _notificationHubClient.CreateOrUpdateInstallationAsync(installation);
            await _installationsRepository.AddOrUpdateAsync(new InstallationItem
            {
                NotificationId = tagsModel.NotificationId,
                InstallationId = tagsModel.InstallationId,
                PushChannel = installation.PushChannel,
                Platform = installation.Platform == NotificationPlatform.Fcm
                    ? MobileOs.Android
                    : MobileOs.Ios,
                Tags = tags.ToArray()
            });
        }
    }
}