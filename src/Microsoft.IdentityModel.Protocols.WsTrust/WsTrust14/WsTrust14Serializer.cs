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
using System.Xml;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.WsFed;
using Microsoft.IdentityModel.Protocols.WsTrust;
using Microsoft.IdentityModel.Xml;

namespace Microsoft.IdentityModel.Protocols.WsTrust14
{
    /// <summary>
    /// Base class for support of serializing versions of WS-Trust 1.4.
    /// </summary>
    internal class WsTrust14Serializer
    {
        public WsTrust14Serializer()
        {
            //  if this class becomes public, we will need to check parameters on public methods
        }

        public virtual InteractiveChallenge ReadInteractiveChallenge(XmlReader reader)
        {
            if (reader == null)
                throw LogHelper.LogArgumentNullException(nameof(reader));

            InteractiveChallenge interactiveChallenge = null;
            if (!reader.IsEmptyElement)
            {
                interactiveChallenge = new InteractiveChallenge();
                reader.ReadStartElement(WsTrust14Elements.InteractiveChallenge, WsTrustConstants.Trust14.Namespace);
                if (reader.IsStartElement(WsTrust14Elements.Title, WsTrustConstants.Trust14.Namespace))
                {
                    interactiveChallenge.Title = reader.ReadElementContentAsString();
                }

                while (reader.IsStartElement(WsTrust14Elements.TextChallenge, WsTrustConstants.Trust14.Namespace))
                {
                    bool isEmptyElement = reader.IsEmptyElement;

                    var refId = reader.GetAttribute(WsTrust14Attributes.RefId);
                    var textChallenge = new TextChallenge(refId);

                    textChallenge.Label = reader.GetAttribute(WsTrust14Attributes.Label);
                    var str = reader.GetAttribute(WsTrust14Attributes.MaxLen);
                    if (!string.IsNullOrEmpty(str))
                    {
                        textChallenge.MaxLen = XmlConvert.ToInt32(str);
                    }

                    str = reader.GetAttribute(WsTrust14Attributes.HideText);
                    if (!string.IsNullOrEmpty(str))
                    {
                        textChallenge.HideText = XmlConvert.ToBoolean(str);
                    }

                    reader.MoveToContent();
                    reader.Read();

                    if (!isEmptyElement)
                    {
                        if (reader.IsStartElement(WsTrust14Elements.Image, WsTrustConstants.Trust14.Namespace))
                        {
                            textChallenge.Image = ReadChallengeImage(reader);
                        }

                        reader.ReadEndElement();
                    }
                    interactiveChallenge.TextChallenge.Add(textChallenge);
                }

                while (reader.IsStartElement(WsTrust14Elements.ChoiceChallenge, WsTrustConstants.Trust14.Namespace))
                {
                    bool isEmptyElement = reader.IsEmptyElement;
                    var refId = reader.GetAttribute(WsTrust14Attributes.RefId);
                    var label = reader.GetAttribute(WsTrust14Attributes.Label);

                    bool? exactlyOne = null;
                    var str = reader.GetAttribute(WsTrust14Attributes.ExactlyOne);
                    if (!string.IsNullOrEmpty(str))
                    {
                        exactlyOne = XmlConvert.ToBoolean(str);
                    }

                    reader.MoveToContent();
                    reader.Read();

                    IList<ChoiceItem> items = new List<ChoiceItem>();
                    while (reader.IsStartElement(WsTrust14Elements.Choice, WsTrustConstants.Trust14.Namespace))
                    {
                        bool isEmptyElement0 = reader.IsEmptyElement;

                        var refId1 = reader.GetAttribute(WsTrust14Attributes.RefId);
                        var label1 = reader.GetAttribute(WsTrust14Attributes.Label);
                        var choiseItem = new ChoiceItem(refId1, label1);

                        reader.MoveToContent();
                        reader.Read();

                        if (!isEmptyElement0)
                        {
                            if (reader.IsStartElement(WsTrust14Elements.Image, WsTrustConstants.Trust14.Namespace))
                            {
                                choiseItem.Image = ReadChallengeImage(reader);
                            }

                            reader.ReadEndElement();
                        }

                        items.Add(choiseItem);
                    }

                    var choiseChallenge = new ChoiceChallenge(refId, label, items) { ExactlyOne = exactlyOne };
                    interactiveChallenge.ChoiceChallenge.Add(choiseChallenge);

                    if (!isEmptyElement)
                    {
                        reader.ReadEndElement();
                    }
                }

                while (reader.IsStartElement(WsTrust14Elements.ContextData, WsTrustConstants.Trust14.Namespace))
                {
                    interactiveChallenge.ContextData.Add(ReadContextData(reader));
                }

                reader.ReadEndElement();
            }

            /*
            if (interactiveChallenge.TextChallenge.Count == 0 && interactiveChallenge.ChoiseChallenge.Count == 0) {
                throw DiagnosticTools.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException("interactiveChallenge read error")));
            }
             */

            return interactiveChallenge;
        }

        public virtual InteractiveChallengeResponse ReadInteractiveChallengeResponse(XmlReader reader)
        {
            if (reader == null)
                throw LogHelper.LogArgumentNullException(nameof(reader));

            InteractiveChallengeResponse interactiveChallengeResponse = null;
            if (!reader.IsEmptyElement)
            {
                interactiveChallengeResponse = new InteractiveChallengeResponse();
                reader.ReadStartElement(WsTrust14Elements.InteractiveChallengeResponse, WsTrustConstants.Trust14.Namespace);

                while (reader.IsStartElement(WsTrust14Elements.TextChallengeResponse, WsTrustConstants.Trust14.Namespace))
                {
                    var refId = reader.GetAttribute(WsTrust14Attributes.RefId);
                    var value = reader.ReadElementContentAsString();

                    interactiveChallengeResponse.TextChallengeResponse.Add(new TextChallengeResponse(refId, value));
                }

                while (reader.IsStartElement(WsTrust14Elements.ChoiceChallengeResponse, WsTrustConstants.Trust14.Namespace))
                {
                    bool isEmptyElement = reader.IsEmptyElement;

                    var refId = reader.GetAttribute(WsTrust14Attributes.RefId);

                    reader.MoveToContent();
                    reader.Read();

                    IList<string> items = new List<string>();
                    while (reader.IsStartElement(WsTrust14Elements.ChoiceSelected, WsTrustConstants.Trust14.Namespace))
                    {
                        items.Add(reader.GetAttribute(WsTrust14Attributes.RefId));

                        reader.MoveToContent();
                        reader.Read();
                    }

                    interactiveChallengeResponse.ChoiceChallengeResponse.Add(new ChoiceChallengeResponse(refId, items));

                    if (!isEmptyElement)
                    {
                        reader.ReadEndElement();
                    }
                }

                while (reader.IsStartElement(WsTrust14Elements.ContextData, WsTrustConstants.Trust14.Namespace))
                {
                    interactiveChallengeResponse.ContextData.Add(ReadContextData(reader));
                }

                if (interactiveChallengeResponse.TextChallengeResponse.Count == 0 && interactiveChallengeResponse.ChoiceChallengeResponse.Count == 0)
                {
                    throw LogHelper.LogExceptionMessage(new XmlReadException("interactiveChallengeResponse read error"));
                }

                reader.ReadEndElement();
            }

            return interactiveChallengeResponse;
        }

        public static void WriteInteractiveChallenge(XmlWriter writer, InteractiveChallenge interactiveChallenge)
        {
            // <wst14:InteractiveChallenge xmlns:wst14="..." ...>
            //   <wst14:Title ...> xs:string </wst14:Title> ?
            //   <wst14:TextChallenge RefId="xs:anyURI" Label="xs:string"?
            //                        MaxLen="xs:int"? HideText="xs:boolean"? ...>
            //     <wst14:Image MimeType="xs:string"> xs:base64Binary </wst14:Image> ?
            //   </wst14:TextChallenge> *
            //   <wst14:ChoiceChallenge RefId="xs:anyURI" Label="xs:string"?
            //                          ExactlyOne="xs:boolean"? ...>
            //     <wst14:Choice RefId="xs:anyURI" Label="xs:string"? ...>
            //       <wst14:Image MimeType="xs:string"> xs:base64Binary </wst14:Image> ?
            //     </wst14:Choice> +
            //   </wst14:ChoiceChallenge> *
            //   < wst14:ContextData RefId="xs:anyURI"> xs:any </wst14:ContextData> *
            //   ...
            // </wst14:InteractiveChallenge>

            if (writer == null)
                throw LogHelper.LogArgumentNullException(nameof(writer));

            if (interactiveChallenge == null)
                throw LogHelper.LogArgumentNullException(nameof(interactiveChallenge));

            writer.WriteStartElement(WsTrustConstants.Trust14.Prefix, WsTrust14Elements.InteractiveChallenge, WsTrustConstants.Trust14.Namespace);

            // Title
            if (!string.IsNullOrEmpty(interactiveChallenge.Title))
                writer.WriteElementString(WsTrust14Elements.Title, WsTrustConstants.Trust14.Namespace, interactiveChallenge.Title);

            // TextChallenge
            foreach (var textChallenge in interactiveChallenge.TextChallenge)
            {
                writer.WriteStartElement(WsTrust14Elements.TextChallenge, WsTrustConstants.Trust14.Namespace);
                writer.WriteAttributeString(WsTrust14Attributes.RefId, textChallenge.RefId);

                if (!string.IsNullOrEmpty(textChallenge.Label))
                    writer.WriteAttributeString(WsTrust14Attributes.Label, textChallenge.Label);

                if (textChallenge.MaxLen.HasValue)
                    writer.WriteAttributeString(WsTrust14Attributes.MaxLen, XmlConvert.ToString(textChallenge.MaxLen.Value));

                if (textChallenge.HideText.HasValue)
                    writer.WriteAttributeString(WsTrust14Attributes.HideText, XmlConvert.ToString(textChallenge.HideText.Value));

                if (textChallenge.Image != null)
                    WriteChallengeImage(writer, textChallenge.Image);

                writer.WriteEndElement();
            }

            // ChoiseChallenge
            foreach (var choiseChallenge in interactiveChallenge.ChoiceChallenge)
            {
                writer.WriteStartElement(WsTrust14Elements.ChoiceChallenge, WsTrustConstants.Trust14.Namespace);
                writer.WriteAttributeString(WsTrust14Attributes.RefId, choiseChallenge.RefId);

                if (!string.IsNullOrEmpty(choiseChallenge.Label))
                    writer.WriteAttributeString(WsTrust14Attributes.Label, choiseChallenge.Label);

                if (choiseChallenge.ExactlyOne.HasValue)
                    writer.WriteAttributeString(WsTrust14Attributes.ExactlyOne, XmlConvert.ToString(choiseChallenge.ExactlyOne.Value));

                foreach (var choiseItme in choiseChallenge.ChoiseItems)
                {
                    writer.WriteStartElement(WsTrust14Elements.Choice, WsTrustConstants.Trust14.Namespace);
                    writer.WriteAttributeString(WsTrust14Attributes.RefId, choiseItme.RefId);

                    if (!string.IsNullOrEmpty(choiseItme.Label))
                        writer.WriteAttributeString(WsTrust14Attributes.Label, choiseItme.Label);

                    if (choiseItme.Image != null)
                        WriteChallengeImage(writer, choiseItme.Image);

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            // Context Data
            foreach (var contextData in interactiveChallenge.ContextData)
            {
                WriteContextData(writer, contextData);
            }

            writer.WriteEndElement();
        }

        public static void WriteInteractiveChallengeResponse(XmlWriter writer, InteractiveChallengeResponse interactiveChallengeResponse)
        {
            // <wst14:InteractiveChallengeResponse xmlns:wst14="..." ...>
            //   <wst14:TextChallengeResponse RefId="xs:anyURI" ...>
            //     xs:string
            //   </wst14:TextChallengeResponse> *
            //   <wst14:ChoiceChallengeResponse RefId="xs:anyURI"> *
            //     <wst14:ChoiceSelected RefId="xs:anyURI" /> *
            //   </wst14:ChoiceChallengeResponse>
            //   <wst14:ContextData RefId="xs:anyURI"> xs:any </wst14:ContextData> *
            //   ...
            // </wst14:InteractiveChallengeResponse>

            if (writer == null)
                throw LogHelper.LogArgumentNullException(nameof(writer));

            if (interactiveChallengeResponse == null)
                throw LogHelper.LogArgumentNullException(nameof(interactiveChallengeResponse));

            writer.WriteStartElement(WsTrustConstants.Trust14.Prefix, WsTrust14Elements.InteractiveChallengeResponse, WsTrustConstants.Trust14.Namespace);

            // TextChallengeResponse
            foreach (var textChallengeResponse in interactiveChallengeResponse.TextChallengeResponse)
            {
                writer.WriteStartElement(WsTrust14Elements.TextChallengeResponse, WsTrustConstants.Trust14.Namespace);
                writer.WriteAttributeString(WsTrust14Attributes.RefId, textChallengeResponse.RefId);
                writer.WriteString(textChallengeResponse.Value);
                writer.WriteEndElement();
            }

            // ChoiceChallengeResponse
            foreach (var choiceChallengeResponse in interactiveChallengeResponse.ChoiceChallengeResponse)
            {
                writer.WriteStartElement(WsTrust14Elements.ChoiceChallengeResponse, WsTrustConstants.Trust14.Namespace);
                writer.WriteAttributeString(WsTrust14Attributes.RefId, choiceChallengeResponse.RefId);

                foreach (var choiceSelected in choiceChallengeResponse.ChoiceSelected)
                {
                    writer.WriteStartElement(WsTrust14Elements.ChoiceSelected, WsTrustConstants.Trust14.Namespace);
                    writer.WriteAttributeString(WsTrust14Attributes.RefId, choiceSelected);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            // Context Data
            foreach (var contextData in interactiveChallengeResponse.ContextData)
            {
                WriteContextData(writer, contextData);
            }

            writer.WriteEndElement();
        }

        private static void WriteContextData(XmlWriter writer, ContextData data)
        {
            if (writer == null)
                throw LogHelper.LogArgumentNullException(nameof(writer));

            if (data == null)
                throw LogHelper.LogArgumentNullException(nameof(data));

            writer.WriteStartElement(WsTrust14Elements.ContextData, WsTrustConstants.Trust14.Namespace);
            writer.WriteAttributeString(WsTrust14Attributes.RefId, data.RefId);

            if (data.Content != null & (data.Content.Elements.Count > 0 || !string.IsNullOrEmpty(data.Content.Context)))
            {
                if (data.Content.Elements.Count != 0)
                {
                    foreach (XmlElement element in data.Content.Elements)
                    {
                        element.WriteTo(writer);
                    }
                }
                else
                {
                    writer.WriteString(data.Content.Context);
                }
            }

            writer.WriteEndElement();
        }

        private static void WriteChallengeImage(XmlWriter writer, ChallengeImage image)
        {
            if (writer == null)
                throw LogHelper.LogArgumentNullException(nameof(writer));

            if (image == null)
                throw LogHelper.LogArgumentNullException(nameof(image));

            writer.WriteStartElement(WsTrust14Elements.Image, WsTrustConstants.Trust14.Namespace);
            writer.WriteAttributeString(WsTrust14Attributes.MimeType, image.MimeType);
            writer.WriteString(Convert.ToBase64String(image.GetBytes()));
            writer.WriteEndElement();
        }

        private static ContextData ReadContextData(XmlReader reader)
        {
            if (reader == null)
                throw LogHelper.LogArgumentNullException(nameof(reader));

            if (reader.IsStartElement(WsTrust14Elements.ContextData, WsTrustConstants.Trust14.Namespace))
            {
                bool isEmptyElement = reader.IsEmptyElement;
                var refId = reader.GetAttribute(WsTrust14Attributes.RefId);

                reader.Read();
                if (isEmptyElement)
                {
                    return null;
                }

                reader.MoveToContent();

                ContextDataContent contextContent;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    XmlDocument document = new XmlDocument() { XmlResolver = null, PreserveWhitespace = true };
                    document.Load(reader.ReadSubtree());

                    contextContent = new ContextDataContent(new XmlElement[] { document.DocumentElement });

                    reader.ReadEndElement();
                }
                else
                {
                    contextContent = new ContextDataContent(reader.ReadContentAsString());
                }

                reader.ReadEndElement();

                return new ContextData(refId, contextContent);
            }

            return null;
        }

        private static ChallengeImage ReadChallengeImage(XmlReader reader)
        {
            if (reader == null)
                throw LogHelper.LogArgumentNullException(nameof(reader));

            if (reader.IsStartElement(WsTrust14Elements.Image, WsTrustConstants.Trust14.Namespace))
            {
                string mimeType = reader.GetAttribute(WsTrust14Attributes.MimeType);

                var str = reader.ReadElementContentAsString();
                return new ChallengeImage(Convert.FromBase64String(str), mimeType);
            }

            return null;
        }
    }
}
