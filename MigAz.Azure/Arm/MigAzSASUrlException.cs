// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace MigAz.Azure.Arm
{
    [Serializable]
    internal class MigAzSASUrlException : Exception
    {
        public MigAzSASUrlException()
        {
        }

        public MigAzSASUrlException(string message) : base(message)
        {
        }

        public MigAzSASUrlException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MigAzSASUrlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}