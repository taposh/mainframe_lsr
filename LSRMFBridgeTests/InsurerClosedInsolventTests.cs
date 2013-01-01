using System;
using System.Xml;
using System.Data;
using System.Threading;
using System.Data.SqlClient;
using System.Collections;

using NUnit.Framework;


namespace WCIRB.LSRMFBridgeTests
{
    /// <summary>
    /// This class contains the methods to test the different scenarios for
    /// CDC_CARRIER to Insurer table 
    /// </summary>
    [TestFixture]
    public class InsurerClosedInsolventTests : NUnitBase
    {
        //Declare the variables
        private XmlDocument insurerInsolventTestdataDoc;
        private string carrierCode;
        private string rowNumber;

        /// <summary>
        /// Default contructor
        /// </summary>
        public InsurerClosedInsolventTests()
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
            // = string.Empty;
            try
            {
                insurerInsolventTestdataDoc = new XmlDocument();
                SqlCommand cmdInsert = new SqlCommand();

                //Load the xml document
                insurerInsolventTestdataDoc.Load(TestConstants.CONST_INSERTS_TEST_DATA_XML_DOC);

                //Set the connect parameters
                cmdInsert.Connection = this.StagingConnection;
                cmdInsert.CommandType = CommandType.StoredProcedure;
                cmdInsert.CommandText = TestConstants.CONST_SP_INSERTTESTDATA;
                cmdInsert.Parameters.AddWithValue(TestConstants.CONST_SP_INPUT_DOC_PATH, insurerInsolventTestdataDoc.OuterXml);
                cmdInsert.Parameters.AddWithValue(TestConstants.CONST_SP_INPUT_TABLE_NAME, insurerInsolventTestdataDoc.SelectSingleNode(TestConstants.CONST_XPATH_CDC_CLOSED_INSOLVENT_RUNOFF).Name);
                cmdInsert.ExecuteNonQuery();

                //Get the carrier code from the xml file 
                carrierCode = insurerInsolventTestdataDoc.SelectSingleNode(TestConstants.CONST_XPATH_CDC_CARRIER).Attributes[TestConstants.CONST_ATTRIBUTE_NAME_CARRIERCODE].Value;
                rowNumber = insurerInsolventTestdataDoc.SelectSingleNode(TestConstants.CONST_XPATH_CDC_CARRIER).Attributes[TestConstants.CONST_ATTRIBUTE_NAME_ROWNUMBER].Value.ToString();

                //Call the Informatica workflow on unix box

                //Wait for the informatica workflow to complete executioon
                //Thread.Sleep(WAIT_FOR_REPLICATION_TIME);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// This method contains the code to test the inserts of data to the 
        /// source table and successfully transformation of the same data to the
        /// target table
        /// </summary>
        [Test]
        public void TestInsurerClosedInsolventInserts()
        {
            try
            {
                //Get the data from target XML file
                Hashtable outputDataList = new Hashtable();
                XmlDocument insurerOutputdataDoc = new XmlDocument();
                //XmlAttributeCollection columnCollection = new XmlAttributeCollection();

                insurerOutputdataDoc.Load(TestConstants.CONST_INSERTS_OUTPUT_DATA_XML_DOC);
                XmlAttributeCollection columnCollection = insurerOutputdataDoc.SelectSingleNode(TestConstants.CONST_XPATH_INSURERCLOSEDINSOLVENT).Attributes;
                int columnCount = columnCollection.Count;
                if (columnCount > 0)
                {
                    for (int count = 0; count < columnCount; count++)
                    {
                        outputDataList.Add(columnCollection.Item(count).Name, columnCollection.Item(count).Value);
                    }
                }

                //Call the Informatica workflow on unix box

                //Wait for the informatica workflow to complete executioon
                //Thread.Sleep(WAIT_FOR_REPLICATION_TIME);

                //Get the data from the Insurer target table
                Hashtable targetDataList = new Hashtable();
                SqlDataReader readData = this.readDataFromLSR("SELECT * FROM InsurerClosedInsolvent ic, Insurer i WHERE ic.InsurerFID = i.InsurerGID AND i.InsurerCode = '" + carrierCode + "'", "InsurerClosedInsolvent");
                string insurerName = String.Empty;

                if (readData.HasRows)
                {
                    readData.Read();
                    columnCount = readData.FieldCount;
                    for (int count = 0; count < columnCount; count++)
                    {
                        targetDataList.Add(readData.GetName(count), readData.GetValue(count));
                    }
                }
                if (readData != null)
                {
                    readData.Close();
                    readData = null;
                }

                string output = CompareData(outputDataList, targetDataList); ;

                Assert.AreEqual(String.Empty, output);

            }
            catch (Exception exp)
            {
                throw exp;
            }

        }

        /// <summary>
        /// This method contains the code to test the update of data to the 
        /// source table and successfully transformation of the same data to the
        /// target table
        /// </summary>
        [Test]
        public void TestInsurerClosedInsolventUpdates()
        {
            try
            {
                //Get the data from target XML file
                Hashtable outputDataList = new Hashtable();
                XmlDocument insurerOutputdataDoc = new XmlDocument();
                SqlCommand cmdInsert = new SqlCommand();

                //Load the xml document
                insurerInsolventTestdataDoc.Load(TestConstants.CONST_UPDATES_TEST_DATA_XML_DOC);

                //Set the connect parameters
                cmdInsert.Connection = this.StagingConnection;
                cmdInsert.CommandType = CommandType.StoredProcedure;
                cmdInsert.CommandText = TestConstants.CONST_SP_INSERTTESTDATA;
                cmdInsert.Parameters.AddWithValue(TestConstants.CONST_SP_INPUT_DOC_PATH, insurerInsolventTestdataDoc.OuterXml);
                cmdInsert.Parameters.AddWithValue(TestConstants.CONST_SP_INPUT_TABLE_NAME, insurerInsolventTestdataDoc.SelectSingleNode(TestConstants.CONST_XPATH_CDC_CARRIER).Name);
                cmdInsert.ExecuteNonQuery();

                //Call the Informatica workflow on unix box

                //Wait for the informatica workflow to complete executioon
                //Thread.Sleep(WAIT_FOR_REPLICATION_TIME);

                insurerOutputdataDoc.Load(TestConstants.CONST_UPDATES_OUTPUT_DATA_XML_DOC);
                XmlAttributeCollection columnCollection = insurerOutputdataDoc.SelectSingleNode(TestConstants.CONST_XPATH_INSURER).Attributes;
                int columnCount = columnCollection.Count;
                if (columnCount > 0)
                {
                    for (int count = 0; count < columnCount; count++)
                    {
                        outputDataList.Add(columnCollection.Item(count).Name, columnCollection.Item(count).Value);
                    }
                }

                //Get the data from the Insurer target table
                Hashtable targetDataList = new Hashtable();
                SqlDataReader readData = this.readDataFromLSR("SELECT * FROM InsurerClosedInsolvent ic, Insurer i WHERE ic.InsurerFID = i.InsurerGID AND i.InsurerCode = '" + carrierCode + "'", "InsurerClosedInsolvent");
                string insurerName = String.Empty;

                if (readData.HasRows)
                {
                    readData.Read();
                    columnCount = readData.FieldCount;
                    for (int count = 0; count < columnCount; count++)
                    {
                        targetDataList.Add(readData.GetName(count), readData.GetValue(count));
                    }
                }
                if (readData != null)
                {
                    readData.Close();
                    readData = null;
                }

                string output = CompareData(outputDataList, targetDataList); ;

                Assert.AreEqual(String.Empty, output);

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// This method contains the code to test the delete of data to the 
        /// source table and successfully transformation of the same data to the
        /// target table
        /// </summary>
        [Test]
        public void TestInsurerClosedInsolventDeletes()
        {
            try
            {
                SqlCommand cmdInsert = new SqlCommand();

                //Load the xml document
                insurerInsolventTestdataDoc.Load(TestConstants.CONST_UPDATES_TEST_DATA_XML_DOC);

                //Set the connect parameters
                cmdInsert.Connection = this.StagingConnection;
                cmdInsert.CommandType = CommandType.StoredProcedure;
                cmdInsert.CommandText = TestConstants.CONST_SP_INSERTTESTDATA;
                cmdInsert.Parameters.AddWithValue(TestConstants.CONST_SP_INPUT_DOC_PATH, insurerInsolventTestdataDoc.OuterXml);
                cmdInsert.Parameters.AddWithValue(TestConstants.CONST_SP_INPUT_TABLE_NAME, insurerInsolventTestdataDoc.SelectSingleNode(TestConstants.CONST_XPATH_CDC_CARRIER).Name);
                cmdInsert.ExecuteNonQuery();

                SqlDataReader readData = this.readDataFromLSR("SELECT ic.IsActive FROM InsurerClosedInsolvent ic, Insurer i WHERE ic.InsurerFID = i.InsurerGID AND i.InsurerCode = '" + carrierCode + "'", "Insurer");
                bool isActive = false;
                if (readData.HasRows)
                {
                    while (readData.Read())
                    {
                        isActive = Convert.ToBoolean(readData.GetString(0));
                    }
                }
                Assert.AreEqual(true, isActive);

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// This method contains the code to test the rowcount of data in the 
        /// target table so that the data has been successfully transformed from 
        /// source to the target table
        /// </summary>
        [Test]
        public void TestInsurerClosedInsolventRowCount()
        {
            try
            {
                SqlDataReader readData = this.readDataFromLSR("SELECT count(1) FROM InsurerClosedInsolvent WHERE InsurerCode = '" + carrierCode + "'", "InsurerClosedInsolvent");
                int rowCount = 0;
                if (readData.HasRows)
                {
                    while (readData.Read())
                    {
                        rowCount = Convert.ToInt32(readData.GetString(0));
                    }
                }
                Assert.AreEqual(1, rowCount);

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
                string stagingDeleteString = "DELETE FROM CDC_CARRIER WHERE CarrierCode = '" + carrierCode + "'";
                string indDeleteString = "DELETE FROM IND_CARRIER WHERE CarrierCode = " + rowNumber + "";
                string lsrDeleteString = "DELETE FROM Insurer WHERE RowNumber = " + carrierCode + "'";
                this.executeTransactionOnStaging(stagingDeleteString, TestConstants.CONST_TABLE_NAME_CDC_CLOSED_INSOLVENT_RUNOFF);
                this.executeTransactionOnLSR(lsrDeleteString, TestConstants.CONST_TABLE_NAME_INSURER);

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
