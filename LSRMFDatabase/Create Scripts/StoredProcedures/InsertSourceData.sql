--Drop the sp if exists
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'InsertSourceData')
	BEGIN
		DROP  Procedure  InsertSourceData
	END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Anitha V
-- Create date: 1/17/2008
-- Description:	This stored procedure will read the data from the
-- xml file and insert into the table specified as part of the input parameter.
-- =============================================
CREATE Procedure InsertSourceData
(
	@docPath	varchar(8000),
	@tableName	varchar(8000)
)
AS
BEGIN
	DECLARE @query nvarchar(4000)

	--Set the dynamic query
	SET @query = N'DECLARE @hdoc int
	EXEC sp_xml_preparedocument @hdoc OUTPUT, ''' +@docPath +'''

	BEGIN TRANSACTION

	INSERT INTO ' + @tableName + '
	SELECT * 
	FROM OPENXML(@hdoc, ''//' + @tableName + ''')
	WITH ' + @tableName + '

	COMMIT

	EXEC sp_xml_removedocument @hdoc '

	--Execute the dynamic sql
	exec sp_executesql @query


END

RETURN
GO 

--Insert Data to Tables Present in Mapping One
--EmployerBAC
--WorkControlRecord
--TestAuditMasterRecord
--Rating
--ComputerModFile
--JobClassification
--JobClassificationSuffix
--Insert Data to Tables Present in Mapping Two
--USR
--USRExposure
--USRLoss
--USRTotal
--USRSubmissionExposure
--USRSubmission

/*
GRANT EXEC ON Stored_Procedure_Name TO PUBLIC

GO
*/

