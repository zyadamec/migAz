// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.Azure.Core.Generator
{
    public enum AlertType
    {
        Error,
        Warning,
        Recommendation
    }

    public class MigAzGeneratorAlert
    {
        public string Message;
        public AlertType AlertType = AlertType.Error;
        public object SourceObject;

        public MigAzGeneratorAlert(AlertType alertType, string message, object sourceObject)
        {
            AlertType = alertType;
            Message = message;
            SourceObject = sourceObject;
        }
    }
}

