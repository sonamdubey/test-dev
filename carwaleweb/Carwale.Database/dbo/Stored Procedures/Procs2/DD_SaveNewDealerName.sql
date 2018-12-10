IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_SaveNewDealerName]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_SaveNewDealerName]
GO

	
-----------------------------------------------------------------------------------------


-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <2/10/2014>
-- Description:	<Save Dealer OutletAddr>
-- =============================================
CREATE PROCEDURE [dbo].[DD_SaveNewDealerName]
	@OutletType			TINYINT ,
	@MakeId				INT ,
	@EmailId			VARCHAR(100) ,
	@DealerName			VARCHAR(100),	
	@NewId				INT		OUTPUT
AS
BEGIN
		SET @NewId = -1
		SELECT @NewId = ID FROM DD_DealerNames WHERE Name = @DealerName
		IF(@@ROWCOUNT < 1 )
		BEGIN
			--SELECT  DD_DealerNamesId 
			--FROM DD_DealerOutlets DO
			--WHERE  OutletType = @OutletType AND MakeId = @MakeId --AND EMailId = @EmailId 
		
			--IF(@@ROWCOUNT < 1 )
			--BEGIN
				INSERT INTO DD_DealerNames(Name,IsActive,CreatedOn,CreatedBy) 
				SELECT TOP 1 Name,IsActive,CreatedOn,CreatedBy 
				FROM DD_TempDealerNames 
				ORDER BY ID DESC
				SET @NewId = SCOPE_IDENTITY()
			--END
		END
END

