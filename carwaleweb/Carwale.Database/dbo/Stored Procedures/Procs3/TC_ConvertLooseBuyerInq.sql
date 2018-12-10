IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ConvertLooseBuyerInq]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ConvertLooseBuyerInq]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 4th March,2013
-- Description:	Updatetion of Buyer Inquiries
-- EXEC TC_ConvertLooseBuyerInq 3912, 116
-- Modified By Vivek Gupta on 4th july,2013, Commented LatestInquiry date update
-- =============================================
CREATE PROCEDURE [dbo].[TC_ConvertLooseBuyerInq]
	-- Add the parameters for the stored procedure here
	@BranchId BIGINT,
	@StockId  BIGINT,
	@InqId   BIGINT,
	@UserId BIGINT 
AS
BEGIN

	DECLARE @CarDetails VARCHAR(Max),@INQLeadId BIGINT
	
	SELECT	@CarDetails=V.Make + ' ' + V.Model + ' '  + V.Version + ' '  
	FROM	TC_Stock S WITH(NOLOCK)
			INNER JOIN vwMMV V ON S.VersionId=V.VersionId
			WHERE S.Id=@StockId
			
	SELECT @INQLeadId=L.TC_InquiriesLeadId FROM TC_InquiriesLead L 
	INNER JOIN TC_BuyerInquiries B WITH(NOLOCK) On B.TC_InquiriesLeadId=L.TC_InquiriesLeadId
	WHERE B.TC_BuyerInquiriesId=@InqId
	
	IF EXISTS(SELECT TOP(1) Id FROM TC_Stock WHERE BranchId = @BranchId AND Id=@StockId AND IsActive = 1)
	BEGIN
		UPDATE TC_BuyerInquiries SET StockId=@StockId WHERE TC_BuyerInquiriesId=@InqId
		
		UPDATE	TC_InquiriesLead 
		SET		CarDetails=@CarDetails, --LatestInquiryDate=GETDATE(), Modified By Vivek Gupta on 4th july,2013
				ModifiedBy=@UserId,ModifiedDate=GETDATE()
		WHERE	TC_InquiriesLeadId=@INQLeadId
		
		IF NOT EXISTS(SELECT StockId FROM TC_StockAnalysis WHERE StockId = @StockId )
		BEGIN
			INSERT INTO TC_StockAnalysis(StockId, CWResponseCount, TCResponseCount) VALUES(@StockId, 0, 0)
		END
		-- Update TCResponseCount to Table.
		UPDATE TC_StockAnalysis Set TCResponseCount = TCResponseCount + 1 WHERE StockId = @StockId
	END
END

/****** Object:  StoredProcedure [dbo].[TC_INQNewCarBuyerSave]    Script Date: 08/22/2013 09:11:02 ******/
SET ANSI_NULLS ON
