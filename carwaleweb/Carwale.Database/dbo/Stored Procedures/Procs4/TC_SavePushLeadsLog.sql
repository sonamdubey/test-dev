IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SavePushLeadsLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SavePushLeadsLog]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 20th Aug 2014
-- Description:	To log all the leads pushed to MFC
-- Modified By Vivek Gupta on 18-02-2015, added @MixMatchLead Parameter
-- =============================================
CREATE PROCEDURE [dbo].[TC_SavePushLeadsLog]
@TC_BuyerInquiryId	INT,
@DealerId			INT,
@StockId			INT,
@Name				VARCHAR(50),
@Email				VARCHAR(100),
@Mobile				VARCHAR(15),
@APIResponse		VARCHAR(250),
@MFCSourceId		INT,
@MixMatchLead       BIT = 0
AS
BEGIN
	INSERT INTO TC_PushLeadLog(TC_BuyerInquiryId,DealerId,StockId,Name,Email,Mobile,APIResponse,MFCSourceId,MixMatchLead)
	VALUES (@TC_BuyerInquiryId,@DealerId,@StockId,@Name,@Email,@Mobile,@APIResponse,@MFCSourceId,@MixMatchLead)
END



