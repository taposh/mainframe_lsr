using System;
using System.Collections.Generic;
using System.Text;

namespace WCIRB.LSRMFBridgeTests
{
    class TestConstants
    {
        #region General Constants

        //InsertTests
        public static string CONST_SS1_INSERTS_TEST_DATA_XML_DOC = "\\SourceCode\\LSRMFBridge\\LSRMFBridgeTests\\XmlTestdata\\SS1InsertsTestData.xml";
        public static string CONST_SS1_UPDATES_TEST_DATA_XML_DOC = "\\SourceCode\\LSRMFBridge\\LSRMFBridgeTests\\XmlTestdata\\SS1UpdatesTestData.xml";
        public static string CONST_SS1_DELETES_TEST_DATA_XML_DOC = "\\SourceCode\\LSRMFBridge\\LSRMFBridgeTests\\XmlTestdata\\SS1DeletesTestData.xml";
        public static string CONST_INSERTS_OUTPUT_DATA_XML_DOC = "\\SourceCode\\LSRMFBridge\\LSRMFBridgeTests\\XmlTestdata\\InsertsOutputData.xml";
        public static string CONST_UPDATES_OUTPUT_DATA_XML_DOC = "\\SourceCode\\LSRMFBridge\\LSRMFBridgeTests\\XmlTestdata\\UpdatesOutputData.xml";
       
        #endregion

        #region TableName Constants
        public static string CONST_TABLE_NAME_EMPLOYERBAC = "EmployerBAC";
        public static string CONST_TABLE_NAME_WORKCONTROLRECORD = "WorkControlRecord";
        public static string CONST_TABLE_NAME_TESTAUDITMASTERRECORDS = "TestAuditMasterRecords";
        public static string CONST_TABLE_NAME_RATING = "Rating";
        public static string CONST_TABLE_NAME_COMPUTERMODFILE = "ComputerModFile";
        public static string CONST_TABLE_NAME_JOBCLASSIFICATION = "JobClassification";
        public static string CONST_TABLE_NAME_JOBCLASSIFICATIONSUFFIX = "JobClassificationSuffix";
        public static string CONST_TABLE_NAME_USR = "USR";
        public static string CONST_TABLE_NAME_USREXPOSURE = "USRExposure";
        public static string CONST_TABLE_NAME_USRLOSS = "USRLoss";
        public static string CONST_TABLE_NAME_USRTOTALS = "USRTotals";
        public static string CONST_TABLE_NAME_CDC_EMPLOYERBAC = "CDC_EmployerBAC";
        public static string CONST_TABLE_NAME_CDC_WORKCONTROLRECORD = "CDC_WorkControlRecord";
        public static string CONST_TABLE_NAME_CDC_TESTAUDITMASTERRECORDS = "CDC_TestAuditMasterRecords";
        public static string CONST_TABLE_NAME_CDC_RATING = "CDC_Rating";
        public static string CONST_TABLE_NAME_CDC_COMPUTERMODFILE = "CDC_ComputerModFile";
        public static string CONST_TABLE_NAME_CDC_JOBCLASSIFICATION = "CDC_JobClassification";
        public static string CONST_TABLE_NAME_CDC_JOBCLASSIFICATIONSUFFIX = "CDC_JobClassificationSuffix";
        public static string CONST_TABLE_NAME_CDC_USR = "CDC_USR";
        public static string CONST_TABLE_NAME_CDC_USREXPOSURE = "CDC_USRExposure";
        public static string CONST_TABLE_NAME_CDC_USRLOSS = "CDC_USRLoss";
        public static string CONST_TABLE_NAME_CDC_USRTOTALS = "CDC_USRTotals";

        #endregion

        #region ColumnName Constants
        #endregion

        #region Xml Constants
        public static string CONST_XPATH_INSERTS = "//Inserts";
        public static string CONST_XPATH_NAIC_GROUP = "//CDC_NAIC_GROUP";
        public static string CONST_XPATH_CDC_TPE = "//CDC_TPE";
        public static string CONST_XPATH_EMPLOYERBAC = "//m_Employer_BAC_Input";
        public static string CONST_XPATH_EMPLOYERBAC_OUTPUT = "//EMPLOYERBAC_OUTPUT";
        public static string CONST_XPATH_CDC_CAR_TPE_LOCATION = "//CDC_CAR_TPE_LOCATION";
        public static string CONST_XPATH_CDC_CARRIER_CONTACT_INFO = "//CDC_CARRIER_CONTACT_INFO";
        public static string CONST_XPATH_CDC_TPE_CONTACT_INFO = "//CDC_TPE_CONTACT_INFO";
        public static string CONST_XPATH_CDC_CLOSED_INSOLVENT_RUNOFF = "//CDC_CLOSED_INSOLVENT_RUNOFF";
        public static string CONST_XPATH_INSURER = "//Insurer";
        public static string CONST_XPATH_TPE = "//TPE";
        public static string CONST_XPATH_NAICGROUP = "//NAICGroup";
        public static string CONST_XPATH_CONTACT = "//Contact";
        public static string CONST_XPATH_CONTACTLOCATIONMAP = "//ContactLocationMap";
        public static string CONST_XPATH_INSURERTPELOCATIONMAP = "//InsurerTPELocationMap";
        public static string CONST_XPATH_INSURERCLOSEDINSOLVENT = "//InsurerClosedInsolvent";
        public static string CONST_ATTRIBUTE_NAME_CARRIERCODE = "CARRIERCODE";
        public static string CONST_ATTRIBUTE_NAME_ROWNUMBER = "RowNumberGID";        
        public static string CONST_ATTRIBUTE_NAME_NAICGROUPCODE = "NAICGROUPCODE";        
        //public static string CONST_ATTRIBUTE_NAME_FEINCODE = "FEINCode";        
        //constant value modified from FEIN Code to TPECODE
        public static string CONST_ATTRIBUTE_NAME_FEINCODE = "TPECODE";        
        public static string CONST_ATTRIBUTE_NAME_LOCATIONID = "LOCATIONID";
        #endregion

        #region StoredProcedure Constants
        public static string CONST_SP_INSERTTESTDATA = "InsertTestData";
        public static string CONST_SP_INSERTSOURCEDATA = "InsertSourceData";
        public static string CONST_SP_INPUT_DOC_PATH = "@docPath";
        public static string CONST_SP_INPUT_TABLE_NAME = "@tableName";
        #endregion
    }
}
