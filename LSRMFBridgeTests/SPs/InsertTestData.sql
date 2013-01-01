/****** Object:  StoredProcedure [dbo].[InsertTestData]    Script Date: 10/29/2007 18:34:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertTestData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[InsertTestData]
GO
CREATE PROCEDURE dbo.InsertTestData
@docPath	varchar(8000),
@tableName	varchar(8000)
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

	exec sp_executesql @query


END

RETURN
GO 



