using LerXML3040BancoCentral.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace LerXML3040BancoCentral.Seguranca
{
    public static class Descriptografar
    {
        private static readonly byte[] Key = new byte[16]{
                244,
                131,
                177,
                208,
                112,
                102,
                115,
                100,
                52,
                51,
                50,
                49,
                164,
                36,
                58,
                198
        };



        public static Credencial ObterCredencial(string caminhoArquivo, string codigoSistema)
        {
            Credencial credencial = new Credencial();
            StreamReader streamReader = new StreamReader(caminhoArquivo);
            string xml = String.Empty;

            if (streamReader != null)
            {
                string s = streamReader.ReadLine();
                streamReader.Close();
                byte[] array = Convert.FromBase64String(s);
                RijndaelManaged rijndaelManaged = new RijndaelManaged();
                rijndaelManaged.Padding = PaddingMode.None;
                rijndaelManaged.Mode = CipherMode.ECB;
                rijndaelManaged.Key = Key;
                ICryptoTransform transform = rijndaelManaged.CreateDecryptor();
                MemoryStream stream = new MemoryStream(array);
                CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read);
                byte[] array2 = new byte[array.Length];
                ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
                cryptoStream.Read(array2, 0, array2.Length);
                xml = aSCIIEncoding.GetString(array2).Replace("\0", "");
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);

                foreach (XmlNode childNode in xmlDocument.SelectSingleNode("//SISTEMAS").ChildNodes)
                {
                    if (childNode.Name.ToUpper() == codigoSistema.ToUpper())
                    {
                        credencial.UserName = childNode.SelectSingleNode("USUARIO").InnerText;
                        credencial.Password = childNode.SelectSingleNode("SENHA").InnerText;
                        break;
                    }

                }
            }

            return credencial;
        }

        public static List<Credencial> ObterCredenciais(string caminhoArquivo)
        {
            List<Credencial> credenciais = new List<Credencial>();
            StreamReader streamReader = new StreamReader(caminhoArquivo);
            string xml = String.Empty;


            if (streamReader != null)
            {
                string s = streamReader.ReadLine();
                streamReader.Close();
                byte[] array = Convert.FromBase64String(s);
                RijndaelManaged rijndaelManaged = new RijndaelManaged();
                rijndaelManaged.Padding = PaddingMode.None;
                rijndaelManaged.Mode = CipherMode.ECB;
                rijndaelManaged.Key = Key;
                ICryptoTransform transform = rijndaelManaged.CreateDecryptor();
                MemoryStream stream = new MemoryStream(array);
                CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read);
                byte[] array2 = new byte[array.Length];
                ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
                cryptoStream.Read(array2, 0, array2.Length);
                xml = aSCIIEncoding.GetString(array2).Replace("\0", "");
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);

                foreach (XmlNode childNode in xmlDocument.SelectSingleNode("//SISTEMAS").ChildNodes)
                {
                    credenciais.Add(new Credencial()
                    {
                        CodSistema = childNode.Name.ToUpper(),
                        UserName = childNode.SelectSingleNode("USUARIO").InnerText,
                        Password = childNode.SelectSingleNode("SENHA").InnerText,
                    }
                    );
                }
            }

            return credenciais;
        }

    }
}
