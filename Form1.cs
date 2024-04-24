using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace YourNamespace
{
    public partial class Form1 : Form
    {
        private Dictionary<string, char> cityCodeMapping = new Dictionary<string, char>()
        {
            {"臺北市", 'A'},
            {"臺中市", 'B'},
            {"基隆市", 'C'},
            {"臺南市", 'D'},
            {"高雄市", 'E'},
            {"新北市", 'F'}, 
            {"宜蘭縣", 'G'},
            {"桃園市", 'H'}, 
            {"新竹縣", 'J'},
            {"苗栗縣", 'K'},
            {"臺中縣", 'L'},
            {"南投縣", 'M'},
            {"彰化縣", 'N'},
            {"雲林縣", 'P'},
            {"嘉義縣", 'Q'},
            {"臺南縣", 'R'},
            {"高雄縣", 'S'},
            {"屏東縣", 'T'},
            {"花蓮縣", 'U'},
            {"臺東縣", 'V'},
            {"澎湖縣", 'X'},
            {"陽明山", 'Y'},
            {"金門縣", 'W'},
            {"連江縣", 'Z'},
            {"嘉義市", 'I'},
            {"新竹市", 'O'}
        };

        public Form1()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string idNumber = GenerateIDNumber();
            MessageBox.Show($"您的身分證字號是：{idNumber}", "身分證字號生成器");

            // 驗證生成的身分證
        //    VerifyIDNumber(idNumber);
        }

        private string GenerateIDNumber()
        {
            Random random = new Random();
            int index = random.Next(cityCodeMapping.Count);
            var city = cityCodeMapping.ElementAt(index);

            int gender = random.Next(1, 3);

            string randomNumber = "";
            for (int i = 0; i < 7; i++)
            {
                randomNumber += random.Next(3, 10);
            }

            string genderMsg = (gender == 1) ? "男性" : "女性";
            string idNumber = $"{city.Value}{gender}{randomNumber}";

            // 添加最後一個驗證碼
            int step1 = EngToNum(idNumber[0]);
            int step2 = step1 % 10 * 9 + step1 / 10;
            int step3 = 0;
            int now = 8;
            for (int i = 1; i < idNumber.Length; i++)
            {
                step3 += (int)(idNumber[i] - '0') * now;
                now--;
            }
            int step4 = step2 + step3;
            int verifyCode = (10 - (step4 % 10)) % 10;
            idNumber += verifyCode;

            return $"{idNumber} ({city.Key}, {genderMsg})";
        }

        private void VerifyIDNumber(string idNumber)
        {
            int step1 = EngToNum(idNumber[0]);
            int step2 = step1 % 10 * 9 + step1 / 10;
            int step3 = 0;
            int now = 8;
            for (int i = 1; i < idNumber.Length - 1; i++)
            {
                step3 += (int)(idNumber[i] - '0') * now;
                now--;
            }
            int step4 = step2 + step3 + (idNumber[idNumber.Length - 1] - '0');
            if (step4 % 10 == 0)
            {
                char genderChar = idNumber[1];
                string gender = (genderChar == '1') ? "男性" : "女性";
                string city = cityCodeMapping.FirstOrDefault(x => x.Value == idNumber[0]).Key;
                MessageBox.Show($"正確 {gender} {city}", "身分證字號驗證");
            }
            else
            {
                MessageBox.Show("錯誤", "身分證字號驗證");
            }
        }

        private int EngToNum(char c)
        {
            int res = 0;
            if ('A' <= c && c <= 'H') res = c - 'A' + 10;
            else if (c == 'I') res = 34;
            else if ('J' <= c && c <= 'N') res = c - 'J' + 18;
            else if (c == 'O') res = 35;
            else if ('P' <= c && c <= 'V') res = c - 'P' + 23;
            else if (c == 'W') res = 32;
            else if (c == 'X') res = 30;
            else if (c == 'Y') res = 31;
            else if (c == 'Z') res = 33;
            return res;
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            string idNumber = txtIDNumber.Text;

            // 檢查使用者是否有輸入資料
            if (string.IsNullOrWhiteSpace(idNumber))
            {
                MessageBox.Show("尚未輸入資料", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // 結束函式，不執行後面的驗證程式碼
            }

            VerifyIDNumber(idNumber);
        }
    }
}
