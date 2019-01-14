using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Net;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Newtonsoft;
using Newtonsoft.Json;
using System.Text;

namespace AWSDemo.Models
{
    public class clsDynamoDB
    {
        static string AccessKey = ConfigurationManager.AppSettings["cloudUserAccessKey"];
        static string SecretKey = ConfigurationManager.AppSettings["cloudUserSecretKey"];
        static string successMessage = string.Empty;
        static int marks = 0;
        static AWSCredentials credentials = new BasicAWSCredentials(AccessKey, SecretKey);
        AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();
        AmazonDynamoDBClient client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast1);

        List<Document> doc = new List<Document>();
        List<Dictionary<string, string>> resultSet = new List<Dictionary<string, string>>();
        public delegate void dTable();
        public delegate int MultiCast(int x,int y);

        public Table GetTable()
        {
            Table table;
            try
            {

                table = Table.LoadTable(client, "Student");

                //Get Items from DynamoDB
                ScanFilter scanFilter = new ScanFilter();
                scanFilter.AddCondition("Id", ScanOperator.Equal, 5);
                Search search = table.Scan(scanFilter);

                while (!search.IsDone)
                {
                    doc = search.GetNextSet();
                }
                if (doc.Count > 0)
                {
                    for (int i = 0; i < doc.Count; i++)
                    {
                        var result = doc[i].ToJson();
                        var arr = doc[i].ToList();
                        //var dc = arr.ToDictionary(arr[0].Key,arr[0].Value);
                        var dicValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                        resultSet.Add(dicValues);
                    }
                }

                
                MultiCast mc = new MultiCast(AddMethod);
                mc += new MultiCast(Subtract);
                mc(50, 85);

                MultiCast mc2 = new MultiCast(Subtract);
                mc -= new MultiCast(AddMethod);
                mc(75, 50);

                dTable delMethod = new dTable(PutItem);
                delMethod();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return table;
        }


        public long WhatDoIDo(long a, long b)
        {

            int sgn = 0;
            sgn = a > 0 ? 1 : -1;
            long output = 0;
            long c = Math.Abs(a);
            long d = Math.Abs(b);
            if (Math.Abs(a) >= Math.Abs(b))
            {
                output = sgn * WhatDoIDo(c - d, Math.Abs(b));
            }
            else
            {
                output = a;

            }
            return output;
        }
        



        public void PutItem()
        {
            try
            {
                StringBuilder sb = new StringBuilder("", 50);
                sb.Append("MS Dhoni");
                var request = new PutItemRequest
                {
                    TableName = "Student",
                    Item = new Dictionary<string, AttributeValue>()
                          {
                              { "Id", new AttributeValue { N = "5" }},
                              { "Branch", new AttributeValue { S = "CSE" }},
                              { "Marks", new AttributeValue { N=marks.ToString() }},
                              { "Name", new AttributeValue { S = sb.ToString() }},
          
                          }
                    //  ,
                    //ExpressionAttributeNames = new Dictionary<string, string>() { 

                    //    {"#Student","Name"}                    
                    //},
                    //ExpressionAttributeValues = new Dictionary<string,AttributeValue>(){

                    //    {":studentname",new AttributeValue{S="Dhoni"}}
                    //},
                    //ConditionExpression = "Name = Dhoni"
                };
                PutItemResponse putResponse = client.PutItem(request);
                successMessage = putResponse.HttpStatusCode == HttpStatusCode.OK ? "Success" : "Error";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateItem(int id) {
            //Update Items
            UpdateItemResponse updateClient = null;

            if (resultSet.Count > 0)
            {
                for (int i = 0; i < resultSet.Count; i++)
                {
                    string attributeValue = "";

                    attributeValue = doc[i]["Id"].AsString();
                    //var value = resultSet.Where(resultSet[0].Keys.Count>0).Select(x => x.Values);
                    UpdateItemRequest updateRequest = new UpdateItemRequest()
                    {
                        TableName = "Student",
                        Key = new Dictionary<string, AttributeValue> { 
                            { "Id",  new AttributeValue { N = attributeValue}}
                        },
                        ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                        {
                           { ":r", new AttributeValue {
                                 N = "10"
                            } },
                            { ":n", new AttributeValue {
                                 N = "1"
                            } }
                        },
                        ConditionExpression = "Marks >= :n",
                        UpdateExpression = "SET Marks = :r",
                        ReturnValues = "UPDATED_NEW"

                    };

                    updateClient = client.UpdateItem(updateRequest);
                }
            }
            successMessage = updateClient.HttpStatusCode == HttpStatusCode.OK ? "Item Updated " : "Error Occured While Updating";

        }

        public void DeleteItem() {

            //Delete Item
            DeleteItemRequest deleteReq = new DeleteItemRequest()
            {
                TableName = "Student",
                Key = new Dictionary<string, AttributeValue>() { { "Id", new AttributeValue { N = "5" } } }
            };
            DeleteItemResponse deleteResponse = new DeleteItemResponse();
            deleteResponse = client.DeleteItem(deleteReq);
            successMessage = deleteResponse.HttpStatusCode == HttpStatusCode.OK ? "Item Deleted " : "Error Occured While Updating";
        }

        public void GetStreamData()
        { }

        public int AddMethod(int x, int y)
        {
            return marks= x + y;
        }
        public int Subtract(int x, int y)
        {
            return marks= x - y;
        }
    }
}