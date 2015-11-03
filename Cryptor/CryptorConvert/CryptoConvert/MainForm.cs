using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security;
using System.Security.Cryptography;

namespace CryptoConvert
{
    public partial class MainForm : Form
    {
        byte[] buffer;
        DESCryptoServiceProvider DesCSP = new DESCryptoServiceProvider();

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] palindata = Encoding.Default.GetBytes(txtyuan.Text);
            byte[] encryptdata = md5.ComputeHash(palindata);
            txtjiami.Text = Convert.ToBase64String(encryptdata);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            CryptoStream cryStream = new CryptoStream(ms, DesCSP.CreateEncryptor(), CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(cryStream);
            sw.WriteLine(txtyuan.Text);
            sw.Close();
            cryStream.Close();
            buffer = ms.ToArray();
            txtjiami.Text = Convert.ToBase64String(buffer);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream(buffer);
            CryptoStream cryStream = new CryptoStream(ms, DesCSP.CreateDecryptor(), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cryStream);
            txthjiemi.Text = sr.ReadLine();
            sr.Close();
            cryStream.Close();
            ms.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CspParameters param = new CspParameters();

            param.KeyContainerName = "Clark";//密匙容器的名称，保持加密解密一致才能解密成功

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {

                byte[] plaindata = Encoding.Default.GetBytes(txtyuan.Text);//将要加密的字符串转换为字节数组

                byte[] encryptdata = rsa.Encrypt(plaindata, false);//将加密后的字节数据转换为新的加密字节数组

                txtjiami.Text = Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为字符串

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CspParameters param = new CspParameters();

            param.KeyContainerName = "Clark";

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {

                byte[] encryptdata = Convert.FromBase64String(this.txtjiami.Text);

                byte[] decryptdata = rsa.Decrypt(encryptdata, false);

                txthjiemi.Text = Encoding.Default.GetString(decryptdata);

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Keys(@"D:\PublicKey.xml", @"D:\PrivateKey.xml");
        }

        static void Keys(string publicKeyFileName, string privateKeyFileName)
        {
            // Variables
            CspParameters cspParams = null;
            RSACryptoServiceProvider rsaProvider = null;
            StreamWriter publicKeyFile = null;
            StreamWriter privateKeyFile = null;
            string publicKey = "";
            string privateKey = "";

            try
            {
                // Create a new key pair on target CSP
                cspParams = new CspParameters();
                cspParams.ProviderType = 1; // PROV_RSA_FULL 
                //cspParams.ProviderName; // CSP name
                cspParams.Flags = CspProviderFlags.UseArchivableKey;
                cspParams.KeyNumber = (int)KeyNumber.Exchange;
                rsaProvider = new RSACryptoServiceProvider(cspParams);

                // Export public key
                publicKey = rsaProvider.ToXmlString(false);

                // Write public key to file
                publicKeyFile = File.CreateText(publicKeyFileName);
                publicKeyFile.Write(publicKey);

                // Export private/public key pair 
                privateKey = rsaProvider.ToXmlString(true);

                // Write private/public key pair to file
                privateKeyFile = File.CreateText(privateKeyFileName);
                privateKeyFile.Write(privateKey);
            }
            catch (Exception ex)
            {
                // Any errors? Show them
                Console.WriteLine("Exception generating a new key pair! More info:");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Do some clean up if needed
                if (publicKeyFile != null)
                {
                    publicKeyFile.Close();
                }
                if (privateKeyFile != null)
                {
                    privateKeyFile.Close();
                }
            }

        } // Keys

        // Encrypt a file
        static void Encrypt(string publicKeyFileName, string plainFileName, string encryptedFileName)
        {
            // Variables
            CspParameters cspParams = null;
            RSACryptoServiceProvider rsaProvider = null;
            StreamReader publicKeyFile = null;
            StreamReader plainFile = null;
            FileStream encryptedFile = null;
            string publicKeyText = "";
            string plainText = "";
            byte[] plainBytes = null;
            byte[] encryptedBytes = null;

            try
            {
                // Select target CSP
                cspParams = new CspParameters();
                cspParams.ProviderType = 1; // PROV_RSA_FULL 
                //cspParams.ProviderName; // CSP name
                rsaProvider = new RSACryptoServiceProvider(cspParams);

                // Read public key from file
                publicKeyFile = File.OpenText(publicKeyFileName);
                publicKeyText = publicKeyFile.ReadToEnd();

                // Import public key
                rsaProvider.FromXmlString(publicKeyText);

                // Read plain text from file
                plainFile = File.OpenText(plainFileName);
                plainText = plainFile.ReadToEnd();

                // Encrypt plain text
                plainBytes = Encoding.Unicode.GetBytes(plainText);
                encryptedBytes = rsaProvider.Encrypt(plainBytes, false);

                // Write encrypted text to file
                encryptedFile = File.Create(encryptedFileName);
                encryptedFile.Write(encryptedBytes, 0, encryptedBytes.Length);
            }
            catch (Exception ex)
            {
                // Any errors? Show them
                Console.WriteLine("Exception encrypting file! More info:");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Do some clean up if needed
                if (publicKeyFile != null)
                {
                    publicKeyFile.Close();
                }
                if (plainFile != null)
                {
                    plainFile.Close();
                }
                if (encryptedFile != null)
                {
                    encryptedFile.Close();
                }
            }

        } // Encrypt

        // Decrypt a file
        static void Decrypt(string privateKeyFileName, string encryptedFileName, string plainFileName)
        {
            // Variables
            CspParameters cspParams = null;
            RSACryptoServiceProvider rsaProvider = null;
            StreamReader privateKeyFile = null;
            FileStream encryptedFile = null;
            StreamWriter plainFile = null;
            string privateKeyText = "";
            string plainText = "";
            byte[] encryptedBytes = null;
            byte[] plainBytes = null;

            try
            {
                // Select target CSP
                cspParams = new CspParameters();
                cspParams.ProviderType = 1; // PROV_RSA_FULL 
                //cspParams.ProviderName; // CSP name
                rsaProvider = new RSACryptoServiceProvider(cspParams);

                // Read private/public key pair from file
                privateKeyFile = File.OpenText(privateKeyFileName);
                privateKeyText = privateKeyFile.ReadToEnd();

                // Import private/public key pair
                rsaProvider.FromXmlString(privateKeyText);

                // Read encrypted text from file
                encryptedFile = File.OpenRead(encryptedFileName);
                encryptedBytes = new byte[encryptedFile.Length];
                encryptedFile.Read(encryptedBytes, 0, (int)encryptedFile.Length);

                // Decrypt text
                plainBytes = rsaProvider.Decrypt(encryptedBytes, false);

                // Write decrypted text to file
                plainFile = File.CreateText(plainFileName);
                plainText = Encoding.Unicode.GetString(plainBytes);
                plainFile.Write(plainText);
            }
            catch (Exception ex)
            {
                // Any errors? Show them
                Console.WriteLine("Exception decrypting file! More info:");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Do some clean up if needed
                if (privateKeyFile != null)
                {
                    privateKeyFile.Close();
                }
                if (encryptedFile != null)
                {
                    encryptedFile.Close();
                }
                if (plainFile != null)
                {
                    plainFile.Close();
                }
            }

        } // Decrypt

    }
}
