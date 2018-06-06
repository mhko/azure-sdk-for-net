// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Search.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Additional parameters for SuggestGet operation.
    /// </summary>
    public partial class XSuggestParameters
    {
        /// <summary>
        /// Initializes a new instance of the XSuggestParameters class.
        /// </summary>
        public XSuggestParameters()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the XSuggestParameters class.
        /// </summary>
        /// <param name="suggesterName">suggesterName</param>
        /// <param name="fuzzy">fuzzy</param>
        public XSuggestParameters(string suggesterName, IList<string> select = default(IList<string>), bool? fuzzy = default(bool?))
        {
            SuggesterName = suggesterName;
            Select = select;
            Fuzzy = fuzzy;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets suggesterName
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string SuggesterName { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public IList<string> Select { get; set; }

        /// <summary>
        /// Gets or sets fuzzy
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool? Fuzzy { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (SuggesterName == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "SuggesterName");
            }
        }
    }
}
