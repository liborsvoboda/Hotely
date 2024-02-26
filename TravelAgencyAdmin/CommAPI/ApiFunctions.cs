using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UbytkacAdmin.GlobalClasses;
using UbytkacAdmin.GlobalOperations;

namespace UbytkacAdmin.Api {

    internal class CommApi {

        public static async Task<Authentification> Authentification(ApiUrls apiUrl, string userName = null, string password = null) {
            using (HttpClient httpClient = new HttpClient()) {
                try {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(userName + ":" + password)));
                    StringContent test = new StringContent("", Encoding.UTF8, "application/json");
                    HttpResponseMessage json = await httpClient.PostAsync(App.appRuntimeData.AppClientSettings.First(b => b.Key == "conn_apiAddress").Value + "/" + apiUrl, test);
                    return JsonConvert.DeserializeObject<Authentification>(await json.Content.ReadAsStringAsync());
                } catch { return new Authentification() { Token = null, Expiration = null }; }
            }
        }

        public static async Task<T> GetApiRequest<T>(ApiUrls apiUrl, string key = null, string token = null) where T : new() {
            string json;
            using (HttpClient httpClient = new HttpClient()) {
                try {
                    if (token != null) { httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); }
                    json = await httpClient.GetStringAsync(App.appRuntimeData.AppClientSettings.First(b => b.Key == "conn_apiAddress").Value + "/" + apiUrl + (!string.IsNullOrWhiteSpace(key) ? "/" + key : ""));
                    return JsonConvert.DeserializeObject<T>(json);
                } catch (Exception ex) {
                    if (ex.Message.Contains("401 (Unauthorized)")) {
                        _ = await MainWindow.ShowMessageOnMainWindow(false, await DBOperations.DBTranslation("connectionWasDisconnected"), false);
                        ((MainWindow)App.Current.MainWindow).ShowLoginDialog();
                    }
                    return new T();
                }
            }
        }

        public static async Task<DBResultMessage> PostApiRequest(ApiUrls apiUrl, HttpContent body, string key = null, string token = null) {
            using (HttpClient httpClient = new HttpClient()) {
                DBResultMessage result;
                try {
                    if (token != null) { httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); }
                    HttpResponseMessage json = await httpClient.PostAsync(App.appRuntimeData.AppClientSettings.First(b => b.Key == "conn_apiAddress").Value + "/" + apiUrl + (!string.IsNullOrWhiteSpace(key) ? "/" + key : ""), body);
                    result = JsonConvert.DeserializeObject<DBResultMessage>(await json.Content.ReadAsStringAsync());
                    if (result != null && result.ErrorMessage == null) { result.ErrorMessage = await json.Content.ReadAsStringAsync(); }
                    else if (result == null && json.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                        _ = await MainWindow.ShowMessageOnMainWindow(false, await DBOperations.DBTranslation("connectionWasDisconnected"), false);
                        ((MainWindow)App.Current.MainWindow).ShowLoginDialog();
                        result = new DBResultMessage() { RecordCount = 0, ErrorMessage = System.Net.HttpStatusCode.Unauthorized.ToString(), Status = "error" };
                    }
                } catch (Exception ex) { result = new DBResultMessage() { RecordCount = 0, ErrorMessage = ex.Message + Environment.NewLine + ex.StackTrace, Status = "error" }; }
                return result;
            }
        }

        public static async Task<DBResultMessage> PutApiRequest(ApiUrls apiUrl, HttpContent body, string key = null, string token = null) {
            using (HttpClient httpClient = new HttpClient()) {
                DBResultMessage result;
                try {
                    if (token != null) { httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); }
                    HttpResponseMessage json = await httpClient.PutAsync(App.appRuntimeData.AppClientSettings.First(b => b.Key == "conn_apiAddress").Value + "/" + apiUrl + (!string.IsNullOrWhiteSpace(key) ? "/" + key : ""), body);
                    result = JsonConvert.DeserializeObject<DBResultMessage>(await json.Content.ReadAsStringAsync());
                    if (result != null && result.ErrorMessage == null) { result.ErrorMessage = await json.Content.ReadAsStringAsync(); }
                    else if (result == null && json.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                        _ = await MainWindow.ShowMessageOnMainWindow(false, await DBOperations.DBTranslation("connectionWasDisconnected"), false);
                        ((MainWindow)App.Current.MainWindow).ShowLoginDialog();
                        result = new DBResultMessage() { RecordCount = 0, ErrorMessage = System.Net.HttpStatusCode.Unauthorized.ToString(), Status = "error" };
                    }
                } catch (Exception ex) { result = new DBResultMessage() { RecordCount = 0, ErrorMessage = ex.Message + Environment.NewLine + ex.StackTrace, Status = "error" }; }
                return result;
            }
        }

        public static async Task<DBResultMessage> DeleteApiRequest(ApiUrls apiUrl, string key = null, string token = null) {
            using (HttpClient httpClient = new HttpClient()) {
                DBResultMessage result;
                try {
                    if (token != null) { httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); }
                    HttpResponseMessage json = await httpClient.DeleteAsync(App.appRuntimeData.AppClientSettings.First(b => b.Key == "conn_apiAddress").Value + "/" + apiUrl + (!string.IsNullOrWhiteSpace(key) ? "/" + key : ""));
                    result = JsonConvert.DeserializeObject<DBResultMessage>(await json.Content.ReadAsStringAsync());
                    if (result != null && result.ErrorMessage == null) { result.ErrorMessage = await json.Content.ReadAsStringAsync(); }
                    else if (result == null && json.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                        _ = await MainWindow.ShowMessageOnMainWindow(false, await DBOperations.DBTranslation("connectionWasDisconnected"), false);
                        ((MainWindow)App.Current.MainWindow).ShowLoginDialog();
                        result = new DBResultMessage() { RecordCount = 0, ErrorMessage = System.Net.HttpStatusCode.Unauthorized.ToString(), Status = "error" };
                    }
                } catch (Exception ex) { result = new DBResultMessage() { RecordCount = 0, ErrorMessage = ex.Message + Environment.NewLine + ex.StackTrace, Status = "error" }; }
                return result;
            }
        }

        public static async Task<bool> CheckApiConnection() {
            using (HttpClient httpClient = new HttpClient()) {
                try {
                    string json = await httpClient.GetStringAsync(App.appRuntimeData.AppClientSettings.First(b => b.Key == "conn_apiAddress").Value + "/" + ApiUrls.BackendCheck);
                    return true;
                } catch { return false; }
            }
        }
    }
}