//======================================================================
//
//        Copyright (C) 2020-2021 个人软件工作室    
//        All rights reserved
//
//        filename :SignXmlHelper.cs
//        description :
//
//        created by 张恭亮 at  2020/10/27 17:53:11
//
//======================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FastAutomationFrame.Diagram
{
    public class SignXmlHelper
    {
        public static void SignXml(string xmlPath)
        {
            try
            {
                CspParameters cspParams = new CspParameters();
                cspParams.KeyContainerName = "XML_DSIG_RSA_KEY";
                RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider(cspParams);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load(xmlPath);
                SignXml(xmlDoc, rsaKey);
                xmlDoc.Save(xmlPath);
            }
            catch (Exception e)
            {
            }
        }

        public static void SignXml(XmlDocument xmlDoc, RSA rsaKey)
        {
            if (xmlDoc == null)
                throw new ArgumentException(nameof(xmlDoc));
            if (rsaKey == null)
                throw new ArgumentException(nameof(rsaKey));

            SignedXml signedXml = new SignedXml(xmlDoc);
            signedXml.SigningKey = rsaKey;
            Reference reference = new Reference();
            reference.Uri = "";
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);
            signedXml.AddReference(reference);
            signedXml.ComputeSignature();
            XmlElement xmlDigitalSignature = signedXml.GetXml();
            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
        }

        public static bool VerifyXml(string xmlPath)
        {
            try
            {
                CspParameters cspParams = new CspParameters();
                cspParams.KeyContainerName = "XML_DSIG_RSA_KEY";
                RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider(cspParams);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load(xmlPath);
                return VerifyXml(xmlDoc, rsaKey);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool VerifyXml(XmlDocument xmlDoc, RSA key)
        {
            if (xmlDoc == null)
                throw new ArgumentException("xmlDoc");
            if (key == null)
                throw new ArgumentException("key");

            SignedXml signedXml = new SignedXml(xmlDoc);
            XmlNodeList nodeList = xmlDoc.GetElementsByTagName("Signature");
            if (nodeList.Count <= 0)
            {
                return false;
            }

            if (nodeList.Count >= 2)
            {
                return false;
            }

            signedXml.LoadXml((XmlElement)nodeList[0]);
            return signedXml.CheckSignature(key);
        }
    }
}
