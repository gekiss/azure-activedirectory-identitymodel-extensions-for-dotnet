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
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WsTrust14
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class ContextDataContent : ISerializable 
        {
        private readonly Collection<XmlElement> elements = new Collection<XmlElement>();
        private readonly string context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextDataContent"/> class.
        /// </summary>
        /// <param name="elements">The context data elements.</param>
        public ContextDataContent(IEnumerable<XmlElement> elements) {
            if (elements == null) {
                throw new ArgumentNullException(nameof(elements));
            }

            foreach (var element in elements) {
                this.elements.Add(element);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextDataContent"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected ContextDataContent(SerializationInfo info, StreamingContext context) {
            if (info == null) {
                throw new ArgumentNullException(nameof(info));
            }

            this.context = info.GetString("context");
            var el = (List<string>)info.GetValue("elements", typeof(List<string>));
            var doc = new XmlDocument() { XmlResolver = null };
            foreach (var item in el) {
#pragma warning disable CA3075 // Insecure DTD processing in XML, object serialization
                doc.LoadXml(item);
#pragma warning restore CA3075
                this.elements.Add(doc.DocumentElement);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextDataContent"/> class.
        /// </summary>
        /// <param name="context">The context data context.</param>
        public ContextDataContent(string context) {
            if (string.IsNullOrEmpty(context)) {
                throw new ArgumentNullException(nameof(context));
            }

            this.context = context;
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        public string Context {
            get {
                return this.context;
            }
        }

        /// <summary>
        /// Gets the elements.
        /// </summary>
        public Collection<XmlElement> Elements {
            get {
                return this.elements;
            }
        }

        /// <summary>
        /// Populates a <see cref="SerializationInfo" /> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="StreamingContext" />) for this serialization.</param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            if (info is null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("context", this.context);
            info.AddValue("elements", this.elements.ToList().ConvertAll<string>(x => x.OuterXml));
        }
    }
}
