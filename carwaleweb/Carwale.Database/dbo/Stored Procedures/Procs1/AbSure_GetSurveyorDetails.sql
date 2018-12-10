IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetSurveyorDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetSurveyorDetails]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 23rd Oct 2015
-- Description:	to get the surveyor details when the surveyor gets mapped to a dealer
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetSurveyorDetails] 
	@SurveyorLoginId VARCHAR(15)
AS
BEGIN
	
	SELECT Id,UserName 
	FROM TC_Users WITH (NOLOCK) 
	WHERE Mobile = @SurveyorLoginId 
		  AND IsActive=1
		  AND IsAgency=0
		  AND BranchId=11165 -- AXA branchid
END

----------------------------


