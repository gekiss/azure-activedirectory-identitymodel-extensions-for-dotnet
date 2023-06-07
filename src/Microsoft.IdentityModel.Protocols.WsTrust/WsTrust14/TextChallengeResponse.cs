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

namespace Microsoft.IdentityModel.Protocols.WsTrust14
{
    using System;

    /// <summary>
    /// The response to a challenge that requires interactive user input.
    /// <para>see: http://docs.oasis-open.org/ws-sx/ws-trust/v1.4/os/ws-trust-1.4-spec-os.html </para>
    /// </summary>
    public class TextChallengeResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextChallengeResponse"/> class.
        /// </summary>
        /// <param name="refId">The reference identifier.</param>
        /// <param name="value">The original text challenge issued.</param>
        public TextChallengeResponse(string refId, string value)
        {
            if (string.IsNullOrEmpty(refId))
            {
                throw new ArgumentNullException(nameof(refId));
            }

            RefId = refId;
            Value = value;
        }

        /// <summary>
        /// Gets the reference identifier.
        /// </summary>
        public string RefId { get; }

        /// <summary>
        /// Gets the original text challenge issued.
        /// </summary>
        public string Value { get; }
    }
}
