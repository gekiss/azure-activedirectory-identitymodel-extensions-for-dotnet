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
using System.Xml;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.WsFed;
using Microsoft.IdentityModel.Xml;

namespace Microsoft.IdentityModel.Protocols.WsIdentity
{
    /// <summary>
    /// Base class for support of serializing versions of WS-Identity.
    /// </summary>
    internal class WsIdentitySerializer
    {
        public WsIdentitySerializer()
        {
            //  if this class becomes public, we will need to check parameters on public methods
        }

        /// <summary>
        /// Creates and populates a <see cref="ClaimType"/> by reading xml.
        /// Expects the <see cref="XmlDictionaryReader"/> to be positioned on the StartElement: "ClaimType" in the namespace passed in.
        /// </summary>
        /// <param name="reader">a <see cref="XmlDictionaryReader"/> positioned at the StartElement: "ClaimType".</param>
        /// <param name="namespace">the namespace for the StartElement.</param>
        /// <returns>a populated <see cref="ClaimType"/>.</returns>
        /// <remarks>Checking for the correct StartElement is as follows.</remarks>
        /// <remarks>if @namespace is null, then <see cref="XmlDictionaryReader.IsLocalName(string)"/> will be called.</remarks>
        /// <remarks>if @namespace is not null or empty, then <see cref="XmlDictionaryReader.IsStartElement(XmlDictionaryString, XmlDictionaryString)"/> will be called.></remarks>
        /// <exception cref="ArgumentNullException">if reader is null.</exception>
        /// <exception cref="XmlReadException">if reader is not positioned on a StartElement.</exception>
        /// <exception cref="XmlReadException">if the StartElement does not match the expectations in remarks.</exception>
        public virtual ClaimType ReadClaimType(XmlDictionaryReader reader, string @namespace)
        {
            // <ic:ClaimType 
            //      Uri="a14bf1a3-a189-4a81-9d9a-7d3dfeb7724a"
            //      Optional="true/false">
            // </ic:ClaimType>

            XmlAttributeHolder[] attributes = XmlAttributeHolder.ReadAttributes(reader);
            string uri = XmlAttributeHolder.GetAttribute(attributes, WsIdentityAttributes.Uri, @namespace);
            if (string.IsNullOrEmpty(uri))
                throw LogHelper.LogExceptionMessage(new XmlReadException(LogHelper.FormatInvariant(WsTrust.LogMessages.IDX15013, WsIdentityElements.ClaimType, WsIdentityAttributes.Uri)));

            string optionalAttribute = XmlAttributeHolder.GetAttribute(attributes, WsIdentityAttributes.Optional, @namespace);
            bool? optional = null;
            if (!string.IsNullOrEmpty(optionalAttribute))
                optional = XmlConvert.ToBoolean(optionalAttribute);

            bool isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (!isEmptyElement)
                reader.ReadEndElement();

            return new ClaimType { Uri = uri, IsOptional = optional };
        }

        public static void WriteClaimType(XmlDictionaryWriter writer, WsSerializationContext serializationContext, ClaimType claimType)
        {
            writer.WriteStartElement(serializationContext.IdentityConstants.Prefix, WsIdentityElements.ClaimType, serializationContext.IdentityConstants.Namespace);
            writer.WriteAttributeString(WsIdentityAttributes.Uri, claimType.Uri);
            if (claimType.IsOptional.HasValue)
                writer.WriteAttributeString(WsIdentityAttributes.Optional, XmlConvert.ToString(claimType.IsOptional.Value));

            writer.WriteEndElement();
        }
    }
}
