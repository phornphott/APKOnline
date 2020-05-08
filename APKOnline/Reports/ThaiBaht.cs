using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ThaiBaht
/// </summary>
public class ThaiBaht
{
    //public ThaiBaht()
    //{
        public static string ThaiBaht1(string txt)
        {
            string bahtTxt, n, bahtTH = "";
            double amount;
            try
            {
                amount = Convert.ToDouble(txt.Replace(",",""));
            }
            catch
            {
                amount = 0;
            }
            bahtTxt = amount.ToString("####.00");
            string[] num = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };
            string[] rank = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน", "สิบล้าน", "ร้อยล้าน", "พันล้าน", "หมื่นล้าน", "แสนล้าน" };
            string[] temp = bahtTxt.Split('.');
            string intVal = temp[0];
            string decVal = temp[1];
            if (Convert.ToDouble(bahtTxt) == 0)
            {
                bahtTH = "ศูนย์บาทถ้วน";
            }
            else
            {
                for (int i = 0; i < intVal.Length; i++)
                {
                    n = intVal.Substring(i, 1);
                    if (n != "0")
                    {
                        if ((i == (intVal.Length - 1)) && (n == "1"))
                        {
                            bahtTH += "เอ็ด";
                        }
                        else if ((i == (intVal.Length - 2)) && (n == "2"))
                        {
                            bahtTH += "ยี่สิบ";
                        }
                        else if ((i == (intVal.Length - 2)) && (n == "1"))
                        {
                            bahtTH += "สิบ";
                        }
                        else
                        {
                            bahtTH += num[Convert.ToInt32(n)];
                            bahtTH += rank[(intVal.Length - i) - 1];
                        }
                    }
                }
                bahtTH += "บาท";
                if (decVal == "00")
                {
                    bahtTH += "ถ้วน";
                }
                else
                {
                    for (int i = 0; i < decVal.Length; i++)
                    {
                        n = decVal.Substring(i, 1);
                        if (n != "0")
                        {
                            if ((i == decVal.Length - 1) && (n == "1"))
                            {
                                bahtTH += "เอ็ด";
                            }
                            else if ((i == (decVal.Length - 2)) && (n == "2"))
                            {
                                bahtTH += "ยี่สิบ";
                            }
                            else if ((i == (decVal.Length - 2)) && (n == "1"))
                            {
                                bahtTH += "สิบ";
                            }
                            else
                            {
                                bahtTH += num[Convert.ToInt32(n)];
                                bahtTH += rank[(decVal.Length - i) - 1];
                            }
                        }
                    }
                    bahtTH += "สตางค์";
                }
            }
            string b = bahtTH.Substring(0, 4);
            if (b == "เอ็ด")
            {
                bahtTH = bahtTH.Replace("เอ็ด", "หนึ่ง");
            }
            return bahtTH;  
	    }
    //}
}