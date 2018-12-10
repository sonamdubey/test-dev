IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MyGarage_UpdateCurrentKms]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MyGarage_UpdateCurrentKms]
GO

	
-- =============================================
-- Author:		Satish Sharma	
-- Create date: Nov 6, 2009
-- Description:	To update 'CurrentKm', 'LastUpdatedDate' of registred car in mygarage
-- =============================================
CREATE PROCEDURE [dbo].[MyGarage_UpdateCurrentKms] 
	-- Add the parameters for the stored procedure here
	@MgId					NUMERIC,
	@CurrentKms				INT,
	@UpdatedDateTime		DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE MyCarwaleCars SET CurrentKm = @CurrentKms, LastUpdatedDate = @UpdatedDateTime WHERE Id = @MgId
END

