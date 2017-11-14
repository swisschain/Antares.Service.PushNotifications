// Code generated by Microsoft (R) AutoRest Code Generator 1.2.2.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Service.PushNotifications.Client.AutorestClient.Models
{
    using Lykke.Service;
    using Lykke.Service.PushNotifications;
    using Lykke.Service.PushNotifications.Client;
    using Lykke.Service.PushNotifications.Client.AutorestClient;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class PushTxDialogModel
    {
        /// <summary>
        /// Initializes a new instance of the PushTxDialogModel class.
        /// </summary>
        public PushTxDialogModel()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the PushTxDialogModel class.
        /// </summary>
        public PushTxDialogModel(double amount, IList<string> notiicationIds = default(IList<string>), string assetId = default(string), string addressFrom = default(string), string addressTo = default(string), string message = default(string))
        {
            NotiicationIds = notiicationIds;
            Amount = amount;
            AssetId = assetId;
            AddressFrom = addressFrom;
            AddressTo = addressTo;
            Message = message;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "NotiicationIds")]
        public IList<string> NotiicationIds { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Amount")]
        public double Amount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AssetId")]
        public string AssetId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AddressFrom")]
        public string AddressFrom { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AddressTo")]
        public string AddressTo { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Message")]
        public string Message { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
        }
    }
}
