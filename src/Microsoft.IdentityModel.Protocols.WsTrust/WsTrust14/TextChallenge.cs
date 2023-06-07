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

namespace Microsoft.IdentityModel.Protocols.WsTrust14
{
    /// <summary>
    /// Challenge that requires textual input from the user.
    /// <para>see: http://docs.oasis-open.org/ws-sx/ws-trust/v1.4/os/ws-trust-1.4-spec-os.html </para>
    /// </summary>
    public class TextChallenge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextChallenge"/> class.
        /// </summary>
        /// <param name="refId">The reference identifier.</param>
        public TextChallenge(string refId)
            : this(refId, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextChallenge"/> class.
        /// </summary>
        /// <param name="refId">The reference identifier.</param>
        /// <param name="label">The label for the text challenge.</param>
        public TextChallenge(string refId, string label)
        {
            if (string.IsNullOrEmpty(refId))
            {
                throw new ArgumentNullException(nameof(refId));
            }

            RefId = refId;
            Label = label;
        }

        /// <summary>
        /// Gets the reference identifier.
        /// </summary>
        public string RefId { get; }

        /// <summary>
        /// Gets or sets the maximum length of the text string.
        /// </summary>
        /// <value>
        /// The maximum length of the text string.
        /// </value>
        public int? MaxLen { get; set; }

        /// <summary>
        /// Gets or sets the text challenge MUST receive treatment as hidden text.
        /// </summary>
        /// <value>
        /// The text challenge MUST receive treatment as hidden text.
        /// </value>
        public bool? HideText { get; set; }

        /// <summary>
        /// Gets or sets the label for the text challenge.
        /// </summary>
        /// <value>
        /// The label for the text challenge.
        /// </value>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the inline image specific to the text challenge item.
        /// </summary>
        /// <value>
        /// The inline image specific to the text challenge item.
        /// </value>
        public ChallengeImage Image { get; set; }
    }
}
