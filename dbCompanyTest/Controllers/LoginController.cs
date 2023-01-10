using dbCompanyTest.Models;
using dbCompanyTest.ViewModels;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace dbCompanyTest.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult checkLogin(string account, string password)
        {
            dbCompanyTestContext db = new dbCompanyTestContext();
            if (password == null)
            {
                return Content("Nopassword");
            }
            var a = db.TestClients.FirstOrDefault(c => c.Email == account && c.密碼 == password);
            if (a != null)
            {
                useSession(a);
                return Content("成功");
            }
            else
            {
                return Content("失敗");
            }
        }

        public IActionResult loginSussess()
        {
            string? formCredential = Request.Form["credential"]; //回傳憑證
            string? formToken = Request.Form["g_csrf_token"]; //回傳令牌
            string? cookiesToken = Request.Cookies["g_csrf_token"]; //Cookie 令牌

            // 驗證 Google Token
            GoogleJsonWebSignature.Payload? payload = VerifyGoogleToken(formCredential, formToken, cookiesToken).Result;
            if (payload == null)
            {
                // 驗證失敗
                //ViewData["Msg"] = "驗證 Google 授權失敗";
                return RedirectToAction("Login");
            }
            else
            {
                dbCompanyTestContext db = new dbCompanyTestContext();
                if (db.TestClients.FirstOrDefault(c => c.Email == payload.Email) == null)
                {
                    TestClient newClient = new TestClient();
                    newClient.客戶編號 = $"CLG-{payload.JwtId.Substring(0, 7)}";
                    newClient.Email = payload.Email;
                    newClient.客戶姓名 = payload.Name;
                    db.TestClients.Add(newClient);
                    db.SaveChanges();
                }
                //驗證成功，取使用者資訊內容
                //ViewData["Msg"] = "驗證 Google 授權成功" + "<br>";
                //ViewData["Msg"] += "Email:" + payload.Email + "<br>";
                //ViewData["Msg"] += "Name:" + payload.Name + "<br>";
                //ViewData["Msg"] += "Picture:" + payload.Picture;
                return RedirectToAction("index", "Home");
            }
        }
        public async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(string? formCredential, string? formToken, string? cookiesToken)
        {
            // 檢查空值
            if (formCredential == null || formToken == null && cookiesToken == null)
            {
                return null;
            }

            GoogleJsonWebSignature.Payload? payload;
            try
            {
                // 驗證 token
                if (formToken != cookiesToken)
                {
                    return null;
                }

                // 驗證憑證
                IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
                string GoogleClientId = Config.GetSection("GoogleClientId").Value;
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { GoogleClientId }
                };
                payload = await GoogleJsonWebSignature.ValidateAsync(formCredential, settings);
                if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals("https://accounts.google.com"))
                {
                    return null;
                }
                if (payload.ExpirationTimeSeconds == null)
                {
                    return null;
                }
                else
                {
                    DateTime now = DateTime.Now.ToUniversalTime();
                    DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
                    if (now > expiration)
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
            return payload;
        }

        public void useSession(TestClient x)
        {
            if (!HttpContext.Session.Keys.Contains(CDittionary.SK_USE_FOR_LOGIN_USER_SESSION))
            {
                string json = JsonSerializer.Serialize(x);
                HttpContext.Session.SetString(CDittionary.SK_USE_FOR_LOGIN_USER_SESSION, json);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(CDittionary.SK_USE_FOR_LOGIN_USER_SESSION);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult getUser()
        {
            string json = HttpContext.Session.GetString(CDittionary.SK_USE_FOR_LOGIN_USER_SESSION);
            string userName = "";
            if (json == null)
                userName = "訪客";
            else
            {
                TestClient x = JsonSerializer.Deserialize<TestClient>(json);
                userName = x.客戶姓名;
            }
            return Content(userName);
        }
    }
}
