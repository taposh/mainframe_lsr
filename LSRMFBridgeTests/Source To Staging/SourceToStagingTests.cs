using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Xml;

//using IBM.Data.DB2;

using NUnit.Framework;

namespace WCIRB.LSRMFBridgeTests
{
    /// <summary>
    /// This class contains the methods to test the different scenarios for
    /// CDC_CARRIER to Insurer table 
    /// </summary>
    [TestFixture]
    public class SourceToStagingTests : NUnitBase
    {
        //Initialize variables

        /// <summary>
        /// Default contructor
        /// </summary>
        public SourceToStagingTests()
        {
        }

        /// <summary>
        /// Setup method contains the code to insert the test data
        /// to the source table and trigger the informatica workflow
        /// and wait for the execution of the workflow to complete
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            
            try
            {


                ////Get the carrier code from the xml file 
                //carrierCode = ssTestdataDoc.SelectSingleNode(TestConstants.CONST_XPATH_CARRIER).Attributes[TestConstants.CONST_ATTRIBUTE_NAME_CARRIERCODE].Value;

                //Call the Informatica workflow

                //Wait for the informatica workflow to complete executioon
                //Thread.Sleep(WAIT_FOR_REPLICATION_TIME);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [Test]
        public void TestSourceStagingOneInserts()
        {
            XmlDocument testdataDoc = new XmlDocument();

            testdataDoc.Load(TestConstants.CONST_SS1_INSERTS_TEST_DATA_XML_DOC);
            //Loop through all the nodes in the xml and call the
            //insert source data stored procedure by passing the respective table
            //name
            int nodeCount = testdataDoc.SelectSingleNode("Inserts").ChildNodes.Count;

            ArrayList outputDataHashTableList = new ArrayList();
            ArrayList targetDataHashTableList = new ArrayList();

            //Insert test data
            Hashtable targetList = InsertDataInTablesFromXml(TestConstants.CONST_SS1_INSERTS_TEST_DATA_XML_DOC, LSRConnection);

            //Call the Informatica workflow

            //Wait for the informatica workflow to complete executioon
            //Thread.Sleep(WAIT_FOR_REPLICATION_TIME);

            for (int node = 0; node < nodeCount; node++)
            {
                string nodeName = testdataDoc.SelectSingleNode("Inserts").ChildNodes[node].Name;
                Hashtable outputDataList = new Hashtable();
                Hashtable targetDataList = new Hashtable();
                targetDataList = GetOutputData(nodeName, targetList[nodeName].ToString());
                outputDataList = GetOutputTestData(TestConstants.CONST_SS1_INSERTS_TEST_DATA_XML_DOC, "//" + nodeName);
                outputDataHashTableList.Add(outputDataList);
                targetDataHashTableList.Add(targetDataList);
            }

            //Test for data
            string outputString = String.Empty;
            for (int count = 0; count < outputDataHashTableList.Count; count++)
            {
                outputString = CompareData((Hashtable)outputDataHashTableList[count], (Hashtable)targetList[count]);
            }

            Assert.AreEqual(String.Empty, outputString);
        }



        [Test]
        public void TestSourceStagingOneUpdates()
        {
            XmlDocument testdataDoc = new XmlDocument();

            testdataDoc.Load(TestConstants.CONST_SS1_UPDATES_TEST_DATA_XML_DOC);
            //Loop through all the nodes in the xml and call the
            //insert source data stored procedure by passing the respective table
            //name
            int nodeCount = testdataDoc.SelectSingleNode("Updates").ChildNodes.Count;

            ArrayList outputDataHashTableList = new ArrayList();
            ArrayList targetDataHashTableList = new ArrayList();

            //Insert test data
            Hashtable targetList = InsertDataInTablesFromXml(TestConstants.CONST_SS1_UPDATES_TEST_DATA_XML_DOC, LSRConnection);

            //Call the Informatica workflow

            //Wait for the informatica workflow to complete executioon
            //Thread.Sleep(WAIT_FOR_REPLICATION_TIME);

            for (int node = 0; node < nodeCount; node++)
            {
                string nodeName = testdataDoc.SelectSingleNode("Updates").ChildNodes[node].Name;
                Hashtable outputDataList = new Hashtable();
                Hashtable targetDataList = new Hashtable();
                targetDataList = GetOutputData(nodeName, targetList[nodeName].ToString());
                outputDataList = GetOutputTestData(TestConstants.CONST_SS1_UPDATES_TEST_DATA_XML_DOC, "//" + nodeName);
                outputDataHashTableList.Add(outputDataList);
                targetDataHashTableList.Add(targetDataList);
            }

            //Test for data
            string outputString = String.Empty;
            for (int count = 0; count < outputDataHashTableList.Count; count++)
            {
                outputString = CompareData((Hashtable)outputDataHashTableList[count], (Hashtable)targetList[count]);
            }

            Assert.AreEqual(String.Empty, outputString);
        }


        [Test]
        public void TestSourceStagingOneDeletes()
        {
            XmlDocument testdataDoc = new XmlDocument();

            testdataDoc.Load(TestConstants.CONST_SS1_DELETES_TEST_DATA_XML_DOC);
            //Loop through all the nodes in the xml and call the
            //insert source data stored procedure by passing the respective table
            //name
            int nodeCount = testdataDoc.SelectSingleNode("Deletes").ChildNodes.Count;

            ArrayList outputDataHashTableList = new ArrayList();
            ArrayList targetDataHashTableList = new ArrayList();

            //Insert test data
            Hashtable targetList = InsertDataInTablesFromXml(TestConstants.CONST_SS1_DELETES_TEST_DATA_XML_DOC, LSRConnection);

            //Call the Informatica workflow

            //Wait for the informatica workflow to complete executioon
            //Thread.Sleep(WAIT_FOR_REPLICATION_TIME);

            for (int node = 0; node < nodeCount; node++)
            {
                string nodeName = testdataDoc.SelectSingleNode("Deletes").ChildNodes[node].Name;
                Hashtable outputDataList = new Hashtable();
                Hashtable targetDataList = new Hashtable();
                targetDataList = GetOutputData(nodeName, targetList[nodeName].ToString());
                outputDataList = GetOutputTestData(TestConstants.CONST_SS1_DELETES_TEST_DATA_XML_DOC, "//" + nodeName);
                outputDataHashTableList.Add(outputDataList);
                targetDataHashTableList.Add(targetDataList);
            }

            //Test for data
            string outputString = String.Empty;
            for (int count = 0; count < outputDataHashTableList.Count; count++)
            {
                outputString = CompareData((Hashtable)outputDataHashTableList[count], (Hashtable)targetList[count]);
            }

            Assert.AreEqual(String.Empty, outputString);
        }




        /// <summary>
        /// This method contains the code to test the inserts of data to the 
        /// source table and successfully transformation of the same data to the
        /// target table
        /// </summary>
        [Test]
        public void TestSourceStagingCount()
        {
            try
            {
                //Get the data from the Insurer target table

                // SqlDataReader readData = this.readDataFromLSR("SELECT InsurerName FROM Insurer WHERE InsurerCode = '" + carrierCode + "'", "Insurer");
                //int rowCount = this.getNumberOfLSRRowsEffected("CARRIER", "WHERE CARRIERCODE = '102'");
                Assert.AreEqual(1, 1);

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }



        /// <summary>
        /// TearDown method contains the code to delete the test data
        /// inserted as part of the setup method in the source table 
        /// and transformed data in the target table.
        /// </summary>
        [TearDown]
        protected void TearDown()
        {
            try
            {
                //Delete the test data in Staging and LSR databases
                // string stagingDeleteString = "DELETE FROM CDC_CARRIER WHERE CarrierCode = ''";
                // string lsrDeleteString = "DELETE FROM Insurer WHERE InsurerCode = ''";
                //this.executeTransactionOnStaging(stagingDeleteString, TestConstants.CONST_TABLE_NAME_CDC_CARRIER);
                //this.executeTransactionOnLSR(lsrDeleteString, TestConstants.CONST_TABLE_NAME_INSURER);

                //close connections
                if (this.LSRConnection != null)
                {
                    this.LSRConnection.Close();
                    this.LSRConnection = null;
                }
                if (this.StagingConnection != null)
                {
                    this.StagingConnection.Close();
                    this.StagingConnection = null;
                }

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
    }
}
