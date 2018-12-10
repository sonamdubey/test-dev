IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ENTRYCARMAKES]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ENTRYCARMAKES]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR CARMAKES TABLE
--BEFORE INSERTING OR UPDATING CHECK FOR THE DUPLICATE ENTRIES

CREATE PROCEDURE [dbo].[ENTRYCARMAKES]
	@CARMAKE	 VARCHAR(60),	--name of the DIVISION
	@ID		NUMERIC,	--ID. IF IT IS -1 THEN IT IS FOR INSERT, ELSE IT IS FOR UPDATE FOR THE ID 		        	
	@STATUS 	INTEGER OUTPUT	--return value, 0 FOR SUCCESSFULL  ATTEMPT, AND -1 FOR DUPLICATE ENTRY
 AS
	
BEGIN
	
	IF @ID = -1
	BEGIN
		--IT IS FOR THE INSERT
		INSERT INTO CARMAKES(NAME) VALUES(@CARMAKE)
	END
	ELSE
	BEGIN
		--IT IS FOR THE UPDATE
		UPDATE CARMAKES SET NAME = @CARMAKE WHERE ID = @ID
	END
	
	--CHECK THE ERROR. IF IT IS NOT 0, THEN THERE IS ERROR IN INSERTING THE QUERY. IN THIS CASE THIS WILL BE DUE TO
	--DUPLICATE ENTRY
	IF @@ERROR <> 0
	BEGIN
		SET @STATUS = @@ERROR
	END
	ELSE
		SET @STATUS = 0	
	
END
