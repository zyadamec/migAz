// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace MigAz.Azure.Interface
{
    public interface IStorageAccount
    {
        string Id { get; }
        string Name { get; }
        string BlobStorageNamespace { get; }

        string AccountType { get; }
        string PrimaryLocation { get; }
        AzureSubscription AzureSubscription { get; }
    }
}

