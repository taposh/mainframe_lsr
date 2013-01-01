
/// <summary>
/// m_ST_USR_Exposure.cs
/// Description: This contains all the methods to test Inserts, Update, and Delete methods of m_ST_USR_Exposure mapping
/// Author: Aashutosh Verma
/// Create Date: Feb 18, 2008
/// </summary>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Xml;
using System.IO;
using System.Configuration;

using NUnit.Framework;
using WCIRB.LSRMFBridgeTests;



namespace WCIRB.LSRMFBridgeTests.SourceMappings_Tests
{
    [TestFixture]
    public class m_ST_USR_Exposure : NUnitBase
    {

        #region Test Methods

        #region Test Inserts Methods

        [Test]
        public void ST_USR_Exposure_Inserts()
        {
            XmlDocument testdataDoc = new XmlDocument();

            testdataDoc.Load(ConfigurationSettings.AppSettings["ST_USR_EXPOSURE_INSERT_TEST_DATA_INOUT_XML_DOC"].ToString());
            //Loop through all the nodes in the xml and call the
            //insert source data stored procedure by passing the respective table
            //name
            int nodeCount = testdataDoc.SelectSingleNode("Inserts").ChildNodes.Count;

            ArrayList outputDataHashTableList = new ArrayList();

            for (int node = 0; node < nodeCount; node++)
            {
                string nodeName = testdataDoc.SelectSingleNode("Inserts").ChildNodes[node].Name;
                Hashtable outputDataList = new Hashtable();
                outputDataList = GetOutputTestData(ConfigurationSettings.AppSettings["ST_USR_EXPOSURE_INSERT_TEST_DATA_INOUT_XML_DOC"].ToString(), "//" + nodeName);
                outputDataHashTableList.Add(outputDataList);
            }

            //Insert test data
            Hashtable targetList = InsertDataInTablesFromXml(ConfigurationSettings.AppSettings["ST_USR_EXPOSURE_INSERT_TEST_DATA_INOUT_XML_DOC"].ToString(), LSRConnection);



            //Call the Informatica workflow
            //Wait for the informatica workflow to complete executioon
            //Thread.Sleep(WAIT_FOR_REPLICATION_TIME);


            //Test for data
            string outputString = String.Empty;
            for (int count = 0; count < outputDataHashTableList.Count; count++)
            {
                outputString = CompareData((Hashtable)outputDataHashTableList[count], (Hashtable)(targetList[count]));
            }

            Assert.AreEqual(String.Empty, outputString);
        }

        #endregion


        #region Test Update Methods

        [Test]
        public void ST_USR_Exposure_Update()
        {
            XmlDocument testdataDoc = new XmlDocument();

            testdataDoc.Load(ConfigurationSettings.AppSettings["ST_USR_EXPOSURE_UPDATE_TEST_DATA_INOUT_XML_DOC"].ToString());
            //Loop through all the nodes in the xml and call the
            //insert source data stored procedure by passing the respective table
            //name
            int nodeCount = testdataDoc.SelectSingleNode("Updates").ChildNodes.Count;

            ArrayList outputDataHashTableList = new ArrayList();

            for (int node = 0; node < nodeCount; node++)
            {
                string nodeName = testdataDoc.SelectSingleNode("Updates").ChildNodes[node].Name;
                Hashtable outputDataList = new Hashtable();
                outputDataList = GetOutputTestData(ConfigurationSettings.AppSettings["ST_USR_EXPOSURE_UPDATE_TEST_DATA_INOUT_XML_DOC"].ToString(), "//" + nodeName);
                outputDataHashTableList.Add(outputDataList);
            }

            //Insert test data
            Hashtable targetList = InsertDataInTablesFromXml(ConfigurationSettings.AppSettings["ST_USR_EXPOSURE_UPDATE_TEST_DATA_INOUT_XML_DOC"].ToString(), LSRConnection);


            //Call the Informatica workflow
            //Wait for the informatica workflow to complete executioon
            //Thread.Sleep(WAIT_FOR_REPLICATION_TIME);


            //Test for data
            string outputString = String.Empty;
            for (int count = 0; count < outputDataHashTableList.Count; count++)
            {
                outputString = CompareData((Hashtable)outputDataHashTableList[count], (Hashtable)(targetList[count]));
            }

            Assert.AreEqual(String.Empty, outputString);

        }

        #endregion


        #region Test Delete Methods

        [Test]
        public void ST_USR_Exposure_Delete()
        {
            XmlDocument testdataDoc = new XmlDocument();

            testdataDoc.Load(ConfigurationSettings.AppSettings["ST_USR_EXPOSURE_DELETE_TEST_DATA_INOUT_XML_DOC"].ToString());
            //Loop through all the nodes in the xml and call the
            //insert source data stored procedure by passing the respective table
            //name
            int nodeCount = testdataDoc.SelectSingleNode("Deletes").ChildNodes.Count;

            ArrayList outputDataHashTableList = new ArrayList();

            for (int node = 0; node < nodeCount; node++)
            {
                string nodeName = testdataDoc.SelectSingleNode("Deletes").ChildNodes[node].Name;
                Hashtable outputDataList = new Hashtable();
                outputDataList = GetOutputTestData(ConfigurationSettings.AppSettings["ST_USR_EXPOSURE_DELETE_TEST_DATA_INOUT_XML_DOC"].ToString(), "//" + nodeName);
                outputDataHashTableList.Add(outputDataList);
            }

            //Insert test data
            Hashtable targetList = InsertDataInTablesFromXml(ConfigurationSettings.AppSettings["ST_USR_EXPOSURE_DELETE_TEST_DATA_INOUT_XML_DOC"].ToString(), LSRConnection);


            //Call the Informatica workflow
            //Wait for the informatica workflow to complete executioon
            //Thread.Sleep(WAIT_FOR_REPLICATION_TIME);


            //Test for data
            string outputString = String.Empty;
            for (int count = 0; count < outputDataHashTableList.Count; count++)
            {
                outputString = CompareData((Hashtable)outputDataHashTableList[count], (Hashtable)(targetList[count]));
            }

            Assert.AreEqual(String.Empty, outputString);
        }

        #endregion

        #endregion


        [TearDown]

        #region Tear Down

        public void TearDown()
        {
            try
            {
                //Delete the test data from the CDC table
                SqlCommand cmddel = new SqlCommand();
                cmddel.CommandText = "Delete from LSR.USRExposure where USRExposureGID in(1001, 1002,1003,1004, 1005)";
                cmddel.Connection = LSRConnection;
                cmddel.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        #endregion



    }
}
