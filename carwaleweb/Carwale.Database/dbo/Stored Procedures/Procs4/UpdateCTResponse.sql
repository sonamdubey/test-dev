IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCTResponse]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCTResponse]
GO

	-- =============================================
-- Author:		Afrose
-- Create date: 17-06-2016
-- Description:	to update CT_IndividualResponse table based on api response
-- Modified on 30-09-2016, logged error logs to new table CT_AllLeadsResponseErrorLogs, commented to old logic 
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCTResponse]
@CWLeadId VARCHAR(100),
@ErrorDescription VARCHAR(100) = NULL	
AS
BEGIN	
	
	SET NOCOUNT ON;

	--UPDATE CT_IndividualResponse SET CTStatusId=@CTStatusId WHERE ID=@CWLeadId;

	--IF(@CTStatusId=0)
	--BEGIN
	--INSERT INTO CT_IndividualResponseErrorLogs(CWLeadId,ErrorDescription) VALUES(@CWLeadId,@ErrorDescription)
	--END

	INSERT INTO CT_AllLeadsResponseErrorLogs(CWLeadId,ErrorDescription) VALUES(@CWLeadId,@ErrorDescription)
	
   
END
