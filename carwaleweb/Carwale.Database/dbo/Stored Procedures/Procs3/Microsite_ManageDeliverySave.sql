IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_ManageDeliverySave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_ManageDeliverySave]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 15-04-2015
-- Description:	save delivery time
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_ManageDeliverySave]
 @DealerId INT,      
 @Id INT,     
 @CityId INT  ,  
 @VersionId INT = NULL,
 @BookingAmount VARCHAR(20),
 @DeliveryTime VARCHAR(50),
 @Status INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SET @Status = 1

    IF(@Id IS NULL)
	BEGIN
		IF NOT EXISTS (SELECT * FROM Microsite_DeliveryTime WITH(NOLOCK) WHERE VersionId = @VersionId AND DealerId = @DealerId AND CityId = @CityId)
		BEGIN
		  INSERT INTO Microsite_DeliveryTime 
				  (VersionId, 
				   BookingAmount, 
				   DeliveryTime, 
				   EntryDate, 
				   DealerId, 
				   CityId)
		  VALUES  (@VersionId, 
				   @BookingAmount, 
				   @DeliveryTime, 
				   GETDATE(), 
				   @DealerId, 
				   @CityId)
		END
		ELSE
		BEGIN
		  SET @Status = 0
		END
	END

	ELSE 
	BEGIN
		 UPDATE Microsite_DeliveryTime 
		 SET  BookingAmount = @BookingAmount, 
			  DeliveryTime = @DeliveryTime, 
			  ModifiedDate = GETDATE()
		 WHERE 
				DealerId = @DealerId
		 AND    CityId = @CityId
		 AND	VersionId = @VersionId
	END

END




