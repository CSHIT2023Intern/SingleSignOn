using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace SingleSignOn
{
    public class ADHelper
    {
        public static bool ValidateUser(string username, string password)
        {
            bool result = false;
            string adConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ADConnectionString"].ConnectionString;
            DirectoryEntry entry = new DirectoryEntry(adConnectionString, username, password);

            try
            {
                // 嘗試在AD中取得使用者資訊
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult sr = search.FindOne();

                if (sr != null)
                {
                    result = true;
                    // 取得使用者資訊
                    string displayName = sr.Properties["cn"][0].ToString();
                    // 在此處新增其他使用者資訊
                }
            }
            catch (Exception ex)
            {
                // 向使用者顯示例外訊息
                ShowErrorMessageToUser($"An error occurred: {ex.Message}");
            }
            finally
            {
                entry.Close();
            }
            return result;
        }

        public static void ShowErrorMessageToUser(string message)
        {
            string script = $@"<script type=""text/javascript"">
                        alert('{message}');
                    </script>";
            Page page = HttpContext.Current.CurrentHandler as Page;
            page.ClientScript.RegisterStartupScript(typeof(Page), "ErrorMessage", script);
        }
    }
}