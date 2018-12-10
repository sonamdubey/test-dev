IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetBmwDealership]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetBmwDealership]
GO

	-- =============================================
-- Author:		<Author,,Ashish Verma>
-- Create date: <Create Date,,14/01/2014>
-- Description:	<Description,,For Getting Bmw Dealer Number based on dealerId>
-- =============================================
CREATE PROCEDURE [dbo].[GetBmwDealership]
	-- Add the parameters for the stored procedure here
	@DealerId Int,
	@DealerNumber Varchar(50) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @DealerNumber = DealerNumber from BmwDealers where DealerId = @DealerId  
END

