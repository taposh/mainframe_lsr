/// <summary>
/// m_USR_Difference_Tests.cs
/// Description: This class contains all the methods for testing Inserts, Update and Delete methods to test m_USR_Difference mapping 
/// Author: Aashutosh Verma
/// Create Date: Jan 26, 2008
/// </summary>



using System;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Xml;
using System.IO;

using NUnit.Framework;
using WCIRB.LSRMFBridgeTests;


namespace WCIRB.LSRMFBridgeTests.Mappings_Tests
{
    [TestFixture]
    public class m_USR_Difference_Tests : NUnitBase
    {
        Int32 RowID;
        [SetUp]

        #region Insert Test Data

        protected void SetUp()
        {
            try
            {
                // Get the data from the xml file and insert into CDC tables
                RowID = InsertDataInTableFromXml(ConfigurationSettings.AppSettings["USR_DIFFERENCE_INSERT_TEST_DATA_INPUT_XML_DOC"].ToString(), TestConstants.CONST_TABLE_NAME_CDC_USR, StagingConnection);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        #endregion




        #region Test Methods

        #region Insert Methods

        [Test]
        public void Test_USR_Difference_Inserts()
        {
            XmlDocument testdataDoc = new XmlDocument();

            testdataDoc.Load(ConfigurationSettings.AppSettings["USR_DIFFERENCE_INSERT_TEST_DATA_OUTPUT_XML_DOC"].ToString());
            //Loop through all the nodes in the xml and call the
            //insert source data stored procedure by passing the respective table
            //name
            int nodeCount = testdataDoc.SelectSingleNode("Inserts").ChildNodes.Count;

            ArrayList outputDataHashTableList = new ArrayList();

            for (int node = 0; node < nodeCount; node++)
            {
                string nodeName = testdataDoc.SelectSingleNode("Inserts").ChildNodes[node].Name;
                Hashtable outputDataList = new Hashtable();
                outputDataList = GetOutputTestData(ConfigurationSettings.AppSettings["USR_DIFFERENCE_INSERT_TEST_DATA_OUTPUT_XML_DOC"].ToString(), "//" + nodeName);
                outputDataHashTableList.Add(outputDataList);
            }

            //Call the Informatica workflow
            //Wait for the informatica workflow to complete executioon
            //Thread.Sleep(WAIT_FOR_REPLICATION_TIME);


            // Get flat file Data
            ArrayList targetlist = GetDataFromFlatFile(ConfigurationSettings.AppSettings["USR_DIFFERENCE_MAPPING_OUTPUT_DOC"].ToString());


            //Test for data
            string outputString = String.Empty;
            for (int count = 0; count < outputDataHashTableList.Count; count++)
            {
                outputString = CompareData((Hashtable)outputDataHashTableList[count], (Hashtable)(targetlist[count]));
            }

            Assert.AreEqual(String.Empty, outputString);
        }

        #endregion

        #region Update Methods

        [Test]
        public void Test_USR_Difference_Update()
        {
            XmlDocument testdataDoc = new XmlDocument();

            /// Insert test data for Updation in CDC Tables
            /// 
            RowID = InsertDataInTableFromXml(ConfigurationSettings.AppSettings["USR_DIFFERENCE_UPDATE_TEST_DATA_INPUT_XML_DOC"].ToString(), TestConstants.CONST_TABLE_NAME_CDC_USR, StagingConnection);


            /// Read Output XML file
            testdataDoc.Load(ConfigurationSettings.AppSettings["USR_DIFFERENCE_UPDATE_TEST_DATA_OUTPUT_XML_DOC"].ToString());
            //Loop through all the nodes in the xml and call the
            //insert source data stored procedure by passing the respective table
            //name
            int nodeCount = testdataDoc.SelectSingleNode("Updates").ChildNodes.Count;

            ArrayList outputDataHashTableList = new ArrayList();

            for (int node = 0; node < nodeCount; node++)
            {
                string nodeName = testdataDoc.SelectSingleNode("Updates").ChildNodes[node].Name;
                Hashtable outputDataList = new Hashtable();
                outputDataList = GetOutputTestData(ConfigurationSettings.AppSettings["USR_DIFFERENCE_UPDATE_TEST_DATA_OUTPUT_XML_DOC"].ToString(), "//" + nodeName);
                outputDataHashTableList.Add(outputDataList);
            }

            //Call the Informatica workflow
            //Wait for the informatica workflow to complete executioon
            //Thread.Sleep(WAIT_FOR_REPLICATION_TIME);


            // Get flat file Data
            ArrayList targetlist = GetDataFromFlatFile(ConfigurationSettings.AppSettings["USR_DIFFERENCE_MAPPING_OUTPUT_DOC"].ToString());


            //Test for data
            string outputString = String.Empty;
            for (int count = 0; count < outputDataHashTableList.Count; count++)
            {
                outputString = CompareData((Hashtable)outputDataHashTableList[count], (Hashtable)(targetlist[count]));
            }

            Assert.AreEqual(String.Empty, outputString);
        }
        #endregion

        #region Delete Methods

        [Test]
        public void Test_USR_Difference_Delete()
        {
            XmlDocument testdataDoc = new XmlDocument();

            /// Insert test data for Updation in CDC Tables
            /// 
            RowID = InsertDataInTableFromXml(ConfigurationSettings.AppSettings["USR_DIFFERENCE_DELETE_TEST_DATA_INPUT_XML_DOC"].ToString(), TestConstants.CONST_TABLE_NAME_CDC_USR, StagingConnection);


            /// Read Output XML file
            testdataDoc.Load(ConfigurationSettings.AppSettings["USR_DIFFERENCE_DELETE_TEST_DATA_OUTPUT_XML_DOC"].ToString());
            //Loop through all the nodes in the xml and call the
            //insert source data stored procedure by passing the respective table
            //name
            int nodeCount = testdataDoc.SelectSingleNode("Deletes").ChildNodes.Count;

            ArrayList outputDataHashTableList = new ArrayList();

            for (int node = 0; node < nodeCount; node++)
            {
                string nodeName = testdataDoc.SelectSingleNode("Deletes").ChildNodes[node].Name;
                Hashtable outputDataList = new Hashtable();
                outputDataList = GetOutputTestData(ConfigurationSettings.AppSettings["USR_DIFFERENCE_DELETE_TEST_DATA_OUTPUT_XML_DOC"].ToString(), "//" + nodeName);
                outputDataHashTableList.Add(outputDataList);
            }

            //Call the Informatica workflow
            //Wait for the informatica workflow to complete executioon
            //Thread.Sleep(WAIT_FOR_REPLICATION_TIME);


            // Get flat file Data
            ArrayList targetlist = GetDataFromFlatFile(ConfigurationSettings.AppSettings["USR_DIFFERENCE_MAPPING_OUTPUT_DOC"].ToString());


            //Test for data
            string outputString = String.Empty;
            for (int count = 0; count < outputDataHashTableList.Count; count++)
            {
                outputString = CompareData((Hashtable)outputDataHashTableList[count], (Hashtable)(targetlist[count]));
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
                cmddel.CommandText = "Delete from CDC_USR where rownumber = " + 1001;
                cmddel.Connection = StagingConnection;
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





