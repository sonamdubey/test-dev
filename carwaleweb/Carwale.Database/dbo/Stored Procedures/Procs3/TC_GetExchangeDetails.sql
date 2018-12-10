IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetExchangeDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetExchangeDetails]
GO
	-- =============================================
-- Author:		Vishal Srivastava
-- Create date: 07/04/2014 1057 HRS IST
-- Description:	Get the Exchange Details
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetExchangeDetails]
	-- Add the parameters for the stored procedure here
	@inqId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT VW.MakeId, VW.ModelId ,TENC.CarVersionId, TENC.Kms, TENC.MakeYear,TENC.ExpectedPrice 
	FROM TC_ExchangeNewCar AS TENC WITH(NOLOCK)
	JOIN vwMMV AS VW WITH(NOLOCK) ON VW.VersionId=TENC.CarVersionId WHERE TC_NewCarInquiriesId=@inqId
END
