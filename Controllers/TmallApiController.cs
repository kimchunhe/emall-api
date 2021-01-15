using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Controllers
{
    [ApiController]
    [Route("api/tmall")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class TmallApiController : ControllerBase
    {
        static readonly HttpClient httpClient = new HttpClient();
        static readonly string serverUrl = "http://gw.api.taobao.com/router/rest";
        [HttpGet]
        [Route("tradebycreated")]
        public async Task<ActionResult> GetTradesByCreated(string appKey, string appSecret, string accessToken, string startDate, string endDate, string pageNo, string pageSize)
        {
            // string serverUrl = "http://gw.api.taobao.com/router/rest";
            
            if(string.IsNullOrEmpty(pageNo)){
                pageNo = "1";
            }
            if(string.IsNullOrWhiteSpace(pageSize)){
                pageSize = "5";
            }
            string method = "taobao.trades.sold.get";
            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("fields", "tid,status,freight,payment,orders,promotion_details");
            param.Add("start_created", startDate);
            param.Add("end_created", endDate);
            param.Add("page_no", pageNo);
            param.Add("page_size", pageSize);
            IDictionary<string, string> result = util.TmallHttpUtil.buildParamMap(param, method, appKey, appSecret, accessToken);
            string queryparam = generateReqString(result);
            
            HttpResponseMessage response = await httpClient.GetAsync(serverUrl+"?"+queryparam);
            response.EnsureSuccessStatusCode();
            string tradeInfo = await response.Content.ReadAsStringAsync();
            // JObject jObject = JObject.Parse(data1);
            return Ok(tradeInfo);
        }
        private string generateReqString(IDictionary<string, string> param) {
            if(param == null) {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            foreach(var entry in param) {
                sb.Append("&").Append(entry.Key).Append("=").Append(entry.Value);
            }
            return sb.ToString().Substring(1);
        }
    }
}