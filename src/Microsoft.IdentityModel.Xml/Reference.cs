// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.IdentityModel.Logging.LogHelper;
using static Microsoft.IdentityModel.Xml.XmlUtil;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Represents a XmlDsig Reference element as per: https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-Reference
    /// </summary>
    public class Reference : DSigElement
    {
        private CanonicalizingTransfrom _canonicalizingTransfrom;
        private string _digestMethod;
        private string _digestValue;
        private XmlTokenStream _tokenStream;

        /// <summary>
        /// Initializes an instance of <see cref="Reference"/>
        /// </summary>
        public Reference()
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="Reference"/>
        /// </summary>
        /// <param name="transform">the <see cref="Transform"/> to apply.</param>
        /// <param name="canonicalizingTransfrom">the <see cref="CanonicalizingTransfrom"/> to use.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="transform"/> is null.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="canonicalizingTransfrom"/> is null.</exception>
        public Reference(Transform transform, CanonicalizingTransfrom canonicalizingTransfrom)
        {
            if (transform == null)
                throw LogArgumentNullException(nameof(transform));

            CanonicalizingTransfrom = canonicalizingTransfrom;
            Transforms.Add(transform);
        }

        /// <summary>
        /// Gets or sets the CanonicalizingTransform
        /// </summary>
        /// <exception cref="ArgumentNullException">if 'value' is null.</exception>
        public CanonicalizingTransfrom CanonicalizingTransfrom
        {
            get => _canonicalizingTransfrom;
            set => _canonicalizingTransfrom = value ?? throw LogArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the DigestMethod to use when creating the hash.
        /// </summary>
        /// <exception cref="ArgumentNullException">if 'value' is null or empty.</exception>
        public string DigestMethod
        {
            get => _digestMethod;
            set => _digestMethod = (string.IsNullOrEmpty(value)) ? throw LogArgumentNullException(nameof(value)) : value;
        }

        /// <summary>
        /// Gets or sets the Base64 encoding of the hashed octets.
        /// </summary>
        /// <exception cref="ArgumentNullException">if 'value' is null or empty.</exception>
        public string DigestValue
        {
            get => _digestValue;
            set => _digestValue = (string.IsNullOrEmpty(value)) ? throw LogArgumentNullException(nameof(value)) : value;
        }

        /// <summary>
        /// Gets or sets the <see cref="XmlTokenStream"/> that is associated with the <see cref="DigestValue"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">if 'value' is null.</exception>
        public XmlTokenStream TokenStream
        {
            get => _tokenStream;
            set => _tokenStream = value ?? throw LogArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets the <see cref="IList{T}"/> of transforms to apply.
        /// </summary>
        public IList<Transform> Transforms
        {
            get;
        } = new List<Transform>();

        /// <summary>
        /// Gets or sets the Type of this Reference.
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Uri of this Reference.
        /// </summary>
        public string Uri
        {
            get;
            set;
        }

        /// <summary>
        /// Verifies that the <see cref="DigestValue" /> equals the hashed value of the <see cref="TokenStream"/> after
        /// <see cref="Transforms"/> have been applied.
        /// </summary>
        /// <param name="cryptoProviderFactory">supplies the <see cref="HashAlgorithm"/>.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="cryptoProviderFactory"/> is null.</exception>
        public void Verify(CryptoProviderFactory cryptoProviderFactory)
        {
            if (cryptoProviderFactory == null)
                throw LogArgumentNullException(nameof(cryptoProviderFactory));

            if (!Utility.AreEqual(ComputeDigest(cryptoProviderFactory), Convert.FromBase64String(DigestValue)))
                throw LogValidationException(LogMessages.IDX30201, Uri ?? Id);
        }

#nullable enable
        /// <summary>
        /// Verifies that the <see cref="DigestValue" /> equals the hashed value of the <see cref="TokenStream"/> after
        /// <see cref="Transforms"/> have been applied.
        /// </summary>
        /// <param name="cryptoProviderFactory">supplies the <see cref="HashAlgorithm"/>.</param>
        /// <param name="callContext"> contextual information for diagnostics.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="cryptoProviderFactory"/> is null.</exception>
        internal ValidationError? Verify(
            CryptoProviderFactory cryptoProviderFactory,
#pragma warning disable CA1801 // Review unused parameters
            CallContext callContext)
#pragma warning restore CA1801
        {
            if (cryptoProviderFactory == null)
                return ValidationError.NullParameter(nameof(cryptoProviderFactory), new System.Diagnostics.StackFrame());

            if (!Utility.AreEqual(ComputeDigest(cryptoProviderFactory), Convert.FromBase64String(DigestValue)))
                return new XmlValidationError(
                    new MessageDetail(
                        LogMessages.IDX30201,
                        Uri ?? Id),
                    ValidationFailureType.XmlValidationFailed,
                    typeof(XmlValidationException),
                    new System.Diagnostics.StackFrame());

            return null;
        }
#nullable restore

        /// <summary>
        /// Writes into a stream and then hashes the bytes.
        /// </summary>
        /// <param name="tokenStream">the set of XML nodes to read.</param>
        /// <param name="hash">the hash algorithm to apply.</param>
        /// <returns>hash of the octets.</returns>
        private static byte[] ProcessAndDigest(XmlTokenStream tokenStream, HashAlgorithm hash)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { CloseOutput = false }))
                using (var dictionaryWriter = XmlDictionaryWriter.CreateDictionaryWriter(writer))
                {
                    tokenStream.WriteTo(dictionaryWriter);
                    dictionaryWriter.Flush();
                }

                stream.Position = 0;
                return hash.ComputeHash(stream);
            }
        }

        /// <summary>
        /// Computes the digest of this reference by applying the transforms over the tokenStream.
        /// </summary>
        /// <param name="cryptoProviderFactory">the <see cref="CryptoProviderFactory"/> that will supply the <see cref="HashAlgorithm"/>.</param>
        /// <returns>The digest over the <see cref="TokenStream"/> after all transforms have been applied.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="cryptoProviderFactory"/> is null.</exception>
        /// <exception cref="XmlValidationException">if <see cref="TokenStream"/> is null.</exception>
        /// <exception cref="XmlValidationException">if <see cref="DigestMethod"/> is not supported.</exception>
        /// <exception cref="XmlValidationException">if <paramref name="cryptoProviderFactory"/>.CreateHashAlgorithm returns null.</exception>
        protected byte[] ComputeDigest(CryptoProviderFactory cryptoProviderFactory)
        {
            if (cryptoProviderFactory == null)
                throw LogArgumentNullException(nameof(cryptoProviderFactory));

            if (TokenStream == null)
                throw LogValidationException(LogMessages.IDX30202, Id);

            if (!cryptoProviderFactory.IsSupportedAlgorithm(DigestMethod))
                throw LogValidationException(LogMessages.IDX30208, cryptoProviderFactory.GetType(), DigestMethod);

            var hashAlg = cryptoProviderFactory.CreateHashAlgorithm(DigestMethod);
            if (hashAlg == null)
                throw LogValidationException(LogMessages.IDX30209, cryptoProviderFactory.GetType(), DigestMethod);

            try
            {
                // specification requires last transform to be a canonicalizing transform
                // see: https://www.w3.org/TR/2001/PR-xmldsig-core-20010820/#sec-ReferenceProcessingModel
                // - If the data object is a node-set and the next transform requires octets, the signature application 
                //   MUST attempt to convert the node-set to an octet stream using the specified canonicalization algorithm.
                for (int i = 0; i < Transforms.Count; i++)
                    TokenStream = Transforms[i].Process(TokenStream);

                if (CanonicalizingTransfrom == null)
                    return ProcessAndDigest(TokenStream, hashAlg);

                // only run canonicalizing transform if it was specified
                return CanonicalizingTransfrom.ProcessAndDigest(TokenStream, hashAlg);
            }
            finally
            {
                if (hashAlg != null)
                    cryptoProviderFactory.ReleaseHashAlgorithm(hashAlg);
            }
        }
    }
}
