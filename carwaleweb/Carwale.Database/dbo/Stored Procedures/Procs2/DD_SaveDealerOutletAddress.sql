IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_SaveDealerOutletAddress]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_SaveDealerOutletAddress]
GO

	


-------------------------------------------------------------------------------------------

-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <29/10/2014>
-- Description:	<Save Dealer OutletAddr>
-- =============================================
CREATE PROCEDURE [dbo].[DD_SaveDealerOutletAddress]
	@Address			VARCHAR(500),
	@AddressId			INT,
	@DD_DealerNamesId	INT,
	@CityId				INT,
	@AreaId				INT,
	@Pincode			VARCHAR(7),
	@Longitude			FLOAT,
	@Latitude			FLOAT,
	@CreatedBy			INT,
	@Map				TINYINT,
	@OutletId			INT,
	@NewId				INT		OUTPUT
AS
BEGIN
		IF (@AddressId <> -1)
		BEGIN
			UPDATE DD_Addresses SET Address=@Address  , CityId=@CityId , AreaId =@AreaId , Pincode = @Pincode , Latitude =@Latitude , Longitude = @Longitude WHERE Id = @AddressId AND DD_DealerNamesId =@DD_DealerNamesId
			SET @NewId = 0
		END
		ELSE
		BEGIN
			IF NOT EXISTS(SELECT * FROM DD_Addresses WHERE Address=@Address  AND CityId=@CityId AND AreaId =@AreaId AND Pincode = @Pincode AND Latitude =@Latitude AND Longitude = @Longitude)--AND DD_DealerNamesId =@DD_DealerNamesId
			BEGIN
				INSERT INTO DD_Addresses (Address , DD_DealerNamesId , CityId , AreaId , Pincode , Latitude , Longitude ,  CreatedBy , CreatedOn) 
				VALUES(@Address , @DD_DealerNamesId , @CityId , @AreaId , @Pincode ,@Latitude, @Longitude , @CreatedBy , GETDATE())
				SET @NewId = SCOPE_IDENTITY()
			END
			IF @Map = 1
			BEGIN
				DECLARE @tmpNewId	INT
				EXEC  [dbo].[DD_MapDealerOutletAddress] @OutletId,@NewId,@CreatedBy,@tmpNewId
			END
		END
END

