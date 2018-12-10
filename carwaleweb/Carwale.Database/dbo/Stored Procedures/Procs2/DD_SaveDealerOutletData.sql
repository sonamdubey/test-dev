IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_SaveDealerOutletData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_SaveDealerOutletData]
GO

	



-------------------------------------------------------------------------------------------

-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <29/10/2014>
-- Description:	<Save Dealer OutletData>
-- =============================================
CREATE PROCEDURE [dbo].[DD_SaveDealerOutletData]
	@OutletName			VARCHAR(250) = NULL,
	@OutletId			INT,
	@DD_DealerNamesId	INT,
	@OutletType			TINYINT = NULL,
	@MakeId				INT = NULL,	
	@Website			VARCHAR(100),
	@EmailId			VARCHAR(100) = NULL,
	@DayType			INT,
	@ContactHours		VARCHAR(20) = NULL,
	@Day				VARCHAR(15),
	@CreatedBy			INT = NULL,
	@NewId				INT		OUTPUT
AS
BEGIN
--IF NOT EXISTS(SELECT ID FROM DD_DealerOutlets WHERE OutletName = @OutletName AND DD_DealerNamesId = @DD_DealerNamesId AND OutletType =@OutletType AND  MakeId =@MakeId AND EMailId = @EmailId )
		IF @OutletId <> -1
		BEGIN
			UPDATE DD_DealerOutlets 
			SET OutletName = @OutletName , OutletType =@OutletType ,Website= @Website , MakeId =@MakeId , EMailId = @EmailId , DayType = @DayType , Day = @Day ,ContactHours = @ContactHours
			WHERE Id = @OutletId AND DD_DealerNamesId = @DD_DealerNamesId 
			SET @NewId = @OutletId
		END
		ELSE
		BEGIN
			--IF NOT EXISTS(SELECT ID FROM DD_DealerOutlets WHERE (OutletType =@OutletType AND  MakeId =@MakeId) OR EMailId = @EmailId )
			INSERT INTO DD_DealerOutlets (OutletName , DD_DealerNamesId , OutletType , MakeId ,Website , EMailId ,DayType, ContactHours , Day, CreatedBy , CreatedOn) 
			VALUES(@OutletName , @DD_DealerNamesId , @OutletType , @MakeId ,  @Website , @EMailId , @DayType , @ContactHours , @Day , @CreatedBy , GETDATE())
			SET @NewId = SCOPE_IDENTITY()
		END
END

