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

using System.Collections.Generic;

namespace Microsoft.IdentityModel.Protocols.WsIdentity
{
    /// <summary>
    /// Constants: WS-Identity namespace and prefix.
    /// <para>see: http://docs.oasis-open.org/imi/identity/v1.0/identity.html </para>
    /// </summary>
    public abstract class WsIdentityConstants : WsConstantsBase
    {
        /// <summary>
        /// Gets the list of namespaces that are recognized by this runtime.
        /// </summary>
        public static IList<string> KnownNamespaces { get; } = new List<string> { "http://schemas.xmlsoap.org/ws/2005/05/identity", "http://schemas.xmlsoap.org/ws/2007/01/identity", "http://docs.oasis-open.org/imi/ns/identity-200810" };

        /// <summary>
        /// Gets constants for WS-Identity 1.0
        /// </summary>
        public static WsIdentity10Constants Identity10 { get; } = new WsIdentity10Constants();

        /// <summary>
        /// Gets the schema location for WS-Identity.
        /// </summary>
        public string SchemaLocation { get; protected set; }
    }

    /// <summary>
    /// Constants: WS-Identity 1.0 namespace and prefix.
    /// </summary>
    public class WsIdentity10Constants : WsIdentityConstants
    {
        /// <summary>
        /// Instantiates WS-Identity 1.0
        /// </summary>
        public WsIdentity10Constants() 
        {
            Prefix = "ic";
            Namespace = "http://schemas.xmlsoap.org/ws/2005/05/identity";
            SchemaLocation = "http://schemas.xmlsoap.org/ws/2005/05/identity/identity.xsd";
        }
    }
}



