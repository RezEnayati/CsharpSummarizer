class Request
{
    public string model {get; set;}
    public List<Message> messages {get; set;}

}

class Message
{
    public string role {get; set;}
    public string content {get; set;}
}

class GPTResponse 
{
    public List<Choice> choices {get; set;}
}

class Choice
{
    public Message message {get; set;}
}