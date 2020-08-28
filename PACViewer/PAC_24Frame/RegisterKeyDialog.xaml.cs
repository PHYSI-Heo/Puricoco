﻿using Newtonsoft.Json.Linq;
using PAC_24Frame.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PAC_24Frame
{
    /// <summary>
    /// RegisterWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RegisterKeyDialog : Window
    {
        private HTTPClient httpClient;

        public RegisterKeyDialog()
        {
            InitializeComponent();
            InitializeObject();
        }

        private void InitializeObject()
        {
            httpClient = new HTTPClient();
        }

        private void Btn_Register_Click(object sender, RoutedEventArgs e)
        {
            string code = Tb_ProductCode.Text;
            if(code == null || code.Length != 6)
            {
                ShowErrorMessage("제품코드(6 자리)를 입력하세요.");
                return;
            }
            Btn_Create.IsEnabled = Btn_Register.IsEnabled = false;
            RequestRegisterKey(code);
            Btn_Create.IsEnabled = Btn_Register.IsEnabled = true;
        }

        private void Btn_Create_Click(object sender, RoutedEventArgs e)
        {
            Tb_ProductCode.Text = GetRandomCode(6);
        }

        public string GetRandomCode(int length)
        {
            Random rand = new Random();
            string input = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var chars = Enumerable.Range(0, length).Select(x => input[rand.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }


        private async void RequestRegisterKey(string code)
        {
            try
            {
                JObject paramObj = new JObject();
                paramObj.Add(HTTPPacket.Param_Device_ID, code);
                JObject obj = await httpClient.Requester(HTTPPacket.Url_Register_ID, "POST", paramObj);           
                
                if(obj != null)
                {
                    string resCode = obj.GetValue(HTTPPacket.Param_Res_Result).ToString();
                    Console.WriteLine("(Register) Register result = {0}", obj.ToString());
                    if (resCode.Equals(HTTPPacket.Res_Success))
                    {
                        SystemEnv.SetProductKey(code);
                        File.WriteAllText(SystemEnv.DEVICE_ID_FILE, code);
                        this.DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        ShowErrorMessage("제품 등록에 실패하였습니다.");
                    }
                }
                else
                {
                    ShowErrorMessage("서버와 연결이 불안정합니다. 잠시 후 다시 시도해주세요.");
                }               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ShowErrorMessage("서버와 연결이 불안정합니다. 잠시 후 다시 시도해주세요.");
            }
        }

        private void ShowErrorMessage(string message)
        {
            Tb_Explain.Text = message;
            Tb_Explain.Foreground = Brushes.Red;
        }
    }
}
