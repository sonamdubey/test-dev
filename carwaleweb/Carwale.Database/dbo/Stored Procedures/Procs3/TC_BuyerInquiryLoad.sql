IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BuyerInquiryLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BuyerInquiryLoad]
GO

	-- Author:		Surendra
-- Create date: 12 Jan 2012
-- Description:	This procedure will be used to bind Controls in Buyer Inquiry ADD form during load
-- Modified By: Tejashree Patil on 22 Nov 2012 at 11 am : Fetched ProfileId value when IsSynchronizedCW=1 using CASE
-- [TC_BuyerInquiryLoad] 5,3548
-- =============================================
CREATE PROCEDURE [dbo].[TC_BuyerInquiryLoad]
(
	@BranchId BIGINT,
	@StockId BIGINT
)
AS
BEGIN
	
	-- Give Inquiry Status
	EXECUTE TC_InquiryStatusSelect
	
	-- Give all Source as Table
	EXECUTE TC_InquirySourceSelect
		
	-- Give all Source as Table
	EXECUTE TC_UsersForInuiryAssignment @BranchId
	
	-- Give all Stock details as Table
	SELECT ( Ma.Name+' '+ Mo.Name+' '+ Ve.Name) AS CarName, RegNo,
			(CASE WHEN St.IsSychronizedCW=1 THEN Si.ID END ) AS profileId -- Modified By: Tejashree Patil on 22 Nov 2012 at 11 am 
	FROM TC_Stock St 
	Inner Join CarVersions Ve WITH(NOLOCK)On Ve.Id = St.VersionId 
	Inner Join CarModels Mo WITH(NOLOCK)On Mo.Id = Ve.CarModelId 
	Inner Join CarMakes Ma WITH(NOLOCK)On Ma.Id = Mo.CarMakeId 
	LEFT Join SellInquiries Si WITH(NOLOCK)On Si.TC_StockId=St.Id 
	WHERE St.Id = @StockId
END