using System.Net.Http;
using Newtonsoft.Json;
using dotenv.net;
using System.Text;
class OpenAIservice 
{   
    private string apiKey;
    private string text;
    const string URL = "https://api.openai.com/v1/chat/completions";
    private static HttpClient client = new HttpClient(); // initialize HttpClinet
    const string Prompt = "Summarize this: ";
    const string Role = "user";
    const string Model = "gpt-3.5-turbo-0125";

    public OpenAIservice(string text) {
        DotEnv.Load();
        apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY"); //get the openAI API key. 
        this.text = text;
    }

    //create the request object 
    private Request createRequest(){
        var request =  new Request{
            model = Model,
            messages = new List<Message> {
                new Message{
                    role = Role,
                    content = Prompt + text
                }
            }
        };
        return request;
    }

    //serialize to json and add headers and get the summary from OpenAI
    private async Task<string> sendRequest() {
        var jsonContent = JsonConvert.SerializeObject(createRequest());
        var request = new HttpRequestMessage(); //make the headers
        request.Method = HttpMethod.Post;
        request.RequestUri = new Uri(URL);
        request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        request.Headers.Add("Authorization", $"Bearer {apiKey}");
        var response = await client.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();
        return body;
    }

    private async Task<string> deserializeResponse() {
        var jsonString = await sendRequest();
        var deserializedResponse = JsonConvert.DeserializeObject<GPTResponse>(jsonString);
        return deserializedResponse.choices[0].message.content;
    }
    public async Task<string> getSummary(){
        return await deserializeResponse();
    }
}