using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using uPLibrary.Networking.M2Mqtt.Internal;
using Newtonsoft.Json;

public class NewBehavourScript2 : MonoBehaviour
{
    private MqttClient client;
    public string brokerHostname = "broker.mqttdashboard.com";
    public int brokerPort = 1883;
    static string subTopic = "testtopic/AR/0";
    private string msg = "Pulso: 75";
    private string enferm = "Enfermedad: sano";
    private string proba = "Probabilidad: 50";
    private string anterior1 = "NA";
    private string anterior2 = "NA";
    TextMesh enf2;
    TextMesh prob2;
    TextMesh text2;
    TextMesh Ant12;
    TextMesh Ant22;
    // Start is called before the first frame update
    void Start()
    {
        if (brokerHostname != null)
        {
            Debug.Log("connecting to " + brokerHostname + ":" + brokerPort);
            Connect();
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE };
            client.Subscribe(new string[] { subTopic }, qosLevels);
        }
        enf2 = GameObject.Find("Enf2")
                         .GetComponent("TextMesh") as TextMesh;
        prob2 = GameObject.Find("Prob2")
                         .GetComponent("TextMesh") as TextMesh;
        text2 = GameObject.Find("Text2")
                         .GetComponent("TextMesh") as TextMesh;
        Ant12 = GameObject.Find("Ant12")
                         .GetComponent("TextMesh") as TextMesh;
        Ant22 = GameObject.Find("Ant22")
                         .GetComponent("TextMesh") as TextMesh;
    }

    private void Connect()
    {
        Debug.Log("about to connect on '" + brokerHostname + "'");
        client = new MqttClient(brokerHostname);
        string clientId = System.Guid.NewGuid().ToString();
        try
        {
            client.Connect(clientId);
        }
        catch (Exception e)
        {
            Debug.LogError("Connection error: " + e);
        }
    }

    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string json = System.Text.Encoding.UTF8.GetString(e.Message);
        Debug.Log("Received message from " + e.Topic + " : " + json);

        List<Info> info = JsonConvert.DeserializeObject<List<Info>>(json);


        msg = "Pulso: " + info[0].mensage;
        enferm = "Enfermedad: " + info[0].enfermedad;
        proba = "Probabilidad: " + info[0].probabilidad;
        anterior1 = "P:" + info[1].mensage + " E: " + info[1].enfermedad + " Pr: " + info[1].probabilidad;
        anterior2 = "P:" + info[2].mensage + " E: " + info[2].enfermedad + " Pr: " + info[2].probabilidad;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("update");

        text2.text = msg;
        enf2.text = enferm;
        prob2.text = proba;
        Ant12.text = anterior1;
        Ant22.text = anterior2;
    }
}


