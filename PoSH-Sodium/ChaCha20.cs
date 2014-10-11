﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sodium;
using System.Management.Automation;

namespace PoSH_Sodium
{
    [Cmdlet("Encrypt", "ChaChaMessage")]
    public class ChaChaEncrypt : PSCmdlet
    {
        protected override void BeginProcessing()
        {
            rawMessage = Message.ToByteArray(Encoding);
        }

        protected override void ProcessRecord()
        {
            var nonce = StreamEncryption.GenerateNonceChaCha20();
            var encryptedMessage = StreamEncryption.EncryptChaCha20(rawMessage, nonce, Key);
            if (Raw.IsTrue())
            {
                var result = new RawEncryptedMessage() { Message = encryptedMessage, Nonce = nonce };
                WriteObject(result);
            }
            else
            {
                var result = new EncryptedMessage() { Message = encryptedMessage.Compress(), Nonce = nonce };
                WriteObject(result);
            }
        }

        private byte[] rawMessage;

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Message to be encrypted")]
        public string Message;

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            Position = 1,
            HelpMessage = "Symmetric key to encrypt the message with")]
        public byte[] Key;

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            Position = 2,
            HelpMessage = "Output is returned as a byte array, otherwise an LZ4 compressed base64 encoded string is returned")]
        public SwitchParameter Raw;

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            Position = 3,
            HelpMessage = "Encoding to use when converting the message to a byte array.  Default is .NET Unicode (UTF16)")]
        [ValidateSet("UTF7", "UTF8", "UTF16", "UTF32", "ASCII", "Unicode", "BigEndianUnicode")]
        public string Encoding;
    }

    [Cmdlet("Decrypt", "ChaChaMessage")]
    public class ChaChaDecrypt : PSCmdlet
    {
        protected override void BeginProcessing()
        {
            rawMessage = Message.Decompress();
        }

        protected override void ProcessRecord()
        {
            byte[] message;
            message = StreamEncryption.DecryptChaCha20(rawMessage, Nonce, Key);
            if (Raw.IsTrue())
            {
                WriteObject(message);
            }
            else
            {
                var plainMessage = message.ToString(Encoding);
                WriteObject(plainMessage);
            }
        }

        private byte[] rawMessage;

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Message to be decrypted")]
        public string Message;

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            Position = 1,
            HelpMessage = "Nonce to decrypt message with")]
        public byte[] Nonce;

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            Position = 2,
            HelpMessage = "Symmetric key to decrypt the message with")]
        public byte[] Key;

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            Position = 3,
            HelpMessage = "Output is returned as a byte array, otherwise an LZ4 compressed base64 encoded string is returned")]
        public SwitchParameter Raw;

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            Position = 4,
            HelpMessage = "Encoding to use when converting the message to a byte array.  Default is .NET Unicode (UTF16)")]
        [ValidateSet("UTF7", "UTF8", "UTF16", "UTF32", "ASCII", "Unicode", "BigEndianUnicode")]
        public string Encoding;
    }

    [Cmdlet("Decrypt", "RawChaChaMessage")]
    public class RawChaChaDecrypt : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            byte[] message;
            message = StreamEncryption.DecryptChaCha20(Message, Nonce, Key);
            if (Raw.IsTrue())
            {
                WriteObject(message);
            }
            else
            {
                var plainMessage = message.ToString(Encoding);
                WriteObject(plainMessage);
            }
        }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Message to be decrypted")]
        public byte[] Message;

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            Position = 1,
            HelpMessage = "Nonce to decrypt message with")]
        public byte[] Nonce;

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            Position = 2,
            HelpMessage = "Symmetric key to decrypt the message with")]
        public byte[] Key;

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            Position = 3,
            HelpMessage = "Output is returned as a byte array, otherwise an LZ4 compressed base64 encoded string is returned")]
        public SwitchParameter Raw;

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            Position = 4,
            HelpMessage = "Encoding to use when converting the message to a byte array.  Default is .NET Unicode (UTF16)")]
        [ValidateSet("UTF7", "UTF8", "UTF16", "UTF32", "ASCII", "Unicode", "BigEndianUnicode")]
        public string Encoding;
    }
}