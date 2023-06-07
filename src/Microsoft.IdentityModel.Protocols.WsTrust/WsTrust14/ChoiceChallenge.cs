//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Protocols.WsTrust14
{
    /// <summary>
    /// Challenge that requires a choice among multiple items by the user
    /// <para>see: http://docs.oasis-open.org/ws-sx/ws-trust/v1.4/os/ws-trust-1.4-spec-os.html </para>
    /// </summary>
    public class ChoiceChallenge {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChoiceChallenge"/> class.
        /// </summary>
        /// <param name="refId">The reference identifier.</param>
        /// <param name="label">The title label for the choice challenge.</param>
        /// <param name="items">The choice items within the choice challenge.</param>
        public ChoiceChallenge(string refId, string label, IEnumerable<ChoiceItem> items)
            : this(refId, label, items, true) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChoiceChallenge"/> class.
        /// </summary>
        /// <param name="refId">The reference identifier.</param>
        /// <param name="label">The title label for the choice challenge.</param>
        /// <param name="items">The choice items within the choice challenge.</param>
        /// <param name="exactlyOne">if set to <c>true</c> exactly once choice must be selected by the user from among the child element choices.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="refId"/> or <paramref name="label"/> is null.</exception>
        public ChoiceChallenge(string refId, string label, IEnumerable<ChoiceItem> items, bool exactlyOne) {
            if (string.IsNullOrEmpty(refId)) {
                throw LogHelper.LogArgumentNullException(nameof(refId));
            }

            if (items == null) {
                throw LogHelper.LogArgumentNullException(nameof(items));
            }

            RefId = refId;
            Label = label;
            ExactlyOne = exactlyOne;

            foreach (var item in items) {
                ChoiseItems.Add(item);
            }
        }

        /// <summary>
        /// Gets the reference identifier.
        /// </summary>
        public string RefId { get; private set; }

        /// <summary>
        /// Gets or sets the title label for the choice challenge.
        /// </summary>
        /// <value>
        /// The title label for the choice challenge.
        /// </value>
        public string Label { get; set; }

        /// <summary>
        /// Gets the choice items within the choice challenge.
        /// </summary>
        public Collection<ChoiceItem> ChoiseItems { get; } = new Collection<ChoiceItem>();

        /// <summary>
        /// Gets or sets the exactly one.
        /// </summary>
        /// <value>
        /// The exactly one.
        /// </value>
        public bool? ExactlyOne { get; set; }
    }
}
