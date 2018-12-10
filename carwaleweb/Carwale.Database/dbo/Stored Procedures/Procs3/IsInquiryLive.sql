IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[IsInquiryLive]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[IsInquiryLive]
GO

	-- =============================================
-- Author:		amit verma
-- Create date: 20-11-2013
-- Description:	check if the input inquiry id is present in livelisting table
--				declare @p1 tinyint  exec IsInquiryLive 2599,@p1 out select @p1
-- =============================================
CREATE PROCEDURE [dbo].[IsInquiryLive] 
	-- Add the parameters for the stored procedure here
	@CarId NUMERIC(18,0),
	@IsLive TinyInt = 0 out
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS (SELECT InquiryId FROM livelistings WITH(NOLOCK) WHERE Inquiryid = @CarId AND SellerType = 2)
		SET @IsLive = 1
	ELSE
		SET @IsLive = 0
END

