using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon;
using DynamodbExample.Models;

namespace DynamodbExample
{
    public class DynamoManager
    {
        private ConfigSettings.DynamoDBSettings dynamoConfig { get; set; }

        private AmazonDynamoDBClient DynamoClient { get; set; }

        public DynamoManager(ConfigSettings.DynamoDBSettings dbSettings, RegionEndpoint endpoint)
        {
            dynamoConfig = dbSettings;
            AmazonDynamoDBConfig awsConfig = new AmazonDynamoDBConfig();
            awsConfig.RegionEndpoint = endpoint;

            DynamoClient = new AmazonDynamoDBClient(dynamoConfig.DynamoID, dynamoConfig.DynamoSecret, awsConfig);
        }

        /// <summary>
        /// Inserts ContactModel
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public async Task<PutItemResponse> PutContactModel(ContactModel contact)
        {
            try
            {
                return await DefaultPutItem(ContactModel.Table, GetDynamoObj(contact));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a contact model from dynamoDB. 
        /// </summary>
        /// <param name="GUID">Unique Identifier</param>
        /// <returns></returns>
        public async Task<ContactModel> GetContactModel(string GUID)
        {
            ContactModel data = null;

            Dictionary<string, AttributeValue> req = new Dictionary<string, AttributeValue>();

            req.Add("GUID", new AttributeValue(GUID));

            GetItemResponse result = await DefaultGetItem(req, ContactModel.Table);

            if (result != null)
            {
                data = new ContactModel();
                if (result.Item.ContainsKey("FirstName")) data.FirstName = result.Item["FirstName"].S;
                if (result.Item.ContainsKey("LastName")) data.LastName = result.Item["LastName"].S;
                if (result.Item.ContainsKey("Email")) data.LastName = result.Item["Email"].S;
                if (result.Item.ContainsKey("PhoneNumber")) data.LastName = result.Item["PhoneNumber"].S;
                if (result.Item.ContainsKey("GUID")) data.GUID = result.Item["GUID"].S;
            }

            return data;
        }

        /// <summary>
        /// Generic method to get model from a database
        /// </summary>
        /// <param name="RequestObj"></param>
        /// <param name="Table">Table Name</param>
        /// <returns></returns>
        private async Task<GetItemResponse> DefaultGetItem(Dictionary<string, AttributeValue> RequestObj, string Table)
        {
            try
            {
                GetItemRequest get = new GetItemRequest(Table, RequestObj);
                GetItemResponse Result = await DynamoClient.GetItemAsync(get);
                return Result;
            }
            catch(Exception ex)
            {
                string err = ex.ToString();
                return null;
            }
        }

        /// <summary>
        /// Generic put object into database
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="DataObj"></param>
        /// <returns></returns>
        private async Task<PutItemResponse> DefaultPutItem(string Table, Dictionary<string, AttributeValue> DataObj)
        {
            PutItemRequest put = new PutItemRequest(Table, DataObj);

            return await DynamoClient.PutItemAsync(put);
        }

        /// <summary>
        /// Generates a put object for a ContactModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Dictionary<string, AttributeValue> GetDynamoObj(ContactModel model)
        {
            Dictionary<string, AttributeValue> collect = new Dictionary<string, AttributeValue>();

            collect.Add("FirstName", new AttributeValue(model.FirstName));
            collect.Add("LastName", new AttributeValue(model.LastName));
            collect.Add("Email", new AttributeValue(model.Email));
            collect.Add("PhoneNumber", new AttributeValue(model.PhoneNumber));
            collect.Add("GUID", new AttributeValue(Guid.NewGuid().ToString()));

            return collect;
        }
    }
}
