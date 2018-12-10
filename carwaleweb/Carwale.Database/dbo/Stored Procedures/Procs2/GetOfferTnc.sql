IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOfferTnc]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetOfferTnc]
GO

	-- =============================================
-- Author:		<Author,,Ashish verma>
-- Create date: <Create Date,29/11/2014,>
-- Description:	<Description,getting offer Tnc ,>
-- =============================================
CREATE PROCEDURE [dbo].[GetOfferTnc] 
	-- Add the parameters for the stored procedure here
	@OfferId Int,
	@Tnc Varchar(5000) output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @Tnc = Conditions from DealerOffers 
	WHERE ID = @OfferId
END

