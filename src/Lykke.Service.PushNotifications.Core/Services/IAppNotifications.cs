﻿using System;
using System.Threading.Tasks;
using Lykke.Service.PushNotifications.Contract.Enums;

namespace Lykke.Service.PushNotifications.Core.Services
{
    public static class EventsAndEntities
    {
        // ReSharper disable once InconsistentNaming
        public const string KYC = "KYC";
        public const string TradingWallet = "TradingWallet";
        public const string MarginWallet = "MarginWallet";

        public const string Ok = "Ok";
        public const string RestrictedArea = "RestrictedArea";
        public const string NeedToFillData = "NeedToFillData";
        public const string TxFailed = "TxFailed";
        public const string TxConfirmed = "TxConfirmed";
        public const string DepositCompleted = "DepositCompleted";
        public const string PositionOpened = "PositionOpened";
        public const string PositionClosed = "PositionClosed";
        public const string MarginCall = "MarginCall";
        public const string NeedTransactionSign = "NeedTransactionSign";
        public const string PushTxDialog = "PushTxDialog";

        public const string Operation = "Operation";
        public const string OperationCreated = "OperationCreated";

        public const string Offchain = "Offchain";
        public const string OffchainRequest = "OffchainRequest";

        public const string ClientDialog = "ClientDialog";
        public const string ClientDialogRequest = "ClientDialogRequest";

        public const string LiveAvailable = "LiveAvailable";

        public const string LimitOrderEvent = "LimitOrderEvent";
        public const string Tier = "Tier";

        public static string GetEntity(NotificationType notification)
        {
            switch (notification)
            {
                case NotificationType.Info:
                    return Ok;
                case NotificationType.KycSucceess:
                    return KYC;
                case NotificationType.KycRestrictedArea:
                case NotificationType.KycNeedToFillDocuments:
                case NotificationType.TierUpgraded:
                case NotificationType.DepositLimitPercentReached:
                    return Tier;
                case NotificationType.TransactionConfirmed:
                case NotificationType.TransctionFailed:
                case NotificationType.AssetsCredited:
                case NotificationType.PushTxDialog:
                    return TradingWallet;

                case NotificationType.PositionOpened:
                case NotificationType.PositionClosed:
                case NotificationType.MarginCall:
                case NotificationType.LiveAvailable:
                    return MarginWallet;
                case NotificationType.NeedTransactionSign:
                    return NeedTransactionSign;
                case NotificationType.OffchainRequest:
                    return Offchain;
                case NotificationType.OperationCreated:
                    return Operation;
                case NotificationType.ClientDialog:
                    return ClientDialog;
                case NotificationType.TradingSessionCreated:
                    return "Session";
                case NotificationType.LimitOrderEvent:
                    return LimitOrderEvent;
                case NotificationType.Wakeup:
                    return NotificationType.Wakeup.ToString();
                default:
                    throw new ArgumentException("Unknown notification");
            }
        }

        public static string GetEvent(NotificationType notification)
        {
            switch (notification)
            {
                case NotificationType.TierUpgraded:
                    return NotificationType.TierUpgraded.ToString();
                case NotificationType.DepositLimitPercentReached:
                    return NotificationType.DepositLimitPercentReached.ToString();
                case NotificationType.Info:
                case NotificationType.KycSucceess:
                    return Ok;
                case NotificationType.KycRestrictedArea:
                    return RestrictedArea;
                case NotificationType.KycNeedToFillDocuments:
                    return NeedToFillData;
                case NotificationType.TransactionConfirmed:
                    return TxConfirmed;
                case NotificationType.TransctionFailed:
                    return TxFailed;
                case NotificationType.AssetsCredited:
                    return DepositCompleted;
                case NotificationType.PositionOpened:
                    return PositionOpened;
                case NotificationType.PositionClosed:
                    return PositionClosed;
                case NotificationType.MarginCall:
                    return MarginCall;
                case NotificationType.OffchainRequest:
                    return OffchainRequest;
                case NotificationType.OperationCreated:
                    return OperationCreated;
                case NotificationType.NeedTransactionSign:
                    return NeedTransactionSign;
                case NotificationType.PushTxDialog:
                    return PushTxDialog;
                case NotificationType.LiveAvailable:
                    return LiveAvailable;
                case NotificationType.ClientDialog:
                    return ClientDialogRequest;
                case NotificationType.TradingSessionCreated:
                    return "TradingSessionCreated";
                case NotificationType.LimitOrderEvent:
                    return LimitOrderEvent;
                default:
                    throw new ArgumentException("Unknown notification");
            }
        }
    }

    public interface IAppNotifications
    {
        Task SendDataNotificationToAllDevicesAsync(string[] notificationIds, string type, string entity, string id = "");

        Task SendTextNotificationAsync(string[] notificationsIds, string type, string message);

        Task SendLimitOrderNotification(string[] notificationsIds, string message, string orderType, string orderStatus);

        Task SendMtOrderChangedNotification(string[] notificationIds, string notificationType, string message, string orderId);

        Task SendPushTxDialogAsync(string[] notificationsIds, double amount, string assetId, string addressFrom,
            string addressTo, string message);

        Task SendAssetsCreditedNotification(string[] notificationsIds, double amount, string assetId, string message);

        Task SendRawIosNotification(string notificationId, string payload);

        Task SendRawAndroidNotification(string notificationId, string payload);

        Task SendWakeupNotificationAsync(string tag, string message);
    }
}
