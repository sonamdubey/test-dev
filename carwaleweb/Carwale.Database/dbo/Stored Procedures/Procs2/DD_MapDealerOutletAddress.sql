IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_MapDealerOutletAddress]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_MapDealerOutletAddress]
GO

	


-------------------------------------------------------------------------------------------


-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <30/10/2014>
-- Description:	<Save DealerOutlet And Outlet Address>
-- =============================================
CREATE PROCEDURE [dbo].[DD_MapDealerOutletAddress]
	@DD_DealerOutletsId	INT,
	@DD_AddressesId		INT,
	@CreatedBy			INT ,
	@NewId				INT		OUTPUT
AS
BEGIN
		IF  EXISTS (SELECT ID FROM DD_DealerOutletAddress WHERE DD_AddressesId = @DD_AddressesId )
			SET @NewId = -1
		ELSE
		BEGIN
			IF NOT EXISTS(SELECT Id FROM DD_DealerOutletAddress WHERE DD_DealerOutletsId = @DD_DealerOutletsId )
			BEGIN
				INSERT INTO DD_DealerOutletAddress (DD_DealerOutletsId , DD_AddressesId , CreatedBy , CreatedOn) 
				VALUES(@DD_DealerOutletsId , @DD_AddressesId , @CreatedBy , GETDATE())
				SET @NewId = SCOPE_IDENTITY()
			END
			ELSE
			BEGIN
			
				BEGIN
					UPDATE DD_DealerOutletAddress SET DD_AddressesId = @DD_AddressesId WHERE DD_DealerOutletsId = @DD_DealerOutletsId
					SET @NewId = 0
					PRINT @NewId
				END
			END 
		END
		
END

