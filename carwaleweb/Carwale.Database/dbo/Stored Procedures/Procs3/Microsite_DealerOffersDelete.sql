IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DealerOffersDelete]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DealerOffersDelete]
GO

	-- =============================================
-- Author:		Chetan Kane
-- Create date: 13/3/2012
-- Description:	To Delete specific Dealer offers whos id is passed
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_DealerOffersDelete]
(  
 @Id INT
) 	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE  Microsite_DealerOffers SET IsDeleted = 1  
	WHERE Id = @Id
	RETURN 1
END



