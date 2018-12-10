IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_SaveInquiryMapping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_SaveInquiryMapping]
GO

	-- =============================================
-- Author:		Sunil M. Yadav
-- Create date: 01st Sept 2016
-- Description:	Save mapping of advantage new car lead into TC_Deals_InquiriesMapping table.
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_SaveInquiryMapping]
	-- Add the parameters for the stored procedure here
	@CwDealerInqId INT,
	@ActualDealerInqId INT,
	@CreatedBy INT,
	@IsPaid BIT,
	@Id INT OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Id = 0;
	INSERT INTO TC_Deals_InquiriesMapping(CwDealerInqId,ActualDealerInqId,CreatedBy,IsPaid) VALUES(@CwDealerInqId,@ActualDealerInqId,@CreatedBy,@IsPaid)
	
	SET @Id = SCOPE_IDENTITY();
	

END

