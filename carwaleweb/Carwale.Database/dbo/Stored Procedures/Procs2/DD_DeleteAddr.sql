IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_DeleteAddr]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_DeleteAddr]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <27/11/2014>
-- Description:	<Delete dealer address>
-- =============================================
CREATE PROCEDURE [dbo].[DD_DeleteAddr]
	@AddressId	INT
AS
BEGIN
	DELETE FROM DD_Addresses WHERE Id = @AddressId
	IF EXISTS(SELECT ID FROM DD_DealerOutletAddress WHERE DD_AddressesId = @AddressId)
	BEGIN
		DELETE FROM DD_DealerOutletAddress WHERE DD_AddressesId = @AddressId
	END
END

