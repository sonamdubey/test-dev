IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDataForVWExcel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDataForVWExcel]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 27 Jun 2013 at 7 pm
-- Description:	To get all related data while import excel for VW
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetDataForVWExcel]
	@BranchId BIGINT,
	@InquiryType SMALLINT	
AS
BEGIN

	EXEC TC_DealerCitiesView @BranchId
	EXEC TC_GetVersionColors NULL
	EXEC TC_UsersForInuiryAssignment @BranchId
	--EXEC TC_INQUserList @BranchId,@UserId
	EXEC TC_InquirySourceDealerWise @BranchId
	EXEC TC_GetAreas @BranchId
	EXEC TC_GetCorporateList @BranchId
	EXEC TC_GetLeadDispositions @InquiryType
	
	SELECT        
		   TL.Name,  
		   TL.TC_LeadDispositionId,  
		   TL.TC_LeadInquiryTypeId   
	FROM   TC_LeadDisposition TL WITH(NOLOCK)   
	WHERE  TC_LeadInquiryTypeId=3 
			AND IsActive=1 
			AND IsClosed=0 
			AND name LIKE '%TD%' 
			AND TC_LeadDispositionId <>26
			
	SELECT  TC_InquiryStatusId AS Value,Status AS Text
	FROM	TC_InquiryStatus
	WHERE	IsActive=1
	
END

