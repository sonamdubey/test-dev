IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ParsePhoneNumber]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[ParsePhoneNumber]
GO

	/*******************************************************************************************************************************************************************************************
//this function will parse the phone number and will remove any space
		//-, and +91 at the start or 91 at the start, and 091 like that
		//if the number happen to be greater than 10 charaters then take the last 10 
		//character
***************************************************************************************************************************************************************************************/

CREATE FUNCTION ParsePhoneNumber
( @RawData AS VARCHAR(50))  
RETURNS VARCHAR(50)
AS
BEGIN

DECLARE	@RETVAL  AS VARCHAR (50) 
	   
	SET @RETVAL = @RawData

	--remove blank space
	SET @RETVAL = Replace(@RETVAL, ' ', '')
			
	--remove -
	SET @RETVAL = Replace(@RETVAL, '-', '')
			
	--if the numer happens to be greater than 10 characters, hen return
	--the last 10 chcracters
			
	IF(LEN(@RETVAL) > 10)
	BEGIN
		SET @RETVAL = Substring(@RETVAL, LEN(@RETVAL) - 10, 10)
	END
			
	--if the numer happens to be less than 5 characters, hen return
	--blank
			
	IF(LEN(@RETVAL) < 5)
	BEGIN
		SET @RETVAL = ''
	END			
	
              RETURN @RETVAL
END



