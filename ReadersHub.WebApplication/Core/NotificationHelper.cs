using System;
using System.Collections.Generic;

namespace ReadersHub.WebApplication.Core
{
    public class NotificationHelper
    {
        public enum MessageType
        {
            Success,
            Warning,
            Error,
            Info,
        }

        public class Notification
        {
            public MessageType MessageType { get; set; }
            public String Title { get; set; }
            public List<String> RowMessage { get; set; }

            public Notification()
            {
                RowMessage = new List<string>();
            }
        }

        public static string Get(MessageType messageType, string message)
        {
            string messageInDiv = "<div style=\"margin-top:25px;\" class=\"col-md-12 col-xs-12 alert ";
            string iconType = "<i class=\"fa fa-check-circle\"></i>";

            switch (messageType)
            {
                case MessageType.Success:
                    messageInDiv += "alert-success";
                    iconType = "<i class=\"fa fa-check-circle\"></i>";
                    break;
                case MessageType.Warning:
                    messageInDiv += "alert-warning";
                    iconType = "<i class=\"fa fa-exclamation-triangle\"></i>";
                    break;
                case MessageType.Error:
                    messageInDiv += "alert-danger";
                    iconType = "<i class=\"fa fa-times-circle\"></i>";
                    break;
                default:
                    messageInDiv += "alert-info";
                    iconType = "<i class=\"fa fa-info-circle\"></i>";
                    break;
            }

            messageInDiv += "\"> <button data-dismiss=\"alert\" class=\"close\"> &times;</button>" + iconType + " " + message + "</div>";

            return messageInDiv;
        }

        public static string Get(Notification notification)
        {
            string messageInDiv = "<div style=\"margin-top:25px;\" class=\"col-md-12 col-xs-12 alert ";
            string iconType = string.Empty;

            switch (notification.MessageType)
            {
                case MessageType.Success:
                    messageInDiv += "alert-success";
                    iconType = "<i class=\"fa fa-check-circle\"></i>";
                    break;
                case MessageType.Warning:
                    messageInDiv += "alert-warning";
                    iconType = "<i class=\"fa fa-exclamation-triangle\"></i>";
                    break;
                case MessageType.Error:
                    messageInDiv += "alert-danger";
                    iconType = "<i class=\"fa fa-times-circle\"></i>";
                    break;
                default:
                    messageInDiv += "alert-info";
                    iconType = "<i class=\"fa fa-info-circle\"></i>";
                    break;
            }

            messageInDiv += "\"> <button data-dismiss=\"alert\" class=\"close\"> &times;</button>" + iconType + " " + notification.Title;

            messageInDiv += "</div>";

            return messageInDiv;
        }
    }
}