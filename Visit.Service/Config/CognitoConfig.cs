namespace Visit.Service.Config
{ 
//  Config must be in following format
//    "AWS": {
//        "Region": "",
//        "UserPoolClientId": "",
//        "UserPoolClientSecret": "",
//        "UserPoolId": ""
//    }
    public class CognitoConfig
    {
        public string Region { get; set; }
        public string UserPoolClientId { get; set; }
        public string UserPoolClientSecret { get; set; }
        public string UserPoolId { get; set; }
    }
}