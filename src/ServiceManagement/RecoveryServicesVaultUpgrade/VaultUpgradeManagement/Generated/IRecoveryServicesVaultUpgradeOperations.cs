// 
// Copyright (c) Microsoft and contributors.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// 
// See the License for the specific language governing permissions and
// limitations under the License.
// 

// Warning: This code was generated by a tool.
// 
// Changes to this file may cause incorrect behavior and will be lost if the
// code is regenerated.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Management.RecoveryServicesVaultUpgrade.Models;

namespace Microsoft.WindowsAzure.Management.RecoveryServicesVaultUpgrade
{
    /// <summary>
    /// Definition of vault upgrade operations for the
    /// RecoveryServicesVaultUpgrade extension.
    /// </summary>
    public partial interface IRecoveryServicesVaultUpgradeOperations
    {
        /// <summary>
        /// Check Prerequisites for Vault Upgrade.
        /// </summary>
        /// <param name='resourceUpgradeInput'>
        /// Input required for the resource to be upgraded.
        /// </param>
        /// <param name='customRequestHeaders'>
        /// Request header parameters.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// The response model for the Azure operations.
        /// </returns>
        Task<AzureResponse> CheckPrerequisitesForRecoveryServicesVaultUpgradeAsync(ResourceUpgradeInput resourceUpgradeInput, CustomRequestHeaders customRequestHeaders, CancellationToken cancellationToken);
        
        /// <summary>
        /// Track Resource Upgrade.
        /// </summary>
        /// <param name='customRequestHeaders'>
        /// Request header parameters.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// The definition of an output object stating the status of resource
        /// upgrade.
        /// </returns>
        Task<TrackResourceUpgradeResponse> TrackResourceUpgradeAsync(CustomRequestHeaders customRequestHeaders, CancellationToken cancellationToken);
        
        /// <summary>
        /// Start Resource Upgrade.
        /// </summary>
        /// <param name='resourceUpgradeInput'>
        /// Input required for resource upgradation.
        /// </param>
        /// <param name='customRequestHeaders'>
        /// Request header parameters.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// The definition of a resource upgrade output object.
        /// </returns>
        Task<ResourceUpgradeDetails> UpgradeResourceAsync(ResourceUpgradeInput resourceUpgradeInput, CustomRequestHeaders customRequestHeaders, CancellationToken cancellationToken);
    }
}
