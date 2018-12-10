IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MMBuyAudit]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MMBuyAudit]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 18-Dec-2013
-- Description:	Audit Mix-N-Match Customer Buy details.
-- Table : TC_MMAudit
-- =============================================
CREATE PROCEDURE [dbo].[TC_MMBuyAudit] 
	@DealerId INT,
	@UserId INT,
	@CustomerId BIGINT,
	@BuyDate DATETIME,
	@ABInquiryId BIGINT,
	@CWInquiryId BIGINT,
	@BuyPoints INT
AS
BEGIN
--SET NOCOUNT ON;
INSERT INTO [dbo].[TC_MMAudit]
           ([DealerId]
           ,[UserId]
           ,[CustomerId]
           ,[BuyDate]
           ,[CWInquiryId]
           ,[BuyPoints]
           ,[ABInquiryId])
     VALUES
           (@DealerId,
		    @UserId,
			@CustomerId,
			@BuyDate,
			@CWInquiryId,
			@BuyPoints,
			@ABInquiryId
           )
END
