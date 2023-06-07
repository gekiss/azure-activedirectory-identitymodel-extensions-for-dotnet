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
    /// <summary>
    /// Constants: WS-Identity element names.
    /// <para>see: http://docs.oasis-open.org/ws-sx/ws-trust/v1.4/os/ws-trust-1.4-spec-os.html </para>
    /// </summary>
    public static class WsTrust14Elements
    {
        /// <summary>
        /// Gets the value for "InteractiveChallenge"
        /// </summary>
        public const string InteractiveChallenge = "InteractiveChallenge";

        /// <summary>
        /// Gets the value for "Title"
        /// </summary>
        public const string Title = "Title";

        /// <summary>
        /// Gets the value for "TextChallenge"
        /// </summary>
        public const string TextChallenge = "TextChallenge";

        /// <summary>
        /// Gets the value for "Image"
        /// </summary>
        public const string Image = "Image";

        /// <summary>
        /// Gets the value for "ChoiceChallenge"
        /// </summary>
        public const string ChoiceChallenge = "ChoiceChallenge";

        /// <summary>
        /// Gets the value for "Choice"
        /// </summary>
        public const string Choice = "Choice";

        /// <summary>
        /// Gets the value for "ChoiceSelected"
        /// </summary>
        public const string ChoiceSelected = "ChoiceSelected";

        /// <summary>
        /// Gets the value for "ContextData"
        /// </summary>
        public const string ContextData = "ContextData";

        /// <summary>
        /// Gets the value for "InteractiveChallengeResponse"
        /// </summary>
        public const string InteractiveChallengeResponse = "InteractiveChallengeResponse";

        /// <summary>
        /// Gets the value for "TextChallengeResponse"
        /// </summary>
        public const string TextChallengeResponse = "TextChallengeResponse";

        /// <summary>
        /// Gets the value for "ChoiceChallengeResponse"
        /// </summary>
        public const string ChoiceChallengeResponse = "ChoiceChallengeResponse";
    }
}
