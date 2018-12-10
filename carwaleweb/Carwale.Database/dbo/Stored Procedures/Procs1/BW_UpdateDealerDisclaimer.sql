IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_UpdateDealerDisclaimer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_UpdateDealerDisclaimer]
GO

	--=========================================================
--Author      : Suresh Prajapati
--Create date : 03rd Dec, 2014.
--Description : To Update Dealer Disclaimer Of Specified Bike Version.
--=========================================================

CREATE PROCEDURE [dbo].[BW_UpdateDealerDisclaimer]
	-- Add the parameters for the stored procedure here
	@DealerId INT
	,@Disclaimer VARCHAR (MAX)
	,@BikeVersionId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Update statements for procedure here
	UPDATE BW_DealerDisclaimer
	SET Disclaimer = @Disclaimer
	WHERE DealerId=@DealerId AND BikeVersionId=@BikeVersionId
END
