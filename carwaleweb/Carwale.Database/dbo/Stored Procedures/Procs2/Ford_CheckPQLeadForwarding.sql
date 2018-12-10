IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Ford_CheckPQLeadForwarding]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Ford_CheckPQLeadForwarding]
GO

	-- =============================================
-- Author:		Vikas
-- Create date: 21.07.2014	
-- Description:	Check if a lead has to be pushed to FORD API
-- =============================================
CREATE PROCEDURE [dbo].[Ford_CheckPQLeadForwarding]
@VersionId int,
@CityId int,
@MakeId int OUTPUT,
@ModelId int OUTPUT,
@ToBeForwaded bit OUTPUT,
@ModelName varchar(100) OUTPUT

AS
BEGIN
	SELECT @MakeId = VM.MakeId, @ModelId = VM.ModelId ,@ModelName = VM.Model FROM vwMMV VM WHERE VM.VersionId = @VersionId
	
	IF ( @MakeId = 5 ) 
		BEGIN
			SET @ToBeForwaded = 1
		END
	ELSE
		BEGIN
		  SET  @ToBeForwaded = 0
		END
		
END

